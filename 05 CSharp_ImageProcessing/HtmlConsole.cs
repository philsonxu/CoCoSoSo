using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CSharp20ImageProcessing
{
    public class HtmlConsole
    {
        private StringBuilder _html = new StringBuilder();

        public HtmlConsole()
        {
            _html.AppendLine("<!DOCTYPE html>");
            _html.AppendLine("<html><head>");
            _html.AppendLine("<style>");
            _html.AppendLine("body { font-family: 'Segoe UI', 微软雅黑, sans-serif; background: #1e1e1e; color: #d4d4d4; padding: 20px; margin:0; line-height: 1.6; }");
            _html.AppendLine("h1 { color: #569cd6; font-size: 24px; border-bottom: 2px solid #3e3e42; padding-bottom: 10px; }");
            _html.AppendLine("h2 { color: #4ec9b0; font-size: 20px; margin-top: 30px; }");
            _html.AppendLine("h3 { color: #ce9178; font-size: 16px; }");
            _html.AppendLine(".code { background: #2d2d30; border-left: 4px solid #569cd6; padding: 12px; margin: 10px 0; font-family: Consolas, monospace; font-size: 13px; white-space: pre-wrap; color:#dcdcaa; border-radius: 4px; }");
            _html.AppendLine(".result { background: #1e3a5f; border-left: 4px solid #007acc; padding: 15px; margin: 10px 0; border-radius: 4px; }");
            _html.AppendLine(".info { background: #2b2b2b; border-left: 4px solid #007acc; padding: 12px; margin: 10px 0; border-radius: 4px; color: #9cdcfe; }");
            _html.AppendLine(".warning { background: #3a2e1a; border-left: 4px solid #ce9178; padding: 12px; margin: 10px 0; border-radius: 4px; color: #ffd700; }");
            _html.AppendLine(".success { background: #1e3a2b; border-left: 4px solid #4ec9b0; padding: 12px; margin: 10px 0; border-radius: 4px; color: #b5cea8; }");
            _html.AppendLine(".error { background: #3a1e1e; border-left: 4px solid #f48771; padding: 12px; margin: 10px 0; border-radius: 4px; color: #f48771; }");
            _html.AppendLine("table { width: 100%; border-collapse: collapse; margin: 10px 0; }");
            _html.AppendLine("th, td { border: 1px solid #3e3e42; padding: 8px 12px; text-align: left; }");
            _html.AppendLine("th { background: #2d2d30; color: #569cd6; }");
            _html.AppendLine("tr:nth-child(even) { background: #252526; }");
            _html.AppendLine(".image-container { display: flex; gap: 20px; margin: 15px 0; flex-wrap: wrap; }");
            _html.AppendLine(".image-card { background: #2d2d30; padding: 10px; border-radius: 6px; text-align: center; }");
            _html.AppendLine(".image-card img { max-width: 240px; max-height: 240px; border: 1px solid #3e3e42; border-radius: 4px; display: block; }");
            _html.AppendLine(".image-card .caption { margin-top: 8px; font-size: 13px; color: #9cdcfe; }");
            _html.AppendLine(".divider { border: none; border-top: 1px dashed #3e3e42; margin: 25px 0; }");
            _html.AppendLine("p { margin: 8px 0; }");
            _html.AppendLine("</style></head><body>");
        }

        // 手写HTML转义，零依赖System.Web
        private static string HtmlEncode(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            StringBuilder sb = new StringBuilder(text.Length);
            foreach (char ch in text)
            {
                switch (ch)
                {
                    case '&': sb.Append("&amp;"); break;
                    case '<': sb.Append("&lt;"); break;
                    case '>': sb.Append("&gt;"); break;
                    case '"': sb.Append("&quot;"); break;
                    case '\'': sb.Append("&#39;"); break;
                    case ' ': sb.Append(" "); break;
                    case '\n': sb.Append("<br/>"); break;
                    case '\r': break;
                    default:
                        if (ch > 127)
                            sb.Append("&#" + (int)ch + ";");
                        else
                            sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        public void WriteLine(string text)
        {
            _html.AppendLine(HtmlEncode(text).Replace("  "," &nbsp;"));
        }

        public void WriteTitle(string title)
        {
            _html.AppendLine("<h1>" + HtmlEncode(title) + "</h1>");
        }

        public void WriteSection(string title)
        {
            _html.AppendLine("<h2>" + HtmlEncode(title) + "</h2>");
        }

        public void WriteSubSection(string title)
        {
            _html.AppendLine("<h3>" + HtmlEncode(title) + "</h3>");
        }

        public void WriteCode(string code)
        {
            _html.AppendLine("<div class=\"code\">" + HtmlEncode(code) + "</div>");
        }

        public void WriteInfo(string text)
        {
            _html.AppendLine("<div class=\"info\">💡 " + HtmlEncode(text) + "</div>");
        }

        public void WriteWarning(string text)
        {
            _html.AppendLine("<div class=\"warning\">⚠️ " + HtmlEncode(text) + "</div>");
        }

        public void WriteSuccess(string text)
        {
            _html.AppendLine("<div class=\"success\">✅ " + HtmlEncode(text) + "</div>");
        }

        public void WriteError(string text)
        {
            _html.AppendLine("<div class=\"error\">❌ " + HtmlEncode(text) + "</div>");
        }

        public void WriteResult(string text)
        {
            _html.AppendLine("<div class=\"result\">📊 运行结果：<br/>" + HtmlEncode(text) + "</div>");
        }

        public void WriteDivider()
        {
            _html.AppendLine("<hr class=\"divider\"/>");
        }

        public void BeginImagePair()
        {
            _html.AppendLine("<div class=\"image-container\">");
        }

        public void AddImage(string caption, Bitmap bmp)
        {
            string base64 = BitmapToBase64(bmp);
            _html.AppendLine("<div class=\"image-card\"><img src=\"data:image/png;base64," + base64 + "\" alt=\"" + caption + "\"/><div class=\"caption\">" + caption + "</div></div>");
        }

        public void EndImagePair()
        {
            _html.AppendLine("</div>");
        }

        public void ShowImages(string caption1, Bitmap img1, string caption2, Bitmap img2)
        {
            BeginImagePair();
            AddImage(caption1, img1);
            AddImage(caption2, img2);
            EndImagePair();
        }

        private string BitmapToBase64(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }

        public string GetHtml()
        {
            _html.AppendLine("</body></html>");
            return _html.ToString();
        }
    }
}
