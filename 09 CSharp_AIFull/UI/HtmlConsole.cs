using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CSharp20AIFull.UI
{
    /// <summary>
    /// HTML控制台 替代Console输出 C#2.0语法
    /// </summary>
    public class HtmlConsole
    {
        private StringBuilder _sb;

        public HtmlConsole()
        {
            _sb = new StringBuilder();
            WriteHeader();
        }

        private void WriteHeader()
        {
            _sb.Append("<!DOCTYPE html><html><head>");
            _sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
            _sb.Append("<style>");
            _sb.Append("body{font-family:'Microsoft YaHei',Segoe UI,Arial,sans-serif;background:#1e1e1e;color:#d4d4d4;padding:20px;line-height:1.7;margin:0;}");
            _sb.Append("h1{color:#569cd6;border-bottom:2px solid #569cd6;padding-bottom:10px;margin-top:30px;font-size:24px;}");
            _sb.Append("h2{color:#4ec9b0;margin-top:25px;font-size:20px;}");
            _sb.Append("h3{color:#ce9178;font-size:17px;}");
            _sb.Append(".code{background:#2d2d2d;border:1px solid #3e3e42;border-left:4px solid #569cd6;padding:12px 16px;margin:10px 0;font-family:'Consolas','Courier New',monospace;font-size:13px;color:#dcdcdc;border-radius:0 4px 4px 0;overflow-x:auto;white-space:pre-wrap;}");
            _sb.Append(".output{background:#1a1a2e;border-left:4px solid #c586c0;padding:12px 16px;margin:10px 0;font-family:'Consolas',monospace;font-size:13px;color:#9cdcfe;border-radius:0 4px 4px 0;}");
            _sb.Append(".info{background:#0e3a5f;border-left:4px solid #3794ff;padding:10px 16px;margin:10px 0;border-radius:0 4px 4px 0;}");
            _sb.Append(".success{background:#1a4d2e;border-left:4px solid #4ec9b0;padding:10px 16px;margin:10px 0;border-radius:0 4px 4px 0;}");
            _sb.Append(".warning{background:#4d3800;border-left:4px solid #dcdcaa;padding:10px 16px;margin:10px 0;border-radius:0 4px 4px 0;}");
            _sb.Append(".error{background:#4d1a1a;border-left:4px solid #f48771;padding:10px 16px;margin:10px 0;border-radius:0 4px 4px 0;}");
            _sb.Append("table{border-collapse:collapse;width:100%;margin:15px 0;}");
            _sb.Append("th{background:#2d2d2d;color:#569cd6;padding:10px 15px;border:1px solid #3e3e42;text-align:left;}");
            _sb.Append("td{padding:8px 15px;border:1px solid #3e3e42;}");
            _sb.Append("tr:nth-child(even){background:#252526;}");
            _sb.Append("hr{border:none;border-top:1px solid #3e3e42;margin:25px 0;}");
            _sb.Append("p{margin:8px 0;}");
            _sb.Append(".progress{height:24px;background:#2d2d2d;border-radius:12px;overflow:hidden;margin:10px 0;}");
            _sb.Append(".progress-bar{height:100%;background:linear-gradient(90deg,#4ec9b0,#569cd6);line-height:24px;text-align:center;color:white;font-size:12px;}");
            _sb.Append("svg{display:block;margin:15px auto;background:#0d1117;border-radius:8px;}");
            _sb.Append(".img-container{text-align:center;margin:15px 0;background:#0d1117;padding:10px;border-radius:8px;}");
            _sb.Append("</style></head><body>");
        }

        public void H1(string text) { _sb.Append("<h1>").Append(HtmlEncode(text)).Append("</h1>"); }
        public void H2(string text) { _sb.Append("<h2>").Append(HtmlEncode(text)).Append("</h2>"); }
        public void H3(string text) { _sb.Append("<h3>").Append(HtmlEncode(text)).Append("</h3>"); }
        public void P(string text) { _sb.Append("<p>").Append(HtmlEncode(text)).Append("</p>"); }
        public void Code(string text) { _sb.Append("<div class='code'>").Append(HtmlEncode(text)).Append("</div>"); }
        public void Output(string text) { _sb.Append("<div class='output'>").Append(HtmlEncode(text)).Append("</div>"); }
        public void Info(string text) { _sb.Append("<div class='info'>ℹ️ ").Append(HtmlEncode(text)).Append("</div>"); }
        public void Success(string text) { _sb.Append("<div class='success'>✅ ").Append(HtmlEncode(text)).Append("</div>"); }
        public void Warning(string text) { _sb.Append("<div class='warning'>⚠️ ").Append(HtmlEncode(text)).Append("</div>"); }
        public void Error(string text) { _sb.Append("<div class='error'>❌ ").Append(HtmlEncode(text)).Append("</div>"); }
        public void Hr() { _sb.Append("<hr/>"); }
        public void Br() { _sb.Append("<br/>"); }

        public void Progress(int percent, string text)
        {
            _sb.Append("<div class='progress'><div class='progress-bar' style='width:").Append(percent).Append("%'>")
                .Append(percent).Append("% ").Append(HtmlEncode(text)).Append("</div></div>");
        }

        public void Table(string[] headers, string[][] rows)
        {
            _sb.Append("<table><thead><tr>");
            for (int i = 0; i < headers.Length; i++)
                _sb.Append("<th>").Append(HtmlEncode(headers[i])).Append("</th>");
            _sb.Append("</tr></thead><tbody>");
            for (int r = 0; r < rows.Length; r++)
            {
                _sb.Append("<tr>");
                for (int c = 0; c < rows[r].Length; c++)
                    _sb.Append("<td>").Append(HtmlEncode(rows[r][c])).Append("</td>");
                _sb.Append("</tr>");
            }
            _sb.Append("</tbody></table>");
        }

        public void WriteLine(string text) { _sb.Append(HtmlEncode(text)).Append("<br/>"); }
        public void Write(string text) { _sb.Append(HtmlEncode(text)); }

        public void SvgChart(List<double> data, string title, int width, int height, string color)
        {
            _sb.Append("<h3>").Append(HtmlEncode(title)).Append("</h3>");
            _sb.Append("<svg width='").Append(width).Append("' height='").Append(height).Append("'>");
            if (data.Count < 2) { _sb.Append("</svg>"); return; }
            double minV = data[0], maxV = data[0];
            for (int i = 1; i < data.Count; i++)
            {
                if (data[i] < minV) minV = data[i];
                if (data[i] > maxV) maxV = data[i];
            }
            double range = maxV - minV; if (range == 0) range = 1;
            int padL = 50, padR = 20, padT = 20, padB = 30;
            int plotW = width - padL - padR, plotH = height - padT - padB;
            // 坐标轴
            _sb.Append("<line x1='").Append(padL).Append("' y1='").Append(padT).Append("' x2='").Append(padL).Append("' y2='").Append(padT + plotH).Append("' stroke='#666' stroke-width='1'/>");
            _sb.Append("<line x1='").Append(padL).Append("' y1='").Append(padT + plotH).Append("' x2='").Append(padL + plotW).Append("' y2='").Append(padT + plotH).Append("' stroke='#666' stroke-width='1'/>");
            // 网格
            for (int i = 0; i <= 4; i++)
            {
                int y = padT + (int)(plotH * i / 4.0);
                _sb.Append("<line x1='").Append(padL).Append("' y1='").Append(y).Append("' x2='").Append(padL + plotW).Append("' y2='").Append(y).Append("' stroke='#333' stroke-width='1'/>");
                double v = maxV - range * i / 4.0;
                _sb.Append("<text x='").Append(padL - 5).Append("' y='").Append(y + 4).Append("' fill='#888' font-size='10' text-anchor='end'>").Append(v.ToString("F3")).Append("</text>");
            }
            // 折线
            StringBuilder path = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                int x = padL + (int)(plotW * i / (double)(data.Count - 1));
                int y = padT + plotH - (int)(plotH * (data[i] - minV) / range);
                if (i == 0) path.Append("M").Append(x).Append(",").Append(y);
                else path.Append(" L").Append(x).Append(",").Append(y);
                _sb.Append("<circle cx='").Append(x).Append("' cy='").Append(y).Append("' r='3' fill='").Append(color).Append("'/>");
            }
            _sb.Append("<path d='").Append(path.ToString()).Append("' fill='none' stroke='").Append(color).Append("' stroke-width='2'/>");
            _sb.Append("</svg>");
        }

        public void DigitImage(double[][] img, int label, int predicted, int size)
        {
            int cellSize = size / 28;
            _sb.Append("<div class='img-container'>");
            _sb.Append("<svg width='").Append(size + 20).Append("' height='").Append(size + 30).Append("'>");
            _sb.Append("<text x='").Append((size + 20) / 2).Append("' y='15' fill='#d4d4d4' font-size='12' text-anchor='middle'>真实:").Append(label);
            if (predicted >= 0) _sb.Append(" 预测:").Append(predicted).Append(predicted == label ? " ✅" : " ❌");
            _sb.Append("</text>");
            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    int v = (int)(img[y][x] * 255);
                    _sb.Append("<rect x='").Append(10 + x * cellSize).Append("' y='").Append(20 + y * cellSize)
                        .Append("' width='").Append(cellSize).Append("' height='").Append(cellSize)
                        .Append("' fill='rgb(").Append(v).Append(",").Append(v).Append(",").Append(v).Append(")'/>");
                }
            }
            _sb.Append("</svg></div>");
        }

        public void ConfusionMatrix(int[,] matrix, string[] labels)
        {
            int n = labels.Length;
            _sb.Append("<h3>混淆矩阵</h3>");
            _sb.Append("<table><thead><tr><th>实际\\预测</th>");
            for (int i = 0; i < n; i++) _sb.Append("<th>").Append(labels[i]).Append("</th>");
            _sb.Append("</tr></thead><tbody>");
            for (int i = 0; i < n; i++)
            {
                _sb.Append("<tr><td><b>").Append(labels[i]).Append("</b></td>");
                for (int j = 0; j < n; j++)
                {
                    string bg = i == j ? "#1a4d2e" : "#2d2d2d";
                    _sb.Append("<td style='background:").Append(bg).Append(";text-align:center;'>").Append(matrix[i, j]).Append("</td>");
                }
                _sb.Append("</tr>");
            }
            _sb.Append("</tbody></table>");
        }

        public string GetHtml()
        {
            return _sb.ToString() + "</body></html>";
        }

        public void Clear()
        {
            _sb.Length = 0;
            WriteHeader();
        }

        private string HtmlEncode(string s)
        {
            if (s == null) return "";
            StringBuilder sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '<': sb.Append("&lt;"); break;
                    case '>': sb.Append("&gt;"); break;
                    case '&': sb.Append("&amp;"); break;
                    case '"': sb.Append("&quot;"); break;
                    case '\'': sb.Append("&#39;"); break;
                    case ' ': sb.Append("&nbsp;"); break;
                    case '\n': sb.Append("<br/>"); break;
                    case '\r': break;
                    default: sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
    }
}
