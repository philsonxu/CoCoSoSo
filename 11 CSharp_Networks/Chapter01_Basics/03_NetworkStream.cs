using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter01_Basics
{
    /// <summary>
    /// 1.3 NetworkStream 网络流基础示例
    /// 知识点：TcpListener/TcpClient、NetworkStream、Read/Write、ReadByte/WriteByte、
    ///         数据编码(Encoding)、同步阻塞模式
    /// </summary>
    public class _03_NetworkStream : ISample
    {
        public string Id { get { return "1.3"; } }
        public string Title { get { return "NetworkStream 网络流基础读写"; } }
        public string Description { get { return "演示基于TcpListener/TcpClient建立TCP连接，使用NetworkStream做字节级、字节数组级同步读写，理解TCP流式特性。"; } }

        private const int Port = 9100;

        public void Run()
        {
            // 在后台线程启动简易服务器
            Thread serverThread = new Thread(new ThreadStart(RunServer));
            serverThread.IsBackground = true;
            serverThread.Start();
            Thread.Sleep(300); // 等待服务器启动

            // 客户端连接并演示各种读写方式
            RunClient();

            serverThread.Join(2000);
        }

        private void RunServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, Port);
            listener.Start();
            Console.WriteLine("[Server] 已启动，监听 {0}", listener.LocalEndpoint);

            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream ns = client.GetStream())
            {
                Console.WriteLine("[Server] 客户端已连接: {0}", client.Client.RemoteEndPoint);

                // 1) 读取字节数组
                byte[] buffer = new byte[1024];
                int n = ns.Read(buffer, 0, buffer.Length);
                string msg = Encoding.UTF8.GetString(buffer, 0, n);
                Console.WriteLine("[Server] Read() 收到: {0}", msg);

                // 2) 回写响应
                string resp = "HTTP/1.1 200 OK\r\nContent-Length: 11\r\n\r\nHello World";
                byte[] respBytes = Encoding.ASCII.GetBytes(resp);
                ns.Write(respBytes, 0, respBytes.Length);
                Console.WriteLine("[Server] Write() 已响应 {0} 字节", respBytes.Length);

                // 3) 单字节读写
                int oneByte = ns.ReadByte();
                Console.WriteLine("[Server] ReadByte() 收到字节: {0} (0x{0:X2})", oneByte);
                ns.WriteByte((byte)(oneByte + 1));
                Console.WriteLine("[Server] WriteByte() 回写字节: {0} (0x{0:X2})", oneByte + 1);
            }
            listener.Stop();
            Console.WriteLine("[Server] 已停止");
        }

        private void RunClient()
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect(IPAddress.Loopback, Port);
                Console.WriteLine("[Client] 已连接服务器");

                using (NetworkStream ns = client.GetStream())
                {
                    // 1) Write 字节数组发送
                    string msg = "GET / HTTP/1.1\r\nHost: localhost\r\n\r\n";
                    byte[] sendBytes = Encoding.ASCII.GetBytes(msg);
                    ns.Write(sendBytes, 0, sendBytes.Length);
                    Console.WriteLine("[Client] Write() 发送 {0} 字节请求", sendBytes.Length);

                    // 2) 用StreamReader读取响应行（演示Stream包装）
                    StreamReader reader = new StreamReader(ns, Encoding.ASCII);
                    string statusLine = reader.ReadLine();
                    Console.WriteLine("[Client] ReadLine() 状态行: {0}", statusLine);
                    string contentLenLine = reader.ReadLine();
                    Console.WriteLine("[Client] ReadLine() Content-Length行: {0}", contentLenLine);
                    string empty = reader.ReadLine();
                    Console.WriteLine("[Client] ReadLine() 空行: \"{0}\"", empty);
                    char[] body = new char[11];
                    int read = reader.Read(body, 0, body.Length);
                    Console.WriteLine("[Client] Read() 响应体({0}字符): {1}", read, new string(body));

                    // 3) 单字节回环测试
                    byte sendByte = 65; // 'A'
                    ns.WriteByte(sendByte);
                    Console.WriteLine("[Client] WriteByte() 发送: {0} ('{1}')", sendByte, (char)sendByte);
                    int recvByte = ns.ReadByte();
                    Console.WriteLine("[Client] ReadByte() 收到: {0} ('{1}')", recvByte, (char)recvByte);
                }
            }
        }
    }
}
