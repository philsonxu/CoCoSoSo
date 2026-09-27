using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharp20Tutorial
{
    /// <summary>
    /// HTML控制台输出类：替代传统Console，将输出拼接为HTML格式
    /// 严格使用C# 2.0语法，无任何高版本特性
    /// </summary>
    public sealed class HtmlConsole
    {
        private StringBuilder _builder;

        public HtmlConsole()
        {
            _builder = new StringBuilder();
            WriteHeader();
        }

        /// <summary>
        /// 写入HTML头部与样式
        /// </summary>
        private void WriteHeader()
        {
            _builder.Append("<!DOCTYPE html>");
            _builder.Append("<html><head>");
            _builder.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            _builder.Append("<style type=\"text/css\">");
            _builder.Append("body { font-family: '微软雅黑', sans-serif; margin: 16px; line-height: 1.6; color: #333; background: #fff; }");
            _builder.Append("h1 { color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px; font-size: 24px; }");
            _builder.Append("h2 { color: #2980b9; border-left: 5px solid #3498db; padding-left: 10px; margin-top: 24px; font-size: 20px; }");
            _builder.Append("h3 { color: #27ae60; margin-top: 18px; font-size: 16px; }");
            _builder.Append(".code-block { background: #f8f9fa; border: 1px solid #e9ecef; border-left: 4px solid #3498db; padding: 12px 16px; font-family: Consolas, 'Courier New', monospace; font-size: 13px; margin: 12px 0; white-space: pre-wrap; overflow-x: auto; }");
            _builder.Append(".output-block { background: #2d3436; color: #dfe6e9; padding: 12px 16px; border-radius: 4px; font-family: Consolas, 'Courier New', monospace; font-size: 13px; margin: 12px 0; white-space: pre-wrap; }");
            _builder.Append(".tip { background: #e3f2fd; border-left: 4px solid #2196f3; padding: 10px 14px; margin: 12px 0; border-radius: 0 4px 4px 0; }");
            _builder.Append(".note { background: #fff3cd; border-left: 4px solid #ffc107; padding: 10px 14px; margin: 12px 0; border-radius: 0 4px 4px 0; }");
            _builder.Append(".success { background: #d4edda; border-left: 4px solid #28a745; padding: 10px 14px; margin: 12px 0; border-radius: 0 4px 4px 0; }");
            _builder.Append(".error { background: #f8d7da; border-left: 4px solid #dc3545; padding: 10px 14px; margin: 12px 0; border-radius: 0 4px 4px 0; }");
            _builder.Append("table { border-collapse: collapse; width: 90%; margin: 12px 0; }");
            _builder.Append("th, td { border: 1px solid #dee2e6; padding: 8px 12px; text-align: left; }");
            _builder.Append("th { background: #e9ecef; font-weight: bold; }");
            _builder.Append("tr:nth-child(even) { background: #f8f9fa; }");
            _builder.Append(".divider { border: none; border-top: 1px dashed #ccc; margin: 20px 0; }");
            _builder.Append("p { margin: 8px 0; }");
            _builder.Append("</style></head><body>");
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

        /// <summary>
        /// 输出一级标题
        /// </summary>
        public void WriteTitle(string text)
        {
            _builder.AppendFormat("<h1>{0}</h1>", Escape(text));
        }

        /// <summary>
        /// 输出二级标题
        /// </summary>
        public void WriteSection(string text)
        {
            _builder.AppendFormat("<h2>{0}</h2>", Escape(text));
        }

        /// <summary>
        /// 输出三级标题
        /// </summary>
        public void WriteSubSection(string text)
        {
            _builder.AppendFormat("<h3>{0}</h3>", Escape(text));
        }

        /// <summary>
        /// 输出普通段落
        /// </summary>
        public void WriteParagraph(string text)
        {
            _builder.AppendFormat("<p>{0}</p>", Escape(text));
        }

        /// <summary>
        /// 输出代码块（源代码展示）
        /// </summary>
        public void WriteCode(string code)
        {
            _builder.Append("<div class=\"code-block\">");
            _builder.Append(Escape(code));
            _builder.Append("</div>");
        }

        /// <summary>
        /// 输出运行结果块（深色背景）
        /// </summary>
        public void WriteOutput(string text)
        {
            _builder.Append("<div class=\"output-block\">");
            _builder.Append(Escape(text));
            _builder.Append("</div>");
        }

        /// <summary>
        /// 输出一行文本（不换行）
        /// </summary>
        public void Write(string text)
        {
            _builder.Append(Escape(text));
        }

        /// <summary>
        /// 输出一行文本（换行）
        /// </summary>
        public void WriteLine(string text)
        {
            _builder.Append(Escape(text));
            _builder.Append("<br/>");
        }

        /// <summary>
        /// 输出空行
        /// </summary>
        public void WriteLine()
        {
            _builder.Append("<br/>");
        }

        /// <summary>
        /// 输出提示框（蓝色）
        /// </summary>
        public void WriteTip(string text)
        {
            _builder.AppendFormat("<div class=\"tip\">💡 {0}</div>", Escape(text));
        }

        /// <summary>
        /// 输出注意框（黄色）
        /// </summary>
        public void WriteNote(string text)
        {
            _builder.AppendFormat("<div class=\"note\">⚠️ {0}</div>", Escape(text));
        }

        /// <summary>
        /// 输出成功提示（绿色）
        /// </summary>
        public void WriteSuccess(string text)
        {
            _builder.AppendFormat("<div class=\"success\">✅ {0}</div>", Escape(text));
        }

        /// <summary>
        /// 输出错误提示（红色）
        /// </summary>
        public void WriteError(string text)
        {
            _builder.AppendFormat("<div class=\"error\">❌ {0}</div>", Escape(text));
        }

        /// <summary>
        /// 输出分隔线
        /// </summary>
        public void WriteDivider()
        {
            _builder.Append("<hr class=\"divider\"/>");
        }

        /// <summary>
        /// 开始输出表格
        /// </summary>
        public void WriteTableStart()
        {
            _builder.Append("<table>");
        }

        /// <summary>
        /// 输出表格表头行
        /// </summary>
        public void WriteTableHeader(params string[] headers)
        {
            _builder.Append("<tr>");
            foreach (string h in headers)
            {
                _builder.AppendFormat("<th>{0}</th>", Escape(h));
            }
            _builder.Append("</tr>");
        }

        /// <summary>
        /// 输出表格数据行
        /// </summary>
        public void WriteTableRow(params string[] cells)
        {
            _builder.Append("<tr>");
            foreach (string c in cells)
            {
                _builder.AppendFormat("<td>{0}</td>", Escape(c));
            }
            _builder.Append("</tr>");
        }

        /// <summary>
        /// 结束表格输出
        /// </summary>
        public void WriteTableEnd()
        {
            _builder.Append("</table>");
        }

        /// <summary>
        /// 获取最终拼接的完整HTML
        /// </summary>
        public string GetHtml()
        {
            _builder.Append("</body></html>");
            string result = _builder.ToString();
            _builder.Length = 0;
            WriteHeader();
            return result;
        }
    }
}
