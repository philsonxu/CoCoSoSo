using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// Transformer Block
    /// 结构：Pre-LN -> Self-Attention -> 残差连接 -> Pre-LN -> 前馈网络 -> 残差连接
    /// 前馈网络结构：Linear -> GELU -> Linear，隐层维度扩展为4倍nEmb
    /// </summary>
    public class TransformerBlock
    {
        private int nEmb;
        private int nHead;
        private int maxSeqLen;

        public LayerNorm ln1;
        public CausalSelfAttention attn;
        public LayerNorm ln2;
        public Linear fc1;
        public GELU gelu;
        public Linear fc2;

        public TransformerBlock(int nEmb, int nHead, int maxSeqLen)
        {
            this.nEmb = nEmb;
            this.nHead = nHead;
            this.maxSeqLen = maxSeqLen;

            ln1 = new LayerNorm(nEmb);
            attn = new CausalSelfAttention(nEmb, nHead, maxSeqLen);
            ln2 = new LayerNorm(nEmb);
            fc1 = new Linear(nEmb, nEmb * 4); // 扩维4倍
            gelu = new GELU();
            fc2 = new Linear(nEmb * 4, nEmb); // 映射回原始维度
        }

        /// <summary>
        /// 前向传播
        /// </summary>
        public double[,] Forward(double[,] x, int batchSize, int seqLen)
        {
            // 第一个残差：自注意力
            double[,] xNorm1 = ln1.Forward(x);
            double[,] attnOut = attn.Forward(xNorm1, batchSize, seqLen);
            x = MatMul.Add(x, attnOut); // 残差连接

            // 第二个残差：前馈网络
            double[,] xNorm2 = ln2.Forward(x);
            double[,] h = fc1.Forward(xNorm2);
            h = gelu.Forward(h);
            double[,] ffOut = fc2.Forward(h);
            x = MatMul.Add(x, ffOut); // 残差连接

            return x;
        }

        /// <summary>
        /// 反向传播
        /// </summary>
        public double[,] Backward(double[,] dx)
        {
            // 反向第二个残差
            double[,] dffOut = dx;
            double[,] dxNorm2 = fc2.Backward(dffOut);
            dxNorm2 = gelu.Backward(dxNorm2);
            dxNorm2 = fc1.Backward(dxNorm2);
            dx = MatMul.Add(dx, ln2.Backward(dxNorm2));

            // 反向第一个残差
            double[,] dattnOut = dx;
            double[,] dxNorm1 = attn.Backward(dattnOut);
            dx = MatMul.Add(dx, ln1.Backward(dxNorm1));

            return dx;
        }

        /// <summary>
        /// 参数更新SGD
        /// </summary>
        public void Update(double lr)
        {
            ln1.Update(lr);
            attn.Update(lr);
            ln2.Update(lr);
            fc1.Update(lr);
            fc2.Update(lr);
            gelu.Update(lr);
        }

        public void ZeroGrad()
        {
            ln1.ZeroGrad();
            attn.ZeroGrad();
            ln2.ZeroGrad();
            fc1.ZeroGrad();
            fc2.ZeroGrad();
            gelu.ZeroGrad();
        }
    }
}
