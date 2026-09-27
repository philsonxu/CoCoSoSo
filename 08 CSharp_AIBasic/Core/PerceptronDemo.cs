using System;
using System.Drawing;

namespace CSharp20AI
{
    class PerceptronDemo : DemoBase
    {
        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("感知机（Perceptron）是1957年由Rosenblatt提出的最简单的人工神经网络模型，是二分类的线性分类模型，也是现代深度学习的基础。它接收多个输入信号，输出一个二值结果（0或1，-1或+1）。");
            c.P("感知机的数学模型非常简单：对于输入向量x，计算加权和z = w·x + b，然后通过阶跃激活函数输出结果：如果z>0则输出1，否则输出0。");
            c.P("学习规则：使用误分类点驱动的梯度下降，每次发现分类错误，就按学习率调整权重w和偏置b，直到所有样本都分类正确。");
            c.Code("感知机核心更新规则：\nif y * (w*x + b) <= 0  // 误分类\n{\n    w = w + lr * y * x;\n    b = b + lr * y;\n}");

            c.H2("核心代码实现");
            c.Code(
@"class Perceptron
{
    public double w;   // 权重
    public double b;   // 偏置
    public double lr;  // 学习率

    public Perceptron(double lr)
    {
        this.lr = lr;
        this.w = 0;
        this.b = 0;
    }

    // 预测函数：阶跃激活
    public int Predict(double x)
    {
        double z = w * x + b;
        return z >= 0 ? 1 : -1;
    }

    // 训练：单样本更新
    public void Update(double x, int y)
    {
        if (y * (w * x + b) <= 0)  // 误分类
        {
            w = w + lr * y * x;
            b = b + lr * y;
        }
    }
}");

            c.H2("运行演示：线性二分类");
            c.P("我们生成两类可线性分离的一维数据点，使用感知机学习分类边界，观察收敛过程：");

            // 生成演示数据：正样本集中在x=2~4，负样本集中在x=-4~-2
            double[] positiveX = new double[] { 2.1, 2.8, 3.3, 3.9, 2.5, 3.0, 3.7 };
            int[] positiveY = new int[] { 1, 1, 1, 1, 1, 1, 1 };
            double[] negativeX = new double[] { -3.8, -3.2, -2.7, -2.1, -3.5, -2.9, -2.3 };
            int[] negativeY = new int[] { -1, -1, -1, -1, -1, -1, -1 };

            int totalCount = positiveX.Length + negativeX.Length;
            double[,] points = new double[totalCount, 2];
            int[] labels = new int[totalCount];
            int idx = 0;
            for (int i = 0; i < positiveX.Length; i++)
            {
                points[idx, 0] = positiveX[i];
                points[idx, 1] = 0;
                labels[idx] = positiveY[i] == 1 ? 0 : 1;
                idx++;
            }
            for (int i = 0; i < negativeX.Length; i++)
            {
                points[idx, 0] = negativeX[i];
                points[idx, 1] = 0;
                labels[idx] = negativeY[i] == 1 ? 0 : 1;
                idx++;
            }

            // 训练感知机
            double w = 0, b = 0;
            double lr = 0.1;
            int epoch = 20;
            Random rand = new Random(42);
            c.Result("开始训练感知机，学习率=0.1");
            for (int e = 0; e < epoch; e++)
            {
                int error = 0;
                // 打乱顺序
                for (int i = totalCount - 1; i > 0; i--)
                {
                    int j = rand.Next(i + 1);
                    double tx = points[i, 0]; points[i, 0] = points[j, 0]; points[j, 0] = tx;
                    int ty = labels[i]; labels[i] = labels[j]; labels[j] = ty;
                }
                for (int i = 0; i < totalCount; i++)
                {
                    double x = points[i, 0];
                    int y = labels[i] == 0 ? 1 : -1;
                    if (y * (w * x + b) <= 0)
                    {
                        w = w + lr * y * x;
                        b = b + lr * y;
                        error++;
                    }
                }
                if (error == 0)
                {
                    c.Result(string.Format("第{0}轮：所有样本分类正确，训练收敛！最终w={1:F3}, b={2:F3}", e + 1, w, b));
                    break;
                }
                if (e % 5 == 0)
                {
                    c.Result(string.Format("第{0}轮：误分类数={1}, 当前w={2:F3}, b={3:F3}", e + 1, error, w, b));
                }
            }

            double boundary = -b / w;
            c.Success(string.Format("学习到的分类边界：x = {0:F3}，当x > {0:F3}时预测为正类，x < {0:F3}时预测为负类", boundary));

            // 绘制可视化图
            Bitmap bmp = new Bitmap(600, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                Pen axisPen = new Pen(Color.Black, 1.5f);
                g.DrawLine(axisPen, 30, 100, 570, 100);
                // 画刻度
                using (Font f = new Font("Arial", 9f))
                {
                    for (double x = -5; x <= 5; x += 1)
                    {
                        int px = 30 + (int)((x + 5) / 10 * 540);
                        g.DrawLine(Pens.Gray, px, 95, px, 105);
                        g.DrawString(x.ToString("F0"), f, Brushes.Gray, px - 8, 110);
                    }
                }
                // 画边界线
                int bx = 30 + (int)((boundary + 5) / 10 * 540);
                g.DrawLine(new Pen(Color.Red, 2f), bx, 20, bx, 180);
                g.DrawString("分类边界", new Font("Microsoft YaHei", 9f), Brushes.Red, bx + 3, 20);
                // 画点
                for (int i = 0; i < positiveX.Length; i++)
                {
                    int px = 30 + (int)((positiveX[i] + 5) / 10 * 540);
                    g.FillEllipse(Brushes.DodgerBlue, px - 5, 95, 10, 10);
                    g.DrawEllipse(Pens.Black, px - 5, 95, 10, 10);
                }
                for (int i = 0; i < negativeX.Length; i++)
                {
                    int px = 30 + (int)((negativeX[i] + 5) / 10 * 540);
                    g.FillEllipse(Brushes.Tomato, px - 5, 95, 10, 10);
                    g.DrawEllipse(Pens.Black, px - 5, 95, 10, 10);
                }
                axisPen.Dispose();
            }
            c.H3("可视化结果");
            c.Image(bmp);
            c.Tip("蓝色点为正类，红色点为负类，红线为感知机学到的分类边界，可以看到感知机成功找到了线性可分数据的分界线。");
            c.H2("总结");
            c.P("感知机是最简单的线性分类模型，只能解决线性可分问题；对于非线性可分问题（如异或问题），单层感知机无法解决，需要多层感知机（神经网络）。");
            c.P("感知机的学习算法是在线学习、错误驱动的，实现简单，是神经网络和支持向量机的基础。");
        }
    }
}
