using System;
using System.Collections.Generic;

namespace CSharp20AIFull.Algorithms.NN
{
    /// <summary>
    /// 全连接层
    /// </summary>
    public class LinearLayer
    {
        public Matrix Weights;
        public Matrix Bias;
        public Matrix WeightGrad;
        public Matrix BiasGrad;
        public Matrix Input;
        public Matrix Output;
        private double _lr;

        public LinearLayer(int inDim, int outDim, double lr)
        {
            _lr = lr;
            // Xavier初始化
            double scale = Math.Sqrt(2.0 / (inDim + outDim));
            Weights = Matrix.Random(inDim, outDim, scale, 42);
            Bias = Matrix.Zeros(1, outDim);
        }

        public Matrix Forward(Matrix input)
        {
            Input = input;
            Output = Matrix.Dot(input, Weights);
            for (int i = 0; i < Output.Rows; i++)
            {
                for (int j = 0; j < Output.Cols; j++)
                {
                    Output.Data[i][j] += Bias.Data[0][j];
                }
            }
            return Output;
        }

        public Matrix Backward(Matrix gradOutput)
        {
            // gradInput = gradOutput * W^T
            Matrix gradInput = Matrix.Dot(gradOutput, Weights.T());
            // weightGrad = Input^T * gradOutput
            WeightGrad = Matrix.Dot(Input.T(), gradOutput);
            // biasGrad = sum(gradOutput)
            BiasGrad = Matrix.Zeros(1, Bias.Cols);
            for (int i = 0; i < gradOutput.Rows; i++)
            {
                for (int j = 0; j < gradOutput.Cols; j++)
                {
                    BiasGrad.Data[0][j] += gradOutput.Data[i][j];
                }
            }
            // 更新参数
            Weights = Weights - WeightGrad * (_lr / gradOutput.Rows);
            Bias = Bias - BiasGrad * (_lr / gradOutput.Rows);
            return gradInput;
        }
    }

    /// <summary>
    /// ReLU激活层
    /// </summary>
    public class ReLULayer
    {
        public Matrix Input;

        public Matrix Forward(Matrix input)
        {
            Input = input;
            Matrix outm = input.Copy();
            outm.ApplyReLU();
            return outm;
        }

        public Matrix Backward(Matrix gradOutput)
        {
            Matrix grad = gradOutput.Copy();
            for (int i = 0; i < grad.Rows; i++)
            {
                for (int j = 0; j < grad.Cols; j++)
                {
                    if (Input.Data[i][j] <= 0) grad.Data[i][j] = 0;
                }
            }
            return grad;
        }
    }

    /// <summary>
    /// Sigmoid激活层
    /// </summary>
    public class SigmoidLayer
    {
        public Matrix Output;

        public Matrix Forward(Matrix input)
        {
            Output = input.Copy();
            Output.ApplySigmoid();
            return Output;
        }

        public Matrix Backward(Matrix gradOutput)
        {
            Matrix grad = Matrix.Zeros(gradOutput.Rows, gradOutput.Cols);
            for (int i = 0; i < grad.Rows; i++)
            {
                for (int j = 0; j < grad.Cols; j++)
                {
                    double o = Output.Data[i][j];
                    grad.Data[i][j] = gradOutput.Data[i][j] * o * (1 - o);
                }
            }
            return grad;
        }
    }

    /// <summary>
    /// 多层感知机 MLP
    /// </summary>
    public class MLP
    {
        private List<object> _layers;
        private double _lr;
        private int[] _layerSizes;

        public MLP(int[] layerSizes, double lr)
        {
            _layerSizes = layerSizes;
            _lr = lr;
            _layers = new List<object>();

            for (int i = 0; i < layerSizes.Length - 1; i++)
            {
                _layers.Add(new LinearLayer(layerSizes[i], layerSizes[i + 1], lr));
                if (i < layerSizes.Length - 2)
                {
                    _layers.Add(new ReLULayer());
                }
            }
        }

        public Matrix Forward(Matrix x)
        {
            Matrix outm = x;
            for (int i = 0; i < _layers.Count; i++)
            {
                object layer = _layers[i];
                if (layer is LinearLayer)
                    outm = ((LinearLayer)layer).Forward(outm);
                else if (layer is ReLULayer)
                    outm = ((ReLULayer)layer).Forward(outm);
                else if (layer is SigmoidLayer)
                    outm = ((SigmoidLayer)layer).Forward(outm);
            }
            // Softmax 输出
            outm.Softmax();
            return outm;
        }

        public double TrainOneBatch(Matrix x, Matrix y)
        {
            Matrix pred = Forward(x);
            // Cross entropy loss
            double loss = 0;
            Matrix grad = pred.Copy();
            for (int i = 0; i < y.Rows; i++)
            {
                for (int j = 0; j < y.Cols; j++)
                {
                    if (y.Data[i][j] == 1)
                    {
                        loss -= Math.Log(pred.Data[i][j] + 1e-10);
                        grad.Data[i][j] = (pred.Data[i][j] - 1);
                    }
                }
            }
            loss /= y.Rows;

            // Backward
            for (int i = _layers.Count - 1; i >= 0; i--)
            {
                object layer = _layers[i];
                if (layer is LinearLayer)
                    grad = ((LinearLayer)layer).Backward(grad);
                else if (layer is ReLULayer)
                    grad = ((ReLULayer)layer).Backward(grad);
                else if (layer is SigmoidLayer)
                    grad = ((SigmoidLayer)layer).Backward(grad);
            }
            return loss;
        }

        public int Predict(double[] x)
        {
            Matrix input = new Matrix(1, x.Length);
            for (int i = 0; i < x.Length; i++) input.Data[0][i] = x[i];
            Matrix pred = Forward(input);
            return MathUtils.ArgMax(pred, 0);
        }

        public double Score(double[][] X, int[] y)
        {
            int correct = 0;
            for (int i = 0; i < X.Length; i++)
            {
                if (Predict(X[i]) == y[i]) correct++;
            }
            return (double)correct / X.Length;
        }
    }
}
