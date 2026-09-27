using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CSharpNetworkProgramming.Chapter05_Mail
{
    /// <summary>
    /// 5.2 POP3 接收邮件（Socket手动实现协议，演示POP3命令流）
    /// 知识点：TcpClient+NetworkStream手动实现POP3协议，
    ///         USER/PASS/STAT/LIST/RETR/DELE/QUIT命令、+OK/-ERR响应、
    ///         Base64/QuotedPrintable解码概念、邮件头解析思路
    /// 注意：本示例为离线演示：不实际连接POP3服务器，而是通过"内建模拟服务器"
    ///       展示完整命令交互流程，避免需要真实邮箱账号。
    /// </summary>
    public class _02_Pop3ReceiveMail : ISample
    {
        public string Id { get { return "5.2"; } }
        public string Title { get { return "POP3 收邮件(协议级演示)"; } }
        public string Description { get { return "演示POP3协议流程：连接、USER/PASS认证、STAT获取邮箱统计、LIST列邮件、RETR下载邮件、DELE删除、QUIT退出。本示例在本地循环演示完整命令-响应交互。"; } }

        public void Run()
        {
            Console.WriteLine("===== POP3 协议交互演示 (本地模拟) =====");
            Console.WriteLine("说明：真实场景替换为 TcpClient.Connect(\"pop3.example.com\", 110) 即可。");
            Console.WriteLine();

            // 用MemoryStream+自定义双向流模拟"服务器"响应，演示完整协议流程
            using (MemoryStream ms = new MemoryStream())
            {
                // 我们直接以"脚本"方式演示命令-响应序列，便于学习
                string[] script = new string[]
                {
                    "[Server] +OK POP3 Welcome",
                    "[Client] USER demo@example.com",
                    "[Server] +OK",
                    "[Client] PASS ********",
                    "[Server] +OK Logged in",
                    "[Client] STAT",
                    "[Server] +OK 3 8421         // 3封邮件，共8421字节",
                    "[Client] LIST",
                    "[Server] +OK 3 messages (8421 bytes)",
                    "[Server] 1 2345",
                    "[Server] 2 3100",
                    "[Server] 3 2976",
                    "[Server] .",
                    "[Client] RETR 1",
                    "[Server] +OK 2345 octets",
                    "[Server] Return-Path: <sender@example.com>",
                    "[Server] From: \"发件人\" <sender@example.com>",
                    "[Server] To: <demo@example.com>",
                    "[Server] Subject: =?utf-8?B?5rWL6K+V6YKu5Lu2?=",
                    "[Server] Date: Mon, 26 Sep 2026 10:30:00 +0800",
                    "[Server] Content-Type: text/plain; charset=utf-8",
                    "[Server] ",
                    "[Server] 这是第一封邮件的正文内容。",
                    "[Server] Hello from POP3!",
                    "[Server] .",
                    "[Client] QUIT",
                    "[Server] +OK Bye"
                };
                foreach (string line in script)
                {
                    Console.WriteLine(line);
                    System.Threading.Thread.Sleep(30);
                }
            }

            Console.WriteLine();
            Console.WriteLine("===== POP3 关键命令速查表 =====");
            Console.WriteLine("  USER name     提交用户名");
            Console.WriteLine("  PASS pwd      提交密码");
            Console.WriteLine("  STAT          返回邮件数量与总大小");
            Console.WriteLine("  LIST [n]      列出邮件大小（不带n列所有）");
            Console.WriteLine("  UIDL [n]      列出邮件唯一ID");
            Console.WriteLine("  RETR n        获取第n封邮件完整内容");
            Console.WriteLine("  TOP n lines   获取第n封邮件头+前N行正文");
            Console.WriteLine("  DELE n        标记第n封邮件为已删除");
            Console.WriteLine("  RSET          取消所有DELE标记");
            Console.WriteLine("  NOOP          无操作，保持连接");
            Console.WriteLine("  QUIT          提交更改并退出");

            Console.WriteLine();
            Console.WriteLine("===== POP3 真实连接代码模板 =====");
            Console.WriteLine(
@"using (TcpClient client = new TcpClient(""pop3.example.com"", 110))
using (NetworkStream ns = client.GetStream())
using (StreamReader sr = new StreamReader(ns, Encoding.ASCII))
using (StreamWriter sw = new StreamWriter(ns, Encoding.ASCII) { NewLine = ""\r\n"", AutoFlush = true })
{
    Console.WriteLine(sr.ReadLine()); // 欢迎语
    sw.WriteLine(""USER user@example.com"");
    Console.WriteLine(sr.ReadLine());
    sw.WriteLine(""PASS password"");
    Console.WriteLine(sr.ReadLine());
    sw.WriteLine(""STAT"");
    Console.WriteLine(sr.ReadLine());
    sw.WriteLine(""QUIT"");
    Console.WriteLine(sr.ReadLine());
}");

            Console.WriteLine();
            Console.WriteLine("===== 邮件头编码说明 =====");
            Console.WriteLine("  Subject: =?utf-8?B?5rWL6K+V6YKu5Lu2?=  (Base64编码, 解码后为「测试邮件」)");
            Console.WriteLine("  Subject: =?gb2312?Q?=B2=E2=CA=D4?=     (QuotedPrintable编码)");
            Console.WriteLine("  正文可能为 Content-Transfer-Encoding: base64/quoted-printable/7bit/8bit");
        }
    }
}
