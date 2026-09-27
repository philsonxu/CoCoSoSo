using System;
using System.Text;
using System.Web;
using System.Drawing;
using System.Windows.Forms;

namespace NumericalComputing.UI
{
    /// <summary>
    /// HTML富文本输出控制台，替代传统Console，纯C#2.0实现
    /// </summary>
    public class HtmlConsole
    {
        private StringBuilder _sb = new StringBuilder();
        private WebBrowser _browser;

        public HtmlConsole(WebBrowser browser)
        {
            _browser = browser;
            InitHtml();
        }

        private void InitHtml()
        {
            _sb.AppendLine("<html><head>");
            _sb.AppendLine("<meta charset='utf-8'>");
            _sb.AppendLine("<style>");
            _sb.AppendLine("body { font-family: 'Microsoft YaHei', 'SimSun', sans-serif; background: #f8f9fa; padding: 20px; line-height: 1.7; color: #2c3e50; }");
            _sb.AppendLine("h1 { color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px; }");
            _sb.AppendLine("h2 { color: #2980b9; margin-top: 25px; border-left: 5px solid #3498db; padding-left: 10px; }");
            _sb.AppendLine("h3 { color: #16a085; margin-top: 20px; }");
            _sb.AppendLine("pre { background: #2d2d2d; color: #f8f8f2; padding: 15px; border-radius: 8px; overflow-x: auto; font-family: Consolas, monospace; font-size: 13px; }");
            _sb.AppendLine("code { font-family: Consolas, monospace; }");
            _sb.AppendLine(".result { background: #1e1e1e; color: #4ec9b0; padding: 15px; border-radius: 8px; font-family: Consolas, monospace; white-space: pre-wrap; margin: 10px 0; }");
            _sb.AppendLine(".info { background: #d1ecf1; border-left: 5px solid #0dcaf0; padding: 12px; margin: 10px 0; border-radius: 4px; }");
            _sb.AppendLine(".success { background: #d4edda; border-left: 5px solid #198754; padding: 12px; margin: 10px 0; border-radius: 4px; }");
            _sb.AppendLine(".warning { background: #fff3cd; border-left: 5px solid #ffc107; padding: 12px; margin: 10px 0; border-radius: 4px; }");
            _sb.AppendLine(".error { background: #f8d7da; border-left: 5px solid #dc3545; padding: 12px; margin: 10px 0; border-radius: 4px; }");
            _sb.AppendLine("table { border-collapse: collapse; width: 100%; margin: 15px 0; background: white; }");
            _sb.AppendLine("th, td { border: 1px solid #dee2e6; padding: 10px; text-align: center; }");
            _sb.AppendLine("th { background: #e9ecef; font-weight: bold; }");
            _sb.AppendLine("img { max-width: 100%; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); margin: 10px 0; }");
            _sb.AppendLine("hr { border: none; border-top: 2px dashed #dee2e6; margin: 30px 0; }");
            _sb.AppendLine("</style></head><body>");
        }

        public void Clear()
        {
            _sb.Length = 0;
            InitHtml();
            Refresh();
        }

        public void H1(string text)
        {
            _sb.AppendLine("<h1>" + HttpUtility.HtmlEncode(text) + "</h1>");
        }

        public void H2(string text)
        {
            _sb.AppendLine("<h2>" + HttpUtility.HtmlEncode(text) + "</h2>");
        }

        public void H3(string text)
        {
            _sb.AppendLine("<h3>" + HttpUtility.HtmlEncode(text) + "</h3>");
        }

        public void P(string text)
        {
            _sb.AppendLine("<p>" + HttpUtility.HtmlEncode(text).Replace("\n", "<br/>") + "</p>");
        }

        public void Code(string code)
        {
            _sb.AppendLine("<pre><code>" + HttpUtility.HtmlEncode(code) + "</code></pre>");
        }

        public void Result(string text)
        {
            _sb.AppendLine("<div class='result'>" + HttpUtility.HtmlEncode(text) + "</div>");
        }

        public void Info(string text)
        {
            _sb.AppendLine("<div class='info'>ℹ️ " + HttpUtility.HtmlEncode(text) + "</div>");
        }

        public void Success(string text)
        {
            _sb.AppendLine("<div class='success'>✅ " + HttpUtility.HtmlEncode(text) + "</div>");
        }

        public void Warning(string text)
        {
            _sb.AppendLine("<div class='warning'>⚠️ " + HttpUtility.HtmlEncode(text) + "</div>");
        }

        public void Error(string text)
        {
            _sb.AppendLine("<div class='error'>❌ " + HttpUtility.HtmlEncode(text) + "</div>");
        }

        public void Table(string[] headers, double[][] rows)
        {
            _sb.AppendLine("<table><thead><tr>");
            for (int i = 0; i < headers.Length; i++)
            {
                _sb.AppendLine("<th>" + HttpUtility.HtmlEncode(headers[i]) + "</th>");
            }
            _sb.AppendLine("</tr></thead><tbody>");
            for (int i = 0; i < rows.Length; i++)
            {
                _sb.AppendLine("<tr>");
                for (int j = 0; j < rows[i].Length; j++)
                {
                    _sb.AppendLine("<td>" + rows[i][j].ToString("F6") + "</td>");
                }
                _sb.AppendLine("</tr>");
            }
            _sb.AppendLine("</tbody></table>");
        }

        public void ImageBase64(string base64)
        {
            _sb.AppendLine("<img src='data:image/png;base64," + base64 + "'/>");
        }

        public void Hr()
        {
            _sb.AppendLine("<hr/>");
        }

        public void Refresh()
        {
            _sb.AppendLine("</body></html>");
            _browser.DocumentText = _sb.ToString();
            _sb.Length -= ("</body></html>").Length;
        }
    }
}