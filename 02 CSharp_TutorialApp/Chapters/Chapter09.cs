using System;
using System.Text;
using System.Collections.Generic;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第9章：委托与匿名方法
    /// </summary>
    public class Chapter09
    {
        // 定义委托类型：可以指向一个参数为string、无返回值的方法
        public delegate void GreetDelegate(string name, StringBuilder sb);

        // 定义一个用于比较的委托
        public delegate int CompareDelegate(int a, int b);

        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第9章 委托与匿名方法");

            console.WriteSection("9.1 委托的基本概念");
            console.WriteParagraph("委托是一种类型安全的函数指针，可以把方法当作参数传递，实现方法的动态调用。委托是C#事件、回调函数的基础。定义委托用delegate关键字，签名要和指向的方法一致。");

            console.WriteCode(@"// 1. 定义委托类型（在类级别）
public delegate void GreetDelegate(string name);

// 2. 创建符合委托签名的方法
static void GreetChinese(string name)
{
    Console.WriteLine(""你好，"" + name);
}
static void GreetEnglish(string name)
{
    Console.WriteLine(""Hello, "" + name);
}

// 3. 创建委托实例，绑定方法
GreetDelegate greet = new GreetDelegate(GreetChinese);
// 4. 调用委托，等价于直接调用绑定的方法
greet(""张三""); // 输出：你好，张三
// 可以随时切换绑定的方法
greet = new GreetDelegate(GreetEnglish);
greet(""Tom""); // 输出：Hello, Tom");

            StringBuilder sb = new StringBuilder();
            GreetDelegate greet = new GreetDelegate(GreetChinese);
            sb.AppendLine("绑定中文问候方法：");
            greet("张三", sb);

            greet = new GreetDelegate(GreetEnglish);
            sb.AppendLine("切换为英文问候方法：");
            greet("Tom", sb);

            console.WriteOutput(sb.ToString());
            console.WriteTip("委托的好处：调用者不需要知道具体执行的是哪个方法，只要符合委托签名就行，可以动态替换逻辑。这是策略模式、事件编程的基础。");
            console.WriteDivider();

            console.WriteSection("9.2 匿名方法（C# 2.0新特性）");
            console.WriteParagraph("C# 2.0引入了匿名方法，不需要单独定义一个具名方法，可以直接在绑定委托的时候写方法体，代码更简洁。语法是delegate(参数列表) { 方法体 }。注意：这不是lambda表达式（lambda是C#3.0才有的），是C#2.0的匿名方法语法。");

            console.WriteCode(@"// 匿名方法：不需要单独定义GreetJapanese方法，直接写
GreetDelegate greetJp = delegate(string name)
{
    Console.WriteLine(""具体执行"" + name);
};
greetJp(""具体执行""); // 直接调用");

            StringBuilder sb2 = new StringBuilder();
            // 匿名方法捕获外部变量（闭包特性）
            int times = 3;
            GreetDelegate repeatGreet = delegate (string name, StringBuilder sb)
            {
                for (int i = 0; i < times; i++)
                {
                    sb2.AppendLine(string.Format("  你好，{0}！（第{1}次）", name, i + 1));
                }
            };
            sb2.AppendLine("使用匿名方法，并捕获外部变量times=3：");
            repeatGreet("李四", sb2);

            console.WriteOutput(sb2.ToString());
            console.WriteNote("匿名方法可以\"捕获\"外部的局部变量，形成闭包，这是非常强大的功能，也是后续lambda表达式的基础。");
            console.WriteDivider();

            console.WriteSection("9.3 委托的实际应用：自定义排序");
            console.WriteParagraph("委托最常见的用途之一是把比较逻辑作为参数传递，实现灵活的自定义排序。List<T>的Sort方法就接受一个Comparison<T>委托。");

            console.WriteCode(@"List<int> nums = new List<int> { 5, 2, 8, 1, 9 };
// 用匿名方法指定排序规则：降序排序
nums.Sort(delegate(int a, int b)
{
    return b - a; // 返回>0表示a应该在b后面
});");

            StringBuilder sb3 = new StringBuilder();
            List<int> nums = new List<int>(new int[] { 5, 2, 8, 1, 9, 3 });
            sb3.AppendLine("原始数组：" + string.Join(", ", nums.ToArray()));

            // 升序
            nums.Sort(delegate (int a, int b) { return a - b; });
            sb3.AppendLine("按升序排序：" + string.Join(", ", nums.ToArray()));

            // 降序
            nums.Sort(delegate (int a, int b) { return b - a; });
            sb3.AppendLine("按降序排序：" + string.Join(", ", nums.ToArray()));

            List<Student2> students = new List<Student2>();
            students.Add(new Student2("张三", 85));
            students.Add(new Student2("李四", 92));
            students.Add(new Student2("王五", 78));
            students.Add(new Student2("赵六", 95));

            sb3.AppendLine("\n按分数升序排序学生：");
            students.Sort(delegate (Student2 s1, Student2 s2) { return s1.Score - s2.Score; });
            foreach (Student2 st in students)
            {
                sb3.AppendLine(string.Format("  {0}：{1}分", st.Name, st.Score));
            }

            console.WriteOutput(sb3.ToString());
            console.WriteSuccess("第9章完成！委托和匿名方法是WinForms事件编程的核心基础，下一章我们就来做实际的WinForms交互。");
        }

        private static void GreetChinese(string name, StringBuilder sb)
        {
            sb.AppendLine(string.Format("  你好，{0}！", name));
        }

        private static void GreetEnglish(string name, StringBuilder sb)
        {
            sb.AppendLine(string.Format("  Hello, {0}!", name));
        }
    }

    // 辅助类
    public class Student2
    {
        private string _name;
        private int _score;

        public string Name
        {
            get { return _name; }
        }

        public int Score
        {
            get { return _score; }
        }

        public Student2(string name, int score)
        {
            _name = name;
            _score = score;
        }
    }

    // 扩展委托类型，支持StringBuilder参数用于输出
    public delegate void GreetDelegate(string name, StringBuilder sb);
}
