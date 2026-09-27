using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace CSharpNetworkProgramming.Chapter08_Advanced
{
    /// <summary>
    /// 8.3 Ping (ICMP) 与主机可达性检测
    /// 知识点：System.Net.NetworkInformation.Ping、PingOptions/PingReply、
    ///         Dns.GetHostEntry+Ping组合检测主机、往返时间(RoundtripTime)、TTL、MTU(Don'tFragment)
    /// </summary>
    public class _03_Ping : ISample
    {
        public string Id { get { return "8.3"; } }
        public string Title { get { return "Ping (ICMP) 主机探测"; } }
        public string Description { get { return "演示Ping类发送ICMP Echo请求，分析PingReply(Status/RTT/TTL)，演示DontFragment选项和多次Ping统计。"; } }

        public void Run()
        {
            string[] targets = new string[] { "127.0.0.1", "localhost", "www.baidu.com" };
            foreach (string t in targets)
            {
                PingOnce(t);
                Console.WriteLine();
            }

            // 多次Ping统计
            Console.WriteLine("---- 对 127.0.0.1 连续Ping 4次统计 ----");
            PingStats("127.0.0.1", 4);

            // PingOptions 演示
            Console.WriteLine();
            Console.WriteLine("---- PingOptions 演示 (TTL=64, DontFragment=true) ----");
            try
            {
                using (Ping p = new Ping())
                {
                    PingOptions opt = new PingOptions(64, true);
                    byte[] buf = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"); // 32字节
                    PingReply reply = p.Send("127.0.0.1", 2000, buf, opt);
                    PrintReply(reply);
                }
            }
            catch (Exception ex) { Console.WriteLine("Ping失败: {0}", ex.Message); }
        }

        private void PingOnce(string host)
        {
            Console.WriteLine("---- Ping {0} ----", host);
            IPAddress addr = null;
            try
            {
                try
                {
                    IPHostEntry entry = Dns.GetHostEntry(host);
                    if (entry.AddressList.Length > 0) addr = entry.AddressList[0];
                    Console.WriteLine("    解析地址: {0}", addr);
                }
                catch
                {
                    Console.WriteLine("    DNS解析失败，跳过");
                    return;
                }
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(addr, 2000, new byte[32], new PingOptions(128, true));
                    PrintReply(reply);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("    Ping异常: {0}", ex.Message);
            }
        }

        private void PingStats(string host, int count)
        {
            int sent = 0, recv = 0;
            long minRtt = long.MaxValue, maxRtt = 0, totalRtt = 0;
            using (Ping ping = new Ping())
            {
                for (int i = 0; i < count; i++)
                {
                    sent++;
                    try
                    {
                        PingReply r = ping.Send(host, 2000, new byte[32]);
                        if (r.Status == IPStatus.Success)
                        {
                            recv++;
                            totalRtt += r.RoundtripTime;
                            if (r.RoundtripTime < minRtt) minRtt = r.RoundtripTime;
                            if (r.RoundtripTime > maxRtt) maxRtt = r.RoundtripTime;
                            Console.WriteLine("    来自 {0} 的回复: 字节={1} 时间={2}ms TTL={3}",
                                r.Address, r.Buffer != null ? r.Buffer.Length : 0, r.RoundtripTime, r.Options != null ? (object)r.Options.Ttl : "?");
                        }
                        else
                        {
                            Console.WriteLine("    请求超时 ({0})", r.Status);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("    Ping异常: {0}", ex.Message);
                    }
                    System.Threading.Thread.Sleep(200);
                }
            }
            double loss = sent > 0 ? (sent - recv) * 100.0 / sent : 0;
            Console.WriteLine("    ---- {0} 的Ping统计 ----", host);
            Console.WriteLine("    数据包: 已发送 = {0}, 已接收 = {1}, 丢失 = {2} ({3:F0}% 丢失)",
                sent, recv, sent - recv, loss);
            if (recv > 0)
            {
                Console.WriteLine("    往返行程: 最短 = {0}ms, 最长 = {1}ms, 平均 = {2:F0}ms",
                    minRtt == long.MaxValue ? 0 : minRtt, maxRtt, totalRtt * 1.0 / recv);
            }
        }

        private void PrintReply(PingReply reply)
        {
            Console.WriteLine("    Status       : {0}", reply.Status);
            Console.WriteLine("    Address      : {0}", reply.Address);
            Console.WriteLine("    RoundtripTime: {0} ms", reply.RoundtripTime);
            if (reply.Options != null)
            {
                Console.WriteLine("    TTL          : {0}", reply.Options.Ttl);
                Console.WriteLine("    DontFragment : {0}", reply.Options.DontFragment);
            }
            if (reply.Buffer != null)
                Console.WriteLine("    Buffer.Length: {0} bytes", reply.Buffer.Length);
        }
    }
}
