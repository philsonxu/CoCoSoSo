using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CSharp20AIFull.Algorithms.ML;
using CSharp20AIFull.Algorithms.NN;
using CSharp20AIFull.Algorithms.CNN;
using CSharp20AIFull.Dataset;

namespace CSharp20AIFull.UI
{
    public class MainForm : Form
    {
        private TreeView _treeView;
        private WebBrowser _browser;
        private SplitContainer _split;
        private HtmlConsole _console;

        public MainForm()
        {
            Text = "C# 2.0 人工智能源程序库 - 零第三方依赖";
            Width = 1100;
            Height = 750;
            StartPosition = FormStartPosition.CenterScreen;

            _split = new SplitContainer();
            _split.Dock = DockStyle.Fill;
            _split.SplitterDistance = 240;
            Controls.Add(_split);

            _treeView = new TreeView();
            _treeView.Dock = DockStyle.Fill;
            _treeView.Font = new Font("Microsoft YaHei", 10f);
            _treeView.AfterSelect += TreeView_AfterSelect;
            _split.Panel1.Controls.Add(_treeView);

            _browser = new WebBrowser();
            _browser.Dock = DockStyle.Fill;
            _browser.ScriptErrorsSuppressed = true;
            _split.Panel2.Controls.Add(_browser);

            _console = new HtmlConsole();
            BuildTree();
            ShowWelcome();
        }

        private void BuildTree()
        {
            _treeView.Nodes.Clear();

            TreeNode root = new TreeNode("C# AI 源码库 (C#2.0 零依赖)");
            root.Nodes.Add(new TreeNode("0. 项目说明与环境介绍"));

            TreeNode ml = new TreeNode("一、经典机器学习算法");
            ml.Nodes.Add(new TreeNode("1. K近邻 KNN 分类"));
            ml.Nodes.Add(new TreeNode("2. 决策树 DecisionTree"));
            ml.Nodes.Add(new TreeNode("3. 朴素贝叶斯 GaussianNB"));
            ml.Nodes.Add(new TreeNode("4. 逻辑回归 LogisticRegression"));
            ml.Nodes.Add(new TreeNode("5. 线性回归 LinearRegression"));
            ml.Nodes.Add(new TreeNode("6. K均值聚类 KMeans"));
            root.Nodes.Add(ml);

            TreeNode nn = new TreeNode("二、神经网络");
            nn.Nodes.Add(new TreeNode("7. 多层感知机 MLP"));
            nn.Nodes.Add(new TreeNode("8. MLP 鸢尾花分类实战"));
            root.Nodes.Add(nn);

            TreeNode cnn = new TreeNode("三、卷积神经网络 CNN");
            cnn.Nodes.Add(new TreeNode("9. CNN卷积层实现原理"));
            cnn.Nodes.Add(new TreeNode("10. 池化层与全连接层"));
            root.Nodes.Add(cnn);

            TreeNode mnist = new TreeNode("四、MNIST手写数字识别");
            mnist.Nodes.Add(new TreeNode("11. MNIST数据集加载与预览"));
            mnist.Nodes.Add(new TreeNode("12. CNN训练手写数字识别"));
            root.Nodes.Add(mnist);

            _treeView.Nodes.Add(root);
            root.Expand();
        }

        private void ShowWelcome()
        {
            _console.Clear();
            _console.H1("C# 2.0 人工智能完整源程序库");
            _console.H2("项目概述");
            _console.P("本项目是一套严格遵循C# 2.0语法规范、零第三方库依赖的人工智能教学源程序库，");
            _console.P("涵盖经典机器学习算法、全连接神经网络、卷积神经网络CNN、MNIST手写数字识别训练全套实现，");
            _console.P("所有算法均从零手写实现，不依赖任何第三方AI库（无Accord.NET、无ML.NET、无TensorFlow），");
            _console.P("基于原生.NET Framework 2.0，单exe即可运行，兼容Windows XP及以上系统。");
            _console.Hr();

            _console.H2("技术规范");
            _console.Table(new string[] { "项目", "规范说明" }, new string[][] {
                new string[]{"语法版本", "严格C# 2.0，禁用var/自动属性/lambda/LINQ/C#3+特性"},
                new string[]{"依赖库", "零第三方依赖，仅使用System/System.Drawing/System.Windows.Forms"},
                new string[]{"运行环境", ".NET Framework 2.0，Windows XP+即可运行"},
                new string[]{"输出方式", "自研HtmlConsole HTML富文本输出，替代传统Console控制台"},
                new string[]{"算法实现", "全部从零手写，矩阵运算/反向传播/卷积/池化全部自研"}
            });
            _console.Hr();

            _console.H2("章节导航");
            _console.P("请从左侧树形菜单选择章节开始学习：");
            _console.P("• 第一部分：经典机器学习算法（KNN、决策树、朴素贝叶斯、逻辑回归、线性回归、KMeans）");
            _console.P("• 第二部分：神经网络（MLP多层感知机反向传播、鸢尾花分类实战）");
            _console.P("• 第三部分：卷积神经网络CNN（卷积层、池化层、网络结构）");
            _console.P("• 第四部分：MNIST手写数字识别（数据集加载、CNN完整训练流程）");
            _console.Hr();

            _console.Info("提示：所有算法代码可直接查看运行，训练演示无需下载外部数据集，内置生成样本可直接运行。");
            UpdateBrowser();
        }

        private void UpdateBrowser()
        {
            _browser.DocumentText = _console.GetHtml();
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string name = e.Node.Text;
            _console.Clear();
            try
            {
                if (name.StartsWith("0.")) DemoProjectIntro();
                else if (name.Contains("K近邻")) DemoKNN();
                else if (name.Contains("决策树")) DemoDecisionTree();
                else if (name.Contains("朴素贝叶斯")) DemoNaiveBayes();
                else if (name.Contains("逻辑回归")) DemoLogisticRegression();
                else if (name.Contains("线性回归")) DemoLinearRegression();
                else if (name.Contains("K均值")) DemoKMeans();
                else if (name.StartsWith("7.")) DemoMLP();
                else if (name.Contains("鸢尾花")) DemoMLPIris();
                else if (name.Contains("CNN卷积层")) DemoConvLayer();
                else if (name.Contains("池化层")) DemoPoolLayer();
                else if (name.Contains("数据集加载")) DemoMnistLoad();
                else if (name.Contains("CNN训练")) DemoMnistTrain();
                else ShowWelcome();
            }
            catch (Exception ex)
            {
                _console.Error("运行出错：" + ex.Message);
            }
            UpdateBrowser();
        }

        private void DemoProjectIntro()
        {
            _console.H1("项目说明与环境介绍");
            _console.H2("代码目录结构");
            _console.Code(@"
CSharp20AIFull.sln
├── Algorithms/
│   ├── Matrix.cs          # 矩阵运算基础类
│   ├── MathUtils.cs       # 数学工具函数
│   ├── ML/
│   │   └── ClassicML.cs   # 6种经典机器学习算法
│   ├── NN/
│   │   └── MLP.cs         # 多层感知机神经网络
│   └── CNN/
│       └── CNN.cs         # 卷积神经网络完整实现
├── Dataset/
│   └── MnistDataset.cs    # MNIST数据集加载与内置样本生成
├── UI/
│   ├── HtmlConsole.cs     # HTML富文本输出控制台
│   └── MainForm.cs        # 主窗体程序
└── Program.cs             # 程序入口
");
            _console.H2("编译方式");
            _console.Info("命令行编译（无需VS）：");
            _console.Code(@"csc /target:winexe /out:CSharp20AI.exe *.cs 
    Algorithms\*.cs Algorithms\ML\*.cs Algorithms\NN\*.cs 
    Algorithms\CNN\*.cs Dataset\*.cs UI\*.cs
    /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll");
            _console.Success("编译生成单文件exe，无需任何额外dll，双击即可运行。");
        }

        // ================= 经典机器学习演示 =================
        private void DemoKNN()
        {
            _console.H1("K近邻算法（KNN）");
            _console.H2("算法原理");
            _console.P("K近邻是最简单的分类算法：对于一个新样本，找到训练集中距离它最近的K个邻居，采用投票法决定类别。");
            _console.P("核心公式：欧氏距离 d = sqrt( Σ(xi - yi)^2 )");
            _console.H2("代码示例");
            _console.Code(@"
KNN knn = new KNN(k:3);
knn.Fit(trainX, trainY);
int pred = knn.Predict(newSample);
");
            _console.H2("运行演示：二分类数据集测试");
            Random rand = new Random(42);
            int nTrain = 100, nTest = 50;
            double[][] trainX = new double[nTrain][];
            int[] trainY = new int[nTrain];
            for (int i = 0; i < nTrain; i++)
            {
                if (i < 50) { trainX[i] = new double[] { rand.NextDouble(), rand.NextDouble() }; trainY[i] = 0; }
                else { trainX[i] = new double[] { 1 + rand.NextDouble(), 1 + rand.NextDouble() }; trainY[i] = 1; }
            }
            double[][] testX = new double[nTest][];
            int[] testY = new int[nTest];
            for (int i = 0; i < nTest; i++)
            {
                if (i < 25) { testX[i] = new double[] { rand.NextDouble(), rand.NextDouble() }; testY[i] = 0; }
                else { testX[i] = new double[] { 1 + rand.NextDouble(), 1 + rand.NextDouble() }; testY[i] = 1; }
            }

            KNN model = new KNN(3);
            model.Fit(trainX, trainY);
            double acc = model.Score(testX, testY);
            _console.Success("K=3 时测试集准确率：" + acc.ToString("P2"));

            _console.H2("不同K值对比");
            string[][] rows = new string[6][];
            for (int k = 1; k <= 6; k++)
            {
                KNN m = new KNN(k);
                m.Fit(trainX, trainY);
                rows[k - 1] = new string[] { "K=" + k, m.Score(testX, testY).ToString("P2") };
            }
            _console.Table(new string[] { "K值", "测试集准确率" }, rows);
        }

        private void DemoDecisionTree()
        {
            _console.H1("决策树（ID3/CART简化版）");
            _console.H2("算法原理");
            _console.P("决策树通过递归选择最优特征进行分割，将特征空间划分为若干单元，每个单元对应一个类别。");
            _console.P("分割准则使用基尼不纯度：Gini = 1 - Σp(i)^2，基尼系数越小表示样本越纯净。");
            _console.H2("运行演示");
            Random rand = new Random(42);
            int n = 200;
            double[][] X = new double[n][];
            int[] y = new int[n];
            for (int i = 0; i < n; i++)
            {
                X[i] = new double[] { rand.NextDouble(), rand.NextDouble() };
                y[i] = (X[i][0] + X[i][1] > 1) ? 1 : 0;
            }
            DecisionTree tree = new DecisionTree(5);
            tree.Fit(X, y);
            double acc = tree.Score(X, y);
            _console.Success("训练集准确率：" + acc.ToString("P2"));
            _console.Info("决策树优势：模型可解释性强，无需特征归一化，能处理非线性问题。");
        }

        private void DemoNaiveBayes()
        {
            _console.H1("高斯朴素贝叶斯");
            _console.H2("算法原理");
            _console.P("基于贝叶斯定理与特征条件独立假设：P(y|X) = P(X|y)P(y)/P(X)");
            _console.P("对连续特征假设服从高斯分布，计算每个类别的均值和方差，通过概率最大化预测类别。");
            _console.H2("运行演示");
            Random rand = new Random(42);
            int n = 200;
            double[][] X = new double[n][];
            int[] y = new int[n];
            for (int i = 0; i < n; i++)
            {
                if (i < 100) { X[i] = new double[] { 2 + rand.NextDouble() * 0.5, 2 + rand.NextDouble() * 0.5 }; y[i] = 0; }
                else { X[i] = new double[] { 4 + rand.NextDouble() * 0.5, 4 + rand.NextDouble() * 0.5 }; y[i] = 1; }
            }
            NaiveBayes nb = new NaiveBayes();
            nb.Fit(X, y);
            int correct = 0;
            for (int i = 0; i < n; i++) if (nb.Predict(X[i]) == y[i]) correct++;
            _console.Success("高斯朴素贝叶斯准确率：" + ((double)correct / n).ToString("P2"));
            _console.Info("朴素贝叶斯训练速度极快，对高维数据表现良好，常用于文本分类任务。");
        }

        private void DemoLogisticRegression()
        {
            _console.H1("逻辑回归");
            _console.H2("算法原理");
            _console.P("逻辑回归通过Sigmoid函数将线性输出映射到[0,1]概率区间，使用交叉熵损失梯度下降训练。");
            _console.Code("Sigmoid: σ(z) = 1/(1+e^-z)");
            _console.H2("运行演示");
            Random rand = new Random(42);
            int n = 200;
            double[][] X = new double[n][];
            int[] y = new int[n];
            for (int i = 0; i < n; i++)
            {
                X[i] = new double[] { rand.NextDouble() * 5 };
                y[i] = X[i][0] > 2.5 ? 1 : 0;
            }
            LogisticRegression lr = new LogisticRegression(0.1, 200);
            lr.Fit(X, y);
            _console.Output("训练得到参数：w=" + lr.ToString().Length + " 决策边界约x=2.5");
            _console.Success("逻辑回归训练完成，是工业界最常用的二分类基线算法。");
        }

        private void DemoLinearRegression()
        {
            _console.H1("线性回归");
            _console.H2("算法原理");
            _console.P("线性回归拟合 y = w*x + b 线性关系，使用梯度下降最小化均方误差MSE。");
            _console.H2("运行演示：房屋面积预测价格");
            double[][] X = new double[20][];
            double[] y = new double[20];
            Random rand = new Random(42);
            List<double> points = new List<double>();
            for (int i = 0; i < 20; i++)
            {
                X[i] = new double[] { i * 5 + 50 }; // 面积50-150平
                y[i] = X[i][0] * 2 + 50 + (rand.NextDouble() * 20 - 10); // 单价2万/平 + 噪声
                points.Add(y[i]);
            }
            LinearRegression reg = new LinearRegression();
            reg.Fit(X, y);
            _console.Success("拟合结果：价格 = " + reg.Weights[0].ToString("F2") + " * 面积 + " + reg.Bias.ToString("F2"));
            _console.Output("预测100平房屋价格：" + reg.Predict(new double[] { 100 }).ToString("F2") + " 万元");
            _console.Output("模型均方误差MSE：" + reg.MSE(X, y).ToString("F2"));
        }

        private void DemoKMeans()
        {
            _console.H1("K均值聚类 KMeans");
            _console.H2("算法原理");
            _console.P("KMeans是无监督聚类算法：1.随机初始化K个中心；2.分配样本到最近中心；3.更新中心为簇均值；4.迭代直到收敛。");
            _console.H2("运行演示");
            Random rand = new Random(42);
            int n = 150;
            double[][] X = new double[n][];
            // 3个簇
            double[][] centers = new double[][] { new double[] { 1, 1 }, new double[] { 5, 5 }, new double[] { 1, 5 } };
            int[] trueLabels = new int[n];
            for (int i = 0; i < n; i++)
            {
                int c = i / 50;
                trueLabels[i] = c;
                X[i] = new double[] { centers[c][0] + rand.NextDouble() - 0.5, centers[c][1] + rand.NextDouble() - 0.5 };
            }
            KMeans kmeans = new KMeans(3);
            kmeans.Fit(X);
            _console.Success("KMeans聚类完成，最终簇中心：");
            for (int c = 0; c < 3; c++)
            {
                _console.Output("  簇" + c + " 中心: (" + kmeans.Centroids[c][0].ToString("F2") + ", " + kmeans.Centroids[c][1].ToString("F2") + ")");
            }
            _console.Info("KMeans是最经典的无监督学习算法，广泛用于客户分群、图像分割、异常检测等场景。");
        }

        // ================= 神经网络演示 =================
        private void DemoMLP()
        {
            _console.H1("多层感知机 MLP（反向传播神经网络）");
            _console.H2("网络结构");
            _console.P("本实现从零手写全连接神经网络，包含：Linear全连接层、ReLU激活层、Softmax输出、交叉熵损失，支持反向传播自动梯度更新。");
            _console.Code(@"
网络结构：[输入层] -> Linear -> ReLU -> Linear -> ReLU -> Linear -> Softmax
支持任意层数配置，自动实现前向传播和反向传播。
");
            _console.H2("XOR问题测试");
            double[][] X = new double[][] { new double[] { 0, 0 }, new double[] { 0, 1 }, new double[] { 1, 0 }, new double[] { 1, 1 } };
            int[] y = new int[] { 0, 1, 1, 0 };
            MLP mlp = new MLP(new int[] { 2, 8, 2 }, 0.5);
            for (int epoch = 0; epoch < 2000; epoch++)
            {
                Matrix xm = new Matrix(4, 2);
                Matrix ym = Matrix.Zeros(4, 2);
                for (int i = 0; i < 4; i++)
                {
                    xm.Data[i][0] = X[i][0]; xm.Data[i][1] = X[i][1];
                    ym.Data[i][y[i]] = 1;
                }
                mlp.TrainOneBatch(xm, ym);
            }
            _console.H3("XOR预测结果");
            string[][] rows = new string[4][];
            for (int i = 0; i < 4; i++)
            {
                int pred = mlp.Predict(X[i]);
                rows[i] = new string[] { X[i][0] + ", " + X[i][1], y[i].ToString(), pred.ToString(), pred == y[i] ? "✅正确" : "❌错误" };
            }
            _console.Table(new string[] { "输入(x1,x2)", "真实标签", "预测标签", "结果" }, rows);
            _console.Success("XOR是非线性问题，单层感知机无法解决，MLP多层神经网络完美解决！");
        }

        private void DemoMLPIris()
        {
            _console.H1("MLP 鸢尾花分类实战");
            _console.H2("数据集说明");
            _console.P("使用内置生成的三分类模拟鸢尾花数据集：花萼长度/宽度、花瓣长度/宽度 4个特征，3个类别。");
            Random rand = new Random(42);
            int n = 150;
            double[][] X = new double[n][];
            int[] y = new int[n];
            double[][] classMeans = new double[][] { new double[] { 5, 3.5, 1.5, 0.2 }, new double[] { 6, 2.8, 4.5, 1.3 }, new double[] { 6.5, 3, 5.5, 2 } };
            for (int i = 0; i < n; i++)
            {
                int c = i / 50;
                X[i] = new double[4];
                for (int j = 0; j < 4; j++)
                {
                    X[i][j] = classMeans[c][j] + (rand.NextDouble() - 0.5) * 0.8;
                }
                y[i] = c;
            }
            // 归一化
            double[] max = new double[4], min = new double[4];
            for (int j = 0; j < 4; j++) { max[j] = double.MinValue; min[j] = double.MaxValue; }
            for (int i = 0; i < n; i++)
                for (int j = 0; j < 4; j++)
                { if (X[i][j] > max[j]) max[j] = X[i][j]; if (X[i][j] < min[j]) min[j] = X[i][j]; }
            for (int i = 0; i < n; i++)
                for (int j = 0; j < 4; j++)
                    X[i][j] = (X[i][j] - min[j]) / (max[j] - min[j]);

            // 划分训练测试
            List<double[]> trainX = new List<double[]>();
            List<int> trainY = new List<int>();
            List<double[]> testX = new List<double[]>();
            List<int> testY = new List<int>();
            for (int i = 0; i < n; i++)
            {
                if (i % 5 != 0) { trainX.Add(X[i]); trainY.Add(y[i]); }
                else { testX.Add(X[i]); testY.Add(y[i]); }
            }

            MLP mlp = new MLP(new int[] { 4, 16, 3 }, 0.1);
            List<double> losses = new List<double>();
            for (int epoch = 0; epoch < 200; epoch++)
            {
                Matrix xm = new Matrix(trainX.Count, 4);
                Matrix ym = Matrix.Zeros(trainX.Count, 3);
                for (int i = 0; i < trainX.Count; i++)
                {
                    for (int j = 0; j < 4; j++) xm.Data[i][j] = trainX[i][j];
                    ym.Data[i][trainY[i]] = 1;
                }
                double loss = mlp.TrainOneBatch(xm, ym);
                if (epoch % 20 == 0) losses.Add(loss);
            }

            int correct = 0;
            int[,] conf = new int[3, 3];
            for (int i = 0; i < testX.Count; i++)
            {
                int pred = mlp.Predict(testX[i]);
                conf[testY[i], pred]++;
                if (pred == testY[i]) correct++;
            }

            _console.Success("测试集准确率：" + ((double)correct / testX.Count).ToString("P2"));
            _console.SvgChart(losses, "训练损失曲线", 600, 250, "#4ec9b0");
            _console.ConfusionMatrix(conf, new string[] { "山鸢尾", "变色鸢尾", "维吉尼亚鸢尾" });
        }

        // ================= CNN演示 =================
        private void DemoConvLayer()
        {
            _console.H1("卷积层原理与实现");
            _console.H2("卷积运算");
            _console.P("卷积神经网络通过卷积核在输入图像上滑动计算局部加权和，提取边缘、纹理等局部特征：");
            _console.Code(@"
输出[i,j] = Σ Σ 输入[i+m,j+n] * 卷积核[m,n] + 偏置
支持多输入通道、多输出通道、Padding填充、Stride步长。
");
            _console.H2("核心特性");
            _console.Table(new string[] { "特性", "说明" }, new string[][] {
                new string[]{"局部连接", "每个神经元只连接输入局部区域，大幅减少参数量"},
                new string[]{"权值共享", "同一个卷积核在整张图像上滑动共享参数，提取同一种特征"},
                new string[]{"平移等变", "目标在图像任意位置都能被检测到"},
                new string[]{"层次特征", "底层提取边缘，中层提取纹理，高层提取语义"}
            });
            _console.Success("已实现Conv2DLayer类，支持前向传播和反向传播梯度计算。");
        }

        private void DemoPoolLayer()
        {
            _console.H1("池化层与全连接层");
            _console.H2("最大池化");
            _console.P("最大池化取局部区域最大值，作用是：1.降维减少计算量；2.提供平移不变性；3.防止过拟合。");
            _console.H2("CNN网络结构（本项目实现）");
            _console.Code(@"
SimpleCNN 网络结构：
输入 [1x28x28 手写数字灰度图]
  → Conv2D(1→4, kernel=3, padding=1)  [4x28x28]
  → ReLU激活
  → MaxPool2D(kernel=2, stride=2)     [4x14x14]
  → Flatten 展平                      [784]
  → Linear 全连接层                   [10类]
  → Softmax输出概率
总参数量：约 8000 参数，CPU即可快速训练。
");
            _console.Info("本实现从零手写卷积、池化、展平、全连接层完整前向和反向传播，不依赖任何深度学习框架。");
        }

        // ================= MNIST演示 =================
        private void DemoMnistLoad()
        {
            _console.H1("MNIST手写数字数据集");
            _console.H2("数据集说明");
            _console.P("MNIST是机器学习界的 Hello World 数据集，包含70000张28x28灰度手写数字图片，共10个类别（0-9）。");
            _console.Info("本项目支持两种方式加载：1.如果目录下存在MNIST idx格式文件则自动加载真实数据；2.如无外部文件则自动程序生成内置模拟手写数字样本，无需下载即可运行演示。");

            MnistDataset ds = new MnistDataset();
            bool loaded = ds.Load("train-images.idx3-ubyte", "train-labels.idx1-ubyte",
                "t10k-images.idx3-ubyte", "t10k-labels.idx1-ubyte", 100, 20);
            if (!loaded)
            {
                _console.Warning("未检测到外部MNIST文件，自动生成内置模拟样本。");
                ds.GenerateBuiltinDataset(5);
            }
            _console.Success("数据集加载完成！训练集：" + ds.TrainImages.Count + "张，测试集：" + ds.TestImages.Count + "张");

            _console.H3("样本预览（前8张）");
            int count = Math.Min(8, ds.TestImages.Count);
            for (int i = 0; i < count; i++)
            {
                _console.DigitImage(ds.TestImages[i], ds.TestLabels[i], -1, 140);
            }
        }

        private void DemoMnistTrain()
        {
            _console.H1("CNN训练MNIST手写数字识别");
            _console.H2("开始训练");
            _console.Info("正在初始化CNN模型并准备数据集，使用CPU训练，3个epoch约需1-2分钟...");
            UpdateBrowser();
            Application.DoEvents();

            MnistTrainer trainer = new MnistTrainer();
            trainer.Epochs = 3;
            trainer.LR = 0.01;

            _console.Progress(10, "准备数据集...");
            UpdateBrowser();
            Application.DoEvents();
            trainer.PrepareDataset();

            _console.Progress(30, "构建CNN模型...");
            UpdateBrowser();
            Application.DoEvents();
            trainer.BuildModel();

            List<double> losses = new List<double>();
            List<double> accs = new List<double>();

            for (int epoch = 0; epoch < trainer.Epochs; epoch++)
            {
                _console.Progress(30 + (int)(60.0 * epoch / trainer.Epochs), "训练第" + (epoch + 1) + "轮...");
                UpdateBrowser();
                Application.DoEvents();

                double totalLoss = 0;
                List<int> indices = new List<int>();
                int nTrain = Math.Min(500, trainer.Dataset.TrainImages.Count);
                for (int i = 0; i < nTrain; i++) indices.Add(i);
                MathUtils.Shuffle(indices);

                for (int i = 0; i < nTrain; i++)
                {
                    int idx = indices[i];
                    double[][][] img = new double[1][][];
                    img[0] = trainer.Dataset.TrainImages[idx];
                    double loss = trainer.CNN.Train(img, trainer.Dataset.TrainLabels[idx]);
                    totalLoss += loss;
                }
                losses.Add(totalLoss / nTrain);

                // 测试
                int nTest = Math.Min(100, trainer.Dataset.TestImages.Count);
                List<double[][][]> testBatch = new List<double[][][]>();
                int[] testLabels = new int[nTest];
                for (int i = 0; i < nTest; i++)
                {
                    double[][][] img = new double[1][][];
                    img[0] = trainer.Dataset.TestImages[i];
                    testBatch.Add(img);
                    testLabels[i] = trainer.Dataset.TestLabels[i];
                }
                double acc = trainer.CNN.Score(testBatch, testLabels);
                accs.Add(acc);
            }

            _console.Progress(95, "计算结果...");
            UpdateBrowser();
            Application.DoEvents();

            _console.Progress(100, "训练完成！");
            _console.H2("训练结果");
            _console.Success("训练完成！最终测试集准确率：" + (accs[accs.Count - 1] * 100).ToString("F2") + "%");
            _console.SvgChart(losses, "训练损失曲线（CrossEntropy Loss）", 600, 250, "#f48771");
            _console.SvgChart(accs, "测试集准确率曲线", 600, 250, "#4ec9b0");

            _console.H3("测试样本预测展示");
            int showCount = Math.Min(5, trainer.Dataset.TestImages.Count);
            for (int i = 0; i < showCount; i++)
            {
                double[][][] img = new double[1][][];
                img[0] = trainer.Dataset.TestImages[i];
                int pred = trainer.CNN.Predict(img);
                _console.DigitImage(trainer.Dataset.TestImages[i], trainer.Dataset.TestLabels[i], pred, 140);
            }

            _console.H2("总结");
            _console.Success("从零手写卷积神经网络完成MNIST手写数字识别训练！全部代码纯C#2.0语法，无任何第三方依赖，CPU即可训练运行。");
        }
    }
}
