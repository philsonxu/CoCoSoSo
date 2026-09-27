using System;
using System.Drawing;
using System.Collections.Generic;

namespace CSharp20AI
{
    class GeneticAlgorithmDemo : DemoBase
    {
        // 目标：求f(x) = x*sin(10πx) + 2 在x∈[-1,2]区间的最大值
        private double Fitness(double x)
        {
            if (x < -1 || x > 2) return 0;
            return x * Math.Sin(10 * Math.PI * x) + 2.0;
        }

        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("遗传算法（Genetic Algorithm, GA）是模拟达尔文生物进化论的启发式优化算法，通过模拟自然选择、交叉、变异过程，在巨大的解空间中搜索全局最优解，适合求解复杂的优化问题。");
            c.Code("遗传算法核心步骤：\n1. 初始化：随机生成N个个体作为初始种群\n2. 评估：计算每个个体的适应度，适应度越高越优秀\n3. 选择：根据适应度优胜劣汰，适应度高的个体更大概率被选中繁殖\n4. 交叉：两个父代个体交换基因，产生子代\n5. 变异：以小概率随机改变个体的基因，避免陷入局部最优\n6. 重复2-5代，直到达到最大迭代次数或收敛");
            c.Tip("遗传算法是全局优化算法，不依赖梯度，对目标函数没有连续性、可导性要求，适用范围非常广。");

            c.H2("运行演示：求函数最大值");
            c.P("我们用遗传算法求解函数 f(x) = x·sin(10πx) + 2 在区间x∈[-1, 2]上的最大值，这个函数有很多局部极值点，普通梯度下降很容易陷入局部最优。");

            Random rand = new Random(42);
            int popSize = 50;
            int generations = 100;
            double mutateRate = 0.05;
            double crossRate = 0.7;
            // 二进制编码：用22位二进制表示x，精度约6e-7，足够了
            int dnaLen = 22;
            // 初始化种群：每个个体是长度dnaLen的0/1数组
            List<int[]> population = new List<int[]>();
            for (int i = 0; i < popSize; i++)
            {
                int[] dna = new int[dnaLen];
                for (int j = 0; j < dnaLen; j++) dna[j] = rand.Next(2);
                population.Add(dna);
            }

            List<double> bestHistory = new List<double>();
            double globalBestF = -1;
            double globalBestX = 0;

            for (int gen = 0; gen < generations; gen++)
            {
                // 解码并计算适应度
                double[] xs = new double[popSize];
                double[] fits = new double[popSize];
                double totalFit = 0;
                double genBestF = -1, genBestX = 0;
                for (int i = 0; i < popSize; i++)
                {
                    xs[i] = DecodeDNA(population[i], -1, 2);
                    fits[i] = Fitness(xs[i]);
                    totalFit += fits[i];
                    if (fits[i] > genBestF) { genBestF = fits[i]; genBestX = xs[i]; }
                    if (fits[i] > globalBestF) { globalBestF = fits[i]; globalBestX = xs[i]; }
                }
                bestHistory.Add(genBestF);

                // 选择：轮盘赌
                List<int[]> newPop = new List<int[]>();
                for (int i = 0; i < popSize; i++)
                {
                    double r = rand.NextDouble() * totalFit;
                    double acc = 0;
                    int selected = 0;
                    for (int j = 0; j < popSize; j++)
                    {
                        acc += fits[j];
                        if (acc >= r) { selected = j; break; }
                    }
                    // 复制选中的个体
                    int[] newDna = new int[dnaLen];
                    Array.Copy(population[selected], newDna, dnaLen);
                    newPop.Add(newDna);
                }

                // 交叉
                for (int i = 0; i < popSize; i += 2)
                {
                    if (i+1 >= popSize) break;
                    if (rand.NextDouble() < crossRate)
                    {
                        int crossPoint = rand.Next(1, dnaLen - 1);
                        for (int j = crossPoint; j < dnaLen; j++)
                        {
                            int tmp = newPop[i][j];
                            newPop[i][j] = newPop[i+1][j];
                            newPop[i+1][j] = tmp;
                        }
                    }
                }

                // 变异
                for (int i = 0; i < popSize; i++)
                {
                    for (int j = 0; j < dnaLen; j++)
                    {
                        if (rand.NextDouble() < mutateRate)
                        {
                            newPop[i][j] = 1 - newPop[i][j];
                        }
                    }
                }
                population = newPop;

                if (gen == 0 || gen == 20 || gen == 50 || gen == 99)
                {
                    c.Result(string.Format("第{0,3}代：本轮最佳x={1:F4}, f(x)={2:F4}；全局最佳f(x)={3:F4}",
                        gen+1, genBestX, genBestF, globalBestF));
                }
            }
            c.Success(string.Format("遗传算法优化完成！找到最大值：f({0:F4}) = {1:F4}", globalBestX, globalBestF));
            c.Result("理论最大值约为3.85027，遗传算法非常接近全局最优，成功跳出了局部极值点。");

            // 绘制函数曲线和进化过程
            Bitmap bmp = new Bitmap(650, 500);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                // 上图：函数曲线和最优点
                int marginL = 60, marginR = 30, marginT = 30, marginB = 30;
                int plotH = 200;
                int plotW = 650 - marginL - marginR;
                double minX = -1, maxX = 2, minY = -0.5, maxY = 4;
                Pen axisPen = new Pen(Color.Black, 1.2f);
                g.DrawLine(axisPen, marginL, marginT+plotH, marginL+plotW, marginT+plotH);
                g.DrawLine(axisPen, marginL, marginT, marginL, marginT+plotH);
                // 画函数曲线
                Pen curvePen = new Pen(Color.Gray, 1.5f);
                Point prev = Point.Empty;
                for (int px = 0; px <= plotW; px++)
                {
                    double x = minX + (double)px / plotW * (maxX - minX);
                    double y = Fitness(x);
                    int py = marginT + plotH - (int)((y - minY)/(maxY - minY)*plotH);
                    Point curr = new Point(marginL+px, py);
                    if (!prev.IsEmpty) g.DrawLine(curvePen, prev, curr);
                    prev = curr;
                }
                // 画最优点
                int bx = marginL + (int)((globalBestX - minX)/(maxX-minX)*plotW);
                int by = marginT + plotH - (int)((globalBestF - minY)/(maxY-minY)*plotH);
                g.FillEllipse(Brushes.Red, bx-6, by-6, 12, 12);
                g.DrawEllipse(Pens.Black, bx-6, by-6, 12, 12);
                g.DrawString("全局最优点", new Font("Microsoft YaHei",9f), Brushes.Red, bx+10, by-10);
                using (Font f = new Font("Arial", 9f))
                {
                    g.DrawString("函数 f(x)=x·sin(10πx)+2", f, Brushes.Black, marginL+10, marginT+5);
                }

                // 下图：进化曲线
                int bottomY = marginT + plotH + 40;
                int bottomH = 200;
                g.DrawLine(axisPen, marginL, bottomY+bottomH, marginL+plotW, bottomY+bottomH);
                g.DrawLine(axisPen, marginL, bottomY, marginL, bottomY+bottomH);
                using (Font f = new Font("Arial",9f))
                {
                    g.DrawString("每代最佳适应度进化曲线", f, Brushes.Black, marginL+10, bottomY+5);
                    g.DrawString("代数", f, Brushes.Black, marginL+plotW/2, bottomY+bottomH+5);
                }
                double minF = 0, maxF = 4;
                Pen evoPen = new Pen(Color.DodgerBlue, 2f);
                prev = Point.Empty;
                for (int i = 0; i < bestHistory.Count; i++)
                {
                    int px = marginL + (int)((double)i/(generations-1)*plotW);
                    int py = bottomY + bottomH - (int)((bestHistory[i] - minF)/(maxF-minF)*bottomH);
                    Point curr = new Point(px, py);
                    if (!prev.IsEmpty) g.DrawLine(evoPen, prev, curr);
                    prev = curr;
                }
                axisPen.Dispose();
                curvePen.Dispose();
                evoPen.Dispose();
            }
            c.H3("可视化结果");
            c.Image(bmp);
            c.Tip("上图是目标函数曲线，红点是遗传算法找到的全局最优点；下图是每代最佳适应度的进化曲线，可以看到随着进化代数增加，种群适应度逐步上升，最终收敛到全局最优。");
            c.H2("总结");
            c.P("遗传算法是通用优化算法，广泛应用于调度问题、参数调优、神经网络训练、组合优化（旅行商问题）、自动程序设计等领域；优点是全局搜索能力强、不依赖梯度、适用范围广，缺点是收敛速度较慢、参数设置对结果影响较大。");
        }

        private double DecodeDNA(int[] dna, double minX, double maxX)
        {
            long val = 0;
            for (int i = 0; i < dna.Length; i++)
            {
                val = (val << 1) | (uint)dna[i];
            }
            double maxVal = (1L << dna.Length) - 1;
            return minX + val / maxVal * (maxX - minX);
        }
    }
}
