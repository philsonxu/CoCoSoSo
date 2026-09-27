using System;
using System.IO;
using System.Net;
using System.Text;

namespace CSharpNetworkProgramming.Chapter04_Http
{
    /// <summary>
    /// 4.1 HTTP GET 请求
    /// 知识点：HttpWebRequest/HttpWebResponse、Method=GET、User-Agent/Accept头、
    ///         GetResponseStream、StreamReader、状态码/响应头读取
    /// </summary>
    public class _01_HttpGet : ISample
    {
        public string Id { get { return "4.1"; } }
        public string Title { get { return "HTTP GET 请求 (HttpWebRequest)"; } }
        public string Description { get { return "演示使用HttpWebRequest发送HTTP GET请求，读取响应状态码、响应头、响应体，支持自定义请求头。"; } }

        public void Run()
        {
            // 1) 请求本地内置的简易HTTP服务器(4.4端口)作为默认测试目标，保证离线可用
            // 先尝试访问4.4服务器，如果没启动，则访问测试index.html文件直接打印
            string url = "http://127.0.0.1:" + _04_SimpleHttpServer.Port + "/";
            Console.WriteLine("目标URL: {0}", url);

            HttpWebRequest req = null;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建请求失败: {0}", ex.Message);
                return;
            }
            req.Method = "GET";
            req.UserAgent = "CSharpNetworkDemo/1.0";
            req.Accept = "text/html,text/plain,*/*";
            req.Timeout = 3000;
            req.ReadWriteTimeout = 3000;

            HttpWebResponse resp = null;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                    resp = (HttpWebResponse)wex.Response;
                else
                {
                    Console.WriteLine("请求失败(请先运行4.4启动本地HTTP服务器): {0}", wex.Message);
                    PrintLocalFallback();
                    return;
                }
            }

            using (resp)
            {
                Console.WriteLine("状态码: {0} ({1})", (int)resp.StatusCode, resp.StatusCode);
                Console.WriteLine("Server: {0}", resp.Server);
                Console.WriteLine("ContentType: {0}", resp.ContentType);
                Console.WriteLine("ContentLength: {0}", resp.ContentLength);
                Console.WriteLine("---- 响应头 ----");
                for (int i = 0; i < resp.Headers.Count; i++)
                {
                    Console.WriteLine("  {0}: {1}", resp.Headers.Keys[i], resp.Headers[i]);
                }
                Console.WriteLine("---- 响应体(前500字符) ----");
                using (Stream s = resp.GetResponseStream())
                using (StreamReader sr = new StreamReader(s, Encoding.UTF8))
                {
                    char[] buf = new char[512];
                    int n = sr.Read(buf, 0, buf.Length);
                    Console.WriteLine(new string(buf, 0, n));
                }
            }
        }

        private void PrintLocalFallback()
        {
            string p = TestData.GetPath("index.html");
            if (File.Exists(p))
            {
                Console.WriteLine("（本地 index.html 内容预览）");
                using (StreamReader sr = new StreamReader(p, Encoding.UTF8))
                {
                    char[] buf = new char[512];
                    int n = sr.Read(buf, 0, buf.Length);
                    Console.WriteLine(new string(buf, 0, n));
                }
            }
        }
    }
}
