using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace CSharpNetworkProgramming.Chapter05_Mail
{
    /// <summary>
    /// 5.1 SMTP 发送邮件 (System.Net.Mail)
    /// 知识点：SmtpClient/MailMessage、MailAddress、CC/Bcc、
    ///         附件(Attachment)、HTML/Plain文本双视图、AlternateView、
    ///         DeliveryNotificationOptions、EnableSsl、Credentials
    /// </summary>
    public class _01_SmtpSendMail : ISample
    {
        public string Id { get { return "5.1"; } }
        public string Title { get { return "SMTP 发送邮件(含HTML/附件)"; } }
        public string Description { get { return "演示用SmtpClient构造并发送邮件：纯文本+HTML双视图、抄送/密送、添加附件、设置优先级与已读回执。本示例只构造并打印MailMessage内容，不实际发送（避免需要真实SMTP账号）。"; } }

        public void Run()
        {
            // 构造邮件
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("sender@example.com", "发件人示例", Encoding.UTF8);
            msg.To.Add(new MailAddress("to1@example.com", "收件人A", Encoding.UTF8));
            msg.To.Add("to2@example.com");
            msg.CC.Add("cc@example.com");
            msg.Bcc.Add("bcc@example.com");
            msg.Subject = "C# SMTP 发送示例 - 测试邮件";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.Priority = MailPriority.Normal;
            msg.Headers.Add("X-Mailer", "CSharpNetworkDemo/1.0");
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            // 纯文本正文
            string plainBody = "您好！\n\n这是一封由C# 2.0程序发送的测试邮件。\n\n—— C# Network Demo";
            // HTML正文
            string htmlBody = "<html><body style='font-family:微软雅黑'>"
                            + "<h2 style='color:#1976d2'>您好！</h2>"
                            + "<p>这是一封由 <b>C# 2.0</b> 程序发送的 <i>HTML</i> 格式测试邮件。</p>"
                            + "<p>支持 HTML 样式：<font color='red'>红色文字</font>、<a href='https://example.com'>链接</a>。</p>"
                            + "<hr/><p>—— C# Network Demo</p>"
                            + "</body></html>";

            AlternateView plainView = AlternateView.CreateAlternateViewFromString(plainBody, Encoding.UTF8, "text/plain");
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, "text/html");
            msg.AlternateViews.Add(plainView);
            msg.AlternateViews.Add(htmlView);

            // 添加附件（如果test.txt存在）
            string attachPath = TestData.GetPath("test.txt");
            if (File.Exists(attachPath))
            {
                Attachment att = new Attachment(attachPath, MediaTypeNames.Text.Plain);
                att.Name = "测试附件.txt";
                att.NameEncoding = Encoding.UTF8;
                att.TransferEncoding = TransferEncoding.Base64;
                msg.Attachments.Add(att);
                Console.WriteLine("已添加附件: {0}", attachPath);
            }
            else
            {
                // 动态生成一个附件
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("这是内存生成的附件内容。\n"));
                Attachment att = new Attachment(ms, "内存附件.txt", MediaTypeNames.Text.Plain);
                att.NameEncoding = Encoding.UTF8;
                msg.Attachments.Add(att);
                Console.WriteLine("已添加内存附件");
            }

            // 打印邮件信息
            Console.WriteLine("---- MailMessage 预览 ----");
            Console.WriteLine("From:     {0}", msg.From);
            foreach (MailAddress a in msg.To) Console.WriteLine("To:       {0}", a);
            foreach (MailAddress a in msg.CC) Console.WriteLine("Cc:       {0}", a);
            foreach (MailAddress a in msg.Bcc) Console.WriteLine("Bcc:      {0}", a);
            Console.WriteLine("Subject:  {0}", msg.Subject);
            Console.WriteLine("Priority: {0}", msg.Priority);
            Console.WriteLine("Body(plain预览):\n{0}", plainBody);
            Console.WriteLine("附件数: {0}", msg.Attachments.Count);
            foreach (Attachment a in msg.Attachments) Console.WriteLine("  - {0} ({1})", a.Name, a.ContentType);

            // 构造 SmtpClient（不实际Send，因为需要真实账号）
            Console.WriteLine();
            Console.WriteLine("---- SmtpClient 配置样例 ----");
            Console.WriteLine("SmtpClient smtp = new SmtpClient(\"smtp.example.com\", 587);");
            Console.WriteLine("smtp.EnableSsl = true;");
            Console.WriteLine("smtp.Credentials = new NetworkCredential(\"user@example.com\", \"password\");");
            Console.WriteLine("smtp.Send(msg);   // <-- 实际发送调用（已注释，需真实SMTP账号）");

            Console.WriteLine();
            Console.WriteLine("提示：如需真实发送，请填入您的SMTP服务器（如smtp.qq.com、smtp.163.com等）、端口、邮箱账号与授权码。");

            // 释放资源
            foreach (Attachment a in msg.Attachments) a.Dispose();
            msg.Dispose();
        }
    }
}
