using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CSharp20AI
{
    class MainForm : Form
    {
        private TreeView _treeView;
        private WebBrowser _webBrowser;
        private Dictionary<int, DemoBase> _demos;

        public MainForm()
        {
            Text = "C#2.0 人工智能入门教程 - 零依赖纯原生实现";
            Size = new Size(1050, 700);
            StartPosition = FormStartPosition.CenterScreen;

            SplitContainer split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.SplitterDistance = 250;
            split.FixedPanel = FixedPanel.Panel1;
            split.Panel1.BackColor = Color.White;
            Controls.Add(split);

            _treeView = new TreeView();
            _treeView.Dock = DockStyle.Fill;
            _treeView.Font = new Font("Microsoft YaHei", 10f);
            _treeView.Indent = 20;
            _treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(_treeView_NodeMouseClick);
            split.Panel1.Controls.Add(_treeView);

            _webBrowser = new WebBrowser();
            _webBrowser.Dock = DockStyle.Fill;
            _webBrowser.IsWebBrowserContextMenuEnabled = false;
            _webBrowser.WebBrowserShortcutsEnabled = false;
            split.Panel2.Controls.Add(_webBrowser);

            _demos = new Dictionary<int, DemoBase>();

            InitChapters();

            if (_treeView.Nodes.Count > 0)
                _treeView.SelectedNode = _treeView.Nodes[0];
        }

        private void InitChapters()
        {
            AddChapter(1, "第1章 感知机 - 最基础的神经网络", new PerceptronDemo());
            AddChapter(2, "第2章 线性回归 - 最小二乘法拟合直线", new LinearRegressionDemo());
            AddChapter(3, "第3章 逻辑回归 - 二分类问题", new LogisticRegressionDemo());
            AddChapter(4, "第4章 K近邻算法 KNN - 惰性分类", new KNNDemo());
            AddChapter(5, "第5章 K均值聚类 K-Means - 无监督聚类", new KMeansDemo());
            AddChapter(6, "第6章 决策树 ID3算法 - 信息增益", new DecisionTreeDemo());
            AddChapter(7, "第7章 朴素贝叶斯分类器 - 概率统计", new NaiveBayesDemo());
            AddChapter(8, "第8章 遗传算法 - 进化优化", new GeneticAlgorithmDemo());
            AddChapter(9, "第9章 BP神经网络 - 反向传播", new BPNNDemo());
            AddChapter(10, "第10章 蒙特卡洛方法 - 随机采样", new MonteCarloDemo());

            TreeNode welcome = new TreeNode("欢迎使用");
            welcome.Tag = 0;
            _treeView.Nodes.Add(welcome);

            TreeNode root = new TreeNode("算法章节");
            _treeView.Nodes.Add(root);
            foreach (TreeNode n in _treeView.Nodes)
            {
                // nothing
            }

            for (int i = 1; i <= 10; i++)
            {
                TreeNode node = new TreeNode(_demos[i].GetTitle());
                node.Tag = i;
                root.Nodes.Add(node);
            }
            root.Expand();
        }

        private void AddChapter(int id, string title, DemoBase demo)
        {
            demo.SetTitle(title);
            _demos[id] = demo;
        }

        void _treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int id = (int)e.Node.Tag;
            if (id == 0)
            {
                ShowWelcome();
            }
            else
            {
                RunDemo(id);
            }
        }

        private void ShowWelcome()
        {
            HtmlConsole c = new HtmlConsole();
            c.H1("C# 2.0 人工智能入门教程");
            c.P("欢迎使用本教程，本程序库完全基于C# 2.0语法编写，零第三方库依赖，所有人工智能算法均从零手写实现，适合学习AI算法底层原理。");
            c.H2("技术特点");
            c.P("✅ 严格C# 2.0语法：不使用var、lambda、LINQ、自动属性等高版本特性，完全兼容.NET Framework 2.0运行时。");
            c.P("✅ 零第三方依赖：所有算法全部从零手写实现，不依赖Math.NET、Accord.NET、ML.NET等任何第三方AI库，也不依赖任何NuGet包。");
            c.P("✅ 桌面GUI程序：基于WinForms开发，左侧导航右侧内容，使用HTML富文本渲染输出，替代传统控制台黑框，直观展示算法原理、代码与运行结果。");
            c.P("✅ 可视化演示：每个算法都配有GDI+绘制的可视化效果图，直观展示算法运行过程与结果，数据点、分类边界、拟合直线一目了然。");
            c.P("✅ 可编译单文件EXE：只需要.NET Framework 2.0运行环境即可运行，单文件即可分发，无需安装额外组件。");
            c.H2("包含10个经典AI算法");
            c.P("从最简单的感知机开始，逐步覆盖线性模型、分类、聚类、决策树、概率模型、进化算法、神经网络、随机算法等人工智能入门核心内容，每个算法都包含原理讲解、核心代码、运行演示与可视化结果。");
            c.Tip("请从左侧树菜单选择章节开始学习，点击章节即可自动运行算法示例，查看原理、代码和可视化结果。");
            _webBrowser.DocumentText = c.GetHtml();
        }

        private void RunDemo(int id)
        {
            DemoBase demo = _demos[id];
            HtmlConsole c = new HtmlConsole();
            demo.Run(c);
            _webBrowser.DocumentText = c.GetHtml();
        }
    }
}
