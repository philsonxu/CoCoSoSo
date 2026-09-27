using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter02_Tcp
{
    /// <summary>
    /// 2.6 TCP 多线程并发服务器（ThreadPool 版，"一问一答"协议）
    /// 知识点：ThreadPool.QueueUserWorkItem、NoDelay/ReceiveTimeout/SendTimeout、
    ///         服务端连接数统计、优雅停止、客户端并发测试
    /// </summary>
    public class _06_TcpMultiThreadServer : ISample
    {
        public string Id { get { return "2.6"; } }
        public string Title { get { return "TCP ThreadPool 并发服务器 + 压测"; } }
        public string Description { get { return "使用线程池实现的TCP并发服务器：演示NoDelay/超时设置、在线计数，并启动多个客户端线程并发请求统计总处理数。"; } }

        public const int Port = 9206;
        private int clientCount = 0;
        private int requestCount = 0;
        private bool running = true;
        private TcpListener listener;

        public void Run()
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("并发服务器已启动，端口 {0}", Port);

            Thread acceptT = new Thread(new ThreadStart(AcceptLoop));
            acceptT.IsBackground = true;
            acceptT.Start();

            // 并发客户端测试
            int concurrentClients = 20;
            int requestsPerClient = 5;
            Console.WriteLine("启动 {0} 个并发客户端，每个发送 {1} 个请求...", concurrentClients, requestsPerClient);
            Thread[] clients = new Thread[concurrentClients];
            for (int i = 0; i < concurrentClients; i++)
            {
                int idx = i;
                clients[i] = new Thread(new ThreadStart(delegate()
                {
                    RunClient(idx, requestsPerClient);
                }));
                clients[i].IsBackground = true;
                clients[i].Start();
            }
            for (int i = 0; i < concurrentClients; i++) clients[i].Join();

            running = false;
            listener.Stop();
            Thread.Sleep(500);

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("并发测试结束。服务器共处理请求: {0}", requestCount);
            Console.WriteLine("客户端并发数: {0} × {1} 请求", concurrentClients, requestsPerClient);
        }

        private void AcceptLoop()
        {
            while (running)
            {
                IAsyncResult ar = listener.BeginAcceptTcpClient(null, null);
                if (!ar.AsyncWaitHandle.WaitOne(500, false)) continue;
                TcpClient client;
                try { client = listener.EndAcceptTcpClient(ar); }
                catch { break; }
                Interlocked.Increment(ref clientCount);
                ThreadPool.QueueUserWorkItem(new WaitCallback(HandleClient), client);
            }
        }

        private void HandleClient(object state)
        {
            TcpClient client = (TcpClient)state;
            client.NoDelay = true;
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;
            try
            {
                using (client)
                using (NetworkStream ns = client.GetStream())
                {
                    byte[] buf = new byte[1024];
                    StringBuilder sb = new StringBuilder();
                    while (running)
                    {
                        int n;
                        try { n = ns.Read(buf, 0, buf.Length); }
                        catch { break; }
                        if (n <= 0) break;
                        for (int i = 0; i < n; i++)
                        {
                            char c = (char)buf[i];
                            if (c == '\n')
                            {
                                string req = sb.ToString().Trim('\r');
                                sb.Length = 0;
                                string resp;
                                if (req.ToLower() == "time")
                                    resp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                else if (req.ToLower() == "count")
                                    resp = "served=" + Interlocked.Increment(ref requestCount);
                                else
                                    resp = "echo:" + req;
                                byte[] rb = Encoding.UTF8.GetBytes(resp + "\n");
                                ns.Write(rb, 0, rb.Length);
                                if (req.ToLower() == "quit") goto END;
                            }
                            else sb.Append(c);
                        }
                    }
                }
            END:;
            }
            finally
            {
                Interlocked.Decrement(ref clientCount);
            }
        }

        private void RunClient(int id, int count)
        {
            try
            {
                using (TcpClient c = new TcpClient("127.0.0.1", Port))
                {
                    c.NoDelay = true;
                    using (NetworkStream ns = c.GetStream())
                    {
                        byte[] buf = new byte[1024];
                        for (int i = 0; i < count; i++)
                        {
                            string req = (i % 2 == 0) ? "time\n" : "count\n";
                            byte[] wb = Encoding.UTF8.GetBytes(req);
                            ns.Write(wb, 0, wb.Length);
                            StringBuilder sb = new StringBuilder();
                            while (true)
                            {
                                int n = ns.Read(buf, 0, buf.Length);
                                if (n <= 0) break;
                                bool eol = false;
                                for (int j = 0; j < n; j++)
                                {
                                    if (buf[j] == (byte)'\n') { eol = true; break; }
                                    sb.Append((char)buf[j]);
                                }
                                if (eol) break;
                            }
                            // Console.WriteLine("  Client#{0} -> {1}", id, sb.ToString());
                            Thread.Sleep(5);
                        }
                        byte[] qb = Encoding.UTF8.GetBytes("quit\n");
                        ns.Write(qb, 0, qb.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client#{0} 异常: {1}", id, ex.Message);
            }
        }
    }
}
