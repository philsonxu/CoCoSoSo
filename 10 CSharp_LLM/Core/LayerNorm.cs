using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// 层归一化（Layer Normalization）
    /// 对每个样本的特征维度做归一化，Transformer Pre-LN结构核心组件
    /// 公式：y = gamma * (x - mean) / sqrt(var + eps) + beta
    /// </summary>
    public class LayerNorm
    {
        private int dim;
        private double eps = 1e-5;

        // 可学习参数
        public double[] gamma; // 缩放参数
        public double[] beta;  // 平移参数

        // 梯度
        public double[] gammaGrad;
        public double[] betaGrad;

        // 反向传播缓存
        private double[,] xNorm;
        private double[,] xCache;
        private double[] meanCache;
        private double[] varCache;

        public LayerNorm(int dim)
        {
            this.dim = dim;
            // 参数初始化：gamma初始为1，beta初始为0
            gamma = new double[dim];
            beta = new double[dim];
            gammaGrad = new double[dim];
            betaGrad = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                gamma[i] = 1.0;
                beta[i] = 0.0;
            }
        }

        /// <summary>
        /// 前向传播层归一化
        /// x形状：[batchSize, seqLen, dim] 或 [N, dim]
        /// 这里实现二维版本：[N, dim]，三维数据展平为[N*seqLen, dim]处理
        /// </summary>
        public double[,] Forward(double[,] x)
        {
            int N = x.GetLength(0);
            int d = x.GetLength(1);
            xCache = x;
            xNorm = new double[N, d];
            meanCache = new double[N];
            varCache = new double[N];

            for (int i = 0; i < N; i++)
            {
                // 计算均值
                double mean = 0;
                for (int j = 0; j < d; j++)
                    mean += x[i, j];
                mean /= d;
                meanCache[i] = mean;

                // 计算方差
                double var = 0;
                for (int j = 0; j < d; j++)
                {
                    double diff = x[i, j] - mean;
                    var += diff * diff;
                }
                var /= d;
                varCache[i] = var;

                // 归一化 + gamma beta变换
                double std = Math.Sqrt(var + eps);
                for (int j = 0; j < d; j++)
                {
                    xNorm[i, j] = (x[i, j] - mean) / std;
                }
            }

            // 应用gamma和beta
            double[,] y = new double[N, d];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    y[i, j] = gamma[j] * xNorm[i, j] + beta[j];
                }
            }
            return y;
        }

        /// <summary>
        /// 反向传播
        /// </summary>
        public double[,] Backward(double[,] dy)
        {
            int N = dy.GetLength(0);
            int d = dy.GetLength(1);
            double[,] dx = new double[N, d];

            // 先清零梯度
            Array.Clear(gammaGrad, 0, d);
            Array.Clear(betaGrad, 0, d);

            for (int i = 0; i < N; i++)
            {
                double std = Math.Sqrt(varCache[i] + eps);
                double invStd = 1.0 / std;

                // beta梯度就是dy求和
                for (int j = 0; j < d; j++)
                {
                    betaGrad[j] += dy[i, j];
                    gammaGrad[j] += dy[i, j] * xNorm[i, j];
                }

                // dx计算
                double dxNormSum = 0;
                double dxNormXSum = 0;
                for (int j = 0; j < d; j++)
                {
                    double dxNorm = dy[i, j] * gamma[j];
                    dxNormSum += dxNorm;
                    dxNormXSum += dxNorm * xNorm[i, j];
                }

                for (int j = 0; j < d; j++)
                {
                    double dxNorm = dy[i, j] * gamma[j];
                    dx[i, j] = (1.0 / d) * invStd * (d * dxNorm - dxNormSum - xNorm[i, j] * dxNormXSum);
                }
            }

            return dx;
        }

        /// <summary>
        /// 参数更新：简单SGD
        /// </summary>
        public void Update(double lr)
        {
            for (int i = 0; i < dim; i++)
            {
                gamma[i] -= lr * gammaGrad[i];
                beta[i] -= lr * betaGrad[i];
            }
        }

        /// <summary>
        /// 清零梯度
        /// </summary>
        public void ZeroGrad()
        {
            Array.Clear(gammaGrad, 0, dim);
            Array.Clear(betaGrad, 0, dim);
            xCache = null;
            xNorm = null;
            meanCache = null;
            varCache = null;
        }
    }
}
