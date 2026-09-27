using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter02_Tcp
{
    /// <summary>
    /// 2.3 TCP 多客户端群聊服务器
    /// 知识点：Thread+TcpClient多线程、客户端列表加锁(ArrayList+lock)、广播(Broadcast)、
    ///         行协议、客户端昵称管理
    /// </summary>
    public class _03_TcpChatServer : ISample
    {
        public string Id { get { return "2.3"; } }
        public string Title { get { return "TCP 多线程群聊服务器"; } }
        public string Description { get { return "使用Thread+lock实现的多客户端广播群聊服务器：每个客户端一个线程，共用客户端列表加锁保护，消息广播给所有人。"; } }

        public const int Port = 9203;
        private ArrayList clients = ArrayList.Synchronized(new ArrayList());
        private bool running = true;

        public void Run()
        {
            Thread listenerThread = new Thread(new ThreadStart(ListenLoop));
            listenerThread.IsBackground = true;
            listenerThread.Start();

            Console.WriteLine("群聊服务器已启动，端口 {0}。请运行 2.4 客户端连接。", Port);
            Console.WriteLine("服务器将运行15秒后自动停止，演示期间请在另一个终端启动客户端...");
            Thread.Sleep(15000);
            running = false;
            lock (clients.SyncRoot)
            {
                foreach (ClientInfo ci in clients)
                {
                    try { ci.Stream.Close(); ci.Client.Close(); } catch { }
                }
            }
            Console.WriteLine("群聊服务器演示结束");
        }

        private void ListenLoop()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            while (running)
            {
                IAsyncResult ar = listener.BeginAcceptTcpClient(null, null);
                if (!ar.AsyncWaitHandle.WaitOne(500, false)) continue;
                TcpClient client;
                try { client = listener.EndAcceptTcpClient(ar); }
                catch { break; }
                ClientInfo ci = new ClientInfo();
                ci.Client = client;
                ci.Stream = client.GetStream();
                ci.Name = "Client" + client.Client.RemoteEndPoint;
                lock (clients.SyncRoot) clients.Add(ci);
                Thread t = new Thread(new ParameterizedThreadStart(ClientLoop));
                t.IsBackground = true;
                t.Start(ci);
                Broadcast("系统", ci.Name + " 加入群聊");
                Console.WriteLine("[+] {0} 加入，当前在线 {1} 人", ci.Name, clients.Count);
            }
            listener.Stop();
        }

        private void ClientLoop(object state)
        {
            ClientInfo ci = (ClientInfo)state;
            byte[] buf = new byte[2048];
            StringBuilder line = new StringBuilder();
            try
            {
                while (running)
                {
                    int n = ci.Stream.Read(buf, 0, buf.Length);
                    if (n <= 0) break;
                    for (int i = 0; i < n; i++)
                    {
                        char c = (char)buf[i];
                        if (c == '\n')
                        {
                            string msg = line.ToString().Trim('\r');
                            line.Length = 0;
                            if (msg.ToLower() == "quit") goto END;
                            if (msg.StartsWith("name:"))
                            {
                                string newName = msg.Substring(5).Trim();
                                if (newName.Length > 0)
                                {
                                    Broadcast("系统", ci.Name + " 改名为 " + newName);
                                    ci.Name = newName;
                                }
                            }
                            else
                            {
                                Broadcast(ci.Name, msg);
                            }
                        }
                        else line.Append(c);
                    }
                }
            }
            catch { }
        END:
            lock (clients.SyncRoot) clients.Remove(ci);
            try { ci.Stream.Close(); ci.Client.Close(); } catch { }
            Broadcast("系统", ci.Name + " 离开群聊");
            Console.WriteLine("[-] {0} 离开，当前在线 {1} 人", ci.Name, clients.Count);
        }

        private void Broadcast(string from, string msg)
        {
            string text = "[" + from + "] " + msg + "\n";
            byte[] data = Encoding.UTF8.GetBytes(text);
            lock (clients.SyncRoot)
            {
                foreach (ClientInfo ci in clients)
                {
                    try { ci.Stream.Write(data, 0, data.Length); } catch { }
                }
            }
        }

        private class ClientInfo
        {
            public TcpClient Client;
            public NetworkStream Stream;
            public string Name;
        }
    }
}
