using System;
using System.IO;
using System.Net;
using System.Text;

namespace CSharpNetworkProgramming.Chapter04_Http
{
    /// <summary>
    /// 4.2 HTTP POST 请求 (application/x-www-form-urlencoded)
    /// 知识点：POST请求体、ContentType=application/x-www-form-urlencoded、
    ///         GetRequestStream 写入请求体、URL编码、表单参数提交
    /// </summary>
    public class _02_HttpPost : ISample
    {
        public string Id { get { return "4.2"; } }
        public string Title { get { return "HTTP POST 表单提交"; } }
        public string Description { get { return "演示HttpWebRequest POST提交URL-Encoded表单数据，并读取响应；演示不同ContentType与手动写body。"; } }

        public void Run()
        {
            string url = "http://127.0.0.1:" + _04_SimpleHttpServer.Port + "/post";
            Console.WriteLine("POST 目标: {0}", url);

            // 构造表单数据 (C# 2.0 无匿名类型，用DictionaryEntry列表)
            string formData = "username=" + UrlEncode("张三")
                            + "&password=" + UrlEncode("p@ss w0rd")
                            + "&age=25"
                            + "&remember=on";
            byte[] body = Encoding.UTF8.GetBytes(formData);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            req.ContentLength = body.Length;
            req.UserAgent = "CSharpNetworkDemo/1.0";
            req.Timeout = 3000;

            // 写入请求体
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(body, 0, body.Length);
            }
            Console.WriteLine("已发送请求体: {0}", formData);

            HttpWebResponse resp;
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
                    Console.WriteLine("(已构造POST请求体, 请求设置完成)");
                    // 演示请求体内容
                    return;
                }
            }

            using (resp)
            {
                Console.WriteLine("状态: {0}", resp.StatusCode);
                using (Stream s = resp.GetResponseStream())
                using (StreamReader sr = new StreamReader(s, Encoding.UTF8))
                {
                    string text = sr.ReadToEnd();
                    Console.WriteLine("响应:\n{0}", text);
                }
            }
        }

        // C# 2.0 下的 URL 编码（兼容HttpUtility依赖问题）
        private static string UrlEncode(string s)
        {
            return System.Web.HttpUtility.UrlEncode(s, Encoding.UTF8);
        }
    }
}
