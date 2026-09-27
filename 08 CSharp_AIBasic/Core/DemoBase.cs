using System;
using System.Drawing;

namespace CSharp20AI
{
    abstract class DemoBase
    {
        protected string _title;

        public void SetTitle(string title)
        {
            _title = title;
        }

        public string GetTitle()
        {
            return _title;
        }

        public abstract void Run(HtmlConsole c);

        // 工具方法：生成测试用随机二维数据点
        protected double[,] GenerateRandomPoints(int count, double cx, double cy, double spread, int seed)
        {
            Random rand = new Random(seed);
            double[,] points = new double[count, 2];
            for (int i = 0; i < count; i++)
            {
                points[i, 0] = cx + (rand.NextDouble() * 2 - 1) * spread;
                points[i, 1] = cy + (rand.NextDouble() * 2 - 1) * spread;
            }
            return points;
        }

        // 工具方法：绘制散点图和分类区域
        protected Bitmap DrawScatterPlot(double[,] points, int[] labels, int width, int height, 
            double minX, double maxX, double minY, double maxY, bool drawBoundary, double boundaryW, double boundaryB)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);

                // 绘制坐标轴网格
                Pen gridPen = new Pen(Color.FromArgb(220, 220, 220), 1f);
                using (Font font = new Font("Arial", 9f))
                using (Brush textBrush = Brushes.Gray)
                {
                    for (double x = minX; x <= maxX; x += (maxX - minX) / 10)
                    {
                        int px = (int)((x - minX) / (maxX - minX) * width);
                        g.DrawLine(gridPen, px, 0, px, height);
                        g.DrawString(x.ToString("F1"), font, textBrush, px + 2, height - 15);
                    }
                    for (double y = minY; y <= maxY; y += (maxY - minY) / 10)
                    {
                        int py = (int)((maxY - y) / (maxY - minY) * height);
                        g.DrawLine(gridPen, 0, py, width, py);
                        g.DrawString(y.ToString("F1"), font, textBrush, 2, py + 2);
                    }
                }

                // 绘制分类边界线
                if (drawBoundary)
                {
                    Pen linePen = new Pen(Color.Red, 2f);
                    int x1 = 0;
                    int y1 = (int)(height - ((boundaryW * minX + boundaryB) - minY) / (maxY - minY) * height);
                    int x2 = width;
                    int y2 = (int)(height - ((boundaryW * maxX + boundaryB) - minY) / (maxY - minY) * height);
                    g.DrawLine(linePen, x1, y1, x2, y2);
                    linePen.Dispose();
                }

                // 绘制数据点
                Brush[] brushes = new Brush[] { Brushes.DodgerBlue, Brushes.Tomato, Brushes.LimeGreen, Brushes.Orange, Brushes.Purple };
                int pointCount = points.GetLength(0);
                for (int i = 0; i < pointCount; i++)
                {
                    double x = points[i, 0];
                    double y = points[i, 1];
                    int px = (int)((x - minX) / (maxX - minX) * width);
                    int py = (int)((maxY - y) / (maxY - minY) * height);
                    int label = labels[i];
                    if (label < 0) label = 0;
                    if (label >= brushes.Length) label = 0;
                    g.FillEllipse(brushes[label], px - 4, py - 4, 8, 8);
                    g.DrawEllipse(Pens.Black, px - 4, py - 4, 8, 8);
                }

                gridPen.Dispose();
            }
            return bmp;
        }

        protected double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }
    }
}
