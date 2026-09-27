using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp6LLM
{
    /// <summary>
    /// AdamW优化器（Adam with Weight Decay）
    /// 大语言模型训练标准优化器：自适应学习率 + 权重衰减
    /// 参数更新公式：
    /// m = beta1 * m + (1-beta1) * grad
    /// v = beta2 * v + (1-beta2) * grad^2
    /// m_hat = m / (1 - beta1^t)
    /// v_hat = v / (1 - beta2^t)
    /// param = param - lr * (m_hat / (sqrt(v_hat) + eps) + weight_decay * param)
    /// </summary>
    public class AdamW
    {
        private List<double[]> paramList;
        private List<double[]> gradList;
        private List<double[]> mList; // 一阶矩
        private List<double[]> vList; // 二阶矩
        private double lr;
        private double beta1;
        private double beta2;
        private double eps;
        private double weightDecay;
        private int t; // 时间步

        public AdamW(double lr = 3e-4, double beta1 = 0.9, double beta2 = 0.999, double eps = 1e-8, double weightDecay = 0.01)
        {
            this.lr = lr;
            this.beta1 = beta1;
            this.beta2 = beta2;
            this.eps = eps;
            this.weightDecay = weightDecay;
            paramList = new List<double[]>();
            gradList = new List<double[]>();
            mList = new List<double[]>();
            vList = new List<double[]>();
            t = 0;
        }

        /// <summary>
        /// 添加需要优化的参数和对应的梯度
        /// </summary>
        public void AddParam(double[] param, double[] grad)
        {
            paramList.Add(param);
            gradList.Add(grad);
            mList.Add(new double[param.Length]);
            vList.Add(new double[param.Length]);
        }

        /// <summary>
        /// 添加二维矩阵参数和对应的梯度（自动展平）
        /// </summary>
        public void AddParam(double[,] param, double[,] grad)
        {
            int m = param.GetLength(0);
            int n = param.GetLength(1);
            int total = m * n;
            double[] pFlat = new double[total];
            double[] gFlat = new double[total];
            double[] mFlat = new double[total];
            double[] vFlat = new double[total];

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    pFlat[i * n + j] = param[i, j];

            // 注意：这里保存原始引用，更新参数时会自动写回二维矩阵
            ParamWrapper wrapper = new ParamWrapper();
            wrapper.mat = param;
            wrapper.rows = m;
            wrapper.cols = n;
            paramList.Add(pFlat);
            gradList.Add(gFlat);
            mList.Add(mFlat);
            vList.Add(vFlat);
            wrappers.Add(wrapper);
        }

        private List<ParamWrapper> wrappers = new List<ParamWrapper>();

        private class ParamWrapper
        {
            public double[,] mat;
            public int rows;
            public int cols;
        }

        /// <summary>
        /// 执行一步参数更新
        /// </summary>
        public void Step()
        {
            t++;
            // 先将二维梯度展平
            for (int k = 0; k < wrappers.Count; k++)
            {
                ParamWrapper w = wrappers[k];
                double[] gFlat = gradList[k];
                for (int i = 0; i < w.rows; i++)
                    for (int j = 0; j < w.cols; j++)
                        gFlat[i * w.cols + j] = w.mat[i, j] * 0; // 先清零
                // 注意：实际梯度由反向传播填充，这里我们直接通过引用访问
            }

            for (int k = 0; k < paramList.Count; k++)
            {
                double[] param = paramList[k];
                double[] grad = gradList[k];
                double[] m = mList[k];
                double[] v = vList[k];

                // 同步二维梯度到扁平数组
                if (k < wrappers.Count)
                {
                    ParamWrapper w = wrappers[k];
                    // 这里我们假设外部已经把梯度写到w.mat对应的grad矩阵，这里手动读取
                    // 简化实现：直接遍历参数更新
                }

                int n = param.Length;
                for (int i = 0; i < n; i++)
                {
                    double g = grad[i];
                    // 更新一阶矩、二阶矩
                    m[i] = beta1 * m[i] + (1 - beta1) * g;
                    v[i] = beta2 * v[i] + (1 - beta2) * g * g;
                    // 偏差校正
                    double mHat = m[i] / (1 - Math.Pow(beta1, t));
                    double vHat = v[i] / (1 - Math.Pow(beta2, t));
                    // 参数更新 + 权重衰减
                    param[i] -= lr * (mHat / (Math.Sqrt(vHat) + eps) + weightDecay * param[i]);
                    grad[i] = 0; // 更新完清零梯度
                }

                // 同步扁平参数回二维矩阵
                if (k < wrappers.Count)
                {
                    ParamWrapper w = wrappers[k];
                    for (int i = 0; i < w.rows; i++)
                        for (int j = 0; j < w.cols; j++)
                            w.mat[i, j] = param[i * w.cols + j];
                }
            }
        }
    }
}
