using System;
using System.Collections.Generic;
using System.IO;

using CSharp20AIFull.Algorithms.CNN;

namespace CSharp20AIFull.Dataset
{
    /// <summary>
    /// MNIST数据集加载器 解析idx格式文件 C#2.0语法
    /// </summary>
    public class MnistDataset
    {
        public List<double[][]> TrainImages;
        public int[] TrainLabels;
        public List<double[][]> TestImages;
        public int[] TestLabels;
        public const int ImgSize = 28;

        public bool Load(string trainImagePath, string trainLabelPath, string testImagePath, string testLabelPath, int trainCount, int testCount)
        {
            TrainImages = new List<double[][]>();
            TestImages = new List<double[][]>();

            try
            {
                LoadImages(trainImagePath, TrainImages, trainCount);
                TrainLabels = LoadLabels(trainLabelPath, trainCount);
                LoadImages(testImagePath, TestImages, testCount);
                TestLabels = LoadLabels(testLabelPath, testCount);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 生成内置小样本手写数字数据集（不依赖外部文件）
        /// </summary>
        public void GenerateBuiltinDataset(int samplesPerClass)
        {
            TrainImages = new List<double[][]>();
            TestImages = new List<double[][]>();
            List<int> trainLabelsList = new List<int>();
            List<int> testLabelsList = new List<int>();
            Random rand = new Random(42);

            // 为每个数字生成原型模式
            for (int digit = 0; digit < 10; digit++)
            {
                double[][] prototype = GenerateDigitPrototype(digit);

                // 生成训练样本 带随机噪声
                for (int s = 0; s < samplesPerClass; s++)
                {
                    double[][] img = new double[ImgSize][];
                    for (int y = 0; y < ImgSize; y++)
                    {
                        img[y] = new double[ImgSize];
                        for (int x = 0; x < ImgSize; x++)
                        {
                            double noise = (rand.NextDouble() * 2 - 1) * 0.15;
                            double v = prototype[y][x] + noise;
                            if (v < 0) v = 0;
                            if (v > 1) v = 1;
                            img[y][x] = v;
                        }
                    }
                    TrainImages.Add(img);
                    trainLabelsList.Add(digit);

                    if (s < samplesPerClass / 5) // 20%作为测试集
                    {
                        double[][] testimg = new double[ImgSize][];
                        for (int y = 0; y < ImgSize; y++)
                        {
                            testimg[y] = new double[ImgSize];
                            for (int x = 0; x < ImgSize; x++)
                            {
                                double noise = (rand.NextDouble() * 2 - 1) * 0.2;
                                double v = prototype[y][x] + noise;
                                if (v < 0) v = 0;
                                if (v > 1) v = 1;
                                testimg[y][x] = v;
                            }
                        }
                        TestImages.Add(testimg);
                        testLabelsList.Add(digit);
                    }
                }
            }

            TrainLabels = trainLabelsList.ToArray();
            TestLabels = testLabelsList.ToArray();
        }

        private double[][] GenerateDigitPrototype(int digit)
        {
            bool[][] pattern = new bool[ImgSize][];
            for (int y = 0; y < ImgSize; y++)
            {
                pattern[y] = new bool[ImgSize];
            }

            switch (digit)
            {
                case 0: // 椭圆
                    DrawCircle(pattern, 14, 14, 10);
                    FillHollow(pattern);
                    break;
                case 1: // 竖线
                    DrawLine(pattern, 14, 4, 14, 24);
                    DrawLine(pattern, 10, 6, 14, 4);
                    DrawLine(pattern, 14, 24, 17, 24);
                    break;
                case 2: // 2
                    DrawArcTop(pattern);
                    DrawDiagonal(pattern, true);
                    DrawLine(pattern, 7, 24, 20, 24);
                    break;
                case 3: // 3
                    DrawArcTop(pattern);
                    DrawArcMiddle(pattern);
                    DrawArcBottom(pattern);
                    break;
                case 4: // 4
                    DrawLine(pattern, 7, 4, 7, 15);
                    DrawLine(pattern, 7, 14, 20, 14);
                    DrawLine(pattern, 20, 4, 20, 24);
                    break;
                case 5: // 5
                    DrawLine(pattern, 18, 4, 8, 4);
                    DrawLine(pattern, 8, 4, 8, 13);
                    DrawArcMiddle(pattern);
                    DrawLine(pattern, 8, 24, 20, 24);
                    break;
                case 6: // 6
                    DrawCircle(pattern, 10, 20, 6);
                    DrawArcTopLeft(pattern);
                    break;
                case 7: // 7
                    DrawLine(pattern, 6, 6, 22, 6);
                    DrawLine(pattern, 18, 6, 10, 24);
                    break;
                case 8: // 8
                    DrawCircle(pattern, 14, 10, 6);
                    DrawCircle(pattern, 14, 20, 6);
                    DrawLine(pattern, 14, 4, 14, 24);
                    break;
                case 9: // 9
                    DrawCircle(pattern, 14, 10, 6);
                    DrawLine(pattern, 20, 14, 14, 24);
                    break;
            }

            double[][] img = new double[ImgSize][];
            for (int y = 0; y < ImgSize; y++)
            {
                img[y] = new double[ImgSize];
                for (int x = 0; x < ImgSize; x++)
                {
                    img[y][x] = pattern[y][x] ? 1.0 : 0.0;
                }
            }
            return img;
        }

        private void DrawCircle(bool[][] p, int cx, int cy, int r)
        {
            for (int y = -r; y <= r; y++)
            {
                for (int x = -r; x <= r; x++)
                {
                    if (x * x + y * y >= (r - 1) * (r - 1) && x * x + y * y <= r * r)
                    {
                        int px = cx + x, py = cy + y;
                        if (px >= 0 && px < ImgSize && py >= 0 && py < ImgSize)
                            p[py][px] = true;
                    }
                }
            }
        }

        private void FillHollow(bool[][] p)
        {
            for (int y = 5; y < 23; y++)
            {
                bool inShape = false;
                for (int x = 0; x < ImgSize; x++)
                {
                    if (p[y][x]) inShape = !inShape;
                    if (inShape) p[y][x] = false;
                }
            }
        }

        private void DrawLine(bool[][] p, int x0, int y0, int x1, int y1)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;
            while (true)
            {
                if (x0 >= 0 && x0 < ImgSize && y0 >= 0 && y0 < ImgSize)
                    p[y0][x0] = true;
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        private void DrawArcTop(bool[][] p) { DrawLine(p, 8, 8, 20, 8); DrawLine(p, 8, 8, 8, 12); DrawLine(p, 20, 8, 20, 14); }
        private void DrawArcMiddle(bool[][] p) { DrawLine(p, 8, 14, 20, 14); DrawLine(p, 20, 14, 20, 20); DrawLine(p, 8, 14, 8, 20); }
        private void DrawArcBottom(bool[][] p) { DrawLine(p, 8, 20, 20, 20); }
        private void DrawDiagonal(bool[][] p, bool dir)
        {
            for (int i = 0; i < 12; i++)
            {
                int x = dir ? 20 - i : 8 + i;
                int y = 14 + i;
                if (x >= 0 && x < ImgSize && y >= 0 && y < ImgSize) p[y][x] = true;
            }
        }
        private void DrawArcTopLeft(bool[][] p) { DrawLine(p, 10, 14, 10, 8); DrawLine(p, 10, 8, 18, 8); DrawLine(p, 18, 8, 18, 14); }

        private void LoadImages(string path, List<double[][]> images, int count)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int magic = ReadInt32BigEndian(br);
                int numImages = ReadInt32BigEndian(br);
                int rows = ReadInt32BigEndian(br);
                int cols = ReadInt32BigEndian(br);
                int n = Math.Min(count, numImages);
                for (int i = 0; i < n; i++)
                {
                    double[][] img = new double[rows][];
                    for (int y = 0; y < rows; y++)
                    {
                        img[y] = new double[cols];
                        for (int x = 0; x < cols; x++)
                        {
                            byte b = br.ReadByte();
                            img[y][x] = b / 255.0;
                        }
                    }
                    images.Add(img);
                }
            }
        }

        private int[] LoadLabels(string path, int count)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int magic = ReadInt32BigEndian(br);
                int numLabels = ReadInt32BigEndian(br);
                int n = Math.Min(count, numLabels);
                int[] labels = new int[n];
                for (int i = 0; i < n; i++)
                {
                    labels[i] = br.ReadByte();
                }
                return labels;
            }
        }

        private int ReadInt32BigEndian(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);
            return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
        }
    }

    /// <summary>
    /// MNIST CNN训练器
    /// </summary>
    public class MnistTrainer
    {
        public List<double> TrainLosses = new List<double>();
        public List<double> Accuracies = new List<double>();
        public SimpleCNN CNN;
        public MnistDataset Dataset;
        public int Epochs = 3;
        public double LR = 0.01;

        public void PrepareDataset()
        {
            Dataset = new MnistDataset();
            // 优先加载真实MNIST，否则使用内置生成数据集
            bool loaded = Dataset.Load("train-images.idx3-ubyte", "train-labels.idx1-ubyte",
                "t10k-images.idx3-ubyte", "t10k-labels.idx1-ubyte", 2000, 500);
            if (!loaded)
            {
                Dataset.GenerateBuiltinDataset(30);
            }
        }

        public void BuildModel()
        {
            CNN = new SimpleCNN(LR);
        }

        public double Train()
        {
            PrepareDataset();
            BuildModel();

            double finalAcc = 0;
            for (int epoch = 0; epoch < Epochs; epoch++)
            {
                double totalLoss = 0;
                // 打乱
                List<int> indices = new List<int>();
                for (int i = 0; i < Dataset.TrainImages.Count; i++) indices.Add(i);
                MathUtils.Shuffle(indices);

                int batchSize = 1;
                for (int i = 0; i < indices.Count; i++)
                {
                    int idx = indices[i];
                    double[][][] img = new double[1][][];
                    img[0] = Dataset.TrainImages[idx];
                    double loss = CNN.Train(img, Dataset.TrainLabels[idx]);
                    totalLoss += loss;
                }

                double avgLoss = totalLoss / indices.Count;
                TrainLosses.Add(avgLoss);

                // 测试准确率
                List<double[][][]> testBatch = new List<double[][][]>();
                int testCount = Math.Min(100, Dataset.TestImages.Count);
                for (int i = 0; i < testCount; i++)
                {
                    double[][][] img = new double[1][][];
                    img[0] = Dataset.TestImages[i];
                    testBatch.Add(img);
                }
                int[] testLabelsSub = new int[testCount];
                Array.Copy(Dataset.TestLabels, testLabelsSub, testCount);
                double acc = CNN.Score(testBatch, testLabelsSub);
                Accuracies.Add(acc);
                finalAcc = acc;
            }
            return finalAcc;
        }
    }
}
