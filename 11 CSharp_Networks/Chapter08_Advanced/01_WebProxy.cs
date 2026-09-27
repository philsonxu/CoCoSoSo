using System;
using System.Net;

namespace CSharpNetworkProgramming.Chapter08_Advanced
{
    /// <summary>
    /// 8.1 Web代理设置 (WebProxy)
    /// 知识点：WebProxy、WebRequest.DefaultWebProxy、Proxy/BypassProxyOnLocal、
    ///         Credentials、IWebProxy、HttpClient(2.0无)概念替代方案、代理自动检测
    /// </summary>
    public class _01_WebProxy : ISample
    {
        public string Id { get { return "8.1"; } }
        public string Title { get { return "Web 代理 (WebProxy) 设置"; } }
        public string Description { get { return "演示为HttpWebRequest设置HTTP代理、BypassProxyOnLocal本地绕过、带身份认证代理、读取系统默认代理。"; } }

        public void Run()
        {
            // 1) 读取系统默认代理
            Console.WriteLine("[1] 系统默认代理:");
            IWebProxy sysProxy = WebRequest.DefaultWebProxy;
            if (sysProxy != null)
            {
                Uri test = new Uri("http://www.example.com/");
                Uri proxyUri = sysProxy.GetProxy(test);
                Console.WriteLine("    目标 {0} 经由代理 {1}", test, proxyUri);
                Console.WriteLine("    IsBypassed: {0}", sysProxy.IsBypassed(new Uri("http://localhost/")));
                Console.WriteLine("    Credentials: {0}", sysProxy.Credentials != null ? "已设置" : "未设置");
            }
            else
            {
                Console.WriteLine("    未配置系统代理");
            }

            // 2) 不使用代理
            Console.WriteLine("[2] 关闭代理（直连）:");
            Console.WriteLine("    req.Proxy = null;");
            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create("http://127.0.0.1/");
            req1.Proxy = null;
            req1.Timeout = 500;
            try
            {
                using (HttpWebResponse r = (HttpWebResponse)req1.GetResponse())
                {
                    Console.WriteLine("    直连结果: {0}", r.StatusCode);
                }
            }
            catch (Exception ex) { Console.WriteLine("    直连失败（预期：本机80无服务）: {0}", ex.Message); }

            // 3) 设置 HTTP 代理（示例代码，不真的使用）
            Console.WriteLine("[3] 设置 HTTP 代理代码示例:");
            Console.WriteLine("    WebProxy proxy = new WebProxy(\"http://127.0.0.1:8888\");");
            Console.WriteLine("    proxy.BypassProxyOnLocal = true;");
            Console.WriteLine("    // 代理认证:");
            Console.WriteLine("    proxy.Credentials = new NetworkCredential(\"proxyUser\", \"proxyPwd\");");
            Console.WriteLine("    req.Proxy = proxy;");

            // 演示WebProxy构造
            WebProxy demoProxy = new WebProxy("http://127.0.0.1:8888");
            demoProxy.BypassProxyOnLocal = true;
            Console.WriteLine("    演示创建的Proxy.Address = {0}", demoProxy.Address);
            Console.WriteLine("    BypassProxyOnLocal = {0}", demoProxy.BypassProxyOnLocal);

            // 4) 绕过列表（特定域名不走代理）
            Console.WriteLine("[4] BypassList 绕过列表:");
            string[] bypass = new string[] { "localhost", "127.0.0.1", "*.intranet.com", "192.168.*" };
            demoProxy.BypassList = bypass;
            foreach (string b in bypass) Console.WriteLine("    绕过: {0}", b);
            Console.WriteLine("    对 http://127.0.0.1/ 是否绕过? {0}",
                demoProxy.IsBypassed(new Uri("http://127.0.0.1/")));
            Console.WriteLine("    对 http://www.example.com/ 是否绕过? {0}",
                demoProxy.IsBypassed(new Uri("http://www.example.com/")));

            // 5) 自动代理检测
            Console.WriteLine("[5] 自动检测代理:");
            Console.WriteLine("    // WebProxy.GetDefaultProxy() 在 2.0 已过时，建议使用:");
            Console.WriteLine("    WebRequest.GetSystemWebProxy()  / WebRequest.DefaultWebProxy");
        }
    }
}
