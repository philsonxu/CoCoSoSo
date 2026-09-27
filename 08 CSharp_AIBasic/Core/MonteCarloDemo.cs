using System;
using System.Drawing;

namespace CSharp20AI
{
    class MonteCarloDemo : DemoBase
    {
        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("蒙特卡洛方法（Monte Carlo Method）是一大类基于随机采样的数值计算方法的统称，核心思想是：当问题难以用解析方法求解时，通过大量随机试验，用频率估计概率，得到问题的近似解。采样越多，结果越精确。");
            c.P("蒙特卡洛方法是人工智能中非常重要的基础工具，广泛用于近似计算、随机采样、强化学习、贝叶斯推断、近似求解NP难问题等场景。");
            c.Code("经典案例：用蒙特卡洛方法计算圆周率π：\n在边长为2的正方形内做内切圆，面积比是π/4\n随机向正方形内投点，统计落在圆内的点比例k≈π/4，因此π≈4k");

            c.H2("运行演示：随机投点计算π值");
            c.P("我们分别投1000、10000、100000、1000000个点，观察随着采样点增加，π的估计值如何逼近真实值3.14159265...");

            Bitmap bmp = new Bitmap(500, 500);
            int inCircle = 0;
            int totalPoints = 10000; // 可视化用1万个点，太多会卡
            Random rand = new Random(42);
            int[,] points = new int[totalPoints, 3]; // x,y,是否在圆内
            for (int i = 0; i < totalPoints; i++)
            {
                double x = rand.NextDouble() * 2 - 1;
                double y = rand.NextDouble() * 2 - 1;
                double dist = x * x + y * y;
                int inside_idx = dist <= 1 ? 1 : 0;
                points[i, 0] = (int)((x + 1) / 2 * 480) + 10;
                points[i, 1] = (int)((1 - y) / 2 * 480) + 10;
                points[i, 2] = inside_idx;
                inCircle += inside_idx;
            }
            double piEst = 4.0 * inCircle / totalPoints;

            // 绘制投点图
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                // 正方形边框
                g.DrawRectangle(Pens.Black, 10, 10, 480, 480);
                // 内切圆
                g.DrawEllipse(new Pen(Color.Gray, 2f), 10, 10, 480, 480);
                // 画点
                Brush bIn = new SolidBrush(Color.FromArgb(180, Color.DodgerBlue));
                Brush bOut = new SolidBrush(Color.FromArgb(150, Color.Tomato));
                for (int i = 0; i < totalPoints; i++)
                {
                    Brush b = points[i, 2] == 1 ? bIn : bOut;
                    g.FillRectangle(b, points[i, 0] - 1, points[i, 1] - 1, 2, 2);
                }
                bIn.Dispose();
                bOut.Dispose();
                using (Font f = new Font("Microsoft YaHei", 10f))
                {
                    g.DrawString(string.Format("投点总数：{0:N0}", totalPoints), f, Brushes.Black, 20, 20);
                    g.DrawString(string.Format("圆内点数：{0:N0}", inCircle), f, Brushes.DodgerBlue, 20, 40);
                    g.DrawString(string.Format("圆外点数：{0:N0}", totalPoints - inCircle), f, Brushes.Tomato, 20, 60);
                    g.DrawString(string.Format("π估计值：{0:F6}", piEst), f, Brushes.Black, 20, 80);
                    g.DrawString("蓝色点=圆内，红色点=圆外", f, Brushes.Black, 20, 460);
                }
            }
            c.H3("1万点投点可视化");
            c.Image(bmp);

            // 不同采样量对比
            c.H3("不同采样点数的精度对比");
            int[] counts = new int[] { 100, 1000, 10000, 100000, 1000000, 10000000 };
            string[] headers = new string[] { "投点总数", "圆内点数", "π估计值", "绝对误差" };
            string[,] rows = new string[counts.Length, 4];
            Random r2 = new Random(42);
            int total = 0;
            int inside = 0;
            for (int ci = 0; ci < counts.Length; ci++)
            {
                int cnt = counts[ci];
                while (total < cnt)
                {
                    double x = r2.NextDouble() * 2 - 1;
                    double y = r2.NextDouble() * 2 - 1;
                    if (x * x + y * y <= 1) inside++;
                    total++;
                }
                double pi = 4.0 * inside / total;
                double err = Math.Abs(pi - Math.PI);
                rows[ci, 0] = cnt.ToString("N0");
                rows[ci, 1] = inside.ToString("N0");
                rows[ci, 2] = pi.ToString("F7");
                rows[ci, 3] = err.ToString("F7");
            }
            c.Table(headers, rows);
            c.Result("真实π值：3.1415926535...");
            c.Success("可以清晰看到蒙特卡洛方法的特性：采样点越多，估计值越精确，误差逐渐减小；1000万点时误差已经小于0.001。");

            c.H2("其他蒙特卡洛应用演示：求定积分");
            c.P("用蒙特卡洛方法求∫₀¹ x² dx，解析解是1/3≈0.3333333。");
            int sampleCount = 1000000;
            Random r3 = new Random(123);
            double sum = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                double x = r3.NextDouble();
                sum += x * x;
            }
            double integral = sum / sampleCount;
            c.Result(string.Format("蒙特卡洛积分结果：{0:F6}", integral));
            c.Success(string.Format("解析解0.3333333，误差仅{0:F6}，结果非常准确。", Math.Abs(integral - 1.0 / 3)));

            c.H2("总结");
            c.P("蒙特卡洛方法优点：简单通用、维度无关（高维问题依然有效）、容易并行化、对问题要求低；缺点是收敛速度慢（O(1/√N)），想要高精度需要大量采样。");
            c.P("在AI领域，蒙特卡洛树搜索（MCTS）是AlphaGo的核心算法之一，蒙特卡洛采样也是强化学习、概率图模型、近似推断中不可或缺的工具。");
        }
    }
}
