using System;
using System.Text;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第5章：方法基础
    /// </summary>
    public class Chapter05
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第5章 方法（函数）基础");

            console.WriteSection("5.1 方法的定义与调用");
            console.WriteParagraph("方法是完成特定功能的代码块，可以重复调用。方法由返回值类型、方法名、参数列表、方法体组成。无返回值用void。");
            
            console.WriteCode(@"// 定义一个无参数无返回值的方法
static void SayHello()
{
    Console.WriteLine(""Hello!"");
}
// 定义一个带参数带返回值的方法
static int Add(int a, int b)
{
    return a + b;
}
// 调用方法
SayHello();
int sum = Add(3, 5); // 结果8");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("调用SayHello()：");
            SayHello(sb);
            int r1 = Add(3, 5);
            sb.AppendLine(string.Format("调用Add(3, 5)，返回：{0}", r1));
            int r2 = Add(100, 200);
            sb.AppendLine(string.Format("调用Add(100, 200)，返回：{0}", r2));
            
            console.WriteOutput(sb.ToString());
            console.WriteDivider();

            console.WriteSection("5.2 参数传递：值参数、ref参数、out参数");
            console.WriteParagraph("C#中参数默认是值传递（传递副本，方法内修改不影响原变量）；ref传递引用（方法内修改影响原变量，变量必须先初始化）；out输出参数（用于返回多个值，方法内必须赋值）。");
            
            console.WriteCode(@"// 值参数：默认传递方式
static void ChangeValue(int x) { x = 100; }
// ref参数：传递变量引用，必须先初始化
static void ChangeRef(ref int x) { x = 100; }
// out参数：输出参数，方法内必须赋值，不需要初始化
static void Divide(int a, int b, out int result, out int remainder)
{
    result = a / b;
    remainder = a % b;
}");

            StringBuilder sb2 = new StringBuilder();
            int a = 10;
            ChangeValue(a);
            sb2.AppendLine(string.Format("值传递：调用ChangeValue前a={0}，调用后a={1}（未改变）", 10, a));
            
            int b = 10;
            ChangeRef(ref b);
            sb2.AppendLine(string.Format("ref传递：调用ChangeRef前b={0}，调用后b={1}（被修改了）", 10, b));
            
            int res, rem;
            Divide(10, 3, out res, out rem);
            sb2.AppendLine(string.Format("out参数：10除以3，商={0}，余数={1}", res, rem));
            
            console.WriteOutput(sb2.ToString());
            console.WriteTip("out参数非常适合一个方法需要返回多个结果的场景，比如TryParse方法就是用out参数返回转换结果。");
            console.WriteDivider();

            console.WriteSection("5.3 方法重载");
            console.WriteParagraph("方法重载是指同一个类中可以有多个同名方法，只要参数列表不同（参数个数、类型、顺序不同）。重载与返回值类型无关。");
            
            console.WriteCode(@"// 以下三个方法构成重载
static int Add(int a, int b) { return a + b; }
static double Add(double a, double b) { return a + b; }
static int Add(int a, int b, int c) { return a + b + c; }");

            StringBuilder sb3 = new StringBuilder();
            sb3.AppendLine(string.Format("Add(1, 2) → {0}（调用int版本）", Add(1, 2)));
            sb3.AppendLine(string.Format("Add(1.5, 2.5) → {0}（调用double版本）", Add(1.5, 2.5)));
            sb3.AppendLine(string.Format("Add(1, 2, 3) → {0}（调用三个参数版本）", Add(1, 2, 3)));
            
            console.WriteOutput(sb3.ToString());
            console.WriteDivider();

            console.WriteSection("5.4 递归方法");
            console.WriteParagraph("递归是方法自己调用自己，适合解决可以分解为相同子问题的场景，比如阶乘、斐波那契数列。必须有终止条件，否则会栈溢出。");
            
            console.WriteCode(@"// 计算阶乘 n! = n * (n-1)! ，0!=1
static int Factorial(int n)
{
    if (n <= 1) return 1; // 终止条件
    return n * Factorial(n - 1); // 递归调用
}");

            StringBuilder sb4 = new StringBuilder();
            for (int i = 0; i <= 7; i++)
            {
                sb4.AppendLine(string.Format("{0}! = {1}", i, Factorial(i)));
            }
            
            console.WriteOutput(sb4.ToString());
            console.WriteNote("注意：递归虽然写法简洁，但深度过大时会有栈溢出风险，而且性能通常不如循环。实际开发中对于深度递归要谨慎使用。");
            
            console.WriteSuccess("第5章完成！你已经掌握了C#中方法的定义、参数传递、重载和递归。");
        }

        // 辅助方法
        private static void SayHello(StringBuilder sb)
        {
            sb.AppendLine("  Hello! 这是一个无参数方法。");
        }

        private static int Add(int a, int b)
        {
            return a + b;
        }

        private static double Add(double a, double b)
        {
            return a + b;
        }

        private static int Add(int a, int b, int c)
        {
            return a + b + c;
        }

        private static void ChangeValue(int x)
        {
            x = 100;
        }

        private static void ChangeRef(ref int x)
        {
            x = 100;
        }

        private static void Divide(int a, int b, out int result, out int remainder)
        {
            result = a / b;
            remainder = a % b;
        }

        private static int Factorial(int n)
        {
            if (n <= 1) return 1;
            return n * Factorial(n - 1);
        }
    }
}
