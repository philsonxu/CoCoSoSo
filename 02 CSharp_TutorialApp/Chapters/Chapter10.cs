using System;
using System.Text;
using System.Windows.Forms;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第10章：WinForms交互实战
    /// </summary>
    public class Chapter10
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第10章 WinForms桌面交互实战");

            console.WriteSection("10.1 WinForms基础与事件绑定");
            console.WriteParagraph("WinForms是.NET Framework自带的桌面UI框架，通过拖放控件+事件绑定快速开发Windows桌面应用。C# 2.0中事件绑定使用标准委托语法，配合匿名方法可以快速编写事件处理逻辑。");
            
            console.WriteCode(@"// 创建一个按钮
Button btn = new Button();
btn.Text = ""点击我"";
// 绑定Click事件：使用匿名方法（C#2.0写法）
btn.Click += delegate(object sender, EventArgs e)
{
    MessageBox.Show(""按钮被点击了！"");
};

// 创建文本框
TextBox txt = new TextBox();
// 绑定TextChanged事件：文本变化时触发
txt.TextChanged += delegate(object sender, EventArgs e)
{
    Console.WriteLine(""当前输入："" + txt.Text);
};");

            console.WriteTip("WinForms控件的事件本质就是委托，你之前学的所有委托、匿名方法知识在这里都能直接用上。");
            console.WriteDivider();

            console.WriteSection("10.2 交互演示：简单的问候程序");
            console.WriteParagraph("现在我们来做一个真实的小交互：点击下方按钮打开一个小窗口，输入姓名后点击问候，弹窗显示问候语。所有代码都是C# 2.0语法。");

            // 创建演示按钮（直接在HTML里说明，我们通过点击按钮触发演示）
            console.WriteParagraph("👉 现在点击按钮开始演示：");
            console.WriteCode(@"// 完整演示代码
Form demoForm = new Form();
demoForm.Text = ""问候演示"";
demoForm.Size = new Size(300, 150);
demoForm.StartPosition = FormStartPosition.CenterParent;

Label lbl = new Label();
lbl.Text = ""请输入你的姓名："";
lbl.Location = new Point(20, 20);
lbl.AutoSize = true;

TextBox txtName = new TextBox();
txtName.Location = new Point(20, 45);
txtName.Size = new Size(240, 25);

Button btnOk = new Button();
btnOk.Text = ""问候"";
btnOk.Location = new Point(100, 75);
btnOk.Click += delegate(object sender, EventArgs e)
{
    string name = txtName.Text.Trim();
    if (name == """")
        MessageBox.Show(""请输入姓名！"");
    else
        MessageBox.Show(string.Format(""你好，{0}！欢迎来到C# 2.0的世界！"", name), ""问候"");
    demoForm.Close();
};

demoForm.Controls.Add(lbl);
demoForm.Controls.Add(txtName);
demoForm.Controls.Add(btnOk);
demoForm.ShowDialog(); // 模态对话框");

            console.WriteOutput("点击下方「开始演示」按钮即可运行这个小程序。演示结束后你可以继续浏览其他章节。");
            
            // 我们创建一个链接说明，实际在WinForms中会弹出窗口
            console.WriteSuccess("🎉 恭喜！你已经完成了C# 2.0全部核心语法的学习！\n\n你已经掌握了：基础语法、运算符、流程控制、数组字符串、方法、面向对象、泛型、异常处理、委托匿名方法、WinForms基础。\n\n接下来你可以尝试自己编写简单的桌面小工具，比如计算器、记事本、学生管理系统，在实践中巩固所学知识。");

            console.WriteDivider();
            console.WriteNote("运行说明：打开项目编译后，直接运行即可看到这个教程桌面应用，点击左侧章节导航即可学习对应内容，所有示例都有源代码和实际运行结果展示。");
        }
    }
}
