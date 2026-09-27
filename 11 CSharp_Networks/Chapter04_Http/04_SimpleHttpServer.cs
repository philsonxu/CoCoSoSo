using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpNetworkProgramming.Chapter04_Http
{
    /// <summary>
    /// 4.4 简易 HTTP 服务器 (基于TcpListener自实现)
    /// 知识点：HTTP协议解析(请求行/请求头/空行)、响应状态行、响应头、
    ///         Content-Length、GET/POST区分、静态文件服务、提供/file下载接口
    /// 本服务器为单线程顺序处理，用于配合前面4.1/4.2/4.3示例测试。
    /// </summary>
    public class _04_SimpleHttpServer : ISample
    {
        public string Id { get { return "4.4"; } }
        public string Title { get { return "简易 HTTP 服务器(TcpListener实现)"; } }
        public string Description { get { return "用TcpListener从零实现一个简单的HTTP/1.0服务器：解析请求行与头、返回200/404/206状态码、支持GET/POST、根目录HTML、/post回显、/file二进制下载。配合4.1-4.3示例使用。"; } }

        public const int Port = 9404;
        private bool running = true;
        private TcpListener listener;

        public void Run()
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("HTTP服务器已启动 http://127.0.0.1:{0}/", Port);
            Console.WriteLine("路由:");
            Console.WriteLine("  GET  /         -> 返回欢迎HTML");
            Console.WriteLine("  GET  /index    -> 返回TestData/index.html");
            Console.WriteLine("  GET  /file     -> 返回50KB随机二进制数据(用于下载测试)");
            Console.WriteLine("  POST /post     -> 回显表单参数");
            Console.WriteLine("服务器运行15秒后自动退出...");

            Thread acceptT = new Thread(new ThreadStart(AcceptLoop));
            acceptT.IsBackground = true;
            acceptT.Start();

            // 同时作为客户端自测一下
            Thread.Sleep(500);
            SelfTest();

            Thread.Sleep(15000 - 500);
            running = false;
            try { listener.Stop(); } catch { }
            Console.WriteLine("HTTP服务器已停止");
        }

        private void AcceptLoop()
        {
            while (running)
            {
                IAsyncResult ar = listener.BeginAcceptTcpClient(null, null);
                if (!ar.AsyncWaitHandle.WaitOne(500, false)) continue;
                TcpClient client;
                try { client = listener.EndAcceptTcpClient(ar); }
                catch { break; }
                try { HandleRequest(client); }
                catch (Exception ex) { Console.WriteLine("处理请求异常: {0}", ex.Message); }
            }
        }

        private void HandleRequest(TcpClient client)
        {
            using (client)
            using (NetworkStream ns = client.GetStream())
            {
                client.ReceiveTimeout = 3000;
                client.SendTimeout = 3000;
                StreamReader reader = new StreamReader(ns, Encoding.ASCII);
                string requestLine = reader.ReadLine();
                if (requestLine == null) return;
                Console.WriteLine("[HTTP] {0}", requestLine);
                string[] parts = requestLine.Split(new char[] { ' ' });
                string method = parts[0];
                string path = parts.Length > 1 ? parts[1] : "/";

                // 读取所有请求头
                int contentLength = 0;
                string line;
                while ((line = reader.ReadLine()) != null && line.Length > 0)
                {
                    if (line.ToLower().StartsWith("content-length:"))
                    {
                        int.TryParse(line.Substring(15).Trim(), out contentLength);
                    }
                }

                // 读POST body
                string body = "";
                if (method.ToUpper() == "POST" && contentLength > 0)
                {
                    char[] bbuf = new char[contentLength];
                    int read = 0;
                    while (read < contentLength)
                    {
                        int n = reader.Read(bbuf, read, contentLength - read);
                        if (n <= 0) break;
                        read += n;
                    }
                    body = new string(bbuf);
                }

                // 路由
                if (path == "/") SendHtml(ns, BuildWelcomePage());
                else if (path == "/index") SendFile(ns, TestData.GetPath("index.html"), "text/html; charset=utf-8");
                else if (path == "/file") SendBinaryFile(ns);
                else if (path.ToLower() == "/post" && method.ToUpper() == "POST") SendHtml(ns, BuildPostEcho(body));
                else Send404(ns, path);
            }
        }

        private void SendResponse(NetworkStream ns, int status, string statusText, string contentType, byte[] content, long rangeStart)
        {
            StringBuilder sb = new StringBuilder();
            if (rangeStart >= 0 && status == 200)
            {
                sb.Append("HTTP/1.0 206 Partial Content\r\n");
            }
            else
            {
                sb.AppendFormat("HTTP/1.0 {0} {1}\r\n", status, statusText);
            }
            sb.AppendFormat("Content-Type: {0}\r\n", contentType);
            sb.AppendFormat("Content-Length: {0}\r\n", content.Length);
            sb.Append("Connection: close\r\n");
            sb.Append("Server: CSharpSimpleHttp/1.0\r\n");
            sb.Append("\r\n");
            byte[] head = Encoding.ASCII.GetBytes(sb.ToString());
            ns.Write(head, 0, head.Length);
            ns.Write(content, 0, content.Length);
        }

        private void SendHtml(NetworkStream ns, string html)
        {
            byte[] data = Encoding.UTF8.GetBytes(html);
            SendResponse(ns, 200, "OK", "text/html; charset=utf-8", data, -1);
        }

        private void SendFile(NetworkStream ns, string path, string contentType)
        {
            if (!File.Exists(path)) { Send404(ns, path); return; }
            byte[] data = File.ReadAllBytes(path);
            SendResponse(ns, 200, "OK", contentType, data, -1);
        }

        private void SendBinaryFile(NetworkStream ns)
        {
            // 生成50KB随机数据（固定种子保证可复现）
            byte[] data = new byte[50 * 1024];
            new Random(12345).NextBytes(data);
            SendResponse(ns, 200, "OK", "application/octet-stream", data, -1);
        }

        private void Send404(NetworkStream ns, string path)
        {
            string html = "<html><body><h1>404 Not Found</h1><p>" + System.Web.HttpUtility.HtmlEncode(path) + "</p></body></html>";
            byte[] data = Encoding.UTF8.GetBytes(html);
            SendResponse(ns, 404, "Not Found", "text/html; charset=utf-8", data, -1);
        }

        private string BuildWelcomePage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta charset='utf-8'><title>C# Simple HTTP</title></head><body>");
            sb.Append("<h1>欢迎使用 C# 简易HTTP服务器</h1>");
            sb.Append("<p>这是示例 4.4 提供的测试服务器，用于配合 4.1/4.2/4.3 演示HTTP客户端编程。</p>");
            sb.Append("<ul>");
            sb.Append("<li><a href='/index'>/index</a> - TestData/index.html 静态页</li>");
            sb.Append("<li><a href='/file'>/file</a> - 50KB 二进制下载测试</li>");
            sb.Append("<li>/post (POST) - 回显表单</li>");
            sb.Append("</ul>");
            sb.Append("<hr/><p>Server Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
            sb.Append("</body></html>");
            return sb.ToString();
        }

        private string BuildPostEcho(string body)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head><meta charset='utf-8'></head><body>");
            sb.Append("<h1>POST 回显</h1>");
            sb.Append("<h3>Raw Body:</h3><pre>");
            sb.Append(System.Web.HttpUtility.HtmlEncode(body));
            sb.Append("</pre>");
            sb.Append("<h3>解析参数:</h3><ul>");
            string[] pairs = body.Split(new char[] { '&' });
            foreach (string p in pairs)
            {
                int eq = p.IndexOf('=');
                if (eq > 0)
                {
                    string k = p.Substring(0, eq);
                    string v = p.Substring(eq + 1);
                    sb.AppendFormat("<li>{0} = {1}</li>",
                        System.Web.HttpUtility.UrlDecode(k, Encoding.UTF8),
                        System.Web.HttpUtility.UrlDecode(v, Encoding.UTF8));
                }
            }
            sb.Append("</ul></body></html>");
            return sb.ToString();
        }

        private void SelfTest()
        {
            // 简单自测：向本机GET一下
            try
            {
                HttpWebRequest r = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:" + Port + "/");
                r.Timeout = 2000;
                using (HttpWebResponse resp = (HttpWebResponse)r.GetResponse())
                {
                    Console.WriteLine("[Self-Test] GET / -> {0}", resp.StatusCode);
                }
            }
            catch (Exception ex) { Console.WriteLine("[Self-Test] 失败: {0}", ex.Message); }
        }
    }
}
