using System;
using System.Windows.Forms;
using CSharp20Tutorial.Chapters;

namespace CSharp20Tutorial
{
    public partial class MainForm : Form
    {
        private HtmlConsole _console;

        public MainForm()
        {
            InitializeComponent();
            _console = new HtmlConsole();
            InitChapters();
        }

        /// <summary>
        /// 初始化章节树
        /// </summary>
        private void InitChapters()
        {
            treeViewChapters.BeginUpdate();
            
            TreeNode root = new TreeNode("C# 2.0 入门教程");
            root.Expand();

            TreeNode n1 = new TreeNode("第1章 Hello World与基础语法");
            n1.Tag = "ch1";
            TreeNode n2 = new TreeNode("第2章 运算符与表达式");
            n2.Tag = "ch2";
            TreeNode n3 = new TreeNode("第3章 条件判断与循环");
            n3.Tag = "ch3";
            TreeNode n4 = new TreeNode("第4章 数组与字符串");
            n4.Tag = "ch4";
            TreeNode n5 = new TreeNode("第5章 方法基础");
            n5.Tag = "ch5";
            TreeNode n6 = new TreeNode("第6章 面向对象基础");
            n6.Tag = "ch6";
            TreeNode n7 = new TreeNode("第7章 泛型（C# 2.0核心特性）");
            n7.Tag = "ch7";
            TreeNode n8 = new TreeNode("第8章 可空类型与异常处理");
            n8.Tag = "ch8";
            TreeNode n9 = new TreeNode("第9章 委托与匿名方法");
            n9.Tag = "ch9";
            TreeNode n10 = new TreeNode("第10章 WinForms交互实战");
            n10.Tag = "ch10";

            root.Nodes.Add(n1);
            root.Nodes.Add(n2);
            root.Nodes.Add(n3);
            root.Nodes.Add(n4);
            root.Nodes.Add(n5);
            root.Nodes.Add(n6);
            root.Nodes.Add(n7);
            root.Nodes.Add(n8);
            root.Nodes.Add(n9);
            root.Nodes.Add(n10);

            treeViewChapters.Nodes.Add(root);
            treeViewChapters.EndUpdate();
            
            treeViewChapters.SelectedNode = n1;
            LoadChapter("ch1");
        }

        /// <summary>
        /// 章节切换事件
        /// </summary>
        private void treeViewChapters_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                LoadChapter(e.Node.Tag.ToString());
            }
        }

        /// <summary>
        /// 加载指定章节内容
        /// </summary>
        private void LoadChapter(string chapterId)
        {
            try
            {
                switch (chapterId)
                {
                    case "ch1":
                        Chapter01.Run(_console);
                        break;
                    case "ch2":
                        Chapter02.Run(_console);
                        break;
                    case "ch3":
                        Chapter03.Run(_console);
                        break;
                    case "ch4":
                        Chapter04.Run(_console);
                        break;
                    case "ch5":
                        Chapter05.Run(_console);
                        break;
                    case "ch6":
                        Chapter06.Run(_console);
                        break;
                    case "ch7":
                        Chapter07.Run(_console);
                        break;
                    case "ch8":
                        Chapter08.Run(_console);
                        break;
                    case "ch9":
                        Chapter09.Run(_console);
                        break;
                    case "ch10":
                        Chapter10.Run(_console);
                        break;
                    default:
                        _console.WriteTitle("欢迎来到C# 2.0入门教程");
                        _console.WriteParagraph("请从左侧选择章节开始学习。");
                        break;
                }
                string html = _console.GetHtml();
                webBrowserOutput.DocumentText = html;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载章节错误：" + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
