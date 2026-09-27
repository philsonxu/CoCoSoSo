using System;
using System.Text;
using System.Drawing;
using System.IO;

namespace CSharp20AI
{
    class HtmlConsole
    {
        private StringBuilder _sb;

        public HtmlConsole()
        {
            _sb = new StringBuilder();
            _sb.Append("<html><head>");
            _sb.Append("<meta http-equiv='Content-Type' content='text/html;charset=utf-8'/>");
            _sb.Append("<style>");
            _sb.Append("*{margin:0;padding:0;box-sizing:border-box;}");
            _sb.Append("body{font-family:'Microsoft YaHei',Arial,sans-serif;padding:20px;background:#f8f9fa;color:#333;line-height:1.8;}");
            _sb.Append("h1{font-size:24px;color:#2c3e50;border-bottom:3px solid #3498db;padding-bottom:10px;margin-bottom:20px;}");
            _sb.Append("h2{font-size:18px;color:#2c3e50;margin:20px 0 10px 0;padding-left:10px;border-left:4px solid #3498db;}");
            _sb.Append("h3{font-size:15px;color:#34495e;margin:15px 0 8px 0;}");
            _sb.Append("p{margin:10px 0;text-indent:2em;}");
            _sb.Append(".code{background:#2d2d2d;color:#f8f8f2;padding:15px;border-radius:6px;font-family:'Consolas',monospace;font-size:13px;margin:10px 0;white-space:pre-wrap;line-height:1.6;overflow-x:auto;}");
            _sb.Append(".result{background:#ecf0f1;border-left:4px solid #27ae60;padding:12px 15px;border-radius:0 6px 6px 0;margin:10px 0;font-family:'Consolas',monospace;font-size:13px;line-height:1.6;}");
            _sb.Append(".tip{background:#e8f4fd;border-left:4px solid #3498db;padding:12px 15px;border-radius:0 6px 6px 0;margin:10px 0;}");
            _sb.Append(".warning{background:#fef5e7;border-left:4px solid #f39c12;padding:12px 15px;border-radius:0 6px 6px 0;margin:10px 0;}");
            _sb.Append(".success{background:#e8f8f5;border-left:4px solid #27ae60;padding:12px 15px;border-radius:0 6px 6px 0;margin:10px 0;}");
            _sb.Append(".error{background:#fdedec;border-left:4px solid #e74c3c;padding:12px 15px;border-radius:0 6px 6px 0;margin:10px 0;}");
            _sb.Append("hr{border:none;border-top:1px dashed #bdc3c7;margin:25px 0;}");
            _sb.Append("table{border-collapse:collapse;width:100%;margin:10px 0;}");
            _sb.Append("th,td{border:1px solid #ddd;padding:8px 12px;text-align:center;}");
            _sb.Append("th{background:#3498db;color:white;}");
            _sb.Append("tr:nth-child(even){background:#f2f2f2;}");
            _sb.Append(".img-box{text-align:center;margin:15px 0;}");
            _sb.Append(".img-box img{max-width:100%;border:1px solid #ddd;border-radius:6px;box-shadow:0 2px 8px rgba(0,0,0,0.1);}");
            _sb.Append("</style>");
            _sb.Append("</head><body>");
        }

        public void H1(string text)
        {
            _sb.Append("<h1>" + text + "</h1>");
        }

        public void H2(string text)
        {
            _sb.Append("<h2>" + text + "</h2>");
        }

        public void H3(string text)
        {
            _sb.Append("<h3>" + text + "</h3>");
        }

        public void P(string text)
        {
            _sb.Append("<p>" + text + "</p>");
        }

        public void Code(string code)
        {
            _sb.Append("<div class='code'>" + System.Web.HttpUtility.HtmlEncode(code) + "</div>");
        }

        public void Result(string text)
        {
            _sb.Append("<div class='result'>" + text.Replace("\n", "<br/>") + "</div>");
        }

        public void Tip(string text)
        {
            _sb.Append("<div class='tip'>💡 " + text + "</div>");
        }

        public void Warning(string text)
        {
            _sb.Append("<div class='warning'>⚠️ " + text + "</div>");
        }

        public void Success(string text)
        {
            _sb.Append("<div class='success'>✅ " + text + "</div>");
        }

        public void Error(string text)
        {
            _sb.Append("<div class='error'>❌ " + text + "</div>");
        }

        public void Hr()
        {
            _sb.Append("<hr/>");
        }

        public void Image(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                string base64 = Convert.ToBase64String(bytes);
                _sb.Append("<div class='img-box'><img src='data:image/png;base64," + base64 + "'/></div>");
            }
        }

        public void Table(string[] headers, string[,] rows)
        {
            _sb.Append("<table><thead><tr>");
            for (int i = 0; i < headers.Length; i++)
            {
                _sb.Append("<th>" + headers[i] + "</th>");
            }
            _sb.Append("</tr></thead><tbody>");
            for (int r = 0; r < rows.GetLength(0); r++)
            {
                _sb.Append("<tr>");
                for (int c = 0; c < rows.GetLength(1); c++)
                {
                    _sb.Append("<td>" + rows[r, c] + "</td>");
                }
                _sb.Append("</tr>");
            }
            _sb.Append("</tbody></table>");
        }

        public string GetHtml()
        {
            _sb.Append("</body></html>");
            return _sb.ToString();
        }
    }
}
