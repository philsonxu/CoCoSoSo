using System;
using NumericalComputing.Core;

namespace NumericalComputing.Algorithms
{
    /// <summary>
    /// 最小二乘拟合，纯C#2.0实现
    /// </summary>
    public class LeastSquares
    {
        /// <summary>
        /// 线性拟合 y = ax + b，返回a,b
        /// </summary>
        public static void Linear(double[] xs, double[] ys, out double a, out double b)
        {
            int n = xs.Length;
            double sx = 0, sy = 0, sxx = 0, sxy = 0;
            for (int i = 0; i < n; i++)
            {
                sx += xs[i];
                sy += ys[i];
                sxx += xs[i] * xs[i];
                sxy += xs[i] * ys[i];
            }
            double denom = n * sxx - sx * sx;
            a = (n * sxy - sx * sy) / denom;
            b = (sy * sxx - sx * sxy) / denom;
        }

        /// <summary>
        /// 计算R²决定系数
        /// </summary>
        public static double R2(double[] ys, double[] yPred)
        {
            int n = ys.Length;
            double mean = 0;
            for (int i = 0; i < n; i++) mean += ys[i];
            mean /= n;
            double ssTot = 0, ssRes = 0;
            for (int i = 0; i < n; i++)
            {
                ssTot += (ys[i] - mean) * (ys[i] - mean);
                ssRes += (ys[i] - yPred[i]) * (ys[i] - yPred[i]);
            }
            return 1 - ssRes / ssTot;
        }

        /// <summary>
        /// 多项式拟合 y = c0 + c1*x + c2*x² + ... + cm*x^m
        /// 返回系数数组c[], c[0]为常数项
        /// </summary>
        public static double[] Polynomial(double[] xs, double[] ys, int m)
        {
            int n = xs.Length;
            double[,] X = new double[n, m + 1];
            for (int i = 0; i < n; i++)
            {
                X[i, 0] = 1.0;
                for (int j = 1; j <= m; j++)
                {
                    X[i, j] = X[i, j - 1] * xs[i];
                }
            }
            Matrix Xmat = new Matrix(X);
            Matrix Xt = Xmat.Transpose();
            Matrix XtX = Matrix.Multiply(Xt, Xmat);
            double[] Xty = new double[m + 1];
            for (int j = 0; j <= m; j++)
            {
                double sum = 0;
                for (int i = 0; i < n; i++)
                {
                    sum += X[i, j] * ys[i];
                }
                Xty[j] = sum;
            }
            return LinearSystem.GaussPivot(XtX, Xty);
        }

        /// <summary>
        /// 计算多项式拟合在x处的值
        /// </summary>
        public static double PolyEval(double[] coeffs, double x)
        {
            double result = 0;
            double xPow = 1;
            for (int i = 0; i < coeffs.Length; i++)
            {
                result += coeffs[i] * xPow;
                xPow *= x;
            }
            return result;
        }
    }

    /// <summary>
    /// 特征值计算，纯C#2.0实现
    /// </summary>
    public class Eigen
    {
        /// <summary>
        /// 幂法求矩阵最大特征值和对应特征向量
        /// </summary>
        public static void PowerMethod(Matrix A, out double eigenvalue, double[] eigenvector, int maxIter = 1000, double tol = 1e-8)
        {
            int n = A.Rows;
            double[] v = new double[n];
            v[0] = 1;
            double lambda = 0;
            for (int iter = 0; iter < maxIter; iter++)
            {
                double[] vNew = new double[n];
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++) sum += A[i, j] * v[j];
                    vNew[i] = sum;
                }
                double maxVal = 0;
                for (int i = 0; i < n; i++)
                {
                    if (Math.Abs(vNew[i]) > Math.Abs(maxVal)) maxVal = vNew[i];
                }
                for (int i = 0; i < n; i++) vNew[i] /= maxVal;
                double err = 0;
                for (int i = 0; i < n; i++) err += Math.Abs(vNew[i] - v[i]);
                v = vNew;
                lambda = maxVal;
                if (err < tol) break;
            }
            eigenvalue = lambda;
            Array.Copy(v, eigenvector, n);
        }

        /// <summary>
        /// 反幂法求最小特征值
        /// </summary>
        public static void InversePowerMethod(Matrix A, out double eigenvalue, double[] eigenvector, int maxIter = 1000, double tol = 1e-8)
        {
            Matrix AInv = A.Inverse();
            double lambda;
            PowerMethod(AInv, out lambda, eigenvector, maxIter, tol);
            eigenvalue = 1.0 / lambda;
        }

        /// <summary>
        /// 雅可比方法求对称矩阵所有特征值和特征向量
        /// </summary>
        public static double[] Jacobi(Matrix A, out Matrix V, int maxIter = 1000, double tol = 1e-10)
        {
            int n = A.Rows;
            Matrix Anew = A.Copy();
            V = Matrix.Identity(n);

            for (int iter = 0; iter < maxIter; iter++)
            {
                int p = 0, q = 1;
                double maxOff = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (Math.Abs(Anew[i, j]) > maxOff)
                        {
                            maxOff = Math.Abs(Anew[i, j]);
                            p = i;
                            q = j;
                        }
                    }
                }
                if (maxOff < tol) break;

                double app = Anew[p, p], aqq = Anew[q, q], apq = Anew[p, q];
                double theta;
                if (Math.Abs(app - aqq) < 1e-12) theta = Math.PI / 4;
                else theta = 0.5 * Math.Atan2(2 * apq, aqq - app);
                double c = Math.Cos(theta), s = Math.Sin(theta);

                Matrix G = Matrix.Identity(n);
                G[p, p] = c; G[p, q] = -s;
                G[q, p] = s; G[q, q] = c;

                Matrix Gt = G.Transpose();
                Anew = Matrix.Multiply(Matrix.Multiply(Gt, Anew), G);
                V = Matrix.Multiply(V, G);
            }

            double[] eigenvalues = new double[n];
            for (int i = 0; i < n; i++) eigenvalues[i] = Anew[i, i];
            return eigenvalues;
        }
    }
}