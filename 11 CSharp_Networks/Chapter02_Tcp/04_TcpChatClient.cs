using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter02_Tcp
{
    /// <summary>
    /// 2.4 TCP 群聊客户端
    /// 知识点：TcpClient、NetworkStream、双线程（收/发）、控制台昵称设置、quit命令
    /// </summary>
    public class _04_TcpChatClient : ISample
    {
        public string Id { get { return "2.4"; } }
        public string Title { get { return "TCP 群聊客户端"; } }
        public string Description { get { return "群聊客户端：收发分离双线程，支持设置昵称(name:xxx)和退出(quit)，服务器由2.3启动。"; } }

        public void Run()
        {
            Console.Write("输入昵称(默认User): ");
            string nick = Console.ReadLine();
            if (nick == null || nick.Trim().Length == 0) nick = "User";

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect("127.0.0.1", _03_TcpChatServer.Port);
                    Console.WriteLine("已连接群聊服务器！输入 name:新昵称 可改名，输入 quit 退出");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接失败：{0}（请先运行 2.3 启动服务器）", ex.Message);
                    Console.WriteLine("演示：模拟三条群聊消息...");
                    Demo(nick);
                    return;
                }

                using (NetworkStream ns = client.GetStream())
                {
                    // 发送昵称
                    byte[] nameData = Encoding.UTF8.GetBytes("name:" + nick + "\n");
                    ns.Write(nameData, 0, nameData.Length);

                    // 接收线程
                    Thread recvT = new Thread(new ParameterizedThreadStart(RecvLoop));
                    recvT.IsBackground = true;
                    recvT.Start(ns);

                    // 主线程发送
                    while (true)
                    {
                        string line = Console.ReadLine();
                        if (line == null) break;
                        if (line.Length == 0) continue;
                        byte[] data = Encoding.UTF8.GetBytes(line + "\n");
                        ns.Write(data, 0, data.Length);
                        if (line.ToLower() == "quit") break;
                    }
                }
            }
        }

        private void RecvLoop(object state)
        {
            NetworkStream ns = (NetworkStream)state;
            byte[] buf = new byte[2048];
            StringBuilder sb = new StringBuilder();
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
                            Console.WriteLine(sb.ToString().Trim('\r'));
                            sb.Length = 0;
                        }
                        else sb.Append(c);
                    }
                }
            }
            catch { }
        }

        private void Demo(string nick)
        {
            Console.WriteLine("[系统] {0} 加入群聊", nick);
            Console.WriteLine("[{0}] 大家好！", nick);
            Thread.Sleep(200);
            Console.WriteLine("[系统] Alice 加入群聊");
            Thread.Sleep(200);
            Console.WriteLine("[Alice] hi~");
            Thread.Sleep(200);
            Console.WriteLine("[{0}] hello Alice", nick);
            Thread.Sleep(200);
            Console.WriteLine("[系统] {0} 离开群聊", nick);
        }
    }
}
