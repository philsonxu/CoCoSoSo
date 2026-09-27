using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// 矩阵运算工具类，提供基础的矩阵乘法、加法、转置等数值运算
    /// </summary>
    public static class MatMul
    {
        /// <summary>
        /// 矩阵乘法 A(m×p) * B(p×n) = C(m×n)
        /// </summary>
        public static double[,] Multiply(double[,] A, double[,] B)
        {
            int m = A.GetLength(0);
            int p = A.GetLength(1);
            int n = B.GetLength(1);
            if (p != B.GetLength(0))
                throw new ArgumentException("矩阵维度不匹配，无法相乘");

            double[,] C = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int k = 0; k < p; k++)
                {
                    double a = A[i, k];
                    if (a == 0) continue; // 稀疏优化
                    for (int j = 0; j < n; j++)
                    {
                        C[i, j] += a * B[k, j];
                    }
                }
            }
            return C;
        }

        /// <summary>
        /// 带偏置的矩阵乘法：A*W + b
        /// </summary>
        public static double[,] MultiplyAdd(double[,] A, double[,] W, double[] b)
        {
            int m = A.GetLength(0);
            int n = W.GetLength(1);
            double[,] C = Multiply(A, W);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] += b[j];
                }
            }
            return C;
        }

        /// <summary>
        /// 矩阵转置：(m×n) → (n×m)
        /// </summary>
        public static double[,] Transpose(double[,] A)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            double[,] T = new double[n, m];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    T[j, i] = A[i, j];
            return T;
        }

        /// <summary>
        /// 矩阵逐元素相加：A + B
        /// </summary>
        public static double[,] Add(double[,] A, double[,] B)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            double[,] C = new double[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        /// <summary>
        /// 向量逐元素相加：a + b
        /// </summary>
        public static double[] Add(double[] a, double[] b)
        {
            int n = a.Length;
            double[] c = new double[n];
            for (int i = 0; i < n; i++)
                c[i] = a[i] + b[i];
            return c;
        }

        /// <summary>
        /// 矩阵逐元素相减：A - B
        /// </summary>
        public static double[,] Sub(double[,] A, double[,] B)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            double[,] C = new double[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }

        /// <summary>
        /// 矩阵逐元素乘法：A ⊙ B (哈达玛积)
        /// </summary>
        public static double[,] ElementMul(double[,] A, double[,] B)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            double[,] C = new double[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    C[i, j] = A[i, j] * B[i, j];
            return C;
        }

        /// <summary>
        /// 矩阵逐元素缩放：A * scalar
        /// </summary>
        public static double[,] Scale(double[,] A, double scalar)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            double[,] C = new double[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    C[i, j] = A[i, j] * scalar;
            return C;
        }

        /// <summary>
        /// 向量缩放
        /// </summary>
        public static double[] Scale(double[] a, double scalar)
        {
            int n = a.Length;
            double[] c = new double[n];
            for (int i = 0; i < n; i++)
                c[i] = a[i] * scalar;
            return c;
        }

        /// <summary>
        /// 矩阵所有元素求和
        /// </summary>
        public static double Sum(double[,] A)
        {
            double sum = 0;
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    sum += A[i, j];
            return sum;
        }

        /// <summary>
        /// 向量求和
        /// </summary>
        public static double Sum(double[] a)
        {
            double sum = 0;
            for (int i = 0; i < a.Length; i++) sum += a[i];
            return sum;
        }
    }
}
