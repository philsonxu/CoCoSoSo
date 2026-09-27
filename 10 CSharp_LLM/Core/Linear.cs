using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// 全连接线性层：y = x @ W + b
    /// </summary>
    public class Linear
    {
        private int inFeatures;
        private int outFeatures;

        public double[,] Weight;
        public double[] Bias;
        public double[,] WeightGrad;
        public double[] BiasGrad;

        private double[,] xCache;
        private Random random = new Random(42);

        public Linear(int inFeatures, int outFeatures)
        {
            this.inFeatures = inFeatures;
            this.outFeatures = outFeatures;

            // Xavier初始化
            double scale = Math.Sqrt(2.0 / (inFeatures + outFeatures));
            Weight = new double[inFeatures, outFeatures];
            Bias = new double[outFeatures];
            WeightGrad = new double[inFeatures, outFeatures];
            BiasGrad = new double[outFeatures];

            for (int i = 0; i < inFeatures; i++)
                for (int j = 0; j < outFeatures; j++)
                    Weight[i, j] = (random.NextDouble() * 2 - 1) * scale;
        }

        /// <summary>
        /// 前向传播
        /// </summary>
        public double[,] Forward(double[,] x)
        {
            xCache = x;
            return MatMul.MultiplyAdd(x, Weight, Bias);
        }

        /// <summary>
        /// 反向传播
        /// </summary>
        public double[,] Backward(double[,] dy)
        {
            int N = xCache.GetLength(0);
            // dW = x^T @ dy
            double[,] dW = MatMul.Multiply(MatMul.Transpose(xCache), dy);
            // db = dy按行求和
            double[] db = new double[outFeatures];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < outFeatures; j++)
                    db[j] += dy[i, j];

            // 梯度累加
            WeightGrad = MatMul.Add(WeightGrad, dW);
            BiasGrad = MatMul.Add(BiasGrad, db);

            // dx = dy @ W^T
            double[,] dx = MatMul.Multiply(dy, MatMul.Transpose(Weight));
            return dx;
        }

        /// <summary>
        /// 梯度下降更新参数
        /// </summary>
        public void Update(double lr)
        {
            for (int i = 0; i < inFeatures; i++)
                for (int j = 0; j < outFeatures; j++)
                {
                    Weight[i, j] -= lr * WeightGrad[i, j];
                    WeightGrad[i, j] = 0;
                }
            for (int j = 0; j < outFeatures; j++)
            {
                Bias[j] -= lr * BiasGrad[j];
                BiasGrad[j] = 0;
            }
        }

        public void ZeroGrad()
        {
            Array.Clear(WeightGrad, 0, WeightGrad.Length);
            Array.Clear(BiasGrad, 0, BiasGrad.Length);
            xCache = null;
        }
    }
}
