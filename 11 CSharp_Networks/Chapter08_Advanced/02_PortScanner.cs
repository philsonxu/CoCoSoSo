using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter08_Advanced
{
    /// <summary>
    /// 8.2 端口扫描器
    /// 知识点：Socket.BeginConnect/Connect、Port扫描、多线程并发扫描、
    ///         常见端口服务对照、扫描超时控制、端口开放/关闭/过滤判断
    /// 警告：仅用于测试本机端口，未经授权扫描他人主机属于违法行为。
    /// </summary>
    public class _02_PortScanner : ISample
    {
        public string Id { get { return "8.2"; } }
        public string Title { get { return "端口扫描器 (本机端口探查)"; } }
        public string Description { get { return "通过TcpClient.Connect(BeginConnect+WaitOne超时)快速扫描本机常见端口，演示并发扫描、超时控制与结果汇总。仅扫描本机127.0.0.1。"; } }

        private int timeoutMs = 200;
        private ArrayList openPorts = ArrayList.Synchronized(new ArrayList());

        public void Run()
        {
            // 扫描本机常见端口（精选前1024中的一些+常用高端口）
            int[] ports = new int[] {
                21, 22, 23, 25, 53, 80, 110, 135, 139, 143, 443, 445,
                993, 995, 1433, 1521, 3306, 3389, 5432, 6379, 8080, 8888,
                9090, 9201, 9203, 9205, 9206, 9301, 9302, 9303, 9304, 9404, 9701, 9703
            };

            Console.WriteLine("开始扫描 127.0.0.1 的 {0} 个端口 (超时 {1}ms)...", ports.Length, timeoutMs);
            Console.WriteLine("(仅扫描本机，请勿扫描他人主机)");
            Console.WriteLine();

            ArrayList threads = new ArrayList();
            DateTime start = DateTime.Now;
            foreach (int port in ports)
            {
                WaitCallback wc = new WaitCallback(delegate(object p)
                {
                    int portN = (int)p;
                    ScanPort(portN);
                });
                ThreadPool.QueueUserWorkItem(wc, port);
            }

            // 等待扫描完成（简单估算：最多2秒+余量）
            Thread.Sleep(timeoutMs * 2 + 500);
            double sec = (DateTime.Now - start).TotalSeconds;

            // 排序并输出结果
            openPorts.Sort();
            Console.WriteLine("---- 扫描结果 ----");
            if (openPorts.Count == 0)
            {
                Console.WriteLine("未发现开放端口。");
            }
            else
            {
                foreach (int p in openPorts)
                {
                    string svc = GetServiceName(p);
                    Console.WriteLine("  [OPEN]  {0,-6}{1}", p, svc);
                }
            }
            Console.WriteLine();
            Console.WriteLine("扫描完成，耗时 {0:F2}s，共发现 {1} 个开放端口", sec, openPorts.Count);
            Console.WriteLine("提示：本示例中9xxx端口(如9201/9203等)是本项目其他示例监听的端口，运行过对应示例后它们会显示为开放。");
        }

        private void ScanPort(int port)
        {
            using (TcpClient c = new TcpClient())
            {
                try
                {
                    IAsyncResult ar = c.BeginConnect("127.0.0.1", port, null, null);
                    bool ok = ar.AsyncWaitHandle.WaitOne(timeoutMs, false);
                    if (ok)
                    {
                        c.EndConnect(ar);
                        openPorts.Add(port);
                    }
                }
                catch { }
            }
        }

        private string GetServiceName(int p)
        {
            switch (p)
            {
                case 21: return "FTP";
                case 22: return "SSH";
                case 23: return "Telnet";
                case 25: return "SMTP";
                case 53: return "DNS";
                case 80: return "HTTP";
                case 110: return "POP3";
                case 135: return "RPC";
                case 139: return "NetBIOS";
                case 143: return "IMAP";
                case 443: return "HTTPS";
                case 445: return "SMB";
                case 993: return "IMAPS";
                case 995: return "POP3S";
                case 1433: return "MSSQL";
                case 1521: return "Oracle";
                case 3306: return "MySQL";
                case 3389: return "RDP";
                case 5432: return "PostgreSQL";
                case 6379: return "Redis";
                case 8080: return "HTTP-Alt";
                case 8888: return "HTTP-Alt/Proxy";
                case 9090: return "HTTP-Alt";
                default:
                    if (p >= 9201 && p <= 9703) return "(本项目示例端口)";
                    return "";
            }
        }
    }
}
