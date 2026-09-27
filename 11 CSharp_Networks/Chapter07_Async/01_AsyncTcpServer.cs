using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter07_Async
{
    /// <summary>
    /// 7.1 基于 APM (Begin/End) 的异步 TCP 服务器
    /// 知识点：APM异步编程模型(BeginAcceptTcpClient/EndAcceptTcpClient、
    ///         BeginRead/EndRead、BeginWrite/EndWrite)、回调链(Accept->Read->Write->Read)、
    ///         AsyncState传递、手动实现echo协议的完全异步服务端、ManualResetEvent信号
    /// </summary>
    public class _01_AsyncTcpServer : ISample
    {
        public string Id { get { return "7.1"; } }
        public string Title { get { return "APM 异步 TCP 服务器"; } }
        public string Description { get { return "使用Begin/End异步模型实现TCP Echo服务器：Accept回调→Read回调→Write回调→再次Read，形成全异步回调链，无阻塞线程。"; } }

        public const int Port = 9701;
        private TcpListener listener;
        private int servedCount = 0;
        private ManualResetEvent stopEvent = new ManualResetEvent(false);

        public void Run()
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("[Async-Server] 启动，监听 {0}", Port);
            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), null);

            Console.WriteLine("异步服务器已Accept进入回调链，下面启动客户端发送5条消息...");
            Thread.Sleep(200);

            // 启动一个测试客户端
            ParameterizedThreadStart pts = new ParameterizedThreadStart(RunTestClient);
            Thread t = new Thread(pts);
            t.IsBackground = true;
            t.Start(5);
            t.Join();

            Thread.Sleep(500);
            listener.Stop();
            Console.WriteLine("[Async-Server] 已停止，共处理消息 {0} 条", servedCount);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            TcpClient client;
            try { client = listener.EndAcceptTcpClient(ar); }
            catch { return; }
            Console.WriteLine("[Async-Server] 客户端连接: {0}", client.Client.RemoteEndPoint);
            // 继续Accept下一连接
            try { listener.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), null); }
            catch { }

            // 开始读
            AsyncState state = new AsyncState();
            state.Client = client;
            state.Stream = client.GetStream();
            state.Buffer = new byte[1024];
            state.Builder = new StringBuilder();
            state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length,
                new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            AsyncState state = (AsyncState)ar.AsyncState;
            int n;
            try { n = state.Stream.EndRead(ar); }
            catch { Cleanup(state); return; }
            if (n <= 0) { Cleanup(state); return; }
            for (int i = 0; i < n; i++)
            {
                char c = (char)state.Buffer[i];
                if (c == '\n')
                {
                    string msg = state.Builder.ToString().Trim('\r');
                    state.Builder.Length = 0;
                    string resp = "echo:" + msg + "\n";
                    byte[] data = Encoding.UTF8.GetBytes(resp);
                    state.PendingSend = data;
                    Interlocked.Increment(ref servedCount);
                    state.Stream.BeginWrite(data, 0, data.Length,
                        new AsyncCallback(WriteCallback), state);
                    if (msg.ToLower() == "quit") return;
                }
                else state.Builder.Append(c);
            }
            // 继续读
            try
            {
                state.Stream.BeginRead(state.Buffer, 0, state.Buffer.Length,
                    new AsyncCallback(ReadCallback), state);
            }
            catch { Cleanup(state); }
        }

        private void WriteCallback(IAsyncResult ar)
        {
            AsyncState state = (AsyncState)ar.AsyncState;
            try { state.Stream.EndWrite(ar); }
            catch { Cleanup(state); return; }
        }

        private void Cleanup(AsyncState state)
        {
            try { state.Stream.Close(); } catch { }
            try { state.Client.Close(); } catch { }
        }

        private void RunTestClient(object cnt)
        {
            int count = (int)cnt;
            using (TcpClient c = new TcpClient("127.0.0.1", Port))
            using (NetworkStream ns = c.GetStream())
            {
                byte[] buf = new byte[1024];
                for (int i = 1; i <= count; i++)
                {
                    string msg = "hello-" + i + "\n";
                    byte[] wb = Encoding.UTF8.GetBytes(msg);
                    ns.Write(wb, 0, wb.Length);
                    StringBuilder sb = new StringBuilder();
                    while (true)
                    {
                        int n = ns.Read(buf, 0, buf.Length);
                        bool eol = false;
                        for (int j = 0; j < n; j++)
                        {
                            if (buf[j] == (byte)'\n') { eol = true; break; }
                            sb.Append((char)buf[j]);
                        }
                        if (eol) break;
                    }
                    Console.WriteLine("[Test-Client] 收到: {0}", sb);
                    Thread.Sleep(50);
                }
                byte[] q = Encoding.UTF8.GetBytes("quit\n");
                ns.Write(q, 0, q.Length);
            }
        }

        private class AsyncState
        {
            public TcpClient Client;
            public NetworkStream Stream;
            public byte[] Buffer;
            public StringBuilder Builder;
            public byte[] PendingSend;
        }
    }
}
