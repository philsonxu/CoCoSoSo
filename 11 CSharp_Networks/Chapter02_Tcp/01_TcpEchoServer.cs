using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSharpNetworkProgramming.Chapter02_Tcp
{
    /// <summary>
    /// 2.1 TCP Echo 同步服务器（单客户端）
    /// 知识点：TcpListener.Start/AcceptTcpClient/Stop、同步Accept、
    ///         NetworkStream 同步Read/Write、行协议(换行分隔消息)
    /// </summary>
    public class _01_TcpEchoServer : ISample
    {
        public string Id { get { return "2.1"; } }
        public string Title { get { return "TCP Echo 同步服务器(单客户端)"; } }
        public string Description { get { return "演示最基本的同步TCP服务器：启动TcpListener监听，AcceptTcpClient接受一个客户端连接，将收到的每一行原样回显，输入exit断开。"; } }

        public const int Port = 9201;

        public void Run()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("Echo服务器已启动，监听端口 {0}。请在另一终端运行 2.2 客户端连接。", Port);
            Console.WriteLine("按回车结束本示例...（等待客户端5秒，若未连接则自动演示）");

            IAsyncResult ar = listener.BeginAcceptTcpClient(null, null);
            bool connected = ar.AsyncWaitHandle.WaitOne(5000, false);
            if (!connected)
            {
                Console.WriteLine("(5秒内无客户端连接，服务器演示结束)");
                listener.Stop();
                return;
            }
            using (TcpClient client = listener.EndAcceptTcpClient(ar))
            {
                Console.WriteLine("客户端已连接: {0}", client.Client.RemoteEndPoint);
                using (NetworkStream ns = client.GetStream())
                {
                    byte[] buf = new byte[1024];
                    StringBuilder line = new StringBuilder();
                    while (true)
                    {
                        int n = ns.Read(buf, 0, buf.Length);
                        if (n <= 0) break;
                        for (int i = 0; i < n; i++)
                        {
                            char c = (char)buf[i];
                            if (c == '\n')
                            {
                                string msg = line.ToString().Trim('\r');
                                Console.WriteLine("收到: {0}", msg);
                                if (msg.ToLower() == "exit")
                                {
                                    byte[] bye = Encoding.UTF8.GetBytes("bye\n");
                                    ns.Write(bye, 0, bye.Length);
                                    goto END;
                                }
                                string echo = "echo:" + msg + "\n";
                                byte[] data = Encoding.UTF8.GetBytes(echo);
                                ns.Write(data, 0, data.Length);
                                line.Length = 0;
                            }
                            else
                            {
                                line.Append(c);
                            }
                        }
                    }
                END:;
                }
            }
            listener.Stop();
            Console.WriteLine("服务器已停止");
        }
    }
}
