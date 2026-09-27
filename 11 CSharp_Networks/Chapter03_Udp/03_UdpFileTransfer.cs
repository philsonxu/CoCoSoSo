using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter03_Udp
{
    /// <summary>
    /// 3.3 UDP 简单文件传输（停等协议，每块带序号+确认）
    /// 知识点：UDP可靠性简单实现、数据分包、序号、ACK确认、超时重发、ReceiveTimeout
    ///         本示例仅用于教学演示UDP不可靠下的简单可靠思路，不追求性能
    /// </summary>
    public class _03_UdpFileTransfer : ISample
    {
        public string Id { get { return "3.3"; } }
        public string Title { get { return "UDP 可靠文件传输(停等协议)"; } }
        public string Description { get { return "演示在UDP上实现简单可靠传输：将文件拆分为带序号的小块，每发一块等待ACK，超时重发，最终保证文件完整。"; } }

        public const int Port = 9303;
        private const int BlockSize = 512; // 小包，便于演示
        private const int TimeoutMs = 1000;
        private const int MaxRetry = 5;

        public void Run()
        {
            Thread srv = new Thread(new ThreadStart(RunServer));
            srv.IsBackground = true;
            srv.Start();
            Thread.Sleep(300);

            RunClient();
            Thread.Sleep(300);
        }

        private void RunServer()
        {
            string outPath = Path.Combine(Path.GetTempPath(), "udp_received.bin");
            using (UdpClient udp = new UdpClient(Port))
            {
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                udp.Client.ReceiveTimeout = 5000;
                Console.WriteLine("[UDP-FT-Server] 监听 {0}, 保存到 {1}", Port, outPath);

                using (FileStream fs = new FileStream(outPath, FileMode.Create, FileAccess.Write))
                {
                    int expectedSeq = 0;
                    while (true)
                    {
                        byte[] data;
                        try { data = udp.Receive(ref remote); }
                        catch { break; }
                        if (data.Length < 5) continue;
                        byte flag = data[0]; // 0=数据块, 1=结束, 2=开始
                        int seq = BitConverter.ToInt32(data, 1);
                        if (flag == 2)
                        {
                            Console.WriteLine("[UDP-FT-Server] 收到开始信号，seq={0}", seq);
                            // 发ACK
                            SendAck(udp, remote, seq);
                            expectedSeq = 0;
                            fs.SetLength(0);
                            continue;
                        }
                        if (flag == 1)
                        {
                            Console.WriteLine("[UDP-FT-Server] 收到结束信号，共写入 {0} 字节", fs.Position);
                            SendAck(udp, remote, seq);
                            break;
                        }
                        if (flag == 0)
                        {
                            if (seq == expectedSeq)
                            {
                                fs.Write(data, 5, data.Length - 5);
                                expectedSeq++;
                            }
                            // 无论是不是期望的都回ACK（防止ACK丢失导致对方重发）
                            SendAck(udp, remote, seq);
                        }
                    }
                }
            }
            Console.WriteLine("[UDP-FT-Server] 完成");
        }

        private void SendAck(UdpClient udp, IPEndPoint remote, int seq)
        {
            byte[] ack = new byte[5];
            ack[0] = (byte)'A';
            BitConverter.GetBytes(seq).CopyTo(ack, 1);
            udp.Send(ack, ack.Length, remote);
        }

        private void RunClient()
        {
            // 准备发送数据：使用 test.bin 或生成一段测试数据
            string src = TestData.GetPath("test.bin");
            byte[] fileData;
            if (File.Exists(src))
                fileData = File.ReadAllBytes(src);
            else
            {
                fileData = new byte[BlockSize * 10 + 100];
                Random rnd = new Random(42);
                rnd.NextBytes(fileData);
            }
            Console.WriteLine("[UDP-FT-Client] 文件大小 {0} 字节, 块大小 {1}", fileData.Length, BlockSize);

            using (UdpClient udp = new UdpClient(0))
            {
                IPEndPoint server = new IPEndPoint(IPAddress.Loopback, Port);
                udp.Client.ReceiveTimeout = TimeoutMs;

                // 发送开始信号
                SendWithRetry(udp, server, MakePacket(2, 0, new byte[0]), 0);

                int seq = 0;
                int offset = 0;
                while (offset < fileData.Length)
                {
                    int size = Math.Min(BlockSize, fileData.Length - offset);
                    byte[] payload = new byte[size];
                    Buffer.BlockCopy(fileData, offset, payload, 0, size);
                    byte[] pkt = MakePacket(0, seq, payload);
                    SendWithRetry(udp, server, pkt, seq);
                    offset += size;
                    seq++;
                    if (seq % 5 == 0)
                        Console.WriteLine("[UDP-FT-Client] 已发送块 {0}, 进度 {1}/{2}", seq, offset, fileData.Length);
                }
                // 发送结束信号
                SendWithRetry(udp, server, MakePacket(1, seq, new byte[0]), seq);
                Console.WriteLine("[UDP-FT-Client] 全部块已发送，共 {0} 块", seq);
            }
        }

        private byte[] MakePacket(byte flag, int seq, byte[] payload)
        {
            byte[] pkt = new byte[5 + payload.Length];
            pkt[0] = flag;
            BitConverter.GetBytes(seq).CopyTo(pkt, 1);
            payload.CopyTo(pkt, 5);
            return pkt;
        }

        private void SendWithRetry(UdpClient udp, IPEndPoint server, byte[] pkt, int expectedAckSeq)
        {
            for (int attempt = 0; attempt < MaxRetry; attempt++)
            {
                udp.Send(pkt, pkt.Length, server);
                try
                {
                    IPEndPoint any = new IPEndPoint(IPAddress.Any, 0);
                    byte[] ack = udp.Receive(ref any);
                    if (ack.Length >= 5 && ack[0] == (byte)'A')
                    {
                        int ackSeq = BitConverter.ToInt32(ack, 1);
                        if (ackSeq == expectedAckSeq) return; // 成功
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("[UDP-FT-Client] 块 {0} 超时，第 {1} 次重发", expectedAckSeq, attempt + 1);
                }
            }
            throw new Exception("发送失败，超过最大重试次数");
        }
    }
}
