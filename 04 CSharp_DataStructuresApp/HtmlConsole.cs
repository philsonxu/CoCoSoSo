using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CSharp20DataStructures
{
    /// <summary>
    /// C# 2.0 数据结构教学专用 HTML 输出控制台
    /// 替代传统 Console，生成富文本格式的演示内容
    /// </summary>
    public class HtmlConsole
    {
        private StringBuilder _html = new StringBuilder();
        //private WebBrowser _browser;

        public HtmlConsole()//WebBrowser browser)
        {
            //_browser = browser;
        }

        public void WriteHead()
        {
            _html.AppendLine(@"<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<style>
* { margin: 0; padding: 0; box-sizing: border-box; font-family: 'Consolas', '微软雅黑', sans-serif; }
body { background: #1e1e1e; color: #d4d4d4; padding: 20px; line-height: 1.6; font-size: 14px; }
h1 { color: #569cd6; font-size: 24px; border-bottom: 2px solid #569cd6; padding-bottom: 10px; margin-bottom: 20px; }
h2 { color: #4ec9b0; font-size: 18px; margin: 20px 0 10px; padding-left: 10px; border-left: 4px solid #4ec9b0; }
h3 { color: #ce9178; font-size: 16px; margin: 15px 0 8px; }
p { margin: 8px 0; }
.code-block { background: #252526; border: 1px solid #3e3e42; border-radius: 4px; padding: 15px; margin: 10px 0; font-family: Consolas, monospace; white-space: pre-wrap; color: #dcdcaa; }
.result-block { background: #2d2d2d; border-left: 4px solid #569cd6; padding: 12px 15px; margin: 10px 0; border-radius: 0 4px 4px 0; }
.tip { background: rgba(86, 156, 214, 0.1); border-left: 4px solid #569cd6; padding: 10px 15px; margin: 10px 0; border-radius: 0 4px 4px 0; color: #9cdcfe; }
.note { background: rgba(255, 215, 0, 0.1); border-left: 4px solid #dcdcaa; padding: 10px 15px; margin: 10px 0; border-radius: 0 4px 4px 0; color: #dcdcaa; }
.success { background: rgba(78, 201, 176, 0.1); border-left: 4px solid #4ec9b0; padding: 10px 15px; margin: 10px 0; border-radius: 0 4px 4px 0; color: #4ec9b0; }
.error { background: rgba(244, 71, 71, 0.1); border-left: 4px solid #f44747; padding: 10px 15px; margin: 10px 0; border-radius: 0 4px 4px 0; color: #f48771; }
hr { border: none; border-top: 1px solid #3e3e42; margin: 20px 0; }
table { width: 100%; border-collapse: collapse; margin: 10px 0; }
th { background: #2d2d2d; color: #569cd6; padding: 8px; border: 1px solid #3e3e42; }
td { padding: 8px; border: 1px solid #3e3e42; text-align: center; }
.keyword { color: #569cd6; font-weight: bold; }
.type { color: #4ec9b0; }
.string { color: #ce9178; }
.comment { color: #6a9955; }
.number { color: #b5cea8; }
</style>
</head>
<body>");
        }

        public void WriteTitle(string text)
        {
            _html.AppendLine("<h1>" + text + "</h1>");
        }

        public void WriteSection(string text)
        {
            _html.AppendLine("<h2>" + text + "</h2>");
        }

        public void WriteSubSection(string text)
        {
            _html.AppendLine("<h3>" + text + "</h3>");
        }

        public void WriteLine(string text)
        {
            _html.AppendLine("<p>" + text + "</p>");
        }

        /// <summary>
        /// 转义HTML特殊字符
        /// </summary>
        private string Escape(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(text,
                "&", "&amp;"),
                "<", "&lt;"),
                ">", "&gt;"),
                "\"", "&quot;"),
                "'", "&#39;");
        }

        public void WriteCode(string code)
        {
#if __OLD__
            code = System.Web.HttpUtility.HtmlEncode(code);
            // 简单关键字高亮
            string[] keywords = { "public", "private", "class", "void", "int", "string", "bool", "new", "return", "if", "else", "for", "foreach", "while", "do", "switch", "case", "break", "continue", "default", "using", "namespace", "static", "this", "null", "true", "false", "get", "set", "ref", "out", "try", "catch", "finally", "throw", "delegate", "where" };
            foreach (string kw in keywords)
            {
                code = code.Replace(kw, "<span class='keyword'>" + kw + "</span>");
            }
            _html.Append("<div class='code-block'>" + code + "</div>");
#endif
            _html.Append("<div class=\"code-block\">");
            _html.Append(Escape(code));
            _html.AppendLine("</div>");
        }

        public void WriteResult(string result)
        {
            _html.AppendLine("<div class='result-block'>" + result.Replace("\n", "<br/>") + "</div>");
        }

        public void WriteTip(string text)
        {
            _html.AppendLine("<div class='tip'>💡 " + text + "</div>");
        }

        public void WriteNote(string text)
        {
            _html.AppendLine("<div class='note'>⚠️ " + text + "</div>");
        }

        public void WriteSuccess(string text)
        {
            _html.AppendLine("<div class='success'>✅ " + text + "</div>");
        }

        public void WriteError(string text)
        {
            _html.AppendLine("<div class='error'>❌ " + text + "</div>");
        }

        public void WriteHr()
        {
            _html.AppendLine("<hr/>");
        }

        public void WriteArray(string title, Array arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<h3>" + title + "</h3>");
            sb.Append("<table><tr><th>索引</th>");
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append("<td>" + i + "</td>");
            }
            sb.Append("</tr><tr><th>值</th>");
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append("<td>" + arr.GetValue(i) + "</td>");
            }
            sb.Append("</tr></table>");
            _html.AppendLine(sb.ToString());
        }

        public void Render()
        {
            _html.AppendLine("</body>");
            _html.AppendLine("</html>");
            //File.WriteAllText(@"001.html", _html.ToString(), Encoding.UTF8);
            //_browser.DocumentText = _html.ToString();
        }

        public string GetHtml()
        {
            return _html.ToString();
        }

        public void Clear()
        {
            _html.Length = 0;
        }
    }
}
