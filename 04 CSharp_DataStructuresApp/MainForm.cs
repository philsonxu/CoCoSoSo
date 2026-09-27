using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSharp20DataStructures.Demos;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures
{
    public partial class MainForm : Form
    {
        private TreeView _treeView;
        private WebBrowser _webBrowser;
        private SplitContainer _splitContainer;
        private HtmlConsole _console;

        public MainForm()
        {
            //InitializeComponent();

            Text = "C# 2.0 数据结构入门教程";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;

            _splitContainer = new SplitContainer();
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.SplitterDistance = 30;
            Controls.Add(_splitContainer);

            _treeView = new TreeView();
            _treeView.Dock = DockStyle.Fill;
            _treeView.Font = new Font("微软雅黑", 10f);
            _treeView.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);
            _splitContainer.Panel1.Controls.Add(_treeView);

            _webBrowser = new WebBrowser();
            _webBrowser.Dock = DockStyle.Fill;
            _webBrowser.IsWebBrowserContextMenuEnabled = false;
            _webBrowser.WebBrowserShortcutsEnabled = false;
            _webBrowser.ScriptErrorsSuppressed = true;
            _splitContainer.Panel2.Controls.Add(_webBrowser);

            _console = new HtmlConsole();// _webBrowser);

            _splitContainer.SuspendLayout();
            _webBrowser.SuspendLayout();
            this.SuspendLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            BrowserReleaseHelper.SetWebBrowserFeatures(11);

            InitTree();
            ShowWelcome();
        }

        private void InitTree()
        {
            _treeView.Nodes.Clear();
            TreeNode root = new TreeNode("C#数据结构教程");
            root.Nodes.Add("0", "📖 课程介绍");
            root.Nodes.Add("1", "1. 顺序表 SeqList");
            root.Nodes.Add("2", "2. 单链表 LinkedList");
            root.Nodes.Add("3", "3. 栈 Stack");
            root.Nodes.Add("4", "4. 队列 Queue");
            root.Nodes.Add("5", "5. 二叉搜索树 BST");
            root.Nodes.Add("6", "6. 哈希表 Hashtable");
            root.Nodes.Add("7", "7. 排序算法");
            root.Nodes.Add("8", "8. 查找算法");
            root.Nodes.Add("9", "9. 字符串匹配 KMP");
            root.Nodes.Add("10", "10. 图与遍历");
            _treeView.Nodes.Add(root);
            root.Expand();
            _treeView.SelectedNode = root.Nodes[0];
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _console.Clear();
            _console.WriteHead();
            switch (e.Node.Name)
            {
                case "0":
                    ShowWelcome();
                    break;
                case "1":
                    SeqListDemo.Run(_console);
                    break;
                case "2":
                    SingleLinkedListDemo.Run(_console);
                    break;
                case "3":
                    StackDemo.Run(_console);
                    break;
                case "4":
                    QueueDemo.Run(_console);
                    break;
                case "5":
                    BinarySearchTreeDemo.Run(_console);
                    break;
                case "6":
                    HashtableDemo.Run(_console);
                    break;
                case "7":
                    SortDemo.Run(_console);
                    break;
                case "8":
                    SearchDemo.Run(_console);
                    break;
                case "9":
                    StringMatchDemo.Run(_console);
                    break;
                case "10":
                    GraphDemo.Run(_console);
                    break;
            }
            string html = _console.GetHtml();
            //File.WriteAllText(@"001.html", html, Encoding.UTF8);
            _webBrowser.DocumentText = html;
        }

        private void ShowWelcome()
        {
            _console.Clear();
            _console.WriteTitle("C# 2.0 数据结构入门教程");
            _console.WriteSection("关于本教程");
            _console.WriteLine("本教程是一套完整的C#数据结构教学程序库，全部采用<strong>C# 2.0语法规范</strong>编写，基于.NET Framework 2.0平台。");
            _console.WriteLine("采用WinForms桌面应用形式，使用HTML富文本替代传统控制台输出，代码讲解与运行演示结合，方便学习。");

            _console.WriteSection("教程特色");
            _console.WriteLine("✅ <strong>纯C# 2.0语法</strong>：不使用var、LINQ、lambda、自动属性等C#3.0+特性");
            _console.WriteLine("✅ <strong>从零实现</strong>：所有数据结构不依赖.NET内置集合类（List/Dictionary等），全部手写实现");
            _console.WriteLine("✅ <strong>完整演示</strong>：每个数据结构包含原理讲解、核心代码、运行示例、复杂度分析");
            _console.WriteLine("✅ <strong>HTML可视化</strong>：格式化输出，代码高亮，表格展示，清晰直观");

            _console.WriteSection("包含章节");
            _console.WriteLine("1. 顺序表（SeqList）：数组实现的动态线性表");
            _console.WriteLine("2. 单链表（LinkedList）：指针链接的链式存储");
            _console.WriteLine("3. 栈（Stack）：后进先出，括号匹配应用");
            _console.WriteLine("4. 队列（Queue）：先进先出，循环数组实现");
            _console.WriteLine("5. 二叉搜索树（BST）：四种遍历方式");
            _console.WriteLine("6. 哈希表（Hashtable）：链地址法解决冲突");
            _console.WriteLine("7. 排序算法：冒泡、选择、插入、快排");
            _console.WriteLine("8. 查找算法：顺序查找、二分查找");
            _console.WriteLine("9. 字符串匹配：朴素匹配、KMP算法");
            _console.WriteLine("10. 图（Graph）：邻接表、BFS、DFS");

            _console.WriteSection("使用说明");
            _console.WriteLine("从左侧树形菜单选择章节即可查看对应内容：");
            _console.WriteLine("• <span class='type'>代码块</span>：展示核心实现代码");
            _console.WriteLine("• <span class='keyword'>结果块</span>：展示代码实际运行输出");
            _console.WriteLine("• 💡 蓝色提示：知识点说明、技巧");
            _console.WriteLine("• ⚠️ 黄色注意：容易出错的地方");
            _console.WriteLine("• ✅ 绿色成功：验证结果、重要结论");

            _console.WriteTip("建议学习顺序：从线性结构（顺序表、链表、栈、队列）开始，再学习树形结构、哈希表，最后学习算法（排序、查找、图）。");
            _console.Render();
        }
    }
}
