using System;
using System.Net;

namespace CSharpNetworkProgramming.Chapter01_Basics
{
    /// <summary>
    /// 1.1 DNS 域名解析示例
    /// 知识点：Dns 类、IPHostEntry、IPAddress、GetHostEntry/GetHostName/DnsPermission
    /// </summary>
    public class _01_DnsResolve : ISample
    {
        public string Id { get { return "1.1"; } }
        public string Title { get { return "DNS 域名解析 (Dns/IPHostEntry)"; } }
        public string Description { get { return "演示本地主机名查询、域名到IP地址的正向解析、IP到主机名的反向解析，以及IPAddress的常用方法。"; } }

        public void Run()
        {
            // 1) 获取本地主机名
            string hostName = Dns.GetHostName();
            Console.WriteLine("[1] 本地主机名: {0}", hostName);

            // 2) 解析本机IP
            IPHostEntry localEntry = Dns.GetHostEntry(hostName);
            Console.WriteLine("[2] 本机绑定的IP地址列表：");
            foreach (IPAddress ip in localEntry.AddressList)
            {
                Console.WriteLine("    - {0}  (AddressFamily={1}, IsIPv6LinkLocal={2})",
                    ip, ip.AddressFamily, ip.IsIPv6LinkLocal);
            }

            // 3) 解析常见域名
            string[] domains = new string[] { "www.baidu.com", "www.microsoft.com", "localhost" };
            foreach (string domain in domains)
            {
                Console.WriteLine("[3] 解析域名 {0} ...", domain);
                try
                {
                    IPHostEntry entry = Dns.GetHostEntry(domain);
                    Console.WriteLine("    HostName = {0}", entry.HostName);
                    foreach (string alias in entry.Aliases)
                    {
                        Console.WriteLine("    Alias   = {0}", alias);
                    }
                    foreach (IPAddress ip in entry.AddressList)
                    {
                        Console.WriteLine("    IP      = {0}", ip);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("    解析失败: {0}", ex.Message);
                }
            }

            // 4) IPAddress 常用方法
            Console.WriteLine("[4] IPAddress 静态方法演示：");
            IPAddress loopback = IPAddress.Loopback;
            Console.WriteLine("    Loopback        = {0}", loopback);
            Console.WriteLine("    Broadcast       = {0}", IPAddress.Broadcast);
            Console.WriteLine("    Any             = {0}", IPAddress.Any);
            Console.WriteLine("    IPv6Any         = {0}", IPAddress.IPv6Any);

            IPAddress parsed;
            if (IPAddress.TryParse("192.168.1.1", out parsed))
            {
                Console.WriteLine("    TryParse 成功   = {0}", parsed);
            }

            byte[] bytes = new byte[] { 192, 168, 0, 1 };
            IPAddress custom = new IPAddress(bytes);
            Console.WriteLine("    字节数组构造    = {0}", custom);
            byte[] back = custom.GetAddressBytes();
            Console.Write("    GetAddressBytes = ");
            for (int i = 0; i < back.Length; i++)
            {
                Console.Write(back[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
