using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// Token嵌入层 + 位置编码层
    /// 将token索引映射为向量，并加上可学习的位置嵌入
    /// </summary>
    public class Embedding
    {
        private int vocabSize;
        private int nEmb;
        private int maxSeqLen;

        public double[,] TokenEmb;    // [vocabSize, nEmb] token嵌入权重
        public double[,] PosEmb;      // [maxSeqLen, nEmb] 位置嵌入权重
        public double[,] TokenEmbGrad;
        public double[,] PosEmbGrad;

        private int[] idxCache;
        private int seqLenCache;
        private Random random = new Random(42);

        public Embedding(int vocabSize, int nEmb, int maxSeqLen)
        {
            this.vocabSize = vocabSize;
            this.nEmb = nEmb;
            this.maxSeqLen = maxSeqLen;

            double scale = Math.Sqrt(2.0 / nEmb);
            TokenEmb = new double[vocabSize, nEmb];
            PosEmb = new double[maxSeqLen, nEmb];
            TokenEmbGrad = new double[vocabSize, nEmb];
            PosEmbGrad = new double[maxSeqLen, nEmb];

            // 随机初始化
            for (int i = 0; i < vocabSize; i++)
                for (int j = 0; j < nEmb; j++)
                    TokenEmb[i, j] = (random.NextDouble() * 2 - 1) * scale * 0.1;
            for (int i = 0; i < maxSeqLen; i++)
                for (int j = 0; j < nEmb; j++)
                    PosEmb[i, j] = (random.NextDouble() * 2 - 1) * scale * 0.1;
        }

        /// <summary>
        /// 前向传播：输入token索引，返回嵌入向量
        /// </summary>
        /// <param name="idx">[batchSize*seqLen] token索引数组</param>
        /// <param name="seqLen">序列长度</param>
        /// <returns>[total, nEmb] 嵌入向量</returns>
        public double[,] Forward(int[] idx, int seqLen)
        {
            int total = idx.Length;
            idxCache = idx;
            seqLenCache = seqLen;
            double[,] x = new double[total, nEmb];

            for (int b = 0; b < total / seqLen; b++)
            {
                for (int t = 0; t < seqLen; t++)
                {
                    int i = b * seqLen + t;
                    int tokenId = idx[i];
                    for (int j = 0; j < nEmb; j++)
                    {
                        x[i, j] = TokenEmb[tokenId, j] + PosEmb[t, j];
                    }
                }
            }
            return x;
        }

        /// <summary>
        /// 反向传播
        /// </summary>
        public double[,] Backward(double[,] dx)
        {
            int total = dx.GetLength(0);
            int seqLen = seqLenCache;

            for (int b = 0; b < total / seqLen; b++)
            {
                for (int t = 0; t < seqLen; t++)
                {
                    int i = b * seqLen + t;
                    int tokenId = idxCache[i];
                    for (int j = 0; j < nEmb; j++)
                    {
                        TokenEmbGrad[tokenId, j] += dx[i, j];
                        PosEmbGrad[t, j] += dx[i, j];
                    }
                }
            }

            // Embedding是第一层，不需要传回dx
            return dx;
        }

        public void Update(double lr)
        {
            // 更新token嵌入
            for (int i = 0; i < vocabSize; i++)
                for (int j = 0; j < nEmb; j++)
                {
                    TokenEmb[i, j] -= lr * TokenEmbGrad[i, j];
                    TokenEmbGrad[i, j] = 0;
                }
            // 更新位置嵌入
            for (int i = 0; i < maxSeqLen; i++)
                for (int j = 0; j < nEmb; j++)
                {
                    PosEmb[i, j] -= lr * PosEmbGrad[i, j];
                    PosEmbGrad[i, j] = 0;
                }
        }

        public void ZeroGrad()
        {
            Array.Clear(TokenEmbGrad, 0, TokenEmbGrad.Length);
            Array.Clear(PosEmbGrad, 0, PosEmbGrad.Length);
            idxCache = null;
        }
    }
}
