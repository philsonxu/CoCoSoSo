using System;
using System.IO;
using System.Net;

namespace CSharpNetworkProgramming.Chapter04_Http
{
    /// <summary>
    /// 4.3 HTTP 文件下载 (断点续传演示)
    /// 知识点：分块下载、Progress进度、HttpWebResponse.ContentLength、AddRange断点续传、
    ///         FileMode.Append/Create、下载速度统计
    /// </summary>
    public class _03_HttpDownload : ISample
    {
        public string Id { get { return "4.3"; } }
        public string Title { get { return "HTTP 文件下载 (含进度/断点)"; } }
        public string Description { get { return "演示通过HttpWebResponse下载文件，显示下载进度、速度；演示Range头做断点续传(从已下载位置继续)。"; } }

        public void Run()
        {
            string url = "http://127.0.0.1:" + _04_SimpleHttpServer.Port + "/file";
            string savePath = Path.Combine(Path.GetTempPath(), "http_download_test.bin");
            Console.WriteLine("下载URL: {0}", url);
            Console.WriteLine("保存路径: {0}", savePath);

            long existLen = 0;
            FileMode fm = FileMode.Create;
            if (File.Exists(savePath))
            {
                existLen = new FileInfo(savePath).Length;
                if (existLen > 0)
                {
                    Console.WriteLine("检测到已下载文件 {0} 字节，尝试断点续传...", existLen);
                    fm = FileMode.Append;
                }
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.Timeout = 5000;
            if (existLen > 0)
                req.AddRange((int)existLen); // 断点续传Range头

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
                    Console.WriteLine("服务器未启动(请先运行4.4)，演示使用本地test.bin模拟下载流程...");
                    SimulateDownload(savePath);
                    return;
                }
            }

            using (resp)
            {
                long totalLen = resp.ContentLength;
                if (resp.StatusCode == HttpStatusCode.PartialContent)
                {
                    Console.WriteLine("服务器支持断点续传(206 Partial Content)");
                    totalLen += existLen;
                }
                Console.WriteLine("待下载总大小: {0} 字节 ({1:F2} KB)", totalLen, totalLen / 1024.0);

                using (Stream src = resp.GetResponseStream())
                using (FileStream fs = new FileStream(savePath, fm, FileAccess.Write))
                {
                    byte[] buf = new byte[4096];
                    long downloaded = existLen;
                    DateTime start = DateTime.Now;
                    int lastPercent = -1;
                    while (true)
                    {
                        int n = src.Read(buf, 0, buf.Length);
                        if (n <= 0) break;
                        fs.Write(buf, 0, n);
                        downloaded += n;
                        int percent = totalLen > 0 ? (int)(downloaded * 100 / totalLen) : 0;
                        if (percent != lastPercent && percent % 10 == 0)
                        {
                            double sec = (DateTime.Now - start).TotalSeconds;
                            double speed = sec > 0 ? downloaded / sec / 1024 : 0;
                            Console.WriteLine("  进度 {0}%  -  {1:F2} KB / {2:F2} KB  -  {3:F1} KB/s",
                                percent, downloaded / 1024.0, totalLen / 1024.0, speed);
                            lastPercent = percent;
                        }
                    }
                    double totalSec = (DateTime.Now - start).TotalSeconds;
                    Console.WriteLine("下载完成，耗时 {0:F2}s，平均 {1:F1} KB/s",
                        totalSec, totalSec > 0 ? downloaded / totalSec / 1024 : 0);
                    Console.WriteLine("文件大小验证: {0} 字节", new FileInfo(savePath).Length);
                }
            }
        }

        private void SimulateDownload(string savePath)
        {
            Console.WriteLine("--- 模拟下载流程 ---");
            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                byte[] data = new byte[20000]; // 20KB
                Random rnd = new Random(0);
                rnd.NextBytes(data);
                int written = 0;
                while (written < data.Length)
                {
                    int chunk = Math.Min(4096, data.Length - written);
                    fs.Write(data, written, chunk);
                    written += chunk;
                    Console.WriteLine("  进度 {0}%  -  {1}/{2} 字节",
                        written * 100 / data.Length, written, data.Length);
                    System.Threading.Thread.Sleep(50);
                }
            }
            Console.WriteLine("模拟下载完成，文件: {0}", savePath);
        }
    }
}
