using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// 迷你版GPT语言模型
    /// 架构：Token Embedding + Position Embedding -> N x Transformer Block -> LayerNorm -> Linear输出到词表
    /// </summary>
    public class NanoGPT
    {
        public int VocabSize;
        public int NEmb;
        public int NHead;
        public int NLayers;
        public int MaxSeqLen;

        public Embedding embedding;
        public List<TransformerBlock> blocks;
        public LayerNorm lnFinal;
        public Linear lmHead;

        private Random random = new Random(42);

        public NanoGPT(int vocabSize, int nEmb = 64, int nHead = 4, int nLayers = 2, int maxSeqLen = 64)
        {
            VocabSize = vocabSize;
            NEmb = nEmb;
            NHead = nHead;
            NLayers = nLayers;
            MaxSeqLen = maxSeqLen;

            // 初始化各层
            embedding = new Embedding(vocabSize, nEmb, maxSeqLen);
            blocks = new List<TransformerBlock>();
            for (int i = 0; i < nLayers; i++)
            {
                blocks.Add(new TransformerBlock(nEmb, nHead, maxSeqLen));
            }
            lnFinal = new LayerNorm(nEmb);
            lmHead = new Linear(nEmb, vocabSize);
        }

        /// <summary>
        /// 前向传播，返回logits
        /// </summary>
        public double[,] Forward(int[] idx, int batchSize, int seqLen)
        {
            double[,] x = embedding.Forward(idx, seqLen);
            for (int i = 0; i < NLayers; i++)
            {
                x = blocks[i].Forward(x, batchSize, seqLen);
            }
            x = lnFinal.Forward(x);
            double[,] logits = lmHead.Forward(x);
            return logits;
        }

        /// <summary>
        /// 计算交叉熵损失
        /// </summary>
        public double ComputeLoss(double[,] logits, int[] targets)
        {
            int N = logits.GetLength(0);
            int V = logits.GetLength(1);
            double totalLoss = 0;

            for (int i = 0; i < N; i++)
            {
                // 找最大值防止溢出
                double maxLogit = logits[i, 0];
                for (int j = 1; j < V; j++) if (logits[i, j] > maxLogit) maxLogit = logits[i, j];
                double sum = 0;
                for (int j = 0; j < V; j++)
                    sum += Math.Exp(logits[i, j] - maxLogit);
                double logProb = logits[i, targets[i]] - maxLogit - Math.Log(sum);
                totalLoss += -logProb;
            }
            return totalLoss / N;
        }

        /// <summary>
        /// 反向传播
        /// </summary>
        public void Backward(double[,] logits, int[] targets)
        {
            int N = logits.GetLength(0);
            int V = logits.GetLength(1);

            // 计算logits梯度 dL/dlogits = softmax - onehot(target)
            double[,] dLogits = new double[N, V];
            for (int i = 0; i < N; i++)
            {
                double maxLogit = logits[i, 0];
                for (int j = 1; j < V; j++) if (logits[i, j] > maxLogit) maxLogit = logits[i, j];
                double sum = 0;
                double[] probs = new double[V];
                for (int j = 0; j < V; j++)
                {
                    probs[j] = Math.Exp(logits[i, j] - maxLogit);
                    sum += probs[j];
                }
                for (int j = 0; j < V; j++)
                {
                    probs[j] /= sum;
                    dLogits[i, j] = (probs[j] - (j == targets[i] ? 1.0 : 0.0)) / N;
                }
            }

            // 反向传播各层
            double[,] dx = lmHead.Backward(dLogits);
            dx = lnFinal.Backward(dx);
            for (int i = NLayers - 1; i >= 0; i--)
            {
                dx = blocks[i].Backward(dx);
            }
            embedding.Backward(dx);
        }

        /// <summary>
        /// SGD更新所有参数
        /// </summary>
        public void Update(double lr)
        {
            embedding.Update(lr);
            for (int i = 0; i < NLayers; i++)
            {
                blocks[i].Update(lr);
            }
            lnFinal.Update(lr);
            lmHead.Update(lr);
        }

        public void ZeroGrad()
        {
            embedding.ZeroGrad();
            for (int i = 0; i < NLayers; i++)
            {
                blocks[i].ZeroGrad();
            }
            lnFinal.ZeroGrad();
            lmHead.ZeroGrad();
        }

        /// <summary>
        /// 自回归生成文本
        /// </summary>
        /// <param name="startIds">起始token</param>
        /// <param name="maxNewTokens">最大生成长度</param>
        /// <param name="temperature">温度，越小越确定</param>
        public int[] Generate(int[] startIds, int maxNewTokens, double temperature = 0.8)
        {
            List<int> generated = new List<int>(startIds);

            for (int step = 0; step < maxNewTokens; step++)
            {
                // 截断到最大序列长度
                int[] inputIds;
                if (generated.Count > MaxSeqLen)
                {
                    inputIds = generated.Skip(generated.Count - MaxSeqLen).ToArray();
                }
                else
                {
                    inputIds = generated.ToArray();
                }

                int seqLen = inputIds.Length;
                int batchSize = 1;

                // 前向传播
                double[,] logits = Forward(inputIds, batchSize, seqLen);

                // 取最后一个位置的logits
                double[] lastLogits = new double[VocabSize];
                int lastRow = seqLen - 1;
                for (int j = 0; j < VocabSize; j++)
                    lastLogits[j] = logits[lastRow, j] / temperature;

                // Softmax
                double maxL = lastLogits[0];
                for (int j = 1; j < VocabSize; j++) if (lastLogits[j] > maxL) maxL = lastLogits[j];
                double sum = 0;
                double[] probs = new double[VocabSize];
                for (int j = 0; j < VocabSize; j++)
                {
                    probs[j] = Math.Exp(lastLogits[j] - maxL);
                    sum += probs[j];
                }
                for (int j = 0; j < VocabSize; j++) probs[j] /= sum;

                // 采样（简化为贪心取最大概率，教学稳定）
                int nextId = 0;
                double maxProb = 0;
                for (int j = 0; j < VocabSize; j++)
                {
                    if (probs[j] > maxProb)
                    {
                        maxProb = probs[j];
                        nextId = j;
                    }
                }

                generated.Add(nextId);

                // 遇到换行提前结束，方便演示
                char nextChar = ' ';
                if (nextId < ShakespeareDatasetGlobal.Chars.Count)
                    nextChar = ShakespeareDatasetGlobal.Chars[nextId];
            }

            return generated.ToArray();
        }

        /// <summary>
        /// 统计模型总参数量
        /// </summary>
        public long CountParameters()
        {
            long count = 0;
            count += VocabSize * NEmb; // token emb
            count += MaxSeqLen * NEmb; // pos emb
            for (int i = 0; i < NLayers; i++)
            {
                // attention: 4个线性层（Q,K,V,O）每个NEmb x NEmb + bias
                count += 4 * (NEmb * NEmb + NEmb);
                // layer norm 2个：gamma + beta
                count += 2 * 2 * NEmb;
                // FFN: fc1 NEmb->4NEmb, fc2 4NEmb->NEmb
                count += NEmb * NEmb * 4 + NEmb * 4;
                count += NEmb * 4 * NEmb + NEmb;
            }
            // final layer norm
            count += 2 * NEmb;
            // lm head
            count += NEmb * VocabSize + VocabSize;
            return count;
        }
    }

    /// <summary>
    /// 全局数据集访问，方便生成时使用
    /// </summary>
    public static class ShakespeareDatasetGlobal
    {
        public static List<char> Chars = new List<char>();
        public static Dictionary<char, int> CharToIdx = new Dictionary<char, int>();
    }
}
