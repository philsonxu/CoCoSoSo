using System;
using System.Drawing;

namespace CSharp20AI
{
    class LinearRegressionDemo : DemoBase
    {
        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("线性回归是回归问题中最基础、最经典的算法，用于拟合自变量x和因变量y之间的线性关系y = wx + b，找到最优的w（斜率）和b（截距），使得所有样本点到直线的误差平方和最小（最小二乘法）。");
            c.P("最小二乘法有解析解，不需要迭代训练，可以直接通过数学公式计算出最优w和b：");
            c.Code("w = (nΣxy - ΣxΣy) / (nΣx² - (Σx)²)\nb = (Σy - wΣx) / n\n其中n是样本数量，Σ表示对所有样本求和。");

            c.H2("核心代码实现");
            c.Code(
@"public static void LinearLeastSquares(double[] x, double[] y, out double w, out double b)
{
    int n = x.Length;
    double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
    for (int i = 0; i < n; i++)
    {
        sumX += x[i];
        sumY += y[i];
        sumXY += x[i] * y[i];
        sumX2 += x[i] * x[i];
    }
    w = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
    b = (sumY - w * sumX) / n;
}");

            c.H2("运行演示：房屋面积与价格预测");
            c.P("我们使用模拟的房屋面积-价格数据，拟合出价格和面积的线性关系，就可以根据面积预测房价。");

            // 模拟数据：面积(平方米)，价格(万元)
            double[] areas = new double[] { 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };
            double[] prices = new double[] { 105, 122, 142, 158, 180, 195, 220, 235, 260, 275, 298 };

            c.H3("原始数据");
            string[] headers = new string[] { "面积(㎡)", "价格(万元)" };
            string[,] rows = new string[areas.Length, 2];
            for (int i = 0; i < areas.Length; i++)
            {
                rows[i, 0] = areas[i].ToString();
                rows[i, 1] = prices[i].ToString();
            }
            c.Table(headers, rows);

            double w, b;
            // 计算最小二乘
            int n = areas.Length;
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
            for (int i = 0; i < n; i++)
            {
                sumX += areas[i];
                sumY += prices[i];
                sumXY += areas[i] * prices[i];
                sumX2 += areas[i] * areas[i];
            }
            w = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            b = (sumY - w * sumX) / n;

            c.Result(string.Format("拟合结果：价格 = {0:F3} * 面积 + {1:F3}", w, b));
            c.Success(string.Format("即每增加1平方米，房价平均上涨约{0:F2}万元", w));

            // 预测测试
            double testArea = 125;
            double predPrice = w * testArea + b;
            c.Result(string.Format("预测：{0}平方米的房屋价格约为 {1:F1} 万元", testArea, predPrice));
            testArea = 160;
            predPrice = w * testArea + b;
            c.Result(string.Format("预测：{0}平方米的房屋价格约为 {1:F1} 万元", testArea, predPrice));

            // 计算R²决定系数，评估拟合优度
            double meanY = sumY / n;
            double ssTot = 0, ssRes = 0;
            for (int i = 0; i < n; i++)
            {
                double pred = w * areas[i] + b;
                ssRes += (prices[i] - pred) * (prices[i] - pred);
                ssTot += (prices[i] - meanY) * (prices[i] - meanY);
            }
            double r2 = 1 - ssRes / ssTot;
            c.Success(string.Format("拟合优度R² = {0:F4}，非常接近1，说明线性拟合效果很好。", r2));

            // 绘制拟合图
            Bitmap bmp = new Bitmap(600, 400);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                // 坐标轴
                int marginL = 60, marginR = 30, marginT = 30, marginB = 50;
                int plotW = 600 - marginL - marginR;
                int plotH = 400 - marginT - marginB;
                Pen axisPen = new Pen(Color.Black, 1.5f);
                g.DrawLine(axisPen, marginL, marginT, marginL, marginT + plotH);
                g.DrawLine(axisPen, marginL, marginT + plotH, marginL + plotW, marginT + plotH);

                double minX = 40, maxX = 160;
                double minY = 80, maxY = 320;
                using (Font f = new Font("Arial", 9f))
                using (Brush tb = Brushes.Black)
                {
                    // X轴刻度
                    for (double x = 50; x <= 150; x += 20)
                    {
                        int px = marginL + (int)((x - minX) / (maxX - minX) * plotW);
                        g.DrawLine(Pens.Gray, px, marginT, px, marginT + plotH);
                        g.DrawString(x.ToString() + "㎡", f, tb, px - 15, marginT + plotH + 5);
                    }
                    // Y轴刻度
                    for (double y = 100; y <= 300; y += 50)
                    {
                        int py = marginT + plotH - (int)((y - minY) / (maxY - minY) * plotH);
                        g.DrawLine(Pens.Gray, marginL, py, marginL + plotW, py);
                        g.DrawString(y.ToString() + "万", f, tb, 5, py - 7);
                    }
                    g.DrawString("房屋面积", f, tb, marginL + plotW / 2 - 20, marginT + plotH + 25);
                    g.DrawString("房屋价格", f, tb, 5, marginT + plotH / 2);
                }

                // 画拟合直线
                Pen linePen = new Pen(Color.Red, 2f);
                int x1 = marginL;
                int y1 = marginT + plotH - (int)((w * minX + b - minY) / (maxY - minY) * plotH);
                int x2 = marginL + plotW;
                int y2 = marginT + plotH - (int)((w * maxX + b - minY) / (maxY - minY) * plotH);
                g.DrawLine(linePen, x1, y1, x2, y2);

                // 画数据点
                for (int i = 0; i < n; i++)
                {
                    int px = marginL + (int)((areas[i] - minX) / (maxX - minX) * plotW);
                    int py = marginT + plotH - (int)((prices[i] - minY) / (maxY - minY) * plotH);
                    g.FillEllipse(Brushes.DodgerBlue, px - 5, py - 5, 10, 10);
                    g.DrawEllipse(Pens.Black, px - 5, py - 5, 10, 10);
                }
                axisPen.Dispose();
                linePen.Dispose();
            }
            c.H3("可视化结果");
            c.Image(bmp);
            c.Tip("蓝色点是原始数据，红色直线是最小二乘法拟合出来的线性回归直线，可以看到直线很好地穿过了所有数据点的中心趋势。");
            c.H2("总结");
            c.P("线性回归是回归任务的基础算法，最小二乘法解析解计算简单高效；当数据存在线性关系时效果很好，是很多其他算法的基础。");
            c.Warning("注意：线性回归对异常值敏感，如果数据中有离群点会显著影响拟合结果；实际应用中需要先清洗数据。");
        }
    }
}
