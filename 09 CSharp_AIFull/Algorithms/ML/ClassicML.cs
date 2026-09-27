using System;
using System.Collections.Generic;

namespace CSharp20AIFull.Algorithms.ML
{
    /// <summary>
    /// K近邻算法 KNN
    /// </summary>
    public class KNN
    {
        private double[][] _trainX;
        private int[] _trainY;
        private int _k;

        public KNN(int k)
        {
            _k = k;
        }

        public void Fit(double[][] X, int[] y)
        {
            _trainX = X;
            _trainY = y;
        }

        public int Predict(double[] x)
        {
            int n = _trainX.Length;
            double[] distances = new double[n];
            for (int i = 0; i < n; i++)
            {
                distances[i] = MathUtils.EuclideanDistance(x, _trainX[i]);
            }

            // 排序取前k个
            int[] indices = new int[n];
            for (int i = 0; i < n; i++) indices[i] = i;
            Array.Sort(distances, indices);

            Dictionary<int, int> count = new Dictionary<int, int>();
            for (int i = 0; i < _k; i++)
            {
                int label = _trainY[indices[i]];
                if (count.ContainsKey(label)) count[label]++;
                else count[label] = 1;
            }

            int maxCount = -1;
            int result = 0;
            foreach (KeyValuePair<int, int> kv in count)
            {
                if (kv.Value > maxCount)
                {
                    maxCount = kv.Value;
                    result = kv.Key;
                }
            }
            return result;
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

    /// <summary>
    /// 决策树（ID3简化版）
    /// </summary>
    public class DecisionTree
    {
        private class TreeNode
        {
            public bool IsLeaf;
            public int Label;
            public int FeatureIndex;
            public double Threshold;
            public TreeNode Left;
            public TreeNode Right;
        }

        private TreeNode _root;
        private int _maxDepth;

        public DecisionTree(int maxDepth)
        {
            _maxDepth = maxDepth;
        }

        public void Fit(double[][] X, int[] y)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < X.Length; i++) indices.Add(i);
            _root = BuildTree(X, y, indices, 0);
        }

        private TreeNode BuildTree(double[][] X, int[] y, List<int> indices, int depth)
        {
            TreeNode node = new TreeNode();
            
            // 检查是否同类别
            bool same = true;
            int first = y[indices[0]];
            for (int i = 1; i < indices.Count; i++)
            {
                if (y[indices[i]] != first) { same = false; break; }
            }

            if (same || depth >= _maxDepth || indices.Count <= 2)
            {
                node.IsLeaf = true;
                node.Label = MajorityVote(y, indices);
                return node;
            }

            // 找最佳分割
            int bestFeature = 0;
            double bestThresh = 0;
            double bestGini = double.MaxValue;

            int numFeatures = X[0].Length;
            for (int f = 0; f < numFeatures; f++)
            {
                // 简单取几个阈值
                for (int t = 0; t < 5; t++)
                {
                    double thresh = (t + 1) / 6.0;
                    double gini = CalculateGini(X, y, indices, f, thresh);
                    if (gini < bestGini)
                    {
                        bestGini = gini;
                        bestFeature = f;
                        bestThresh = thresh;
                    }
                }
            }

            node.FeatureIndex = bestFeature;
            node.Threshold = bestThresh;

            List<int> left = new List<int>();
            List<int> right = new List<int>();
            for (int i = 0; i < indices.Count; i++)
            {
                int idx = indices[i];
                if (X[idx][bestFeature] <= bestThresh) left.Add(idx);
                else right.Add(idx);
            }

            if (left.Count == 0 || right.Count == 0)
            {
                node.IsLeaf = true;
                node.Label = MajorityVote(y, indices);
                return node;
            }

            node.Left = BuildTree(X, y, left, depth + 1);
            node.Right = BuildTree(X, y, right, depth + 1);
            return node;
        }

        private int MajorityVote(int[] y, List<int> indices)
        {
            Dictionary<int, int> cnt = new Dictionary<int, int>();
            for (int i = 0; i < indices.Count; i++)
            {
                int l = y[indices[i]];
                if (cnt.ContainsKey(l)) cnt[l]++;
                else cnt[l] = 1;
            }
            int max = -1, res = 0;
            foreach (KeyValuePair<int, int> kv in cnt)
            {
                if (kv.Value > max) { max = kv.Value; res = kv.Key; }
            }
            return res;
        }

        private double CalculateGini(double[][] X, int[] y, List<int> indices, int f, double thresh)
        {
            int n = indices.Count;
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            for (int i = 0; i < n; i++)
            {
                int idx = indices[i];
                if (X[idx][f] <= thresh) left.Add(y[idx]);
                else right.Add(y[idx]);
            }
            return (double)left.Count / n * GiniImpurity(left) + (double)right.Count / n * GiniImpurity(right);
        }

        private double GiniImpurity(List<int> labels)
        {
            if (labels.Count == 0) return 0;
            Dictionary<int, int> cnt = new Dictionary<int, int>();
            for (int i = 0; i < labels.Count; i++)
            {
                int l = labels[i];
                if (cnt.ContainsKey(l)) cnt[l]++;
                else cnt[l] = 1;
            }
            double impurity = 1;
            foreach (KeyValuePair<int, int> kv in cnt)
            {
                double p = (double)kv.Value / labels.Count;
                impurity -= p * p;
            }
            return impurity;
        }

        public int Predict(double[] x)
        {
            TreeNode node = _root;
            while (!node.IsLeaf)
            {
                if (x[node.FeatureIndex] <= node.Threshold) node = node.Left;
                else node = node.Right;
            }
            return node.Label;
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

    /// <summary>
    /// 朴素贝叶斯分类器
    /// </summary>
    public class NaiveBayes
    {
        private int _numClasses;
        private double[] _classPrior;
        private double[][] _mean;
        private double[][] _var;
        private const double VarSmoothing = 1e-9;

        public void Fit(double[][] X, int[] y)
        {
            int n = X.Length;
            int d = X[0].Length;
            
            // 统计类别数
            HashSet<int> classes = new HashSet<int>();
            for (int i = 0; i < n; i++) classes.Add(y[i]);
            _numClasses = classes.Count;
            _classPrior = new double[_numClasses];
            _mean = new double[_numClasses][];
            _var = new double[_numClasses][];
            for (int c = 0; c < _numClasses; c++)
            {
                _mean[c] = new double[d];
                _var[c] = new double[d];
            }

            // 按类别统计
            for (int c = 0; c < _numClasses; c++)
            {
                List<double[]> samples = new List<double[]>();
                for (int i = 0; i < n; i++)
                {
                    if (y[i] == c) samples.Add(X[i]);
                }
                _classPrior[c] = (double)samples.Count / n;

                // 均值
                for (int j = 0; j < d; j++)
                {
                    double sum = 0;
                    for (int i = 0; i < samples.Count; i++) sum += samples[i][j];
                    _mean[c][j] = sum / samples.Count;
                }

                // 方差
                for (int j = 0; j < d; j++)
                {
                    double sum = 0;
                    for (int i = 0; i < samples.Count; i++)
                    {
                        sum += (samples[i][j] - _mean[c][j]) * (samples[i][j] - _mean[c][j]);
                    }
                    _var[c][j] = sum / samples.Count + VarSmoothing;
                }
            }
        }

        private double GaussianPDF(double x, double mean, double var)
        {
            return Math.Exp(-(x - mean) * (x - mean) / (2 * var)) / Math.Sqrt(2 * Math.PI * var);
        }

        public int Predict(double[] x)
        {
            double[] logProb = new double[_numClasses];
            for (int c = 0; c < _numClasses; c++)
            {
                logProb[c] = Math.Log(_classPrior[c]);
                for (int j = 0; j < x.Length; j++)
                {
                    logProb[c] += Math.Log(GaussianPDF(x[j], _mean[c][j], _var[c][j]));
                }
            }
            return MathUtils.ArgMax(logProb);
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

    /// <summary>
    /// 逻辑回归
    /// </summary>
    public class LogisticRegression
    {
        private double[] _weights;
        private double _bias;
        private double _lr;
        private int _epochs;

        public LogisticRegression(double lr, int epochs)
        {
            _lr = lr;
            _epochs = epochs;
        }

        public void Fit(double[][] X, int[] y)
        {
            int n = X.Length;
            int d = X[0].Length;
            _weights = new double[d];
            _bias = 0;

            Random rand = new Random(42);
            for (int i = 0; i < d; i++) _weights[i] = (rand.NextDouble() * 2 - 1) * 0.01;

            for (int epoch = 0; epoch < _epochs; epoch++)
            {
                for (int i = 0; i < n; i++)
                {
                    double z = _bias;
                    for (int j = 0; j < d; j++) z += _weights[j] * X[i][j];
                    double pred = MathUtils.Sigmoid(z);
                    double error = pred - y[i];

                    for (int j = 0; j < d; j++)
                    {
                        _weights[j] -= _lr * error * X[i][j];
                    }
                    _bias -= _lr * error;
                }
            }
        }

        public double PredictProb(double[] x)
        {
            double z = _bias;
            for (int j = 0; j < x.Length; j++) z += _weights[j] * x[j];
            return MathUtils.Sigmoid(z);
        }

        public int Predict(double[] x)
        {
            return PredictProb(x) >= 0.5 ? 1 : 0;
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

    /// <summary>
    /// 线性回归
    /// </summary>
    public class LinearRegression
    {
        public double[] Weights;
        public double Bias;

        public void Fit(double[][] X, double[] y)
        {
            int n = X.Length;
            int d = X[0].Length;

            // 梯度下降
            double lr = 0.01;
            Weights = new double[d];
            Bias = 0;
            Random rand = new Random(42);
            for (int i = 0; i < d; i++) Weights[i] = (rand.NextDouble() * 2 - 1) * 0.01;

            for (int epoch = 0; epoch < 500; epoch++)
            {
                double[] dw = new double[d];
                double db = 0;
                for (int i = 0; i < n; i++)
                {
                    double pred = Bias;
                    for (int j = 0; j < d; j++) pred += Weights[j] * X[i][j];
                    double err = pred - y[i];
                    for (int j = 0; j < d; j++) dw[j] += err * X[i][j];
                    db += err;
                }
                for (int j = 0; j < d; j++) Weights[j] -= lr * dw[j] / n;
                Bias -= lr * db / n;
            }
        }

        public double Predict(double[] x)
        {
            double res = Bias;
            for (int j = 0; j < x.Length; j++) res += Weights[j] * x[j];
            return res;
        }

        public double MSE(double[][] X, double[] y)
        {
            double err = 0;
            for (int i = 0; i < X.Length; i++)
            {
                double pred = Predict(X[i]);
                err += (pred - y[i]) * (pred - y[i]);
            }
            return err / X.Length;
        }
    }

    /// <summary>
    /// K均值聚类
    /// </summary>
    public class KMeans
    {
        public int K;
        public double[][] Centroids;
        public int[] Labels;
        public int MaxIter = 100;

        public KMeans(int k)
        {
            K = k;
        }

        public void Fit(double[][] X)
        {
            int n = X.Length;
            int d = X[0].Length;
            Labels = new int[n];

            // 随机初始化中心
            List<int> indices = new List<int>();
            for (int i = 0; i < n; i++) indices.Add(i);
            MathUtils.Shuffle(indices);
            Centroids = new double[K][];
            for (int k = 0; k < K; k++)
            {
                Centroids[k] = (double[])X[indices[k]].Clone();
            }

            for (int iter = 0; iter < MaxIter; iter++)
            {
                // 分配标签
                for (int i = 0; i < n; i++)
                {
                    double minDist = double.MaxValue;
                    int minLabel = 0;
                    for (int k = 0; k < K; k++)
                    {
                        double dist = MathUtils.EuclideanDistance(X[i], Centroids[k]);
                        if (dist < minDist) { minDist = dist; minLabel = k; }
                    }
                    Labels[i] = minLabel;
                }

                // 更新中心
                int[] counts = new int[K];
                double[][] newCentroids = new double[K][];
                for (int k = 0; k < K; k++) newCentroids[k] = new double[d];

                for (int i = 0; i < n; i++)
                {
                    int l = Labels[i];
                    counts[l]++;
                    for (int j = 0; j < d; j++) newCentroids[l][j] += X[i][j];
                }

                bool changed = false;
                for (int k = 0; k < K; k++)
                {
                    if (counts[k] > 0)
                    {
                        for (int j = 0; j < d; j++)
                        {
                            newCentroids[k][j] /= counts[k];
                            if (Math.Abs(newCentroids[k][j] - Centroids[k][j]) > 1e-6) changed = true;
                        }
                        Centroids[k] = newCentroids[k];
                    }
                }

                if (!changed) break;
            }
        }

        public int Predict(double[] x)
        {
            double minDist = double.MaxValue;
            int minLabel = 0;
            for (int k = 0; k < K; k++)
            {
                double dist = MathUtils.EuclideanDistance(x, Centroids[k]);
                if (dist < minDist) { minDist = dist; minLabel = k; }
            }
            return minLabel;
        }
    }
}
