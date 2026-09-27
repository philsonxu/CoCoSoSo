using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// GELU激活函数：Gaussian Error Linear Unit
    /// Transformer标准激活函数，计算公式：0.5*x*(1 + tanh(sqrt(2/pi)*(x + 0.044715*x^3)))
    /// </summary>
    public class GELU
    {
        private double[,] xCache; // 前向传播缓存，反向传播用

        /// <summary>
        /// 前向传播计算GELU激活
        /// </summary>
        public double[,] Forward(double[,] x)
        {
            xCache = x;
            int m = x.GetLength(0);
            int n = x.GetLength(1);
            double[,] y = new double[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    y[i, j] = GeluSingle(x[i, j]);
                }
            }
            return y;
        }

        /// <summary>
        /// 反向传播计算GELU对输入的导数
        /// dGELU/dx ≈ 0.5*tanh(0.79788x + 0.03567x^3) + (0.05351*x^2 + 0.79788)*x*(1 - tanh(...)^2)*0.5 + 0.5
        /// </summary>
        public double[,] Backward(double[,] dy)
        {
            int m = xCache.GetLength(0);
            int n = xCache.GetLength(1);
            double[,] dx = new double[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double x = xCache[i, j];
                    double grad = GeluGrad(x);
                    dx[i, j] = dy[i, j] * grad;
                }
            }
            return dx;
        }

        /// <summary>
        /// 单个值GELU计算
        /// </summary>
        public static double GeluSingle(double x)
        {
            // 0.5*x*(1 + tanh(sqrt(2/pi)*(x + 0.044715*x^3)))
            double c = Math.Sqrt(2.0 / Math.PI);
            return 0.5 * x * (1.0 + Math.Tanh(c * (x + 0.044715 * x * x * x)));
        }

        /// <summary>
        /// 单个值GELU导数计算
        /// </summary>
        public static double GeluGrad(double x)
        {
            double c = Math.Sqrt(2.0 / Math.PI);
            double inner = c * (x + 0.044715 * x * x * x);
            double tanhVal = Math.Tanh(inner);
            double tanhGrad = 1.0 - tanhVal * tanhVal;
            double derivative = 0.5 * (1.0 + tanhVal) + 0.5 * x * tanhGrad * c * (1.0 + 3 * 0.044715 * x * x);
            return derivative;
        }

        /// <summary>
        /// 重置梯度缓存
        /// </summary>
        public void ZeroGrad()
        {
            xCache = null;
        }

        /// <summary>
        /// 参数更新（GELU无参数，留空接口）
        /// </summary>
        public void Update(double lr)
        {
            // GELU没有可学习参数
        }
    }
}
