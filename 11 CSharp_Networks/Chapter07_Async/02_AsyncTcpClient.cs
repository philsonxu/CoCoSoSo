using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter07_Async
{
    /// <summary>
    /// 7.2 基于 APM 的异步 TCP 客户端
    /// 知识点：TcpClient.BeginConnect/EndConnect、NetworkStream.BeginRead/BeginWrite、
    ///         连接-写-读-回写的异步链、ManualResetEvent同步等待异步完成
    /// </summary>
    public class _02_AsyncTcpClient : ISample
    {
        public string Id { get { return "7.2"; } }
        public string Title { get { return "APM 异步 TCP 客户端"; } }
        public string Description { get { return "演示异步TCP客户端：BeginConnect→BeginWrite→BeginRead，使用ManualResetEvent在回调中信号主线程，完全无阻塞调用。"; } }

        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent recvDone = new ManualResetEvent(false);
        private StringBuilder received = new StringBuilder();
        private Exception lastError;

        public void Run()
        {
            // 先尝试连接 7.1 服务器；如果未运行，则在本地启动一个简单echo服务器供演示
            Thread srv = new Thread(new ThreadStart(delegate()
            {
                TcpListener l = new TcpListener(IPAddress.Any, _01_AsyncTcpServer.Port);
                try
                {
                    l.Start();
                    IAsyncResult ar = l.BeginAcceptTcpClient(null, null);
                    if (!ar.AsyncWaitHandle.WaitOne(3000, false)) { l.Stop(); return; }
                    using (TcpClient c = l.EndAcceptTcpClient(ar))
                    using (NetworkStream ns = c.GetStream())
                    {
                        byte[] buf = new byte[1024];
                        int n = ns.Read(buf, 0, buf.Length);
                        string msg = Encoding.UTF8.GetString(buf, 0, n).Trim('\r', '\n');
                        string resp = "echo:" + msg + "\n";
                        byte[] rb = Encoding.UTF8.GetBytes(resp);
                        ns.Write(rb, 0, rb.Length);
                    }
                    l.Stop();
                }
                catch { }
            }));
            srv.IsBackground = true;
            srv.Start();
            Thread.Sleep(200);

            // 异步连接
            Console.WriteLine("[Async-Client] 开始异步连接 127.0.0.1:{0}", _01_AsyncTcpServer.Port);
            TcpClient client = new TcpClient();
            client.BeginConnect(IPAddress.Loopback, _01_AsyncTcpServer.Port,
                new AsyncCallback(ConnectCallback), client);
            if (!connectDone.WaitOne(3000, false))
            {
                Console.WriteLine("连接超时");
                return;
            }
            if (lastError != null)
            {
                Console.WriteLine("连接异常: {0}", lastError.Message);
                return;
            }
            Console.WriteLine("[Async-Client] 连接成功");

            // 异步发送
            string msg = "Async Hello APM\n";
            byte[] sendBytes = Encoding.UTF8.GetBytes(msg);
            NetworkStream ns = client.GetStream();
            ns.BeginWrite(sendBytes, 0, sendBytes.Length,
                new AsyncCallback(SendCallback), ns);
            sendDone.WaitOne(3000, false);
            Console.WriteLine("[Async-Client] 已发送: {0}", msg.TrimEnd('\n'));

            // 异步接收
            byte[] buf = new byte[1024];
            AsyncState state = new AsyncState();
            state.Stream = ns;
            state.Buffer = buf;
            ns.BeginRead(buf, 0, buf.Length, new AsyncCallback(ReceiveCallback), state);
            if (!recvDone.WaitOne(3000, false))
                Console.WriteLine("接收超时");
            else
                Console.WriteLine("[Async-Client] 收到: {0}", received.ToString().Trim('\r', '\n'));

            client.Close();
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                TcpClient c = (TcpClient)ar.AsyncState;
                c.EndConnect(ar);
            }
            catch (Exception ex) { lastError = ex; }
            connectDone.Set();
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                NetworkStream ns = (NetworkStream)ar.AsyncState;
                ns.EndWrite(ar);
            }
            catch (Exception ex) { lastError = ex; }
            sendDone.Set();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                AsyncState st = (AsyncState)ar.AsyncState;
                int n = st.Stream.EndRead(ar);
                received.Append(Encoding.UTF8.GetString(st.Buffer, 0, n));
            }
            catch (Exception ex) { lastError = ex; }
            recvDone.Set();
        }

        private class AsyncState
        {
            public NetworkStream Stream;
            public byte[] Buffer;
        }
    }
}
