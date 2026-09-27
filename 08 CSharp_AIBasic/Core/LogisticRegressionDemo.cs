using System;
using System.Drawing;

namespace CSharp20AI
{
    class LogisticRegressionDemo : DemoBase
    {
        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("逻辑回归（Logistic Regression）虽然名字带回归，但实际上是经典的二分类算法。它在线性回归的基础上，通过Sigmoid函数将输出压缩到0~1之间，表示样本属于正类的概率。");
            c.P("Sigmoid函数：σ(z) = 1/(1+e^(-z))，当z=0时输出0.5，z越大越接近1，z越小越接近0，天然适合表示概率。");
            c.P("逻辑回归没有解析解，使用梯度下降法迭代训练，最小化交叉熵损失函数，更新权重w和偏置b。");
            c.Code("梯度下降更新规则：\ndw = (1/n)Σx*(σ(wx+b) - y)\ndb = (1/n)Σ(σ(wx+b) - y)\nw = w - lr * dw\nb = b - lr * db");

            c.H2("核心代码实现");
            c.Code(
@"// Sigmoid激活函数
double Sigmoid(double x) { return 1.0/(1.0+Math.Exp(-x)); }

// 训练逻辑回归
for (int iter = 0; iter < maxIter; iter++)
{
    double dw = 0, db = 0;
    for (int i = 0; i < n; i++)
    {
        double z = w * x[i] + b;
        double a = Sigmoid(z);
        dw += x[i] * (a - y[i]);
        db += (a - y[i]);
    }
    dw /= n; db /= n;
    w = w - lr * dw;
    b = b - lr * db;
}");

            c.H2("运行演示：考试通过率预测");
            c.P("根据学生的学习时长预测是否通过考试：学习时间越长，通过概率越高，逻辑回归可以输出通过概率。");

            // 模拟数据：学习时长(小时)，是否通过(0=未通过，1=通过)
            double[] hours = new double[] { 0.5, 0.75, 1.0, 1.25, 1.5, 1.75, 2.0, 2.25, 2.5, 2.75, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0 };
            int[] pass = new int[] { 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1 };
            int n = hours.Length;

            // 梯度下降训练
            double w = 0, b = 0;
            double lr = 0.5;
            int maxIter = 10000;
            for (int iter = 0; iter < maxIter; iter++)
            {
                double dw = 0, db = 0;
                for (int i = 0; i < n; i++)
                {
                    double z = w * hours[i] + b;
                    double a = Sigmoid(z);
                    dw += hours[i] * (a - pass[i]);
                    db += (a - pass[i]);
                }
                dw /= n;
                db /= n;
                w = w - lr * dw;
                b = b - lr * db;
            }
            c.Result(string.Format("训练完成：w={0:F3}, b={1:F3}", w, b));
            double boundary = -b / w;
            c.Success(string.Format("决策边界：学习时长{0:F2}小时，此时通过概率为50%", boundary));

            // 预测测试
            double[] testHours = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
            c.H3("预测结果");
            foreach (double h in testHours)
            {
                double prob = Sigmoid(w * h + b);
                int res = prob >= 0.5 ? 1 : 0;
                c.Result(string.Format("学习{0}小时：通过概率={1:P1}，预测结果={2}", h, prob, res == 1 ? "通过" : "不通过"));
            }

            // 绘制Sigmoid曲线和数据点
            Bitmap bmp = new Bitmap(600, 400);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                int marginL = 60, marginR = 30, marginT = 30, marginB = 50;
                int plotW = 600 - marginL - marginR;
                int plotH = 400 - marginT - marginB;
                Pen axisPen = new Pen(Color.Black, 1.5f);
                g.DrawLine(axisPen, marginL, marginT, marginL, marginT + plotH);
                g.DrawLine(axisPen, marginL, marginT + plotH, marginL + plotW, marginT + plotH);

                double minX = 0, maxX = 7;
                double minY = 0, maxY = 1;
                using (Font f = new Font("Arial", 9f))
                using (Brush tb = Brushes.Black)
                {
                    for (double x = 0; x <= 7; x += 1)
                    {
                        int px = marginL + (int)((x - minX) / (maxX - minX) * plotW);
                        g.DrawLine(Pens.LightGray, px, marginT, px, marginT + plotH);
                        g.DrawString(x.ToString(), f, tb, px - 5, marginT + plotH + 5);
                    }
                    for (double y = 0; y <= 1; y += 0.2)
                    {
                        int py = marginT + plotH - (int)((y - minY) / (maxY - minY) * plotH);
                        g.DrawLine(Pens.LightGray, marginL, py, marginL + plotW, py);
                        g.DrawString(y.ToString("F1"), f, tb, 10, py - 7);
                    }
                    // 50%概率线
                    Pen p50 = new Pen(Color.Gray, 1f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                    int py50 = marginT + plotH - (int)((0.5 - minY) / (maxY - minY) * plotH);
                    g.DrawLine(p50, marginL, py50, marginL + plotW, py50);
                    p50.Dispose();
                    g.DrawString("学习时长(小时)", f, tb, marginL + plotW/2 - 30, marginT + plotH + 25);
                    g.DrawString("通过概率", f, tb, 5, marginT + plotH/2);
                }

                // 画Sigmoid曲线
                Pen curvePen = new Pen(Color.Red, 2f);
                Point prev = Point.Empty;
                for (int px = 0; px <= plotW; px++)
                {
                    double x = minX + (double)px / plotW * (maxX - minX);
                    double y = Sigmoid(w * x + b);
                    int py = marginT + plotH - (int)((y - minY) / (maxY - minY) * plotH);
                    Point curr = new Point(marginL + px, py);
                    if (!prev.IsEmpty) g.DrawLine(curvePen, prev, curr);
                    prev = curr;
                }

                // 画数据点
                for (int i = 0; i < n; i++)
                {
                    int px = marginL + (int)((hours[i] - minX) / (maxX - minX) * plotW);
                    int py = marginT + plotH - (int)((pass[i] - minY) / (maxY - minY) * plotH);
                    Brush brush = pass[i] == 1 ? Brushes.DodgerBlue : Brushes.Tomato;
                    g.FillEllipse(brush, px - 5, py - 5, 10, 10);
                    g.DrawEllipse(Pens.Black, px - 5, py - 5, 10, 10);
                }
                axisPen.Dispose();
                curvePen.Dispose();
            }
            c.H3("可视化结果");
            c.Image(bmp);
            c.Tip("蓝色点是通过考试的学生，红色点是未通过；红色S形曲线是逻辑回归拟合的通过概率曲线，中间虚线是50%概率决策边界。");
            c.H2("总结");
            c.P("逻辑回归是工业界最常用的分类算法之一，优点是：模型简单可解释、输出天然是概率、训练速度快，广泛用于风控、广告点击预测、医疗诊断等场景。");
        }
    }
}
