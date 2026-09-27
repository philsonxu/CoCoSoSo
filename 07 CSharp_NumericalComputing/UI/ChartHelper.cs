using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Text;

namespace NumericalComputing.UI
{
    /// <summary>
    /// GDI+绘图辅助类，纯C#2.0原生绘制，零第三方依赖
    /// </summary>
    public class ChartHelper
    {
        /// <summary>
        /// 绘制多条曲线，返回base64 PNG字符串可直接嵌入HTML
        /// </summary>
        public static string PlotLine(double[] xs, double[][] ysSeries, string[] labels, string title, int width = 600, int height = 400)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            int padding = 50;
            int plotW = width - 2 * padding;
            int plotH = height - 2 * padding;

            double xMin = double.MaxValue, xMax = double.MinValue;
            double yMin = double.MaxValue, yMax = double.MinValue;
            for (int i = 0; i < xs.Length; i++)
            {
                if (xs[i] < xMin) xMin = xs[i];
                if (xs[i] > xMax) xMax = xs[i];
            }
            for (int s = 0; s < ysSeries.Length; s++)
            {
                for (int i = 0; i < ysSeries[s].Length; i++)
                {
                    if (ysSeries[s][i] < yMin) yMin = ysSeries[s][i];
                    if (ysSeries[s][i] > yMax) yMax = ysSeries[s][i];
                }
            }
            double yRange = yMax - yMin;
            if (Math.Abs(yRange) < 1e-12) yRange = 1;
            yMin -= yRange * 0.1;
            yMax += yRange * 0.1;
            double xRange = xMax - xMin;
            if (Math.Abs(xRange) < 1e-12) xRange = 1;

            // 绘制坐标轴
            Pen axisPen = new Pen(Color.Black, 1.5f);
            g.DrawLine(axisPen, padding, padding, padding, height - padding);
            g.DrawLine(axisPen, padding, height - padding, width - padding, height - padding);

            // 绘制网格
            Pen gridPen = new Pen(Color.LightGray, 0.5f);
            Font labelFont = new Font("宋体", 9);
            Brush labelBrush = Brushes.Black;
            int gridCount = 5;
            for (int i = 0; i <= gridCount; i++)
            {
                int x = padding + (int)(plotW * i / (double)gridCount);
                int y = height - padding - (int)(plotH * i / (double)gridCount);
                g.DrawLine(gridPen, x, padding, x, height - padding);
                g.DrawLine(gridPen, padding, y, width - padding, y);
                double xVal = xMin + xRange * i / gridCount;
                double yVal = yMin + (yMax - yMin) * i / gridCount;
                g.DrawString(xVal.ToString("F2"), labelFont, labelBrush, x - 15, height - padding + 5);
                g.DrawString(yVal.ToString("F2"), labelFont, labelBrush, 5, y - 7);
            }

            // 标题
            Font titleFont = new Font("宋体", 12, FontStyle.Bold);
            g.DrawString(title, titleFont, Brushes.DarkBlue, width / 2 - title.Length * 8, 10);

            // 绘制曲线
            Color[] colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Purple, Color.Brown };
            Pen[] pens = new Pen[ysSeries.Length];
            for (int s = 0; s < ysSeries.Length; s++)
            {
                pens[s] = new Pen(colors[s % colors.Length], 2f);
            }

            for (int s = 0; s < ysSeries.Length; s++)
            {
                PointF[] points = new PointF[xs.Length];
                for (int i = 0; i < xs.Length; i++)
                {
                    float px = (float)(padding + plotW * (xs[i] - xMin) / xRange);
                    float py = (float)(height - padding - plotH * (ysSeries[s][i] - yMin) / (yMax - yMin));
                    points[i] = new PointF(px, py);
                }
                g.DrawLines(pens[s], points);
            }

            // 图例
            if (labels != null && labels.Length == ysSeries.Length)
            {
                for (int s = 0; s < labels.Length; s++)
                {
                    int ly = padding + s * 20;
                    g.DrawLine(pens[s], width - padding - 100, ly + 8, width - padding - 70, ly + 8);
                    g.DrawString(labels[s], labelFont, Brushes.Black, width - padding - 65, ly);
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                g.Dispose();
                bmp.Dispose();
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// 绘制散点图+拟合曲线
        /// </summary>
        public static string PlotScatterFit(double[] xs, double[] ys, double[] fitXs, double[] fitYs, string title, int width = 600, int height = 400)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            int padding = 50;
            int plotW = width - 2 * padding;
            int plotH = height - 2 * padding;

            double xMin = double.MaxValue, xMax = double.MinValue;
            double yMin = double.MaxValue, yMax = double.MinValue;
            for (int i = 0; i < xs.Length; i++)
            {
                if (xs[i] < xMin) xMin = xs[i];
                if (xs[i] > xMax) xMax = xs[i];
                if (ys[i] < yMin) yMin = ys[i];
                if (ys[i] > yMax) yMax = ys[i];
            }
            for (int i = 0; i < fitXs.Length; i++)
            {
                if (fitXs[i] < xMin) xMin = fitXs[i];
                if (fitXs[i] > xMax) xMax = fitXs[i];
                if (fitYs[i] < yMin) yMin = fitYs[i];
                if (fitYs[i] > yMax) yMax = fitYs[i];
            }
            double yRange = yMax - yMin;
            if (Math.Abs(yRange) < 1e-12) yRange = 1;
            yMin -= yRange * 0.1;
            yMax += yRange * 0.1;
            double xRange = xMax - xMin;
            if (Math.Abs(xRange) < 1e-12) xRange = 1;

            Pen axisPen = new Pen(Color.Black, 1.5f);
            g.DrawLine(axisPen, padding, padding, padding, height - padding);
            g.DrawLine(axisPen, padding, height - padding, width - padding, height - padding);

            Pen gridPen = new Pen(Color.LightGray, 0.5f);
            Font labelFont = new Font("宋体", 9);
            for (int i = 0; i <= 5; i++)
            {
                int x = padding + (int)(plotW * i / 5.0);
                int y = height - padding - (int)(plotH * i / 5.0);
                g.DrawLine(gridPen, x, padding, x, height - padding);
                g.DrawLine(gridPen, padding, y, width - padding, y);
            }

            Font titleFont = new Font("宋体", 12, FontStyle.Bold);
            g.DrawString(title, titleFont, Brushes.DarkBlue, width / 2 - title.Length * 8, 10);

            // 散点
            Brush pointBrush = Brushes.Blue;
            for (int i = 0; i < xs.Length; i++)
            {
                float px = (float)(padding + plotW * (xs[i] - xMin) / xRange);
                float py = (float)(height - padding - plotH * (ys[i] - yMin) / (yMax - yMin));
                g.FillEllipse(pointBrush, px - 3, py - 3, 6, 6);
            }

            // 拟合曲线
            PointF[] points = new PointF[fitXs.Length];
            for (int i = 0; i < fitXs.Length; i++)
            {
                float px = (float)(padding + plotW * (fitXs[i] - xMin) / xRange);
                float py = (float)(height - padding - plotH * (fitYs[i] - yMin) / (yMax - yMin));
                points[i] = new PointF(px, py);
            }
            Pen fitPen = new Pen(Color.Red, 2f);
            g.DrawLines(fitPen, points);

            g.DrawString("● 原始数据点", labelFont, Brushes.Blue, width - padding - 100, padding);
            g.DrawLine(fitPen, width - padding - 100, padding + 20, width - padding - 70, padding + 20);
            g.DrawString("拟合曲线", labelFont, Brushes.Red, width - padding - 65, padding + 13);

            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                g.Dispose();
                bmp.Dispose();
                return Convert.ToBase64String(bytes);
            }
        }
    }
}