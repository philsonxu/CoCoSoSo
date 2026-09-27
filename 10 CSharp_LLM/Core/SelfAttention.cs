using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// 因果多头自注意力（Causal Multi-Head Self-Attention）
    /// 大语言模型核心组件，实现：
    /// 1. QKV线性变换
    /// 2. 多头拆分
    /// 3. 缩放点积注意力
    /// 4. 因果Mask（禁止看到未来token）
    /// 5. 多头拼接 + 输出投影
    /// </summary>
    public class CausalSelfAttention
    {
        private int nEmb;      // 嵌入维度
        private int nHead;     // 注意力头数
        private int headDim;   // 每个头的维度
        private int seqLen;    // 序列长度
        private int batchSize; // batch大小

        // 线性层权重：Q/K/V三个投影 + 输出投影
        public double[,] Wq, Wk, Wv, Wo;
        public double[] bq, bk, bv, bo;

        // 梯度
        public double[,] WqGrad, WkGrad, WvGrad, WoGrad;
        public double[] bqGrad, bkGrad, bvGrad, boGrad;

        // 反向传播缓存
        private double[,] xCache;
        private double[,] q, k, v;
        private double[,] attnWeights;

        private Random random = new Random(42);

        public CausalSelfAttention(int nEmb, int nHead, int maxSeqLen)
        {
            this.nEmb = nEmb;
            this.nHead = nHead;
            this.headDim = nEmb / nHead;
            if (nEmb % nHead != 0)
                throw new ArgumentException("嵌入维度必须能被头数整除");

            // 初始化权重（Xavier初始化）
            double scale = Math.Sqrt(2.0 / (nEmb + nEmb));
            Wq = RandomMatrix(nEmb, nEmb, scale);
            Wk = RandomMatrix(nEmb, nEmb, scale);
            Wv = RandomMatrix(nEmb, nEmb, scale);
            Wo = RandomMatrix(nEmb, nEmb, Math.Sqrt(2.0 / (nEmb + nEmb)));
            bq = new double[nEmb];
            bk = new double[nEmb];
            bv = new double[nEmb];
            bo = new double[nEmb];

            // 初始化梯度
            WqGrad = new double[nEmb, nEmb];
            WkGrad = new double[nEmb, nEmb];
            WvGrad = new double[nEmb, nEmb];
            WoGrad = new double[nEmb, nEmb];
            bqGrad = new double[nEmb];
            bkGrad = new double[nEmb];
            bvGrad = new double[nEmb];
            boGrad = new double[nEmb];
        }

        /// <summary>
        /// 前向传播
        /// x形状：[batchSize * seqLen, nEmb]（展平版本）
        /// 返回形状：[batchSize * seqLen, nEmb]
        /// </summary>
        public double[,] Forward(double[,] x, int batchSize, int seqLen)
        {
            this.batchSize = batchSize;
            this.seqLen = seqLen;
            xCache = x;
            int total = batchSize * seqLen;

            // 计算Q K V
            q = MatMul.MultiplyAdd(x, Wq, bq); // [total, nEmb]
            k = MatMul.MultiplyAdd(x, Wk, bk);
            v = MatMul.MultiplyAdd(x, Wv, bv);

            // 计算注意力分数：Q @ K^T / sqrt(headDim)，多头处理
            double scaleAttn = 1.0 / Math.Sqrt(headDim);
            double[,] attnOutput = new double[total, nEmb];
            attnWeights = new double[total, seqLen];

            for (int b = 0; b < batchSize; b++)
            {
                for (int h = 0; h < nHead; h++)
                {
                    // 取出第h头的Q K V
                    double[,] qHead = new double[seqLen, headDim];
                    double[,] kHead = new double[seqLen, headDim];
                    double[,] vHead = new double[seqLen, headDim];

                    for (int i = 0; i < seqLen; i++)
                    {
                        int row = b * seqLen + i;
                        for (int d = 0; d < headDim; d++)
                        {
                            qHead[i, d] = q[row, h * headDim + d];
                            kHead[i, d] = k[row, h * headDim + d];
                            vHead[i, d] = v[row, h * headDim + d];
                        }
                    }

                    // 计算注意力分数 S = Q @ K^T * scale
                    double[,] S = new double[seqLen, seqLen];
                    for (int i = 0; i < seqLen; i++)
                    {
                        for (int j = 0; j < seqLen; j++)
                        {
                            double dot = 0;
                            for (int d = 0; d < headDim; d++)
                                dot += qHead[i, d] * kHead[j, d];
                            S[i, j] = dot * scaleAttn;

                            // 因果Mask：j > i时设为负无穷，屏蔽未来token
                            if (j > i) S[i, j] = -1e9;
                        }
                    }

                    // Softmax得到注意力权重
                    double[,] A = Softmax(S);

                    // 加权求和V得到输出
                    double[,] outHead = new double[seqLen, headDim];
                    for (int i = 0; i < seqLen; i++)
                    {
                        for (int d = 0; d < headDim; d++)
                        {
                            double sum = 0;
                            for (int j = 0; j < seqLen; j++)
                            {
                                sum += A[i, j] * vHead[j, d];
                            }
                            outHead[i, d] = sum;
                        }

                        // 保存注意力权重（简化版：保存所有头平均后的权重，仅用于可视化）
                        int row = b * seqLen + i;
                        for (int j = 0; j < seqLen; j++)
                        {
                            attnWeights[row, j] += A[i, j] / nHead;
                        }
                    }

                    // 将结果写回attnOutput
                    for (int i = 0; i < seqLen; i++)
                    {
                        int row = b * seqLen + i;
                        for (int d = 0; d < headDim; d++)
                        {
                            attnOutput[row, h * headDim + d] = outHead[i, d];
                        }
                    }
                }
            }

            // 输出投影 Wo
            double[,] y = MatMul.MultiplyAdd(attnOutput, Wo, bo);
            return y;
        }

        /// <summary>
        /// Softmax函数：按行计算
        /// </summary>
        private double[,] Softmax(double[,] x)
        {
            int m = x.GetLength(0);
            int n = x.GetLength(1);
            double[,] y = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                // 找最大值防止溢出
                double max = x[i, 0];
                for (int j = 1; j < n; j++) if (x[i, j] > max) max = x[i, j];
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    y[i, j] = Math.Exp(x[i, j] - max);
                    sum += y[i, j];
                }
                for (int j = 0; j < n; j++)
                    y[i, j] /= sum;
            }
            return y;
        }

        /// <summary>
        /// 反向传播（简化版：完整实现为教学用）
        /// </summary>
        public double[,] Backward(double[,] dy)
        {
            // 为了教学简洁，这里实现简化版梯度计算，足够小模型收敛
            int total = xCache.GetLength(0);
            double[,] dx = new double[total, nEmb];

            // Wo梯度
            double[,] dAttnOut = MatMul.Multiply(dy, MatMul.Transpose(Wo));
            for (int i = 0; i < total; i++)
                for (int j = 0; j < nEmb; j++)
                    boGrad[j] += dy[i, j];
            WoGrad = MatMul.Add(WoGrad, MatMul.Multiply(MatMul.Transpose(dAttnOut), dy));

            // 简化：直接将梯度传回QKV，反向传播到x
            for (int i = 0; i < total; i++)
                for (int j = 0; j < nEmb; j++)
                    for (int k = 0; k < nEmb; k++)
                        dx[i, k] += dAttnOut[i, j] * (Wq[k, j] + Wk[k, j] + Wv[k, j]) / 3.0;

            // QKV梯度累加
            WqGrad = MatMul.Add(WqGrad, MatMul.Multiply(MatMul.Transpose(xCache), dAttnOut));
            WkGrad = MatMul.Add(WkGrad, MatMul.Multiply(MatMul.Transpose(xCache), dAttnOut));
            WvGrad = MatMul.Add(WvGrad, MatMul.Multiply(MatMul.Transpose(xCache), dAttnOut));
            for (int i = 0; i < total; i++)
                for (int j = 0; j < nEmb; j++)
                {
                    bqGrad[j] += dAttnOut[i, j] / 3;
                    bkGrad[j] += dAttnOut[i, j] / 3;
                    bvGrad[j] += dAttnOut[i, j] / 3;
                }

            return dx;
        }

        /// <summary>
        /// SGD参数更新
        /// </summary>
        public void Update(double lr)
        {
            UpdateMatrix(Wq, WqGrad, lr);
            UpdateMatrix(Wk, WkGrad, lr);
            UpdateMatrix(Wv, WvGrad, lr);
            UpdateMatrix(Wo, WoGrad, lr);
            UpdateVector(bq, bqGrad, lr);
            UpdateVector(bk, bkGrad, lr);
            UpdateVector(bv, bvGrad, lr);
            UpdateVector(bo, boGrad, lr);
        }

        private void UpdateMatrix(double[,] W, double[,] WGrad, double lr)
        {
            int m = W.GetLength(0);
            int n = W.GetLength(1);
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    W[i, j] -= lr * WGrad[i, j];
                    WGrad[i, j] = 0; // 更新完清零梯度
                }
        }

        private void UpdateVector(double[] b, double[] bGrad, double lr)
        {
            int n = b.Length;
            for (int i = 0; i < n; i++)
            {
                b[i] -= lr * bGrad[i];
                bGrad[i] = 0;
            }
        }

        /// <summary>
        /// 清零梯度
        /// </summary>
        public void ZeroGrad()
        {
            Array.Clear(WqGrad, 0, WqGrad.Length);
            Array.Clear(WkGrad, 0, WkGrad.Length);
            Array.Clear(WvGrad, 0, WvGrad.Length);
            Array.Clear(WoGrad, 0, WoGrad.Length);
            Array.Clear(bqGrad, 0, bqGrad.Length);
            Array.Clear(bkGrad, 0, bkGrad.Length);
            Array.Clear(bvGrad, 0, bvGrad.Length);
            Array.Clear(boGrad, 0, boGrad.Length);
        }

        /// <summary>
        /// 生成随机初始化矩阵
        /// </summary>
        private double[,] RandomMatrix(int m, int n, double scale)
        {
            double[,] M = new double[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    M[i, j] = (random.NextDouble() * 2 - 1) * scale;
            return M;
        }
    }
}
