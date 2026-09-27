using System;
using System.IO;
using System.Text;

namespace CSharpNetworkProgramming
{
    /// <summary>
    /// 测试数据生成器
    /// 纯 C# 2.0 实现，程序启动时自动生成 test.txt / test.bin / index.html，
    /// 不依赖任何外部脚本（如 Python/Bat/PowerShell），确保项目完全自包含。
    /// </summary>
    public static class TestDataGenerator
    {
        /// <summary>
        /// 确保 TestData 目录下的所有测试数据均已存在；若缺失则自动生成。
        /// 在 Program.Main 启动时调用一次即可。
        /// </summary>
        public static void EnsureTestData()
        {
            string dir = GetTestDataDir();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string txtPath  = Path.Combine(dir, "test.txt");
            string binPath  = Path.Combine(dir, "test.bin");
            string htmlPath = Path.Combine(dir, "index.html");

            if (!File.Exists(txtPath))  GenerateTestText(txtPath);
            if (!File.Exists(binPath))  GenerateTestBinary(binPath, 10240); // 10KB
            if (!File.Exists(htmlPath)) GenerateTestHtml(htmlPath);

            Console.WriteLine("[TestData] 测试数据目录: " + dir);
            Console.WriteLine("[TestData] test.txt / test.bin / index.html 已就绪");
        }

        /// <summary>
        /// 定位 TestData 目录：优先使用项目相对路径（..\..\TestData），回落到可执行文件目录下的 TestData。
        /// </summary>
        private static string GetTestDataDir()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string devPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "TestData"));
            if (Directory.Exists(Path.GetDirectoryName(devPath)))
            {
                if (!Directory.Exists(devPath)) Directory.CreateDirectory(devPath);
                return devPath;
            }
            string runPath = Path.Combine(baseDir, "TestData");
            if (!Directory.Exists(runPath)) Directory.CreateDirectory(runPath);
            return runPath;
        }

        /// <summary>
        /// 生成文本测试文件：包含中英文字符、数字、特殊符号、多行文本
        /// </summary>
        private static void GenerateTestText(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("这是 C# 网络编程示例库的测试文本文件。");
            sb.AppendLine("=====================================");
            sb.AppendLine();
            sb.AppendLine("本文件由纯 C# 2.0 代码自动生成（TestDataGenerator.cs），");
            sb.AppendLine("用于 TCP/UDP/HTTP 文件传输等示例的测试数据。");
            sb.AppendLine("包含中文、英文、数字、特殊符号：");
            sb.AppendLine("  - 编号: 001");
            sb.AppendLine("  - 名称: 网络测试文件");
            sb.AppendLine("  - 内容: Hello, World! 你好，世界！");
            sb.AppendLine("  - 时间: " + DateTime.Now.ToString("yyyy-MM-dd"));
            sb.AppendLine();
            sb.AppendLine("行尾测试：");
            sb.AppendLine("Line1");
            sb.AppendLine("Line2");
            sb.AppendLine("Line3");
            sb.AppendLine("-------------------------------------");
            sb.AppendLine("The quick brown fox jumps over the lazy dog.");
            sb.AppendLine("1234567890 !@#$%^&*()_+-=[]{}|;':\",./<>?");
            sb.AppendLine("=====================================");

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// 生成二进制测试文件：写入 4 字节魔数 "CST1" + 4 字节长度头 + 可预测递增字节序列，
        /// 便于文件传输示例校验完整性
        /// </summary>
        private static void GenerateTestBinary(string path, int length)
        {
            // 保证最小长度为 1024
            if (length < 1024) length = 1024;

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                // 4 字节魔数，用于文件类型识别
                bw.Write((byte)'C');
                bw.Write((byte)'S');
                bw.Write((byte)'T');
                bw.Write((byte)'1');

                // 4 字节小端序长度（后续 payload 大小）
                int payload = length - 8;
                bw.Write((byte)(payload & 0xFF));
                bw.Write((byte)((payload >> 8) & 0xFF));
                bw.Write((byte)((payload >> 16) & 0xFF));
                bw.Write((byte)((payload >> 24) & 0xFF));

                // 写入可预测的递增字节序列（从 0x00 开始循环，步长 7，方便校验）
                byte b = 0;
                for (int i = 0; i < payload; i++)
                {
                    bw.Write(b);
                    b = (byte)((b + 7) & 0xFF);
                }
            }
        }

        /// <summary>
        /// 生成静态 HTML 测试页面，用于 SimpleHttpServer 和 HttpGet 示例
        /// </summary>
        private static void GenerateTestHtml(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"zh-CN\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"UTF-8\">");
            sb.AppendLine("    <title>C# 网络编程示例 - 测试页面</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine("        body { font-family: \"Microsoft YaHei\", Arial, sans-serif; margin: 40px; background:#f5f5f5; }");
            sb.AppendLine("        h1 { color: #1976d2; border-bottom: 2px solid #1976d2; padding-bottom:8px; }");
            sb.AppendLine("        h2 { color: #333; margin-top:24px; }");
            sb.AppendLine("        .info { background:#fff; padding:16px 24px; border-radius:8px; box-shadow:0 2px 4px rgba(0,0,0,0.1); }");
            sb.AppendLine("        ul { line-height:1.8; }");
            sb.AppendLine("        .code { background:#272822; color:#f8f8f2; padding:12px 16px; border-radius:4px; font-family:Consolas,monospace; }");
            sb.AppendLine("        a { color: #1976d2; }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class=\"info\">");
            sb.AppendLine("        <h1>C# 网络编程示例库</h1>");
            sb.AppendLine("        <p>欢迎！这是 <b>TestData/index.html</b> 静态页面，由纯 C# 2.0 代码自动生成，用于 HTTP 示例测试。</p>");
            sb.AppendLine();
            sb.AppendLine("        <h2>内容列表</h2>");
            sb.AppendLine("        <ul>");
            sb.AppendLine("            <li>第一章：网络基础 (DNS/IPEndPoint/NetworkStream)</li>");
            sb.AppendLine("            <li>第二章：TCP 编程 (Echo/聊天/文件传输/并发服务器)</li>");
            sb.AppendLine("            <li>第三章：UDP 编程 (Echo/聊天/文件传输/广播)</li>");
            sb.AppendLine("            <li>第四章：HTTP 编程 (GET/POST/下载/简易服务器)</li>");
            sb.AppendLine("            <li>第五章：邮件 (SMTP/POP3)</li>");
            sb.AppendLine("            <li>第六章：FTP 客户端</li>");
            sb.AppendLine("            <li>第七章：异步编程 (APM Begin/End)</li>");
            sb.AppendLine("            <li>第八章：高级 (代理/端口扫描/Ping)</li>");
            sb.AppendLine("            <li>第九章：工具 (URL编码/二进制序列化)</li>");
            sb.AppendLine("        </ul>");
            sb.AppendLine();
            sb.AppendLine("        <h2>表单测试（POST）</h2>");
            sb.AppendLine("        <form action=\"/post\" method=\"post\">");
            sb.AppendLine("            <p>用户名：<input type=\"text\" name=\"username\" value=\"张三\" /></p>");
            sb.AppendLine("            <p>密　码：<input type=\"password\" name=\"password\" value=\"secret\" /></p>");
            sb.AppendLine("            <p><input type=\"submit\" value=\"提交测试\" /></p>");
            sb.AppendLine("        </form>");
            sb.AppendLine();
            sb.AppendLine("        <h2>下载测试</h2>");
            sb.AppendLine("        <p><a href=\"/file\">下载二进制测试文件</a></p>");
            sb.AppendLine();
            sb.AppendLine("        <h2>代码片段</h2>");
            sb.AppendLine("        <div class=\"code\">");
            sb.AppendLine("HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);<br/>");
            sb.AppendLine("req.Method = \"GET\";<br/>");
            sb.AppendLine("using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())<br/>");
            sb.AppendLine("using (Stream s = resp.GetResponseStream()) { /* ... */ }");
            sb.AppendLine("        </div>");
            sb.AppendLine();
            sb.AppendLine("        <hr/>");
            sb.AppendLine("        <p style=\"color:#888; font-size:12px;\">CSharpNetworkProgramming Demo Page &copy; 2026 (generated by C# code, no Python)</p>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
