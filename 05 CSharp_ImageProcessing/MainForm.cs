using System;
using System.Drawing;
using System.Windows.Forms;

namespace CSharp20ImageProcessing
{
    public class MainForm : Form
    {
        private SplitContainer splitContainer;
        private TreeView tvChapters;
        private WebBrowser webBrowser;

        public MainForm()
        {
            this.Text = "C# 2.0 图像处理教程（零第三方库）";
            this.Width = 1000;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;

            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.SplitterDistance = 260;
            splitContainer.FixedPanel = FixedPanel.Panel1;

            tvChapters = new TreeView();
            tvChapters.Dock = DockStyle.Fill;
            tvChapters.Font = new Font("微软雅黑", 9f);
            tvChapters.AfterSelect += new TreeViewEventHandler(tvChapters_AfterSelect);
            BuildTree();

            webBrowser = new WebBrowser();
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.ScriptErrorsSuppressed = true;

            splitContainer.Panel1.Controls.Add(tvChapters);
            splitContainer.Panel2.Controls.Add(webBrowser);
            this.Controls.Add(splitContainer);

            ShowWelcome();
        }

        private void BuildTree()
        {
            tvChapters.Nodes.Clear();
            TreeNode root = new TreeNode("📚 C# 2.0 图像处理入门");
            root.Nodes.Add("第1章：GDI+基础与测试图生成");
            root.Nodes.Add("第2章：灰度化算法（平均值/加权）");
            root.Nodes.Add("第3章：二值化与阈值分割");
            root.Nodes.Add("第4章：亮度与对比度调整");
            root.Nodes.Add("第5章：颜色特效（反色/棕褐色）");
            root.Nodes.Add("第6章：图像滤镜（模糊/浮雕）");
            root.Nodes.Add("第7章：几何变换（缩放/旋转/镜像）");
            root.Nodes.Add("第8章：灰度直方图统计");
            root.Nodes.Add("第9章：LockBits高性能像素操作");
            root.Nodes.Add("第10章：综合实战——简单画板");
            root.Expand();
            tvChapters.Nodes.Add(root);
        }

        private void ShowWelcome()
        {
            HtmlConsole c = new HtmlConsole();
            c.WriteTitle("欢迎使用 C# 2.0 原生图像处理教程");
            c.WriteSection("🎯 项目特点");
            c.WriteSuccess("✅ 100%纯C# 2.0语法，无var、无LINQ、无lambda、无自动属性");
            c.WriteSuccess("✅ 零第三方库依赖，完全基于System.Drawing原生GDI+");
            c.WriteSuccess("✅ LockBits+unsafe指针手写全部图像处理算法，适合学习底层原理");
            c.WriteSuccess("✅ HTML富文本输出，处理效果前后对比直观展示");
            c.WriteSuccess("✅ WinForms桌面应用，.NET Framework 2.0即可运行，兼容性极强");
            c.WriteSection("📖 章节导航");
            c.WriteInfo("请从左侧树形菜单选择章节开始学习：\n\n1. GDI+基础\n2. 灰度化\n3. 二值化\n4. 亮度对比度调整\n5. 颜色特效\n6. 模糊与浮雕滤镜\n7. 缩放旋转镜像\n8. 直方图\n9. 高性能像素操作\n10. 综合绘图实战");
            c.WriteWarning("注意：项目开启了unsafe代码编译选项（仅用于指针直接操作内存，这是高性能图像处理必需的，不涉及任何不安全行为）。");
            webBrowser.DocumentText = c.GetHtml();
        }

        private void tvChapters_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                ShowWelcome();
                return;
            }
            int index = e.Node.Index;
            this.Cursor = Cursors.WaitCursor;
            string html = "";
            try
            {
                switch (index)
                {
                    case 0: html = ImageExamples.RunChapter1(); break;
                    case 1: html = ImageExamples.RunChapter2(); break;
                    case 2: html = ImageExamples.RunChapter3(); break;
                    case 3: html = ImageExamples.RunChapter4(); break;
                    case 4: html = ImageExamples.RunChapter5(); break;
                    case 5: html = ImageExamples.RunChapter6(); break;
                    case 6: html = ImageExamples.RunChapter7(); break;
                    case 7: html = ImageExamples.RunChapter8(); break;
                    case 8: html = ImageExamples.RunChapter9(); break;
                    case 9: html = ImageExamples.RunChapter10(); break;
                }
                webBrowser.DocumentText = html;
            }
            catch (Exception ex)
            {
                MessageBox.Show("运行出错：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
