using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter03_Udp
{
    /// <summary>
    /// 3.2 UDP 群聊（自维护客户端列表，无需连接）
    /// 知识点：UDP无连接多客户端、ArrayList存客户端EndPoint、过期清理、广播循环发送、
    ///         简单心跳机制（每发一条算活跃）
    /// </summary>
    public class _02_UdpChat : ISample
    {
        public string Id { get { return "3.2"; } }
        public string Title { get { return "UDP 无连接群聊(服务端转发)"; } }
        public string Description { get { return "UDP群聊：服务器记录每个发送过消息的客户端IPEndPoint，收到消息后转发给所有已记录端点，演示UDP无连接场景下的客户端管理。"; } }

        public const int Port = 9302;
        private Hashtable clients = new Hashtable(); // EndPoint -> last seen tick
        private bool running = true;

        public void Run()
        {
            Thread srv = new Thread(new ThreadStart(RunServer));
            srv.IsBackground = true;
            srv.Start();
            Thread.Sleep(200);

            // 启动3个模拟客户端
            Thread c1 = new Thread(new ParameterizedThreadStart(RunClient));
            Thread c2 = new Thread(new ParameterizedThreadStart(RunClient));
            Thread c3 = new Thread(new ParameterizedThreadStart(RunClient));
            c1.IsBackground = c2.IsBackground = c3.IsBackground = true;
            c1.Start("Alice"); c2.Start("Bob"); c3.Start("Charlie");
            c1.Join(); c2.Join(); c3.Join();

            running = false;
            Thread.Sleep(300);
            Console.WriteLine("UDP群聊演示结束");
        }

        private void RunServer()
        {
            using (UdpClient udp = new UdpClient(Port))
            {
                Console.WriteLine("[UDP-Chat-Server] 监听 {0}", Port);
                while (running)
                {
                    IAsyncResult ar = udp.BeginReceive(null, null);
                    if (!ar.AsyncWaitHandle.WaitOne(300, false))
                    {
                        CleanupStale();
                        continue;
                    }
                    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data;
                    try { data = udp.EndReceive(ar, ref remote); }
                    catch { continue; }
                    string msg = Encoding.UTF8.GetString(data);
                    lock (clients.SyncRoot)
                    {
                        clients[remote] = Environment.TickCount;
                    }
                    Console.WriteLine("[Chat-Srv] {0} -> {1}", remote, msg);
                    // 广播给所有人（含自己）
                    Broadcast(data, remote);
                }
            }
        }

        private void Broadcast(byte[] data, EndPoint from)
        {
            lock (clients.SyncRoot)
            {
                foreach (DictionaryEntry de in clients)
                {
                    IPEndPoint ep = (IPEndPoint)de.Key;
                    try
                    {
                        using (UdpClient s = new UdpClient())
                        {
                            s.Send(data, data.Length, ep);
                        }
                    }
                    catch { }
                }
            }
        }

        private void CleanupStale()
        {
            int now = Environment.TickCount;
            lock (clients.SyncRoot)
            {
                ArrayList stale = new ArrayList();
                foreach (DictionaryEntry de in clients)
                {
                    int last = (int)de.Value;
                    if (now - last > 10000) stale.Add(de.Key);
                }
                foreach (object k in stale) clients.Remove(k);
            }
        }

        private void RunClient(object nameObj)
        {
            string name = (string)nameObj;
            using (UdpClient udp = new UdpClient(0))
            {
                IPEndPoint server = new IPEndPoint(IPAddress.Loopback, Port);
                udp.Client.ReceiveTimeout = 2000;

                // 接收线程
                Thread recvT = new Thread(new ParameterizedThreadStart(delegate(object o)
                {
                    UdpClient u = (UdpClient)o;
                    IPEndPoint any = new IPEndPoint(IPAddress.Any, 0);
                    while (running)
                    {
                        try
                        {
                            byte[] d = u.Receive(ref any);
                            string m = Encoding.UTF8.GetString(d);
                            // 只打印不是自己发的（UDP服务器会回显自己消息，简单忽略）
                            if (!m.StartsWith(name + ":"))
                                Console.WriteLine("  ({0} 收到) {1}", name, m);
                        }
                        catch { break; }
                    }
                }));
                recvT.IsBackground = true;
                recvT.Start(udp);

                string[] lines = new string[] { "大家好", "UDP真方便", "再见" };
                for (int i = 0; i < lines.Length; i++)
                {
                    string full = name + ": " + lines[i];
                    byte[] d = Encoding.UTF8.GetBytes(full);
                    udp.Send(d, d.Length, server);
                    Thread.Sleep(400);
                }
            }
        }
    }
}
