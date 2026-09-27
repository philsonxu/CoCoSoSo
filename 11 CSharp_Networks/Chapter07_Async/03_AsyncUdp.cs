using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter07_Async
{
    /// <summary>
    /// 7.3 异步 UDP 收发
    /// 知识点：UdpClient.BeginReceive/EndReceive、BeginSend/EndSend、异步回调、
    ///         AsyncState传递双向消息
    /// </summary>
    public class _03_AsyncUdp : ISample
    {
        public string Id { get { return "7.3"; } }
        public string Title { get { return "APM 异步 UDP 收发"; } }
        public string Description { get { return "演示UdpClient的异步Begin/End收发：服务器在回调中循环Receive并回显；客户端异步Send后BeginReceive等待响应。"; } }

        public const int Port = 9703;
        private ManualResetEvent recvEvt = new ManualResetEvent(false);
        private string recvMsg;

        public void Run()
        {
            // 启动异步服务器
            bool serverStarted = false;
            using (UdpClient serverUdp = new UdpClient(Port))
            {
                serverUdp.BeginReceive(new AsyncCallback(ServerRecvCallback), serverUdp);
                serverStarted = true;
                Console.WriteLine("[Async-UDP-Server] 开始异步监听 {0}", Port);
                Thread.Sleep(200);

                // 异步客户端发送3条消息
                using (UdpClient clientUdp = new UdpClient(0))
                {
                    IPEndPoint server = new IPEndPoint(IPAddress.Loopback, Port);
                    string[] msgs = new string[] { "async-1", "async-2", "async-3" };
                    foreach (string m in msgs)
                    {
                        byte[] data = Encoding.UTF8.GetBytes(m);
                        recvEvt.Reset();
                        clientUdp.BeginSend(data, data.Length, server,
                            new AsyncCallback(SendCallback), clientUdp);
                        // 异步接收
                        ClientState st = new ClientState();
                        st.Client = clientUdp;
                        st.WaitEvent = recvEvt;
                        IPEndPoint any = new IPEndPoint(IPAddress.Any, 0);
                        clientUdp.BeginReceive(new AsyncCallback(ClientRecvCallback), st);
                        if (recvEvt.WaitOne(2000, false))
                        {
                            Console.WriteLine("[Async-UDP-Client] 发送 {0}，收到 {1}", m, recvMsg);
                        }
                        else
                        {
                            Console.WriteLine("[Async-UDP-Client] 发送 {0}，接收超时", m);
                        }
                        Thread.Sleep(100);
                    }
                }
                Thread.Sleep(300);
                serverUdp.Close();
            }
        }

        private void ServerRecvCallback(IAsyncResult ar)
        {
            UdpClient udp = (UdpClient)ar.AsyncState;
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            byte[] data;
            try { data = udp.EndReceive(ar, ref remote); }
            catch { return; }
            string msg = Encoding.UTF8.GetString(data);
            string echo = "echo:" + msg;
            byte[] eb = Encoding.UTF8.GetBytes(echo);
            udp.BeginSend(eb, eb.Length, remote, new AsyncCallback(ServerSendCallback), udp);
        }

        private void ServerSendCallback(IAsyncResult ar)
        {
            UdpClient udp = (UdpClient)ar.AsyncState;
            try
            {
                udp.EndSend(ar);
                // 继续监听
                udp.BeginReceive(new AsyncCallback(ServerRecvCallback), udp);
            }
            catch { }
        }

        private void SendCallback(IAsyncResult ar)
        {
            UdpClient udp = (UdpClient)ar.AsyncState;
            try { udp.EndSend(ar); }
            catch { }
        }

        private void ClientRecvCallback(IAsyncResult ar)
        {
            ClientState st = (ClientState)ar.AsyncState;
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                byte[] data = st.Client.EndReceive(ar, ref remote);
                recvMsg = Encoding.UTF8.GetString(data);
            }
            catch { recvMsg = "<error>"; }
            st.WaitEvent.Set();
        }

        private class ClientState
        {
            public UdpClient Client;
            public ManualResetEvent WaitEvent;
        }
    }
}
