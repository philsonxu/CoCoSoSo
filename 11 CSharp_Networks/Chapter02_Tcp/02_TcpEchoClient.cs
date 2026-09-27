using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter02_Tcp
{
    /// <summary>
    /// 2.2 TCP Echo 客户端
    /// 知识点：TcpClient.Connect、NetworkStream 同步读写、控制台交互、双线程收发
    /// </summary>
    public class _02_TcpEchoClient : ISample
    {
        public string Id { get { return "2.2"; } }
        public string Title { get { return "TCP Echo 客户端"; } }
        public string Description { get { return "TCP Echo客户端：连接服务器，一个线程读取控制台输入并发给服务器，另一个线程接收服务器回显。"; } }

        public void Run()
        {
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect("127.0.0.1", _01_TcpEchoServer.Port);
                    Console.WriteLine("已连接到服务器 127.0.0.1:{0}", _01_TcpEchoServer.Port);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接失败：{0}（请先运行示例 2.1 启动服务器）", ex.Message);
                    // 即使无法连接，也演示一下发送/接收流程的代码逻辑
                    Console.WriteLine("演示：发送 3 条测试消息...");
                    DemoSend();
                    return;
                }

                using (NetworkStream ns = client.GetStream())
                {
                    // 接收线程
                    Thread recvThread = new Thread(new ParameterizedThreadStart(ReceiveLoop));
                    recvThread.IsBackground = true;
                    recvThread.Start(ns);

                    // 主线程从控制台读取并发送
                    Console.WriteLine("请输入消息（每行一条，输入 exit 退出）：");
                    while (true)
                    {
                        string line = Console.ReadLine();
                        if (line == null) break;
                        if (line.Length == 0) continue;
                        byte[] data = Encoding.UTF8.GetBytes(line + "\n");
                        ns.Write(data, 0, data.Length);
                        if (line.ToLower() == "exit") break;
                    }
                }
            }
            Thread.Sleep(300);
            Console.WriteLine("客户端已退出");
        }

        private void ReceiveLoop(object state)
        {
            NetworkStream ns = (NetworkStream)state;
            byte[] buf = new byte[1024];
            StringBuilder line = new StringBuilder();
            try
            {
                while (true)
                {
                    int n = ns.Read(buf, 0, buf.Length);
                    if (n <= 0) break;
                    for (int i = 0; i < n; i++)
                    {
                        char c = (char)buf[i];
                        if (c == '\n')
                        {
                            Console.WriteLine("<< {0}", line.ToString().Trim('\r'));
                            line.Length = 0;
                        }
                        else line.Append(c);
                    }
                }
            }
            catch { }
        }

        private void DemoSend()
        {
            // 在无法连接真实服务器时，演示发送/接收流程的代码结构
            string[] messages = new string[] { "hello", "world", "exit" };
            foreach (string m in messages)
            {
                Console.WriteLine(">> {0}", m);
                Console.WriteLine("<< echo:{0}", m);
                Thread.Sleep(200);
            }
        }
    }
}
