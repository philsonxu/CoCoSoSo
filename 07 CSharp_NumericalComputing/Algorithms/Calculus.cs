using System;

namespace NumericalComputing.Algorithms
{
    /// <summary>
    /// 单变量函数委托，C#2.0原生支持
    /// </summary>
    public delegate double Function(double x);

    /// <summary>
    /// 非线性方程求根，纯C#2.0实现
    /// </summary>
    public class RootFinding
    {
        /// <summary>
        /// 二分法求根
        /// </summary>
        public static double Bisection(Function f, double a, double b, double tol = 1e-8, int maxIter = 100)
        {
            if (f(a) * f(b) > 0)
                throw new ArgumentException("区间两端函数值必须异号");

            double fa = f(a), fb = f(b);
            for (int i = 0; i < maxIter; i++)
            {
                double c = (a + b) / 2;
                double fc = f(c);
                if (Math.Abs(b - a) < tol || Math.Abs(fc) < tol)
                    return c;
                if (fa * fc < 0)
                {
                    b = c;
                    fb = fc;
                }
                else
                {
                    a = c;
                    fa = fc;
                }
            }
            return (a + b) / 2;
        }

        /// <summary>
        /// 牛顿迭代法求根
        /// </summary>
        public static double Newton(Function f, Function df, double x0, double tol = 1e-8, int maxIter = 100)
        {
            double x = x0;
            for (int i = 0; i < maxIter; i++)
            {
                double fx = f(x);
                double dfx = df(x);
                if (Math.Abs(dfx) < 1e-12)
                    throw new InvalidOperationException("导数为零，牛顿法失败");
                double xNew = x - fx / dfx;
                if (Math.Abs(xNew - x) < tol) return xNew;
                x = xNew;
            }
            return x;
        }

        /// <summary>
        /// 割线法求根，不需要显式求导
        /// </summary>
        public static double Secant(Function f, double x0, double x1, double tol = 1e-8, int maxIter = 100)
        {
            double f0 = f(x0), f1 = f(x1);
            for (int i = 0; i < maxIter; i++)
            {
                if (Math.Abs(f1 - f0) < 1e-12)
                    throw new InvalidOperationException("割线斜率为零，方法失败");
                double xNew = x1 - f1 * (x1 - x0) / (f1 - f0);
                double fNew = f(xNew);
                if (Math.Abs(xNew - x1) < tol) return xNew;
                x0 = x1;
                f0 = f1;
                x1 = xNew;
                f1 = fNew;
            }
            return x1;
        }

        /// <summary>
        /// 不动点迭代法
        /// </summary>
        public static double FixedPoint(Function g, double x0, double tol = 1e-8, int maxIter = 100)
        {
            double x = x0;
            for (int i = 0; i < maxIter; i++)
            {
                double xNew = g(x);
                if (Math.Abs(xNew - x) < tol) return xNew;
                x = xNew;
            }
            return x;
        }
    }

    /// <summary>
    /// 数值积分，纯C#2.0实现
    /// </summary>
    public class Integration
    {
        /// <summary>
        /// 梯形公式
        /// </summary>
        public static double Trapezoid(Function f, double a, double b, int n = 1000)
        {
            double h = (b - a) / n;
            double sum = (f(a) + f(b)) / 2.0;
            for (int i = 1; i < n; i++)
            {
                sum += f(a + i * h);
            }
            return sum * h;
        }

        /// <summary>
        /// 辛普森公式（抛物线公式）
        /// </summary>
        public static double Simpson(Function f, double a, double b, int n = 1000)
        {
            if (n % 2 != 0) n++;
            double h = (b - a) / n;
            double sum = f(a) + f(b);
            for (int i = 1; i < n; i += 2) sum += 4 * f(a + i * h);
            for (int i = 2; i < n; i += 2) sum += 2 * f(a + i * h);
            return sum * h / 3.0;
        }

        /// <summary>
        /// 自适应辛普森积分
        /// </summary>
        public static double AdaptiveSimpson(Function f, double a, double b, double eps = 1e-8)
        {
            double c = (a + b) / 2;
            double fa = f(a), fb = f(b), fc = f(c);
            return AdaptiveSimpsonRec(f, a, b, fa, fb, fc, eps);
        }

        private static double AdaptiveSimpsonRec(Function f, double a, double b, double fa, double fb, double fc, double eps)
        {
            double c = (a + b) / 2;
            double d = (a + c) / 2, e = (c + b) / 2;
            double fd = f(d), fe = f(e);
            double S = (b - a) * (fa + 4 * fc + fb) / 6;
            double S2 = (b - a) * (fa + 4 * fd + 2 * fc + 4 * fe + fb) / 12;
            if (Math.Abs(S2 - S) <= 15 * eps)
                return S2 + (S2 - S) / 15;
            return AdaptiveSimpsonRec(f, a, c, fa, fc, fd, eps / 2) + AdaptiveSimpsonRec(f, c, b, fc, fb, fe, eps / 2);
        }

        /// <summary>
        /// 龙贝格积分
        /// </summary>
        public static double Romberg(Function f, double a, double b, int m = 10)
        {
            double[,] R = new double[m, m];
            double h = b - a;
            R[0, 0] = h * (f(a) + f(b)) / 2;
            for (int i = 1; i < m; i++)
            {
                h /= 2;
                double sum = 0;
                for (int k = 1; k <= (1 << (i - 1)); k++)
                {
                    sum += f(a + (2 * k - 1) * h);
                }
                R[i, 0] = R[i - 1, 0] / 2 + h * sum;
                for (int j = 1; j <= i; j++)
                {
                    R[i, j] = R[i, j] + ((1 << (2 * j)) * R[i, j - 1] - R[i - 1, j - 1]) / ((1 << (2 * j)) - 1);
                }
                if (Math.Abs(R[i, i] - R[i - 1, i - 1]) < 1e-12)
                    return R[i, i];
            }
            return R[m - 1, m - 1];
        }
    }
}