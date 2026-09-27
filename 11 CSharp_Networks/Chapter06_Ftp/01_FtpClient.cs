using System;
using System.IO;
using System.Net;
using System.Text;

namespace CSharpNetworkProgramming.Chapter06_Ftp
{
    /// <summary>
    /// 6.1 FTP 客户端示例 (FtpWebRequest)
    /// 知识点：FtpWebRequest/FtpWebResponse、Method=ListDirectory/GetFileSize/
    ///         DownloadFile/UploadFile/DeleteFile/MakeDirectory/RemoveDirectory、
    ///         NetworkCredential、UseBinary/UsePassive、KeepAlive
    /// 注意：本示例演示所有API的调用方法，并对公有FTP服务器做连通性测试；
    ///       若网络不可用，将用本地文件模拟目录列表/上传下载逻辑，保证示例可运行。
    /// </summary>
    public class _01_FtpClient : ISample
    {
        public string Id { get { return "6.1"; } }
        public string Title { get { return "FTP 客户端 (FtpWebRequest)"; } }
        public string Description { get { return "演示FtpWebRequest支持的主要操作：列出目录、获取文件大小、下载文件、上传文件、删除文件、创建/删除目录；演示被动模式、二进制模式、Credentials。"; } }

        public void Run()
        {
            Console.WriteLine("===== FtpWebRequest API 一览 =====");
            PrintMethodList();

            // 尝试连接公共FTP服务器测试ListDirectory
            string publicFtp = "ftp://ftp.gnu.org/";
            Console.WriteLine();
            Console.WriteLine("尝试连接 {0} 列出目录(3秒超时)...", publicFtp);
            bool online = false;
            try
            {
                online = TryListDirectory(publicFtp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("连接失败: {0}", ex.Message);
            }

            if (!online)
            {
                Console.WriteLine();
                Console.WriteLine("公有FTP不可达，下面演示本地模拟：构造FTP请求参数、演示上传/下载代码模板。");
                PrintCodeTemplates();
                SimulateUpDownload();
            }
        }

        private bool TryListDirectory(string url)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(url);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            req.Timeout = 3000;
            req.ReadWriteTimeout = 3000;
            req.UsePassive = true;
            req.UseBinary = true;
            req.KeepAlive = false;
            // 匿名FTP
            req.Credentials = new NetworkCredential("anonymous", "demo@example.com");

            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
            {
                Console.WriteLine("状态: {0} - {1}", resp.StatusCode, resp.StatusDescription);
                using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.ASCII))
                {
                    int count = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null && count < 10)
                    {
                        Console.WriteLine("  {0}", line);
                        count++;
                    }
                    if (count == 10) Console.WriteLine("  ... (只列出前10项)");
                }
                return true;
            }
        }

        private void PrintMethodList()
        {
            Console.WriteLine("常用 WebRequestMethods.Ftp 命令：");
            string[,] methods = new string[,]
            {
                {"ListDirectory", "列出目录(短名)"},
                {"ListDirectoryDetails", "列出目录详细信息(类ls -l)"},
                {"GetFileSize", "获取文件大小"},
                {"DownloadFile", "下载文件"},
                {"UploadFile", "上传文件"},
                {"DeleteFile", "删除文件"},
                {"MakeDirectory", "创建目录"},
                {"RemoveDirectory", "删除目录"},
                {"Rename", "重命名(RenameTo)"},
                {"AppendFile", "追加到文件"},
                {"PrintWorkingDirectory", "打印当前工作目录"},
                {"GetDateTimestamp", "获取文件时间戳"},
            };
            for (int i = 0; i < methods.GetLength(0); i++)
            {
                Console.WriteLine("  {0,-22}{1}", methods[i, 0], methods[i, 1]);
            }
        }

        private void PrintCodeTemplates()
        {
            Console.WriteLine("----- FTP 上传文件代码模板 -----");
            Console.WriteLine(
@"FtpWebRequest req = (FtpWebRequest)WebRequest.Create(""ftp://example.com/test.txt"");
req.Method = WebRequestMethods.Ftp.UploadFile;
req.Credentials = new NetworkCredential(""user"", ""pass"");
req.UseBinary = true;
req.UsePassive = true;
byte[] data = File.ReadAllBytes(@""local.txt"");
using (Stream s = req.GetRequestStream()) { s.Write(data, 0, data.Length); }
using (FtpWebResponse r = (FtpWebResponse)req.GetResponse())
    Console.WriteLine(r.StatusDescription);");

            Console.WriteLine("----- FTP 下载文件代码模板 -----");
            Console.WriteLine(
@"FtpWebRequest req = (FtpWebRequest)WebRequest.Create(""ftp://example.com/test.txt"");
req.Method = WebRequestMethods.Ftp.DownloadFile;
req.Credentials = new NetworkCredential(""user"", ""pass"");
using (FtpWebResponse r = (FtpWebResponse)req.GetResponse())
using (Stream src = r.GetResponseStream())
using (FileStream fs = File.Create(@""local.txt""))
{
    byte[] buf = new byte[8192];
    int n;
    while ((n = src.Read(buf, 0, buf.Length)) > 0) fs.Write(buf, 0, n);
}");
        }

        private void SimulateUpDownload()
        {
            string tmpDir = Path.Combine(Path.GetTempPath(), "FtpDemo");
            Directory.CreateDirectory(tmpDir);
            string localFile = Path.Combine(tmpDir, "upload.txt");
            string downloadedFile = Path.Combine(tmpDir, "download.txt");

            // 模拟上传：写一个文件
            File.WriteAllText(localFile, "Hello FTP Demo!\n这是模拟上传的文件内容。\n", Encoding.UTF8);
            Console.WriteLine("(模拟上传) 写入 {0}, 大小 {1} 字节", localFile, new FileInfo(localFile).Length);

            // 模拟下载：复制文件
            File.Copy(localFile, downloadedFile, true);
            Console.WriteLine("(模拟下载) 复制到 {0}, 内容验证：", downloadedFile);
            Console.WriteLine(File.ReadAllText(downloadedFile, Encoding.UTF8));

            // 模拟LIST
            Console.WriteLine("(模拟ListDirectory) {0} 下的文件：", tmpDir);
            foreach (string f in Directory.GetFiles(tmpDir))
            {
                Console.WriteLine("  {0}  ({1} bytes)", Path.GetFileName(f), new FileInfo(f).Length);
            }
        }
    }
}
