using System;
using NumericalComputing.Core;

namespace NumericalComputing.Algorithms
{
    /// <summary>
    /// 线性方程组求解，纯C#2.0实现
    /// </summary>
    public class LinearSystem
    {
        /// <summary>
        /// 列主元高斯消元法求解 Ax=b
        /// </summary>
        public static double[] GaussPivot(Matrix A, double[] b)
        {
            int n = A.Rows;
            Matrix aug = new Matrix(n, n + 1);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) aug[i, j] = A[i, j];
                aug[i, n] = b[i];
            }

            for (int k = 0; k < n; k++)
            {
                int pivot = aug.Pivot(k, k);
                if (pivot != k) aug.SwapRows(k, pivot);
                if (Math.Abs(aug[k, k]) < 1e-12)
                    throw new InvalidOperationException("矩阵奇异，无解");

                for (int i = k + 1; i < n; i++)
                {
                    double factor = aug[i, k] / aug[k, k];
                    for (int j = k; j < n + 1; j++)
                    {
                        aug[i, j] -= factor * aug[k, j];
                    }
                }
            }

            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = aug[i, n];
                for (int j = i + 1; j < n; j++)
                {
                    x[i] -= aug[i, j] * x[j];
                }
                x[i] /= aug[i, i];
            }
            return x;
        }

        /// <summary>
        /// LU分解，PA=LU，返回L,U,P
        /// </summary>
        public static void LUDecomposition(Matrix A, out Matrix L, out Matrix U, out Matrix P)
        {
            int n = A.Rows;
            L = Matrix.Zero(n, n);
            U = Matrix.Zero(n, n);
            P = Matrix.Identity(n);
            Matrix M = A.Copy();

            for (int k = 0; k < n; k++)
            {
                int pivot = M.Pivot(k, k);
                if (pivot != k)
                {
                    M.SwapRows(k, pivot);
                    P.SwapRows(k, pivot);
                    for (int j = 0; j < k; j++)
                    {
                        double tmp = L[k, j];
                        L[k, j] = L[pivot, j];
                        L[pivot, j] = tmp;
                    }
                }

                L[k, k] = 1.0;
                U[k, k] = M[k, k];
                for (int i = k + 1; i < n; i++)
                {
                    L[i, k] = M[i, k] / M[k, k];
                    U[k, i] = M[k, i];
                }

                for (int i = k + 1; i < n; i++)
                {
                    for (int j = k + 1; j < n; j++)
                    {
                        M[i, j] -= L[i, k] * U[k, j];
                    }
                }
            }
        }

        /// <summary>
        /// LU分解求解方程组
        /// </summary>
        public static double[] LU(Matrix A, double[] b)
        {
            Matrix L, U, P;
            LUDecomposition(A, out L, out U, out P);

            int n = A.Rows;
            double[] Pb = new double[n];
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += P[i, j] * b[j];
                }
                Pb[i] = sum;
            }

            // Ly=Pb，下三角
            double[] y = new double[n];
            for (int i = 0; i < n; i++)
            {
                y[i] = Pb[i];
                for (int j = 0; j < i; j++) y[i] -= L[i, j] * y[j];
            }

            // Ux=y，上三角
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = y[i];
                for (int j = i + 1; j < n; j++) x[i] -= U[i, j] * x[j];
                x[i] /= U[i, i];
            }
            return x;
        }

        /// <summary>
        /// 高斯赛德尔迭代法求解
        /// </summary>
        public static double[] GaussSeidel(Matrix A, double[] b, double[] init = null, int maxIter = 1000, double tol = 1e-8)
        {
            int n = A.Rows;
            double[] x = init == null ? new double[n] : (double[])init.Clone();
            for (int iter = 0; iter < maxIter; iter++)
            {
                double[] xNew = (double[])x.Clone();
                double err = 0;
                for (int i = 0; i < n; i++)
                {
                    double sum = b[i];
                    for (int j = 0; j < n; j++) if (j != i) sum -= A[i, j] * xNew[j];
                    xNew[i] = sum / A[i, i];
                    err += Math.Abs(xNew[i] - x[i]);
                }
                x = xNew;
                if (err < tol) return x;
            }
            return x;
        }

        /// <summary>
        /// 雅可比迭代法求解
        /// </summary>
        public static double[] Jacobi(Matrix A, double[] b, double[] init = null, int maxIter = 1000, double tol = 1e-8)
        {
            int n = A.Rows;
            double[] x = init == null ? new double[n] : (double[])init.Clone();
            for (int iter = 0; iter < maxIter; iter++)
            {
                double[] xNew = (double[])x.Clone();
                double err = 0;
                for (int i = 0; i < n; i++)
                {
                    double sum = b[i];
                    for (int j = 0; j < n; j++) if (j != i) sum -= A[i, j] * x[j];
                    xNew[i] = sum / A[i, i];
                    err += Math.Abs(xNew[i] - x[i]);
                }
                x = xNew;
                if (err < tol) return x;
            }
            return x;
        }
    }
}