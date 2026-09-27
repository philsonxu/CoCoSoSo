using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter03_Udp
{
    /// <summary>
    /// 3.4 UDP 广播 (Broadcast)
    /// 知识点：Socket.EnableBroadcast、IPAddress.Broadcast(255.255.255.255)、
    ///         定向子网广播(如192.168.1.255)、多个接收者接收同一广播包、SO_BROADCAST
    /// </summary>
    public class _04_UdpBroadcast : ISample
    {
        public string Id { get { return "3.4"; } }
        public string Title { get { return "UDP 广播消息"; } }
        public string Description { get { return "演示UDP广播：在指定端口启动多个监听者，向255.255.255.255广播消息，局域网内所有监听者都能收到。"; } }

        public const int Port = 9304;
        private const int ListenerCount = 3;

        public void Run()
        {
            // 启动多个监听者（不同端口不行，广播必须同一端口；演示中用同一端口在本机上通过UdpClient复用）
            // 由于同机同端口需允许多次绑定，使用 Socket.ExclusiveAddressUse = false
            Thread[] listeners = new Thread[ListenerCount];
            for (int i = 0; i < ListenerCount; i++)
            {
                int idx = i + 1;
                listeners[i] = new Thread(new ThreadStart(delegate() { RunListener(idx); }));
                listeners[i].IsBackground = true;
                listeners[i].Start();
            }
            Thread.Sleep(300);

            // 发送3次广播
            RunBroadcaster();

            Thread.Sleep(800);
            Console.WriteLine("UDP广播演示结束（注意：同一进程内多监听者在部分系统上只有一个能收到包，这是正常现象；在不同主机/网卡上则所有监听者均可收到）");
        }

        private void RunListener(int id)
        {
            try
            {
                using (UdpClient udp = new UdpClient())
                {
                    udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    udp.Client.ExclusiveAddressUse = false;
                    udp.Client.Bind(new IPEndPoint(IPAddress.Any, Port));
                    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    // 异步等待
                    for (int i = 0; i < 5; i++)
                    {
                        IAsyncResult ar = udp.BeginReceive(null, null);
                        if (!ar.AsyncWaitHandle.WaitOne(2000, false)) break;
                        byte[] data = udp.EndReceive(ar, ref remote);
                        string msg = Encoding.UTF8.GetString(data);
                        Console.WriteLine("[Listener#{0}] 收到广播来自 {1}: {2}", id, remote, msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Listener#{0}] 异常: {1}", id, ex.Message);
            }
        }

        private void RunBroadcaster()
        {
            using (UdpClient udp = new UdpClient())
            {
                udp.EnableBroadcast = true;
                IPEndPoint bcast = new IPEndPoint(IPAddress.Broadcast, Port);
                for (int i = 1; i <= 3; i++)
                {
                    string msg = string.Format("BroadCast Msg #{0} at {1:HH:mm:ss}", i, DateTime.Now);
                    byte[] data = Encoding.UTF8.GetBytes(msg);
                    int sent = udp.Send(data, data.Length, bcast);
                    Console.WriteLine("[Broadcaster] 广播 {0} 字节: {1}", sent, msg);
                    Thread.Sleep(300);
                }
                // 同时演示向Loopback发送，确保本机测试至少有一个监听者收到
                IPEndPoint loop = new IPEndPoint(IPAddress.Loopback, Port);
                byte[] extra = Encoding.UTF8.GetBytes("Loopback direct msg");
                udp.Send(extra, extra.Length, loop);
            }
        }
    }
}
