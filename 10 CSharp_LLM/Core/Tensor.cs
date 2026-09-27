using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// 简化版张量类，支持自动微分
    /// 为了教学简洁，二维张量版本足以支撑大语言模型教学演示
    /// </summary>
    public class Tensor
    {
        public double[,] Data;
        public double[,] Grad;
        public Func<double[,]> BackwardFunc;
        public List<Tensor> Parents;
        public string Name;

        public int Rows { get { return Data.GetLength(0); } }
        public int Cols { get { return Data.GetLength(1); } }

        public Tensor(int rows, int cols, string name = "")
        {
            Data = new double[rows, cols];
            Grad = new double[rows, cols];
            Parents = new List<Tensor>();
            Name = name;
        }

        public Tensor(double[,] data, string name = "")
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);
            Data = new double[rows, cols];
            Array.Copy(data, Data, data.Length);
            Grad = new double[rows, cols];
            Parents = new List<Tensor>();
            Name = name;
        }

        /// <summary>
        /// 清零梯度
        /// </summary>
        public void ZeroGrad()
        {
            Array.Clear(Grad, 0, Grad.Length);
        }

        /// <summary>
        /// 反向传播入口
        /// </summary>
        public void Backward()
        {
            // 拓扑排序，从输出到输入反向传播
            List<Tensor> topo = new List<Tensor>();
            HashSet<Tensor> visited = new HashSet<Tensor>();
            BuildTopo(this, topo, visited);

            // 输出梯度初始化为1
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    Grad[i, j] = 1.0;

            // 反向遍历执行反向传播
            for (int i = topo.Count - 1; i >= 0; i--)
            {
                if (topo[i].BackwardFunc != null)
                    topo[i].BackwardFunc();
            }
        }

        private void BuildTopo(Tensor node, List<Tensor> topo, HashSet<Tensor> visited)
        {
            if (visited.Contains(node)) return;
            visited.Add(node);
            foreach (Tensor parent in node.Parents)
                BuildTopo(parent, topo, visited);
            topo.Add(node);
        }

        // ---------------- 算子定义 ----------------

        /// <summary>
        /// 矩阵乘法：this @ other
        /// </summary>
        public Tensor MatMul(Tensor other)
        {
            Tensor outT = new Tensor(Rows, other.Cols);
            outT.Data = CSharp6LLM.MatMul.Multiply(Data, other.Data);
            outT.Parents.Add(this);
            outT.Parents.Add(other);
            outT.BackwardFunc = () =>
            {
                // dL/dA = dL/dC @ B^T
                double[,] dA = CSharp6LLM.MatMul.Multiply(outT.Grad, CSharp6LLM.MatMul.Transpose(other.Data));
                // dL/dB = A^T @ dL/dC
                double[,] dB = CSharp6LLM.MatMul.Multiply(CSharp6LLM.MatMul.Transpose(Data), outT.Grad);
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        Grad[i, j] += dA[i, j];
                for (int i = 0; i < other.Rows; i++)
                    for (int j = 0; j < other.Cols; j++)
                        other.Grad[i, j] += dB[i, j];
                return Grad;
            };
            return outT;
        }

        /// <summary>
        /// 逐元素加
        /// </summary>
        public Tensor Add(Tensor other)
        {
            Tensor outT = new Tensor(Rows, Cols);
            outT.Data = CSharp6LLM.MatMul.Add(Data, other.Data);
            outT.Parents.Add(this);
            outT.Parents.Add(other);
            outT.BackwardFunc = () =>
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        Grad[i, j] += outT.Grad[i, j];
                        other.Grad[i, j] += outT.Grad[i, j];
                    }
                }
                return Grad;
            };
            return outT;
        }

        /// <summary>
        /// 加偏置（向量广播到每一行）
        /// </summary>
        public Tensor AddBias(double[] bias)
        {
            Tensor outT = new Tensor(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    outT.Data[i, j] = Data[i, j] + bias[j];
            outT.Parents.Add(this);
            outT.BackwardFunc = () =>
            {
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        Grad[i, j] += outT.Grad[i, j];
                return Grad;
            };
            return outT;
        }

        /// <summary>
        /// GELU激活
        /// </summary>
        public Tensor GELU()
        {
            Tensor outT = new Tensor(Rows, Cols);
            double[,] x = Data;
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    outT.Data[i, j] = CSharp6LLM.GELU.GeluSingle(x[i, j]);
            outT.Parents.Add(this);
            outT.BackwardFunc = () =>
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        double grad = CSharp6LLM.GELU.GeluGrad(x[i, j]);
                        Grad[i, j] += outT.Grad[i, j] * grad;
                    }
                }
                return Grad;
            };
            return outT;
        }

        /// <summary>
        /// Softmax交叉熵损失：输入是logits，返回损失标量（这里简化为1x1张量）
        /// </summary>
        public Tensor CrossEntropy(int[] targets)
        {
            int N = Rows;
            int V = Cols;
            double totalLoss = 0;
            double[,] probs = new double[N, V];

            // 计算softmax概率和loss
            for (int i = 0; i < N; i++)
            {
                double max = Data[i, 0];
                for (int j = 1; j < V; j++) if (Data[i, j] > max) max = Data[i, j];
                double sum = 0;
                for (int j = 0; j < V; j++)
                {
                    probs[i, j] = Math.Exp(Data[i, j] - max);
                    sum += probs[i, j];
                }
                for (int j = 0; j < V; j++)
                    probs[i, j] /= sum;

                int t = targets[i];
                totalLoss += -Math.Log(probs[i, t] + 1e-9);
            }
            totalLoss /= N;

            Tensor loss = new Tensor(1, 1);
            loss.Data[0, 0] = totalLoss;
            loss.Parents.Add(this);
            loss.BackwardFunc = () =>
            {
                // dL/dlogits = (probs - target) / N
                double scale = outGradScale / N;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < V; j++)
                    {
                        Grad[i, j] = (probs[i, j] - (j == targets[i] ? 1.0 : 0.0)) * scale;
                    }
                }
                return Grad;
            };
            return loss;
        }

        private double outGradScale = 1.0;

        public void SetOutGradScale(double s)
        {
            outGradScale = s;
        }
    }
}
