using System;
using System.Collections.Generic;

namespace CSharp20AIFull.Algorithms.CNN
{
    /// <summary>
    /// 卷积层 2D C#2.0语法
    /// </summary>
    public class Conv2DLayer
    {
        public int InChannels;
        public int OutChannels;
        public int KernelSize;
        public int Stride;
        public int Padding;
        // 卷积核 [outC][inC][kH][kW]
        public double[][][][] Kernels;
        public double[] Biases;
        public double[][][][] KernelGrads;
        public double[] BiasGrads;
        private double _lr;
        // 缓存
        public double[][][] Input; // [inC][H][W]
        public int InputH, InputW;

        public Conv2DLayer(int inC, int outC, int kSize, int stride, int padding, double lr)
        {
            InChannels = inC;
            OutChannels = outC;
            KernelSize = kSize;
            Stride = stride;
            Padding = padding;
            _lr = lr;

            double scale = Math.Sqrt(2.0 / (inC * kSize * kSize));
            Random rand = new Random(42);
            Kernels = new double[outC][][][];
            KernelGrads = new double[outC][][][];
            for (int oc = 0; oc < outC; oc++)
            {
                Kernels[oc] = new double[inC][][];
                KernelGrads[oc] = new double[inC][][];
                for (int ic = 0; ic < inC; ic++)
                {
                    Kernels[oc][ic] = new double[kSize][];
                    KernelGrads[oc][ic] = new double[kSize][];
                    for (int kh = 0; kh < kSize; kh++)
                    {
                        Kernels[oc][ic][kh] = new double[kSize];
                        KernelGrads[oc][ic][kh] = new double[kSize];
                        for (int kw = 0; kw < kSize; kw++)
                        {
                            Kernels[oc][ic][kh][kw] = (rand.NextDouble() * 2 - 1) * scale;
                        }
                    }
                }
            }
            Biases = new double[outC];
            BiasGrads = new double[outC];
        }

        public double[][][] Forward(double[][][] input)
        {
            Input = input;
            int cIn = InChannels;
            int hIn = input[0].Length;
            int wIn = input[0][0].Length;
            InputH = hIn;
            InputW = wIn;
            int hOut = (hIn + 2 * Padding - KernelSize) / Stride + 1;
            int wOut = (wIn + 2 * Padding - KernelSize) / Stride + 1;

            double[][][] output = new double[OutChannels][][];
            for (int oc = 0; oc < OutChannels; oc++)
            {
                output[oc] = new double[hOut][];
                for (int oh = 0; oh < hOut; oh++)
                {
                    output[oc][oh] = new double[wOut];
                    for (int ow = 0; ow < wOut; ow++)
                    {
                        double sum = Biases[oc];
                        for (int ic = 0; ic < cIn; ic++)
                        {
                            for (int kh = 0; kh < KernelSize; kh++)
                            {
                                for (int kw = 0; kw < KernelSize; kw++)
                                {
                                    int ih = oh * Stride - Padding + kh;
                                    int iw = ow * Stride - Padding + kw;
                                    if (ih >= 0 && ih < hIn && iw >= 0 && iw < wIn)
                                    {
                                        sum += input[ic][ih][iw] * Kernels[oc][ic][kh][kw];
                                    }
                                }
                            }
                        }
                        output[oc][oh][ow] = sum;
                    }
                }
            }
            return output;
        }

        public double[][][] Backward(double[][][] gradOutput)
        {
            int hOut = gradOutput[0].Length;
            int wOut = gradOutput[0][0].Length;
            int hIn = InputH;
            int wIn = InputW;
            double[][][] gradInput = new double[InChannels][][];
            for (int ic = 0; ic < InChannels; ic++)
            {
                gradInput[ic] = new double[hIn][];
                for (int ih = 0; ih < hIn; ih++)
                    gradInput[ic][ih] = new double[wIn];
            }

            // 清零梯度
            for (int oc = 0; oc < OutChannels; oc++)
            {
                BiasGrads[oc] = 0;
                for (int ic = 0; ic < InChannels; ic++)
                    for (int kh = 0; kh < KernelSize; kh++)
                        for (int kw = 0; kw < KernelSize; kw++)
                            KernelGrads[oc][ic][kh][kw] = 0;
            }

            for (int oc = 0; oc < OutChannels; oc++)
            {
                for (int oh = 0; oh < hOut; oh++)
                {
                    for (int ow = 0; ow < wOut; ow++)
                    {
                        double go = gradOutput[oc][oh][ow];
                        BiasGrads[oc] += go;
                        for (int ic = 0; ic < InChannels; ic++)
                        {
                            for (int kh = 0; kh < KernelSize; kh++)
                            {
                                for (int kw = 0; kw < KernelSize; kw++)
                                {
                                    int ih = oh * Stride - Padding + kh;
                                    int iw = ow * Stride - Padding + kw;
                                    if (ih >= 0 && ih < hIn && iw >= 0 && iw < wIn)
                                    {
                                        KernelGrads[oc][ic][kh][kw] += go * Input[ic][ih][iw];
                                        gradInput[ic][ih][iw] += go * Kernels[oc][ic][kh][kw];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 更新参数
            for (int oc = 0; oc < OutChannels; oc++)
            {
                Biases[oc] -= _lr * BiasGrads[oc];
                for (int ic = 0; ic < InChannels; ic++)
                    for (int kh = 0; kh < KernelSize; kh++)
                        for (int kw = 0; kw < KernelSize; kw++)
                            Kernels[oc][ic][kh][kw] -= _lr * KernelGrads[oc][ic][kh][kw];
            }
            return gradInput;
        }
    }

    /// <summary>
    /// 最大池化层
    /// </summary>
    public class MaxPool2DLayer
    {
        public int PoolSize;
        public int Stride;
        public int[][][][] MaxIndices; // [c][oh][ow][2] 记录最大值位置
        public int InputH, InputW;

        public MaxPool2DLayer(int poolSize, int stride)
        {
            PoolSize = poolSize;
            Stride = stride;
        }

        public double[][][] Forward(double[][][] input)
        {
            int c = input.Length;
            int hIn = input[0].Length;
            int wIn = input[0][0].Length;
            InputH = hIn;
            InputW = wIn;
            int hOut = (hIn - PoolSize) / Stride + 1;
            int wOut = (wIn - PoolSize) / Stride + 1;

            MaxIndices = new int[c][][][];
            double[][][] output = new double[c][][];
            for (int ci = 0; ci < c; ci++)
            {
                output[ci] = new double[hOut][];
                MaxIndices[ci] = new int[hOut][][];
                for (int oh = 0; oh < hOut; oh++)
                {
                    output[ci][oh] = new double[wOut];
                    MaxIndices[ci][oh] = new int[wOut][];
                    for (int ow = 0; ow < wOut; ow++)
                    {
                        double max = double.MinValue;
                        int maxH = 0, maxW = 0;
                        for (int ph = 0; ph < PoolSize; ph++)
                        {
                            for (int pw = 0; pw < PoolSize; pw++)
                            {
                                int ih = oh * Stride + ph;
                                int iw = ow * Stride + pw;
                                if (input[ci][ih][iw] > max)
                                {
                                    max = input[ci][ih][iw];
                                    maxH = ih;
                                    maxW = iw;
                                }
                            }
                        }
                        output[ci][oh][ow] = max;
                        MaxIndices[ci][oh][ow] = new int[] { maxH, maxW };
                    }
                }
            }
            return output;
        }

        public double[][][] Backward(double[][][] gradOutput)
        {
            int c = gradOutput.Length;
            int hOut = gradOutput[0].Length;
            int wOut = gradOutput[0][0].Length;
            double[][][] gradInput = new double[c][][];
            for (int ci = 0; ci < c; ci++)
            {
                gradInput[ci] = new double[InputH][];
                for (int ih = 0; ih < InputH; ih++)
                    gradInput[ci][ih] = new double[InputW];

                for (int oh = 0; oh < hOut; oh++)
                {
                    for (int ow = 0; ow < wOut; ow++)
                    {
                        int[] idx = MaxIndices[ci][oh][ow];
                        gradInput[ci][idx[0]][idx[1]] += gradOutput[ci][oh][ow];
                    }
                }
            }
            return gradInput;
        }
    }

    /// <summary>
    /// 展平层
    /// </summary>
    public class FlattenLayer
    {
        public int C, H, W;
        public double[] InputShape;

        public double[] Forward(double[][][] input)
        {
            C = input.Length;
            H = input[0].Length;
            W = input[0][0].Length;
            double[] output = new double[C * H * W];
            int idx = 0;
            for (int ci = 0; ci < C; ci++)
                for (int hi = 0; hi < H; hi++)
                    for (int wi = 0; wi < W; wi++)
                        output[idx++] = input[ci][hi][wi];
            return output;
        }

        public double[][][] Backward(double[] gradOutput)
        {
            double[][][] gradInput = new double[C][][];
            int idx = 0;
            for (int ci = 0; ci < C; ci++)
            {
                gradInput[ci] = new double[H][];
                for (int hi = 0; hi < H; hi++)
                {
                    gradInput[ci][hi] = new double[W];
                    for (int wi = 0; wi < W; wi++)
                    {
                        gradInput[ci][hi][wi] = gradOutput[idx++];
                    }
                }
            }
            return gradInput;
        }
    }

    /// <summary>
    /// 简单CNN 网络 （适配小样本MNIST 快速训练）
    /// 结构：Conv1(1->4, k=3) -> ReLU -> MaxPool(2) -> Flatten -> Linear -> Softmax
    /// </summary>
    public class SimpleCNN
    {
        private Conv2DLayer _conv1;
        private ReLUActivation _relu1;
        private MaxPool2DLayer _pool1;
        private FlattenLayer _flatten;
        private LinearLayerDense _fc1;
        private SoftmaxActivation _softmax;
        private double _lr;

        public SimpleCNN(double lr)
        {
            _lr = lr;
            _conv1 = new Conv2DLayer(1, 4, 3, 1, 1, lr);
            _relu1 = new ReLUActivation();
            _pool1 = new MaxPool2DLayer(2, 2);
            _flatten = new FlattenLayer();
            // 28x28 -> conv -> 28x28 -> pool -> 14x14, 4*14*14=784
            _fc1 = new LinearLayerDense(4 * 14 * 14, 10, lr);
            _softmax = new SoftmaxActivation();
        }

        public double[] Forward(double[][][] image)
        {
            double[][][] x = _conv1.Forward(image);
            x = _relu1.Forward(x);
            x = _pool1.Forward(x);
            double[] flat = _flatten.Forward(x);
            double[] logits = _fc1.Forward(flat);
            double[] prob = _softmax.Forward(logits);
            return prob;
        }

        public double Train(double[][][] image, int label)
        {
            // Forward
            double[][][] x1 = _conv1.Forward(image);
            double[][][] x2 = _relu1.Forward(x1);
            double[][][] x3 = _pool1.Forward(x2);
            double[] x4 = _flatten.Forward(x3);
            double[] x5 = _fc1.Forward(x4);
            double[] prob = _softmax.Forward(x5);

            // Loss
            double loss = -Math.Log(prob[label] + 1e-10);

            // Backward
            double[] gradProb = new double[10];
            for (int i = 0; i < 10; i++) gradProb[i] = prob[i];
            gradProb[label] -= 1;

            double[] gradFc = _fc1.Backward(gradProb);
            double[][][] gradFlat = _flatten.Backward(gradFc);
            double[][][] gradPool = _pool1.Backward(gradFlat);
            double[][][] gradRelu = _relu1.Backward(gradPool);
            double[][][] gradConv = _conv1.Backward(gradRelu);
            return loss;
        }

        public int Predict(double[][][] image)
        {
            double[] prob = Forward(image);
            int idx = 0;
            double max = prob[0];
            for (int i = 1; i < 10; i++)
            {
                if (prob[i] > max) { max = prob[i]; idx = i; }
            }
            return idx;
        }

        public double Score(List<double[][][]> images, int[] labels)
        {
            int correct = 0;
            for (int i = 0; i < images.Count; i++)
            {
                if (Predict(images[i]) == labels[i]) correct++;
            }
            return (double)correct / images.Count;
        }
    }

    // ====================== 辅助层（用于CNN）======================
    public class ReLUActivation
    {
        public double[][][] Input;
        public double[][][] Forward(double[][][] x)
        {
            Input = x;
            int c = x.Length, h = x[0].Length, w = x[0][0].Length;
            double[][][] outm = new double[c][][];
            for (int i = 0; i < c; i++)
            {
                outm[i] = new double[h][];
                for (int j = 0; j < h; j++)
                {
                    outm[i][j] = new double[w];
                    for (int k = 0; k < w; k++)
                        outm[i][j][k] = Math.Max(0, x[i][j][k]);
                }
            }
            return outm;
        }
        public double[][][] Backward(double[][][] grad)
        {
            int c = grad.Length, h = grad[0].Length, w = grad[0][0].Length;
            double[][][] g = new double[c][][];
            for (int i = 0; i < c; i++)
            {
                g[i] = new double[h][];
                for (int j = 0; j < h; j++)
                {
                    g[i][j] = new double[w];
                    for (int k = 0; k < w; k++)
                        g[i][j][k] = Input[i][j][k] > 0 ? grad[i][j][k] : 0;
                }
            }
            return g;
        }
    }

    public class SoftmaxActivation
    {
        public double[] Output;
        public double[] Forward(double[] x)
        {
            int n = x.Length;
            double max = x[0];
            for (int i = 1; i < n; i++) if (x[i] > max) max = x[i];
            double sum = 0;
            double[] outm = new double[n];
            for (int i = 0; i < n; i++) { outm[i] = Math.Exp(x[i] - max); sum += outm[i]; }
            for (int i = 0; i < n; i++) outm[i] /= sum;
            Output = outm;
            return outm;
        }
    }

    public class LinearLayerDense
    {
        public double[] Weights; // [inDim * outDim]
        public double[] Biases;
        public int InDim, OutDim;
        private double _lr;
        public double[] Input;

        public LinearLayerDense(int inDim, int outDim, double lr)
        {
            InDim = inDim;
            OutDim = outDim;
            _lr = lr;
            double scale = Math.Sqrt(2.0 / (inDim + outDim));
            Random rand = new Random(42);
            Weights = new double[inDim * outDim];
            for (int i = 0; i < Weights.Length; i++)
                Weights[i] = (rand.NextDouble() * 2 - 1) * scale;
            Biases = new double[outDim];
        }

        public double[] Forward(double[] input)
        {
            Input = input;
            double[] output = new double[OutDim];
            Array.Copy(Biases, output, OutDim);
            for (int o = 0; o < OutDim; o++)
            {
                for (int i = 0; i < InDim; i++)
                {
                    output[o] += input[i] * Weights[i * OutDim + o];
                }
            }
            return output;
        }

        public double[] Backward(double[] gradOutput)
        {
            double[] gradInput = new double[InDim];
            for (int o = 0; o < OutDim; o++)
            {
                for (int i = 0; i < InDim; i++)
                {
                    gradInput[i] += gradOutput[o] * Weights[i * OutDim + o];
                    Weights[i * OutDim + o] -= _lr * gradOutput[o] * Input[i];
                }
                Biases[o] -= _lr * gradOutput[o];
            }
            return gradInput;
        }
    }
}
