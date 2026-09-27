using System;
using NumericalComputing.Core;

namespace NumericalComputing.Algorithms
{
    /// <summary>
    /// 插值算法，纯C#2.0实现
    /// </summary>
    public class Interpolation
    {
        /// <summary>
        /// 拉格朗日插值
        /// </summary>
        public static double Lagrange(double[] xs, double[] ys, double x)
        {
            int n = xs.Length;
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                double term = ys[i];
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        term *= (x - xs[j]) / (xs[i] - xs[j]);
                    }
                }
                result += term;
            }
            return result;
        }

        /// <summary>
        /// 牛顿差商插值
        /// </summary>
        public static double NewtonDividedDifference(double[] xs, double[] ys, double x)
        {
            int n = xs.Length;
            double[,] diff = new double[n, n];
            for (int i = 0; i < n; i++) diff[i, 0] = ys[i];
            for (int j = 1; j < n; j++)
            {
                for (int i = j; i < n; i++)
                {
                    diff[i, j] = (diff[i, j - 1] - diff[i - 1, j - 1]) / (xs[i] - xs[i - j]);
                }
            }

            double result = diff[0, 0];
            double product = 1.0;
            for (int i = 1; i < n; i++)
            {
                product *= (x - xs[i - 1]);
                result += diff[i, i] * product;
            }
            return result;
        }

        /// <summary>
        /// 分段线性插值
        /// </summary>
        public static double Linear(double[] xs, double[] ys, double x)
        {
            int n = xs.Length;
            if (x <= xs[0]) return ys[0];
            if (x >= xs[n - 1]) return ys[n - 1];
            int idx = 0;
            for (int i = 0; i < n - 1; i++)
            {
                if (x >= xs[i] && x <= xs[i + 1])
                {
                    idx = i;
                    break;
                }
            }
            double t = (x - xs[idx]) / (xs[idx + 1] - xs[idx]);
            return ys[idx] * (1 - t) + ys[idx + 1] * t;
        }

        /// <summary>
        /// 三次样条插值，自然边界条件
        /// </summary>
        public static double[] CubicSpline(double[] xs, double[] ys)
        {
            int n = xs.Length;
            double[] h = new double[n - 1];
            for (int i = 0; i < n - 1; i++) h[i] = xs[i + 1] - xs[i];

            Matrix M = new Matrix(n, n);
            double[] d = new double[n];
            M[0, 0] = 1; M[n - 1, n - 1] = 1;
            d[0] = 0; d[n - 1] = 0;

            for (int i = 1; i < n - 1; i++)
            {
                M[i, i - 1] = h[i - 1];
                M[i, i] = 2 * (h[i - 1] + h[i]);
                M[i, i + 1] = h[i];
                d[i] = 6 * ((ys[i + 1] - ys[i]) / h[i] - (ys[i] - ys[i - 1]) / h[i - 1]);
            }

            return LinearSystem.GaussPivot(M, d);
        }

        /// <summary>
        /// 三次样条单点插值
        /// </summary>
        public static double CubicSplineInterp(double[] xs, double[] ys, double[] m, double x)
        {
            int n = xs.Length;
            if (x <= xs[0]) return ys[0];
            if (x >= xs[n - 1]) return ys[n - 1];
            int i = 0;
            for (int k = 0; k < n - 1; k++)
            {
                if (x >= xs[k] && x <= xs[k + 1])
                {
                    i = k;
                    break;
                }
            }
            double hi = xs[i + 1] - xs[i];
            double xi = xs[i], xi1 = xs[i + 1];
            double t = (x - xi) / hi;
            double h1 = m[i] * (xi1 - x) * (xi1 - x) * (x - xi) / (6 * hi);
            double h2 = m[i + 1] * (x - xi) * (x - xi) * (xi1 - x) / (6 * hi);
            double h3 = ys[i] * (xi1 - x) / hi;
            double h4 = ys[i + 1] * (x - xi) / hi;
            return h1 + h2 + h3 + h4;
        }
    }

    /// <summary>
    /// 双变量函数委托
    /// </summary>
    public delegate double Function2D(double x, double y);

    /// <summary>
    /// 常微分方程数值求解，纯C#2.0实现
    /// </summary>
    public class ODE
    {
        /// <summary>
        /// 欧拉法
        /// </summary>
        public static double[] Euler(Function2D f, double y0, double x0, double xn, int n)
        {
            double h = (xn - x0) / n;
            double[] y = new double[n + 1];
            double[] x = new double[n + 1];
            y[0] = y0;
            x[0] = x0;
            for (int i = 0; i < n; i++)
            {
                y[i + 1] = y[i] + h * f(x[i], y[i]);
                x[i + 1] = x[i] + h;
            }
            return y;
        }

        /// <summary>
        /// 改进欧拉法（Heun法）
        /// </summary>
        public static double[] ImprovedEuler(Function2D f, double y0, double x0, double xn, int n)
        {
            double h = (xn - x0) / n;
            double[] y = new double[n + 1];
            double[] x = new double[n + 1];
            y[0] = y0;
            x[0] = x0;
            for (int i = 0; i < n; i++)
            {
                double k1 = f(x[i], y[i]);
                double k2 = f(x[i] + h, y[i] + h * k1);
                y[i + 1] = y[i] + h * (k1 + k2) / 2;
                x[i + 1] = x[i] + h;
            }
            return y;
        }

        /// <summary>
        /// 四阶龙格库塔法RK4
        /// </summary>
        public static double[] RK4(Function2D f, double y0, double x0, double xn, int n)
        {
            double h = (xn - x0) / n;
            double[] y = new double[n + 1];
            double[] x = new double[n + 1];
            y[0] = y0;
            x[0] = x0;
            for (int i = 0; i < n; i++)
            {
                double k1 = f(x[i], y[i]);
                double k2 = f(x[i] + h / 2, y[i] + h * k1 / 2);
                double k3 = f(x[i] + h / 2, y[i] + h * k2 / 2);
                double k4 = f(x[i] + h, y[i] + h * k3);
                y[i + 1] = y[i] + h * (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                x[i + 1] = x[i] + h;
            }
            return y;
        }

        /// <summary>
        /// 获取ODE的x轴序列
        /// </summary>
        public static double[] GetXAxis(double x0, double xn, int n)
        {
            double h = (xn - x0) / n;
            double[] x = new double[n + 1];
            for (int i = 0; i <= n; i++) x[i] = x0 + i * h;
            return x;
        }
    }
}