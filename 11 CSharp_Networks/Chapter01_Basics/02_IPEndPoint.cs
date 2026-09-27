using System;
using System.Net;

namespace CSharpNetworkProgramming.Chapter01_Basics
{
    /// <summary>
    /// 1.2 IPEndPoint 网络端点示例
    /// 知识点：EndPoint、IPEndPoint、端口号、AddressFamily、序列化端口端点、端口号范围约定
    /// </summary>
    public class _02_IPEndPoint : ISample
    {
        public string Id { get { return "1.2"; } }
        public string Title { get { return "IPEndPoint 网络端点"; } }
        public string Description { get { return "演示IPEndPoint的构造、属性、端口范围判断、常用端口列表打印以及端点序列化(Serialize/Create)。"; } }

        public void Run()
        {
            // 1) 基本构造
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep1 = new IPEndPoint(ip, 8080);
            Console.WriteLine("[1] 基本端点: {0}", ep1);
            Console.WriteLine("    Address       = {0}", ep1.Address);
            Console.WriteLine("    Port          = {0}", ep1.Port);
            Console.WriteLine("    AddressFamily = {0}", ep1.AddressFamily);

            // 2) 通配端点
            IPEndPoint epAny = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("[2] 通配端点: {0}", epAny);

            // 3) 静态字段：最大/最小端口
            Console.WriteLine("[3] 端口范围：MinPort={0}, MaxPort={1}",
                IPEndPoint.MinPort, IPEndPoint.MaxPort);

            // 4) 修改端口
            ep1.Port = 9090;
            Console.WriteLine("[4] 修改端口后: {0}", ep1);

            // 5) Create 方法：从SocketAddress重建端点
            SocketAddress sa = ep1.Serialize();
            Console.WriteLine("[5] Serialize 得到 SocketAddress: {0} (Size={1})", sa, sa.Size);
            EndPoint ep2 = ep1.Create(sa);
            Console.WriteLine("    Create 还原: {0}", ep2);

            // 6) 常用端口对照表（部分）
            Console.WriteLine("[6] 常用端口参考：");
            int[] wellKnownPorts = new int[] { 21, 22, 23, 25, 53, 69, 80, 110, 135,
                139, 143, 443, 445, 993, 995, 1433, 1521, 3306, 3389, 5432, 6379, 8080, 27017 };
            string[] serviceNames = new string[] { "FTP", "SSH", "Telnet", "SMTP", "DNS",
                "TFTP", "HTTP", "POP3", "RPC", "NetBIOS", "IMAP", "HTTPS", "SMB", "IMAPS",
                "POP3S", "MSSQL", "Oracle", "MySQL", "RDP", "PostgreSQL", "Redis", "HTTP-Alt", "MongoDB" };
            for (int i = 0; i < wellKnownPorts.Length; i++)
            {
                Console.WriteLine("    {0,-6}{1}", wellKnownPorts[i], serviceNames[i]);
            }

            // 7) TryParse (.NET 2.0 下手动实现 IP:Port 解析示例)
            Console.WriteLine("[7] 手动解析 192.168.1.100:7788：");
            string text = "192.168.1.100:7788";
            int idx = text.LastIndexOf(':');
            if (idx > 0)
            {
                string ipStr = text.Substring(0, idx);
                string portStr = text.Substring(idx + 1);
                IPAddress addr;
                int port;
                if (IPAddress.TryParse(ipStr, out addr) && int.TryParse(portStr, out port))
                {
                    IPEndPoint ep3 = new IPEndPoint(addr, port);
                    Console.WriteLine("    解析成功: {0}", ep3);
                }
            }
        }
    }
}
