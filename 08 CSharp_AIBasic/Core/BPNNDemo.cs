using System;
using System.Collections.Generic;

namespace CSharp20AI
{
    class BPNNDemo : DemoBase
    {
        public override void Run(HtmlConsole c)
        {
            c.H1(_title);
            c.H2("算法原理");
            c.P("BP（Back Propagation，反向传播）神经网络是最经典的多层前馈神经网络，解决了单层感知机无法解决的非线性问题（如异或问题）。它包含输入层、隐藏层、输出层，通过反向传播误差，用梯度下降更新各层权重。");
            c.Code("BP神经网络训练两步走：\n1. 前向传播：输入从输入层→隐藏层→输出层，逐层计算输出\n2. 反向传播：从输出层→隐藏层→输入层，逐层计算误差梯度，更新权重和偏置\n激活函数使用Sigmoid：σ(x)=1/(1+e^-x)，导数：σ'(x) = σ(x)(1-σ(x))");
            c.Tip("理论证明：单隐藏层的神经网络只要隐藏层神经元足够多，可以以任意精度拟合任意连续函数。");

            c.H2("运行演示：解决XOR异或问题");
            c.P("异或问题是经典的非线性问题：输入相同输出0，输入不同输出1，单层感知机无法解决，我们用单隐藏层BP神经网络来解决。");
            c.H3("异或问题真值表");
            string[] headers = new string[]{"输入A", "输入B", "输出A XOR B"};
            string[,] rows = new string[,]
            {
                {"0", "0", "0"},
                {"0", "1", "1"},
                {"1", "0", "1"},
                {"1", "1", "0"}
            };
            c.Table(headers, rows);

            // 网络结构：2输入 -> 4隐藏 -> 1输出
            int inputSize = 2, hiddenSize = 4, outputSize = 1;
            double lr = 0.5;
            Random rand = new Random(123);
            // 初始化权重和偏置，(-1,1)随机初始化
            double[,] W1 = new double[inputSize, hiddenSize];
            double[] b1 = new double[hiddenSize];
            double[,] W2 = new double[hiddenSize, outputSize];
            double[] b2 = new double[outputSize];
            for (int i = 0; i < inputSize; i++)
                for (int j = 0; j < hiddenSize; j++)
                    W1[i,j] = rand.NextDouble()*2 - 1;
            for (int j = 0; j < hiddenSize; j++) b1[j] = rand.NextDouble()*2-1;
            for (int j = 0; j < hiddenSize; j++)
                for (int k = 0; k < outputSize; k++)
                    W2[j,k] = rand.NextDouble()*2-1;
            for (int k = 0; k < outputSize; k++) b2[k] = rand.NextDouble()*2-1;

            // 训练数据
            double[][] X = new double[][]
            {
                new double[]{0,0}, new double[]{0,1},
                new double[]{1,0}, new double[]{1,1}
            };
            double[][] Y = new double[][]
            {
                new double[]{0}, new double[]{1},
                new double[]{1}, new double[]{0}
            };
            int epochs = 10000;
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                double totalLoss = 0;
                for (int sample = 0; sample < 4; sample++)
                {
                    double[] x = X[sample];
                    double[] y = Y[sample];
                    // 前向传播
                    double[] z1 = new double[hiddenSize];
                    double[] a1 = new double[hiddenSize];
                    for (int j = 0; j < hiddenSize; j++)
                    {
                        z1[j] = b1[j];
                        for (int i = 0; i < inputSize; i++) z1[j] += x[i] * W1[i,j];
                        a1[j] = Sigmoid(z1[j]);
                    }
                    double[] z2 = new double[outputSize];
                    double[] a2 = new double[outputSize];
                    for (int k = 0; k < outputSize; k++)
                    {
                        z2[k] = b2[k];
                        for (int j = 0; j < hiddenSize; j++) z2[k] += a1[j] * W2[j,k];
                        a2[k] = Sigmoid(z2[k]);
                    }
                    // 计算损失MSE
                    for (int k = 0; k < outputSize; k++)
                        totalLoss += (a2[k] - y[k]) * (a2[k] - y[k]) * 0.5;
                    // 反向传播
                    double[] dz2 = new double[outputSize];
                    for (int k = 0; k < outputSize; k++)
                        dz2[k] = (a2[k] - y[k]) * a2[k] * (1 - a2[k]);
                    double[,] dW2 = new double[hiddenSize, outputSize];
                    double[] db2 = new double[outputSize];
                    for (int k = 0; k < outputSize; k++)
                    {
                        db2[k] = dz2[k];
                        for (int j = 0; j < hiddenSize; j++)
                            dW2[j,k] = a1[j] * dz2[k];
                    }
                    double[] dz1 = new double[hiddenSize];
                    for (int j = 0; j < hiddenSize; j++)
                    {
                        dz1[j] = 0;
                        for (int k = 0; k < outputSize; k++)
                            dz1[j] += W2[j,k] * dz2[k];
                        dz1[j] *= a1[j] * (1 - a1[j]);
                    }
                    double[,] dW1 = new double[inputSize, hiddenSize];
                    double[] db1 = new double[hiddenSize];
                    for (int j = 0; j < hiddenSize; j++)
                    {
                        db1[j] = dz1[j];
                        for (int i = 0; i < inputSize; i++)
                            dW1[i,j] = x[i] * dz1[j];
                    }
                    // 更新参数
                    for (int i = 0; i < inputSize; i++)
                        for (int j = 0; j < hiddenSize; j++)
                            W1[i,j] -= lr * dW1[i,j];
                    for (int j = 0; j < hiddenSize; j++) b1[j] -= lr * db1[j];
                    for (int j = 0; j < hiddenSize; j++)
                        for (int k = 0; k < outputSize; k++)
                            W2[j,k] -= lr * dW2[j,k];
                    for (int k = 0; k < outputSize; k++) b2[k] -= lr * db2[k];
                }
                totalLoss /= 4;
                if (epoch == 0 || epoch == 100 || epoch == 1000 || epoch == 9999)
                {
                    c.Result(string.Format("第{0,5}轮：平均损失MSE = {1:F6}", epoch+1, totalLoss));
                }
            }
            c.Success("训练完成！神经网络成功学习到了异或逻辑，损失已经非常接近0。");

            c.H3("训练后预测结果");
            for (int sample = 0; sample < 4; sample++)
            {
                double[] x = X[sample];
                // 前向传播预测
                double[] a1 = new double[hiddenSize];
                for (int j = 0; j < hiddenSize; j++)
                {
                    double z = b1[j];
                    for (int i = 0; i < inputSize; i++) z += x[i] * W1[i,j];
                    a1[j] = Sigmoid(z);
                }
                double pred = 0;
                for (int k = 0; k < outputSize; k++)
                {
                    double z = b2[k];
                    for (int j = 0; j < hiddenSize; j++) z += a1[j] * W2[j,k];
                    pred = Sigmoid(z);
                }
                int predInt = pred >= 0.5 ? 1 : 0;
                c.Result(string.Format("输入({0}, {1}) → 输出概率={2:F4}，预测结果={3}，正确标签={4} {5}",
                    (int)x[0], (int)x[1], pred, predInt, (int)Y[sample][0], predInt == (int)Y[sample][0] ? "✅" : "❌"));
            }
            c.H2("总结");
            c.P("BP神经网络是现代深度学习的基础，通过增加隐藏层和神经元，可以拟合任意复杂的非线性关系；缺点是容易陷入局部最优、需要大量数据、训练慢、超参数多。");
            c.P("本章节实现的是最基础的全连接神经网络，加深网络层、改用ReLU/Softmax激活、加入BatchNorm/正则化等，就构成了现代深度学习的基础组件。");
        }
    }
}
