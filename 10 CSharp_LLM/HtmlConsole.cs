using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CSharp6LLM
{
    /// <summary>
    /// HTML富文本输出控制台，替代传统Console.WriteLine
    /// 支持标题、代码块、结果块、提示框、表格、分隔线、进度条、SVG图表
    /// </summary>
    public class HtmlConsole
    {
        private StringBuilder html;

        public HtmlConsole()
        {
            html = new StringBuilder();
            WriteHeader();
        }

        private void WriteHeader()
        {
            html.Append(@"<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<style>
body { font-family: 'Microsoft YaHei', 'Segoe UI', sans-serif; background: #1e1e1e; color: #e0e0e0; padding: 20px; line-height: 1.7; font-size: 14px; }
h1 { color: #569cd6; border-bottom: 2px solid #569cd6; padding-bottom: 8px; margin-top: 30px; }
h2 { color: #4ec9b0; border-left: 4px solid #4ec9b0; padding-left: 10px; margin-top: 25px; }
h3 { color: #ce9178; margin-top: 20px; }
.code-block { background: #252526; border: 1px solid #3e3e42; border-radius: 6px; padding: 12px 16px; font-family: 'Consolas', monospace; font-size: 13px; white-space: pre-wrap; color: #dcdcaa; overflow-x: auto; }
.result-block { background: #0a2a0a; border: 1px solid #2d5a2d; border-radius: 6px; padding: 12px 16px; font-family: 'Consolas', monospace; font-size: 13px; white-space: pre-wrap; color: #b5cea8; margin: 8px 0; }
.info { background: #0e3a5a; border-left: 4px solid #569cd6; padding: 10px 15px; margin: 10px 0; border-radius: 4px; color: #9cdcfe; }
.success { background: #0e3a1a; border-left: 4px solid #4ec9b0; padding: 10px 15px; margin: 10px 0; border-radius: 4px; color: #4ec9b0; }
.warning { background: #3a2a0e; border-left: 4px solid #ce9178; padding: 10px 15px; margin: 10px 0; border-radius: 4px; color: #dcdcaa; }
.error { background: #3a0e0e; border-left: 4px solid #f44747; padding: 10px 15px; margin: 10px 0; border-radius: 4px; color: #f48771; }
hr { border: none; border-top: 1px solid #3e3e42; margin: 25px 0; }
table { border-collapse: collapse; width: 100%; margin: 15px 0; background: #252526; }
th, td { border: 1px solid #3e3e42; padding: 8px 12px; text-align: left; }
th { background: #2d2d30; color: #569cd6; }
.progress-container { width: 100%; background: #2d2d30; border-radius: 4px; height: 24px; margin: 10px 0; overflow: hidden; }
.progress-bar { height: 100%; background: linear-gradient(90deg, #569cd6, #4ec9b0); text-align: center; line-height: 24px; color: white; font-weight: bold; transition: width 0.3s; }
svg { background: #252526; border-radius: 6px; margin: 10px 0; max-width: 100%; }
.log-line { font-family: Consolas, monospace; color: #b5cea8; margin: 2px 0; }
</style>
</head>
<body>
");
        }

        public void H1(string text) { html.Append($"<h1>{HttpUtility.HtmlEncode(text)}</h1>\n"); }
        public void H2(string text) { html.Append($"<h2>{HttpUtility.HtmlEncode(text)}</h2>\n"); }
        public void H3(string text) { html.Append($"<h3>{HttpUtility.HtmlEncode(text)}</h3>\n"); }
        public void P(string text) { html.Append($"<p>{HttpUtility.HtmlEncode(text)}</p>\n"); }
        public void Br() { html.Append("<br>\n"); }
        public void Hr() { html.Append("<hr>\n"); }

        public void Code(string code)
        {
            html.Append($"<div class='code-block'>{HttpUtility.HtmlEncode(code)}</div>\n");
        }

        public void Result(string text)
        {
            html.Append($"<div class='result-block'>{HttpUtility.HtmlEncode(text)}</div>\n");
        }

        public void Log(string text)
        {
            html.Append($"<div class='log-line'>{HttpUtility.HtmlEncode(text)}</div>\n");
        }

        public void Info(string text) { html.Append($"<div class='info'>ℹ️ {HttpUtility.HtmlEncode(text)}</div>\n"); }
        public void Success(string text) { html.Append($"<div class='success'>✅ {HttpUtility.HtmlEncode(text)}</div>\n"); }
        public void Warning(string text) { html.Append($"<div class='warning'>⚠️ {HttpUtility.HtmlEncode(text)}</div>\n"); }
        public void Error(string text) { html.Append($"<div class='error'>❌ {HttpUtility.HtmlEncode(text)}</div>\n"); }

        public void WriteRaw(string rawHtml)
        {
            html.Append(rawHtml);
        }

        public void Progress(double percent, string text = "")
        {
            int p = (int)(percent * 100);
            if (p > 100) p = 100;
            html.Append($"<div class='progress-container'><div class='progress-bar' style='width:{p}%'>{HttpUtility.HtmlEncode(text)} {p}%</div></div>\n");
        }

        public void StartTable(params string[] headers)
        {
            html.Append("<table><tr>");
            foreach (string h in headers)
                html.Append($"<th>{HttpUtility.HtmlEncode(h)}</th>");
            html.Append("</tr>\n");
        }

        public void TableRow(params string[] cells)
        {
            html.Append("<tr>");
            foreach (string c in cells)
                html.Append($"<td>{HttpUtility.HtmlEncode(c)}</td>");
            html.Append("</tr>\n");
        }

        public void EndTable()
        {
            html.Append("</table>\n");
        }

        public string GetHtml()
        {
            return html.ToString() + "</body></html>";
        }

        public void Clear()
        {
            html.Clear();
            WriteHeader();
        }
    }
}
