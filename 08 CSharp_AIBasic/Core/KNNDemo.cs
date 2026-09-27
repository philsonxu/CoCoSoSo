using System;
using System.Drawing;
using System.Collections.Generic;

namespace CSharp20AI
{
    class KNNDemo : DemoBase
    {
        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("K近邻（K-Nearest Neighbors, KNN）是最简单直观的机器学习算法，属于惰性学习法：它不需要训练过程，预测时只需要找到离待预测点最近的K个邻居，用这K个邻居中出现最多的类别作为预测结果。");
            c.P("KNN三要素：K值的选择、距离度量（通常用欧氏距离）、分类决策规则（多数投票）。");
            c.Code("KNN预测步骤：\n1. 计算待预测点与所有训练样本的距离\n2. 找出距离最小的K个样本\n3. 统计这K个样本中最多的类别，作为预测结果");
            c.Warning("注意：K值太小容易受噪声点影响过拟合，K值太大会忽略样本局部特性欠拟合，通常取奇数避免平票。");

            c.H2("核心代码实现");
            c.Code(
@"// 欧氏距离
double Distance(double[] a, double[] b)
{
    double sum = 0;
    for (int i = 0; i < a.Length; i++)
        sum += (a[i]-b[i])*(a[i]-b[i]);
    return Math.Sqrt(sum);
}

// KNN预测
int Predict(List<double[]> X, List<int> y, double[] x, int k)
{
    // 计算所有距离，保存(距离, 标签)
    List<KeyValuePair<double, int>> dists = new List<KeyValuePair<double,int>>();
    for (int i = 0; i < X.Count; i++)
        dists.Add(new KeyValuePair<double,int>(Distance(X[i], x), y[i]));
    // 按距离从小到大排序
    dists.Sort(delegate(KeyValuePair<double,int> a, KeyValuePair<double,int> b)
    {
        return a.Key.CompareTo(b.Key);
    });
    // 统计前K个邻居的类别投票
    int[] votes = new int[10];
    for (int i = 0; i < k; i++) votes[dists[i].Value]++;
    // 找票数最多的类别
    int maxV = 0, res = 0;
    for (int i = 0; i < votes.Length; i++)
        if (votes[i] > maxV) { maxV = votes[i]; res = i; }
    return res;
}");

            c.H2("运行演示：二维点二分类");
            c.P("我们生成两类二维高斯分布点，使用KNN（K=3）对随机生成的测试点进行分类，并绘制散点图观察。");

            Random rand = new Random(42);
            List<double[]> X = new List<double[]>();
            List<int> y = new List<int>();
            // A类：中心(2,2)，B类：中心(-2,-2)
            int perClass = 30;
            for (int i = 0; i < perClass; i++)
            {
                X.Add(new double[] { 2 + (rand.NextDouble()*2-1)*1.5, 2 + (rand.NextDouble()*2-1)*1.5 });
                y.Add(0);
                X.Add(new double[] { -2 + (rand.NextDouble()*2-1)*1.5, -2 + (rand.NextDouble()*2-1)*1.5 });
                y.Add(1);
            }

            // 转换为绘图数组
            int total = X.Count;
            double[,] points = new double[total,2];
            int[] labels = new int[total];
            for (int i = 0; i < total; i++)
            {
                points[i,0] = X[i][0];
                points[i,1] = X[i][1];
                labels[i] = y[i];
            }

            // 生成测试点
            double[][] testPoints = new double[][]
            {
                new double[] {1.5, 1.5},
                new double[] {-1.5, -1.5},
                new double[] {0, 0},
                new double[] {2.5, 0.5},
                new double[] {-0.5, -2.5}
            };
            int k = 3;
            c.H3("预测结果（K=3）");
            int[] testPred = new int[testPoints.Length];
            for (int t = 0; t < testPoints.Length; t++)
            {
                double[] tx = testPoints[t];
                // 计算距离排序
                List<KeyValuePair<double, int>> dists = new List<KeyValuePair<double, int>>();
                for (int i = 0; i < total; i++)
                {
                    double dx = X[i][0] - tx[0];
                    double dy = X[i][1] - tx[1];
                    double d = Math.Sqrt(dx*dx + dy*dy);
                    dists.Add(new KeyValuePair<double, int>(d, y[i]));
                }
                dists.Sort(delegate(KeyValuePair<double, int> a, KeyValuePair<double, int> b) { return a.Key.CompareTo(b.Key); });
                int[] votes = new int[2];
                for (int i = 0; i < k; i++) votes[dists[i].Value]++;
                int pred = votes[0] > votes[1] ? 0 : 1;
                testPred[t] = pred;
                c.Result(string.Format("测试点({0:F1}, {1:F1})：最近3个邻居投票[0类={2},1类={3}]，预测为{4}类",
                    tx[0], tx[1], votes[0], votes[1], pred == 0 ? "A(蓝)" : "B(红)"));
            }

            // 绘制散点图
            Bitmap bmp = new Bitmap(600, 500);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                int marginL = 50, marginR = 30, marginT = 30, marginB = 50;
                int plotW = 600 - marginL - marginR;
                int plotH = 500 - marginT - marginB;
                double minX = -5, maxX = 5, minY = -5, maxY = 5;

                // 网格和坐标轴
                Pen gridPen = new Pen(Color.FromArgb(230,230,230), 1f);
                using (Font f = new Font("Arial", 9f))
                using (Brush tb = Brushes.Black)
                {
                    g.DrawLine(Pens.Black, marginL, marginT, marginL, marginT+plotH);
                    g.DrawLine(Pens.Black, marginL, marginT+plotH, marginL+plotW, marginT+plotH);
                    for (double x = -4; x <= 4; x += 1)
                    {
                        int px = marginL + (int)((x-minX)/(maxX-minX)*plotW);
                        g.DrawLine(gridPen, px, marginT, px, marginT+plotH);
                        g.DrawString(x.ToString(), f, tb, px-5, marginT+plotH+5);
                    }
                    for (double yv = -4; yv <= 4; yv += 1)
                    {
                        int py = marginT + plotH - (int)((yv-minY)/(maxY-minY)*plotH);
                        g.DrawLine(gridPen, marginL, py, marginL+plotW, py);
                        g.DrawString(yv.ToString(), f, tb, 10, py-7);
                    }
                }

                // 画训练点
                Brush[] brushes = new Brush[] { Brushes.DodgerBlue, Brushes.Tomato };
                for (int i = 0; i < total; i++)
                {
                    int px = marginL + (int)((points[i,0]-minX)/(maxX-minX)*plotW);
                    int py = marginT + plotH - (int)((points[i,1]-minY)/(maxY-minY)*plotH);
                    g.FillEllipse(brushes[labels[i]], px-4, py-4, 8, 8);
                }

                // 画测试点，用大星号标记
                for (int t = 0; t < testPoints.Length; t++)
                {
                    int px = marginL + (int)((testPoints[t][0]-minX)/(maxX-minX)*plotW);
                    int py = marginT + plotH - (int)((testPoints[t][1]-minY)/(maxY-minY)*plotH);
                    Brush b = testPred[t] == 0 ? Brushes.Blue : Brushes.Red;
                    g.FillEllipse(b, px-8, py-8, 16, 16);
                    g.DrawEllipse(Pens.Black, px-8, py-8, 16, 16);
                    g.DrawString("?", new Font("Arial", 10f, FontStyle.Bold), Brushes.White, px-4, py-6);
                }
                gridPen.Dispose();
            }
            c.H3("可视化结果");
            c.Image(bmp);
            c.Tip("小圆点是训练样本，蓝色为A类、红色为B类；大黑边圆点是待预测测试点，圆点颜色就是KNN给出的分类结果，可以看到结果符合直觉。");
            c.H2("总结");
            c.P("KNN优点：原理简单易懂、无需训练、对异常值不敏感、适合多分类问题；缺点：预测时需要计算所有样本距离，大数据集速度慢、高维数据效果差（维度灾难）、需要归一化。");
        }
    }
}
