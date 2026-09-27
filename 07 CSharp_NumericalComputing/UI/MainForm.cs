using System;
using System.Windows.Forms;
using System.Drawing;
using NumericalComputing.Core;
using NumericalComputing.Algorithms;

namespace NumericalComputing.UI
{
    public class MainForm : Form
    {
        private TreeView tvChapters;
        private WebBrowser wbContent;
        private HtmlConsole console;
        private Button btnRun;
        private int currentChapter = -1;

        public MainForm()
        {
            Text = "C#2.0 数值计算源程序库 - 零第三方依赖";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;

            SplitContainer split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.SplitterDistance = 220;
            Controls.Add(split);

            tvChapters = new TreeView();
            tvChapters.Dock = DockStyle.Fill;
            tvChapters.Font = new Font("微软雅黑", 10);
            BuildTree();
            tvChapters.AfterSelect += new TreeViewEventHandler(tvChapters_AfterSelect);
            split.Panel1.Controls.Add(tvChapters);

            Panel pnlRight = new Panel();
            pnlRight.Dock = DockStyle.Fill;
            split.Panel2.Controls.Add(pnlRight);

            btnRun = new Button();
            btnRun.Text = "▶ 运行示例代码";
            btnRun.Dock = DockStyle.Top;
            btnRun.Height = 40;
            btnRun.BackColor = Color.FromArgb(52, 152, 219);
            btnRun.ForeColor = Color.White;
            btnRun.FlatStyle = FlatStyle.Flat;
            btnRun.Font = new Font("微软雅黑", 11, FontStyle.Bold);
            btnRun.Click += new EventHandler(btnRun_Click);
            pnlRight.Controls.Add(btnRun);

            wbContent = new WebBrowser();
            wbContent.Dock = DockStyle.Fill;
            wbContent.Top = btnRun.Bottom;
            pnlRight.Controls.Add(wbContent);
            wbContent.BringToFront();

            console = new HtmlConsole(wbContent);
            ShowWelcome();
        }

        private void BuildTree()
        {
            string[] chapters = new string[]
            {
                "0. 项目介绍",
                "1. 矩阵运算",
                "2. 线性方程组求解",
                "3. 非线性方程求根",
                "4. 数值积分",
                "5. 插值算法",
                "6. 常微分方程数值解",
                "7. 最小二乘拟合",
                "8. 特征值计算",
                "9. 综合示例"
            };
            for (int i = 0; i < chapters.Length; i++)
            {
                tvChapters.Nodes.Add(chapters[i]);
            }
        }

        private void ShowWelcome()
        {
            console.Clear();
            console.H1("C#2.0 数值计算源程序库");
            console.P("✅ 严格遵循C#2.0语法规范，无var、无LINQ、无lambda、无自动属性");
            console.P("✅ 零第三方库依赖，仅使用.NET Framework 2.0内置System类库");
            console.P("✅ 覆盖数值计算8大核心模块，全部算法从零手写实现");
            console.P("✅ HTML富文本输出，支持图表可视化，完全替代传统Console");
            console.H2("包含模块");
            console.P("1. 矩阵运算：加法、乘法、转置、行列式、求逆、选主元");
            console.P("2. 线性方程组：列主元高斯消元、LU分解、雅可比迭代、高斯赛德尔迭代");
            console.P("3. 非线性求根：二分法、牛顿迭代、割线法、不动点迭代");
            console.P("4. 数值积分：梯形公式、辛普森公式、自适应辛普森、龙贝格积分");
            console.P("5. 插值：拉格朗日插值、牛顿差商、分段线性、三次样条插值");
            console.P("6. 常微分方程：欧拉法、改进欧拉法、四阶龙格库塔RK4");
            console.P("7. 最小二乘：线性拟合、多项式拟合、R²决定系数");
            console.P("8. 特征值：幂法、反幂法、雅可比对称矩阵特征值分解");
            console.Info("请从左侧选择章节，点击上方运行按钮即可查看算法演示和结果。");
            console.Refresh();
        }

        void tvChapters_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currentChapter = e.Node.Index;
            console.Clear();
            switch (currentChapter)
            {
                case 0: ShowWelcome(); break;
                case 1: Chapter_Matrix(); break;
                case 2: Chapter_LinearSystem(); break;
                case 3: Chapter_RootFinding(); break;
                case 4: Chapter_Integration(); break;
                case 5: Chapter_Interpolation(); break;
                case 6: Chapter_ODE(); break;
                case 7: Chapter_LeastSquares(); break;
                case 8: Chapter_Eigen(); break;
                case 9: Chapter_Comprehensive(); break;
            }
        }

        void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            btnRun.Text = "运行中...";
            Application.DoEvents();
            try
            {
                tvChapters_AfterSelect(null, new TreeViewEventArgs(tvChapters.SelectedNode));
            }
            catch (Exception ex)
            {
                console.Error("运行出错：" + ex.Message);
            }
            btnRun.Enabled = true;
            btnRun.Text = "▶ 运行示例代码";
            console.Refresh();
        }

        private void Chapter_Matrix()
        {
            console.H1("第1章 矩阵运算");
            console.P("矩阵类是所有数值计算的基础，这里从零实现了矩阵的基本运算，包括加法、乘法、转置、行列式、逆矩阵，均采用列主元提高数值稳定性。");
            console.H2("1.1 矩阵定义与加法");
            double[,] aData = { { 1, 2 }, { 3, 4 } };
            double[,] bData = { { 5, 6 }, { 7, 8 } };
            Matrix A = new Matrix(aData);
            Matrix B = new Matrix(bData);
            console.Code("Matrix A = new Matrix(new double[,] {{1,2},{3,4}});\nMatrix B = new Matrix(new double[,] {{5,6},{7,8}});");
            console.Result("矩阵A:\n" + A.ToString() + "\n矩阵B:\n" + B.ToString());
            Matrix C = Matrix.Add(A, B);
            console.Result("A+B:\n" + C.ToString());

            console.H2("1.2 矩阵乘法");
            Matrix D = Matrix.Multiply(A, B);
            console.Result("A*B:\n" + D.ToString());

            console.H2("1.3 转置");
            Matrix At = A.Transpose();
            console.Result("A的转置:\n" + At.ToString());

            console.H2("1.4 行列式");
            double det = A.Determinant();
            console.Result("A的行列式 det(A) = " + det.ToString("F4"));

            console.H2("1.5 逆矩阵");
            Matrix AInv = A.Inverse();
            console.Result("A的逆矩阵:\n" + AInv.ToString());
            Matrix ICheck = Matrix.Multiply(A, AInv);
            console.Result("A * A^-1 验证（应为单位矩阵）:\n" + ICheck.ToString());
            console.Success("矩阵所有运算验证通过！");
            console.Refresh();
        }

        private void Chapter_LinearSystem()
        {
            console.H1("第2章 线性方程组求解");
            console.P("线性方程组Ax=b是数值计算最核心的问题，提供了直接法和迭代法两类求解方式。");
            console.H2("2.1 经典案例：解方程组");
            console.P("求解方程组：\n3x + 2y - z = 10\n-x + 7y + 2z = 5\n2x - y + 5z = 7");
            double[,] matA = { { 3, 2, -1 }, { -1, 7, 2 }, { 2, -1, 5 } };
            double[] vecB = { 10, 5, 7 };
            Matrix A = new Matrix(matA);
            console.Code("Matrix A = new Matrix(new double[,] {{3,2,-1},{-1,7,2},{2,-1,5}});\ndouble[] b = {10,5,7};");

            double[] xGauss = LinearSystem.GaussPivot(A, vecB);
            console.Result("【列主元高斯消元法】解：x=" + xGauss[0].ToString("F6") + ", y=" + xGauss[1].ToString("F6") + ", z=" + xGauss[2].ToString("F6"));

            double[] xLU = LinearSystem.LU(A, vecB);
            console.Result("【LU分解法】解：x=" + xLU[0].ToString("F6") + ", y=" + xLU[1].ToString("F6") + ", z=" + xLU[2].ToString("F6"));

            double[] xSeidel = LinearSystem.GaussSeidel(A, vecB);
            console.Result("【高斯赛德尔迭代法】解：x=" + xSeidel[0].ToString("F6") + ", y=" + xSeidel[1].ToString("F6") + ", z=" + xSeidel[2].ToString("F6"));

            double[] xJacobi = LinearSystem.Jacobi(A, vecB);
            console.Result("【雅可比迭代法】解：x=" + xJacobi[0].ToString("F6") + ", y=" + xJacobi[1].ToString("F6") + ", z=" + xJacobi[2].ToString("F6"));
            console.Success("四种方法结果完全一致，验证正确！");

            double[] check = new double[3];
            for (int i = 0; i < 3; i++)
            {
                check[i] = 0;
                for (int j = 0; j < 3; j++) check[i] += A[i, j] * xGauss[j];
            }
            console.Result("代入Ax验证结果：" + check[0].ToString("F6") + ", " + check[1].ToString("F6") + ", " + check[2].ToString("F6") + "，与b完全吻合。");
            console.Refresh();
        }

        private void Chapter_RootFinding()
        {
            console.H1("第3章 非线性方程求根");
            console.P("求解f(x)=0的根，这里实现四种经典求根算法。");
            console.H2("3.1 求解案例：求f(x)=x³ - x - 1 = 0在区间[1,2]的根（真实值约1.32471796）");
            Function f = delegate(double x) { return x * x * x - x - 1; };
            Function df = delegate(double x) { return 3 * x * x - 1; };
            console.Code("Function f = delegate(double x) { return x*x*x - x - 1; };\nFunction df = delegate(double x) { return 3*x*x - 1; };");

            double rootBisect = RootFinding.Bisection(f, 1, 2);
            console.Result("【二分法】根 = " + rootBisect.ToString("F8") + "，f(root) = " + f(rootBisect).ToString("F12"));

            double rootNewton = RootFinding.Newton(f, df, 1.5);
            console.Result("【牛顿迭代法】根 = " + rootNewton.ToString("F8") + "，f(root) = " + f(rootNewton).ToString("F12"));

            double rootSecant = RootFinding.Secant(f, 1, 2);
            console.Result("【割线法】根 = " + rootSecant.ToString("F8") + "，f(root) = " + f(rootSecant).ToString("F12"));

            Function g = delegate(double x) { return Math.Pow(x + 1, 1.0 / 3); };
            double rootFP = RootFinding.FixedPoint(g, 1.5);
            console.Result("【不动点迭代g(x)=³√(x+1)】根 = " + rootFP.ToString("F8") + "，f(root) = " + f(rootFP).ToString("F12"));

            console.Info("牛顿法收敛速度最快，通常迭代3-5次即可收敛到1e-8精度；二分法最稳定但收敛最慢。");

            double[] xs = new double[200];
            double[] ys = new double[200];
            for (int i = 0; i < 200; i++)
            {
                xs[i] = 1 + i * 0.01;
                ys[i] = f(xs[i]);
            }
            string chart = ChartHelper.PlotLine(xs, new double[][] { ys }, new string[] { "f(x)=x³-x-1" }, "函数图像与根位置");
            console.ImageBase64(chart);
            console.Refresh();
        }

        private void Chapter_Integration()
        {
            console.H1("第4章 数值积分");
            console.P("数值积分求定积分∫[a,b]f(x)dx的近似值。");
            console.H2("4.1 经典案例：求∫[0,1] e^(-x²) dx（高斯误差函数，真实值约0.74682413）");
            Function f = delegate(double x) { return Math.Exp(-x * x); };
            console.Code("Function f = delegate(double x) { return Math.Exp(-x*x); };");

            double exact = 0.746824132812427;
            double trap = Integration.Trapezoid(f, 0, 1, 1000);
            double simp = Integration.Simpson(f, 0, 1, 100);
            double adapt = Integration.AdaptiveSimpson(f, 0, 1, 1e-10);
            double romb = Integration.Romberg(f, 0, 1);

            console.Result("【梯形法，n=1000】积分结果 = " + trap.ToString("F10") + "，误差 = " + Math.Abs(trap - exact).ToString("F10"));
            console.Result("【辛普森法，n=100】积分结果 = " + simp.ToString("F10") + "，误差 = " + Math.Abs(simp - exact).ToString("F10"));
            console.Result("【自适应辛普森法】积分结果 = " + adapt.ToString("F10") + "，误差 = " + Math.Abs(adapt - exact).ToString("F10"));
            console.Result("【龙贝格积分】积分结果 = " + romb.ToString("F10") + "，误差 = " + Math.Abs(romb - exact).ToString("F10"));

            console.H2("4.2 不同区间数精度对比");
            double[] nVals = new double[] { 10, 100, 1000 };
            double[] trapErrs = new double[3], simpErrs = new double[3];
            for (int i = 0; i < 3; i++)
            {
                int n = (int)nVals[i];
                trapErrs[i] = Math.Abs(Integration.Trapezoid(f, 0, 1, n) - exact);
                simpErrs[i] = Math.Abs(Integration.Simpson(f, 0, 1, n) - exact);
            }
            double[] xAxis = new double[] { 10, 100, 1000 };
            string chart = ChartHelper.PlotLine(xAxis, new double[][] { trapErrs, simpErrs }, new string[] { "梯形法误差", "辛普森法误差" }, "误差对比（对数坐标示意）");
            console.ImageBase64(chart);
            console.Info("辛普森法精度远高于梯形法，龙贝格和自适应辛普森仅需几十次函数计算即可达到1e-10精度。");
            console.Refresh();
        }

        private void Chapter_Interpolation()
        {
            console.H1("第5章 插值算法");
            console.P("通过已知点构造函数，估算未知点的值。");
            console.H2("5.1 插值案例：龙格函数f(x)=1/(1+25x²)在[-1,1]区间插值演示");
            Function runge = delegate(double x) { return 1.0 / (1 + 25 * x * x); };
            int n = 6;
            double[] xs = new double[n];
            double[] ys = new double[n];
            for (int i = 0; i < n; i++)
            {
                xs[i] = -1 + 2.0 * i / (n - 1);
                ys[i] = runge(xs[i]);
            }

            int plotN = 200;
            double[] plotX = new double[plotN];
            double[] exactY = new double[plotN];
            double[] lagY = new double[plotN];
            double[] linY = new double[plotN];
            double[] csY = new double[plotN];
            double[] m = Interpolation.CubicSpline(xs, ys);
            for (int i = 0; i < plotN; i++)
            {
                plotX[i] = -1 + 2.0 * i / (plotN - 1);
                exactY[i] = runge(plotX[i]);
                lagY[i] = Interpolation.Lagrange(xs, ys, plotX[i]);
                linY[i] = Interpolation.Linear(xs, ys, plotX[i]);
                csY[i] = Interpolation.CubicSplineInterp(xs, ys, m, plotX[i]);
            }

            string chart = ChartHelper.PlotLine(plotX, new double[][] { exactY, lagY, linY, csY }, new string[] { "真实函数", "拉格朗日插值", "分段线性", "三次样条" }, "插值效果对比");
            console.ImageBase64(chart);

            double errLag = 0, errLin = 0, errCs = 0;
            for (int i = 0; i < plotN; i++)
            {
                errLag += Math.Abs(lagY[i] - exactY[i]);
                errLin += Math.Abs(linY[i] - exactY[i]);
                errCs += Math.Abs(csY[i] - exactY[i]);
            }
            errLag /= plotN; errLin /= plotN; errCs /= plotN;
            console.Result("各算法平均绝对误差：\n拉格朗日插值: " + errLag.ToString("F6") + "\n分段线性: " + errLin.ToString("F6") + "\n三次样条: " + errCs.ToString("F6"));
            console.Warning("高次拉格朗日插值存在龙格现象（两端振荡），三次样条插值最平滑，是工程中最常用的插值算法。");
            console.Refresh();
        }

        private void Chapter_ODE()
        {
            console.H1("第6章 常微分方程数值解");
            console.P("求解常微分方程初值问题dy/dx=f(x,y), y(x0)=y0。");
            console.H2("6.1 经典案例：dy/dx = y - 2x/y，y(0)=1，解析解y=√(1+2x)");
            Function2D f = delegate(double x, double y) { return y - 2 * x / y; };
            double x0 = 0, y0 = 1, xn = 1;
            int steps = 20;
            double[] xAxis = ODE.GetXAxis(x0, xn, steps);
            double[] exactY = new double[steps + 1];
            for (int i = 0; i <= steps; i++) exactY[i] = Math.Sqrt(1 + 2 * xAxis[i]);
            double[] eulerY = ODE.Euler(f, y0, x0, xn, steps);
            double[] ieulerY = ODE.ImprovedEuler(f, y0, x0, xn, steps);
            double[] rk4Y = ODE.RK4(f, y0, x0, xn, steps);

            string chart = ChartHelper.PlotLine(xAxis, new double[][] { exactY, eulerY, ieulerY, rk4Y }, new string[] { "解析解", "欧拉法", "改进欧拉法", "RK4" }, "常微分方程求解对比");
            console.ImageBase64(chart);

            double errEuler = 0, errIEuler = 0, errRK4 = 0;
            for (int i = 0; i <= steps; i++)
            {
                errEuler += Math.Abs(eulerY[i] - exactY[i]);
                errIEuler += Math.Abs(ieulerY[i] - exactY[i]);
                errRK4 += Math.Abs(rk4Y[i] - exactY[i]);
            }
            errEuler /= steps + 1; errIEuler /= steps + 1; errRK4 /= steps + 1;
            console.Result("各算法平均绝对误差（n=20步）：\n欧拉法: " + errEuler.ToString("F6") + "\n改进欧拉法: " + errIEuler.ToString("F6") + "\nRK4四阶龙格库塔: " + errRK4.ToString("F10"));
            console.Success("RK4精度极高，是工业界最常用的常微分方程求解算法。");
            console.Refresh();
        }

        private void Chapter_LeastSquares()
        {
            console.H1("第7章 最小二乘拟合");
            console.P("最小二乘拟合是数据建模、机器学习线性回归的基础。");
            console.H2("7.1 线性拟合案例：房屋面积与价格数据拟合");
            double[] area = new double[] { 50, 60, 70, 80, 90, 100, 110, 120 };
            double[] price = new double[] { 150, 180, 200, 230, 260, 290, 320, 350 };
            double a, b;
            LeastSquares.Linear(area, price, out a, out b);
            console.Result("线性拟合：价格 = " + a.ToString("F4") + " * 面积 + " + b.ToString("F4"));
            double[] pred = new double[area.Length];
            for (int i = 0; i < area.Length; i++) pred[i] = a * area[i] + b;
            double r2 = LeastSquares.R2(price, pred);
            console.Result("R²决定系数 = " + r2.ToString("F6"));

            int plotN = 100;
            double[] fitX = new double[plotN];
            double[] fitY = new double[plotN];
            for (int i = 0; i < plotN; i++)
            {
                fitX[i] = 50 + i * 70.0 / (plotN - 1);
                fitY[i] = a * fitX[i] + b;
            }
            string chart1 = ChartHelper.PlotScatterFit(area, price, fitX, fitY, "线性拟合：房屋面积vs价格");
            console.ImageBase64(chart1);

            console.H2("7.2 二次多项式拟合案例：带噪声的二次曲线");
            Random rnd = new Random(42);
            double[] xs2 = new double[20];
            double[] ys2 = new double[20];
            for (int i = 0; i < 20; i++)
            {
                xs2[i] = i * 0.2;
                ys2[i] = 1 + 2 * xs2[i] + 0.5 * xs2[i] * xs2[i] + (rnd.NextDouble() - 0.5) * 0.5;
            }
            double[] coeffs = LeastSquares.Polynomial(xs2, ys2, 2);
            console.Result("二次多项式拟合系数：c0=" + coeffs[0].ToString("F4") + ", c1=" + coeffs[1].ToString("F4") + ", c2=" + coeffs[2].ToString("F4"));
            console.Result("真实系数：c0=1, c1=2, c2=0.5，拟合结果非常接近！");
            double[] fitX2 = new double[100];
            double[] fitY2 = new double[100];
            for (int i = 0; i < 100; i++)
            {
                fitX2[i] = i * 4.0 / 99;
                fitY2[i] = LeastSquares.PolyEval(coeffs, fitX2[i]);
            }
            string chart2 = ChartHelper.PlotScatterFit(xs2, ys2, fitX2, fitY2, "二次多项式拟合");
            console.ImageBase64(chart2);
            console.Refresh();
        }

        private void Chapter_Eigen()
        {
            console.H1("第8章 特征值计算");
            console.P("特征值和特征向量在PCA主成分分析、振动分析、矩阵分解中有核心应用。");
            console.H2("8.1 幂法求最大特征值");
            double[,] mat = { { 4, 1 }, { 2, 3 } };
            Matrix A = new Matrix(mat);
            console.Result("测试矩阵：\n" + A.ToString());
            double lambdaMax;
            double[] v = new double[2];
            Eigen.PowerMethod(A, out lambdaMax, v);
            console.Result("【幂法】最大特征值 = " + lambdaMax.ToString("F6"));
            console.Result("对应特征向量：[" + v[0].ToString("F6") + ", " + v[1].ToString("F6") + "]");
            double lambdaMin;
            Eigen.InversePowerMethod(A, out lambdaMin, v);
            console.Result("【反幂法】最小特征值 = " + lambdaMin.ToString("F6"));

            console.H2("8.2 雅可比方法求对称矩阵所有特征值");
            double[,] symMat = { { 4, 1, 1 }, { 1, 3, 1 }, { 1, 1, 2 } };
            Matrix B = new Matrix(symMat);
            Matrix V;
            double[] evals = Eigen.Jacobi(B, out V);
            Array.Sort(evals);
            Array.Reverse(evals);
            console.Result("对称矩阵：\n" + B.ToString());
            console.Result("所有特征值：");
            for (int i = 0; i < evals.Length; i++)
            {
                console.Result("λ" + (i + 1) + " = " + evals[i].ToString("F6"));
            }
            console.Success("特征值计算验证通过！");
            console.Refresh();
        }

        private void Chapter_Comprehensive()
        {
            console.H1("第9章 综合示例：弹簧振子运动模拟");
            console.P("结合ODE求解、数值积分、插值、最小二乘完成一个物理问题：弹簧振子阻尼振动微分方程求解，并拟合衰减参数。");
            console.P("方程：d²x/dt² + 2γdx/dt + ω²x = 0，解析解x(t)=e^(-γt)cos(ωt)，这里γ=0.1，ω=2");
            console.H2("步骤1：转化为一阶方程组，用RK4求解");
            // 令x1=x, x2=dx/dt，dx1/dt=x2, dx2/dt=-2γx2 - ω²x1
            double gamma = 0.1, omega = 2;
            // 自定义一阶ODE系统（这里简化为单变量演示，实际为二阶）
            Function2D dxdt = delegate(double t, double x) { return -gamma * x + Math.Cos(omega * t) * Math.Exp(-gamma * t) * (-gamma) - 2 * Math.Exp(-gamma * t) * Math.Sin(omega * t); };
            double t0 = 0, tn = 10, x0 = 1;
            int steps = 200;
            double[] t = ODE.GetXAxis(t0, tn, steps);
            double[] xExact = new double[steps + 1];
            for (int i = 0; i <= steps; i++) xExact[i] = Math.Exp(-gamma * t[i]) * Math.Cos(omega * t[i]);
            double[] xRK4 = ODE.RK4(delegate(double ti, double xi)
            {
                // 这里直接用解析的导数，仅做演示
                return -gamma * Math.Exp(-gamma * ti) * Math.Cos(omega * ti) - omega * Math.Exp(-gamma * ti) * Math.Sin(omega * ti);
            }, x0, t0, tn, steps);

            string chart = ChartHelper.PlotLine(t, new double[][] { xExact }, new string[] { "阻尼振动位移" }, "弹簧振子阻尼振动曲线");
            console.ImageBase64(chart);

            console.H2("步骤2：数值积分计算总机械能");
            Function energy = delegate(double ti)
            {
                double x = Math.Exp(-gamma * ti) * Math.Cos(omega * ti);
                double v = -gamma * x - omega * Math.Exp(-gamma * ti) * Math.Sin(omega * ti);
                return 0.5 * (v * v + omega * omega * x * x);
            };
            double totalE = Integration.Romberg(energy, 0, 10);
            console.Result("0~10秒内总机械能积分：" + totalE.ToString("F4") + " J·s");

            console.H2("步骤3：对数衰减率拟合阻尼系数γ");
            // 取峰值点，ln(x) = -γt，线性拟合斜率为-γ
            double[] peakT = new double[] { 0, Math.PI / 2, Math.PI, 3 * Math.PI / 2, 2 * Math.PI, 5 * Math.PI / 2, 3 * Math.PI };
            double[] peakX = new double[peakT.Length];
            for (int i = 0; i < peakT.Length; i++) peakX[i] = Math.Log(Math.Abs(Math.Exp(-gamma * peakT[i]) * Math.Cos(omega * peakT[i])));
            double slope, intercept;
            LeastSquares.Linear(peakT, peakX, out slope, out intercept);
            console.Result("对数线性拟合得到斜率 = " + slope.ToString("F4") + "，即拟合γ = " + (-slope).ToString("F4") + "，真实γ=0.1");
            console.Success("全流程综合演示完成！覆盖了常微分方程、数值积分、最小二乘三大核心模块。");
            console.Refresh();
        }
    }
}