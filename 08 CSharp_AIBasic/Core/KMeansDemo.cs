using System;
using System.Drawing;
using System.Collections.Generic;

namespace CSharp20AI
{
    class KMeansDemo : DemoBase
    {
        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("K-Means是最经典的无监督聚类算法，不需要标注数据，自动将相似的样本聚成K个簇。核心思想是：簇内样本距离尽量近，簇间距离尽量远。");
            c.P("算法步骤非常简单，迭代执行直到收敛：");
            c.Code("K-Means迭代步骤：\n1. 随机选择K个点作为初始簇中心\n2. 分配：每个样本分配给离它最近的簇中心\n3. 更新：重新计算每个簇的中心（簇内所有点的均值）\n4. 重复步骤2-3，直到簇中心不再变化或达到最大迭代次数");
            c.Warning("注意：K-Means对初始中心敏感，可能收敛到局部最优；实际应用中通常多次运行取最好结果；需要预先指定K值。");

            c.H2("核心代码实现");
            c.Code(
@"// K-Means聚类
void KMeans(double[,] points, int k, int[] labels, double[,] centers)
{
    int n = points.GetLength(0);
    Random rand = new Random();
    // 1. 随机初始化簇中心
    for (int i = 0; i < k; i++)
    {
        int idx = rand.Next(n);
        centers[i,0] = points[idx,0];
        centers[i,1] = points[idx,1];
    }
    for (int iter = 0; iter < 100; iter++)
    {
        bool changed = false;
        // 2. 分配样本到最近中心
        for (int i = 0; i < n; i++)
        {
            double minD = double.MaxValue;
            int minC = 0;
            for (int j = 0; j < k; j++)
            {
                double dx = points[i,0]-centers[j,0];
                double dy = points[i,1]-centers[j,1];
                double d = dx*dx + dy*dy;
                if (d < minD) { minD = d; minC = j; }
            }
            if (labels[i] != minC) { labels[i] = minC; changed = true; }
        }
        // 3. 更新簇中心为均值
        double[,] sum = new double[k,2];
        int[] cnt = new int[k];
        for (int i = 0; i < n; i++)
        {
            sum[labels[i],0] += points[i,0];
            sum[labels[i],1] += points[i,1];
            cnt[labels[i]]++;
        }
        for (int j = 0; j < k; j++)
        {
            if (cnt[j] > 0)
            {
                centers[j,0] = sum[j,0]/cnt[j];
                centers[j,1] = sum[j,1]/cnt[j];
            }
        }
        if (!changed) break; // 收敛
    }
}");

            c.H2("运行演示：自动聚成3类");
            c.P("我们生成3簇不同位置的二维点，不告诉算法标签，让K-Means自动把它们聚成3类。");
            Random rand = new Random(123);
            int n = 150;
            int k = 3;
            double[,] points = new double[n,2];
            // 生成三个簇的点
            for (int i = 0; i < 50; i++)
            {
                points[i,0] = -3 + (rand.NextDouble()*2-1)*1.2;
                points[i,1] = 3 + (rand.NextDouble()*2-1)*1.2;
            }
            for (int i = 50; i < 100; i++)
            {
                points[i,0] = 3 + (rand.NextDouble()*2-1)*1.2;
                points[i,1] = 3 + (rand.NextDouble()*2-1)*1.2;
            }
            for (int i = 100; i < 150; i++)
            {
                points[i,0] = 0 + (rand.NextDouble()*2-1)*1.2;
                points[i,1] = -3 + (rand.NextDouble()*2-1)*1.2;
            }

            int[] labels = new int[n];
            double[,] centers = new double[k,2];
            // 运行K-Means
            {
                for (int i = 0; i < k; i++)
                {
                    int idx = rand.Next(n);
                    centers[i,0] = points[idx,0];
                    centers[i,1] = points[idx,1];
                }
                int iter = 0;
                for (iter = 0; iter < 100; iter++)
                {
                    bool changed = false;
                    for (int i = 0; i < n; i++)
                    {
                        double minD = double.MaxValue;
                        int minC = 0;
                        for (int j = 0; j < k; j++)
                        {
                            double dx = points[i,0] - centers[j,0];
                            double dy = points[i,1] - centers[j,1];
                            double d = dx*dx + dy*dy;
                            if (d < minD) { minD = d; minC = j; }
                        }
                        if (labels[i] != minC) { labels[i] = minC; changed = true; }
                    }
                    double[,] sum = new double[k,2];
                    int[] cnt = new int[k];
                    for (int i = 0; i < n; i++)
                    {
                        sum[labels[i],0] += points[i,0];
                        sum[labels[i],1] += points[i,1];
                        cnt[labels[i]]++;
                    }
                    for (int j = 0; j < k; j++)
                    {
                        if (cnt[j] > 0)
                        {
                            centers[j,0] = sum[j,0]/cnt[j];
                            centers[j,1] = sum[j,1]/cnt[j];
                        }
                    }
                    if (!changed) break;
                }
                c.Result(string.Format("K-Means迭代{0}次后收敛", iter+1));
                for (int j = 0; j < k; j++)
                {
                    c.Result(string.Format("簇{0}中心位置：({1:F2}, {2:F2})", j, centers[j,0], centers[j,1]));
                }
            }
            c.Success("可以看到K-Means成功自动找到了我们预设的三个簇的中心位置。");

            // 绘制结果图
            Bitmap bmp = new Bitmap(600, 500);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                int marginL = 50, marginR = 30, marginT = 30, marginB = 50;
                int plotW = 600 - marginL - marginR;
                int plotH = 500 - marginT - marginB;
                double minX = -5, maxX = 5, minY = -5, maxY = 5;

                using (Font f = new Font("Arial", 9f))
                using (Brush tb = Brushes.Black)
                {
                    g.DrawLine(Pens.Black, marginL, marginT, marginL, marginT+plotH);
                    g.DrawLine(Pens.Black, marginL, marginT+plotH, marginL+plotW, marginT+plotH);
                    for (double x = -4; x <= 4; x += 1)
                    {
                        int px = marginL + (int)((x-minX)/(maxX-minX)*plotW);
                        g.DrawLine(Pens.LightGray, px, marginT, px, marginT+plotH);
                        g.DrawString(x.ToString(), f, tb, px-5, marginT+plotH+5);
                    }
                    for (double yv = -4; yv <= 4; yv += 1)
                    {
                        int py = marginT + plotH - (int)((yv-minY)/(maxY-minY)*plotH);
                        g.DrawLine(Pens.LightGray, marginL, py, marginL+plotW, py);
                        g.DrawString(yv.ToString(), f, tb, 10, py-7);
                    }
                }

                // 画样本点，按簇着色
                Brush[] brushes = new Brush[] { Brushes.DodgerBlue, Brushes.Tomato, Brushes.LimeGreen };
                for (int i = 0; i < n; i++)
                {
                    int px = marginL + (int)((points[i,0]-minX)/(maxX-minX)*plotW);
                    int py = marginT + plotH - (int)((points[i,1]-minY)/(maxY-minY)*plotH);
                    g.FillEllipse(brushes[labels[i]], px-4, py-4, 8, 8);
                }

                // 画簇中心，大十字标记
                Pen centerPen = new Pen(Color.Black, 2.5f);
                for (int j = 0; j < k; j++)
                {
                    int px = marginL + (int)((centers[j,0]-minX)/(maxX-minX)*plotW);
                    int py = marginT + plotH - (int)((centers[j,1]-minY)/(maxY-minY)*plotH);
                    g.DrawLine(centerPen, px-10, py, px+10, py);
                    g.DrawLine(centerPen, px, py-10, px, py+10);
                    g.FillEllipse(Brushes.Yellow, px-6, py-6, 12, 12);
                    g.DrawEllipse(Pens.Black, px-6, py-6, 12, 12);
                }
                centerPen.Dispose();
            }
            c.H3("可视化结果");
            c.Image(bmp);
            c.Tip("三种颜色的点是K-Means自动划分的三个簇，黄底黑十字是算法找到的簇中心，可以看到三类点被完美分开，中心位置非常准确。");
            c.H2("总结");
            c.P("K-Means是工业界最常用的聚类算法，优点是原理简单、收敛速度快、聚类效果好；适合簇形状为球形、簇大小相近的数据；广泛用于用户分群、图像分割、异常检测、数据压缩等场景。");
        }
    }
}
