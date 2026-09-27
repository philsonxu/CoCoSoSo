using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter03_Udp
{
    /// <summary>
    /// 3.1 UDP Echo 客户端+服务器（自演示）
    /// 知识点：UdpClient、JoinMulticastGroup/单点、Receive/Send、IPEndPoint任意端点、
    ///         UDP无连接特性、消息边界保留
    /// </summary>
    public class _01_UdpEcho : ISample
    {
        public string Id { get { return "3.1"; } }
        public string Title { get { return "UDP Echo (无连接收发)"; } }
        public string Description { get { return "演示UdpClient创建、服务器绑定端口循环Receive、客户端Send+Receive、IPEndPoint.MinusOne获取远程端点。"; } }

        public const int Port = 9301;

        public void Run()
        {
            Thread srv = new Thread(new ThreadStart(RunServer));
            srv.IsBackground = true;
            srv.Start();
            Thread.Sleep(200);

            RunClient();

            Thread.Sleep(300);
        }

        private void RunServer()
        {
            using (UdpClient udp = new UdpClient(Port))
            {
                Console.WriteLine("[UDP-Server] 监听端口 {0}", Port);
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                for (int i = 0; i < 5; i++) // 处理5条后退出
                {
                    byte[] data = udp.Receive(ref remote);
                    string msg = Encoding.UTF8.GetString(data);
                    Console.WriteLine("[UDP-Server] 收到 {0} 来自 {1}: {2}", data.Length, remote, msg);
                    // 原样回显
                    byte[] echo = Encoding.UTF8.GetBytes("echo:" + msg);
                    udp.Send(echo, echo.Length, remote);
                    Console.WriteLine("[UDP-Server] 回显 {0} 字节", echo.Length);
                }
            }
            Console.WriteLine("[UDP-Server] 退出");
        }

        private void RunClient()
        {
            using (UdpClient udp = new UdpClient(0)) // 0=随机端口
            {
                IPEndPoint server = new IPEndPoint(IPAddress.Loopback, Port);
                string[] msgs = new string[] { "hello", "udp", "no connection", "message boundary", "quit" };
                foreach (string m in msgs)
                {
                    byte[] data = Encoding.UTF8.GetBytes(m);
                    udp.Send(data, data.Length, server);
                    Console.WriteLine("[UDP-Client] 发送: {0}", m);

                    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    byte[] resp = udp.Receive(ref remote);
                    string respStr = Encoding.UTF8.GetString(resp);
                    Console.WriteLine("[UDP-Client] 收到: {0}", respStr);
                    Thread.Sleep(100);
                }
            }
        }
    }
}
