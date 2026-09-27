using System;
using System.Text;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第1章：Hello World与基础语法
    /// 仅使用C# 2.0特性
    /// </summary>
    public class Chapter01
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第1章 Hello World与基础语法");
            
            console.WriteSection("1.1 第一个C#程序：Hello World");
            console.WriteParagraph("C# 2.0最经典的入门程序，完整的程序结构如下：");
            
            console.WriteCode(@"using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
            Console.WriteLine(""欢迎来到C# 2.0的世界！"");
        }
    }
}");
            
            console.WriteSubSection("运行结果：");
            console.WriteOutput("Hello World!\n欢迎来到C# 2.0的世界！");
            
            console.WriteTip("Main方法是C#程序的固定入口点，static表示静态方法，void表示无返回值。");
            console.WriteDivider();

            console.WriteSection("1.2 基本数据类型");
            console.WriteParagraph("C#是强类型语言，每个变量都有明确的类型。C# 2.0提供以下常用值类型：");
            
            console.WriteTableStart();
            console.WriteTableHeader("类型", "别名", "说明", "示例值");
            console.WriteTableRow("System.Int32", "int", "32位整数", "-1, 0, 100, 9999");
            console.WriteTableRow("System.Double", "double", "双精度浮点数", "3.14, 1.0, -2.5");
            console.WriteTableRow("System.Boolean", "bool", "布尔值", "true / false");
            console.WriteTableRow("System.Char", "char", "单个Unicode字符", "'A', '中', '1'");
            console.WriteTableRow("System.String", "string", "字符串（引用类型）", "\"你好\"");
            console.WriteTableRow("System.Decimal", "decimal", "高精度十进制数", "100.50m（财务计算用）");
            console.WriteTableEnd();

            console.WriteCode(@"int age = 25;
double price = 99.9;
bool isStudent = true;
char grade = 'A';
string name = ""张三"";
decimal money = 1000.50m;

console.WriteLine(string.Format(""姓名：{0}，年龄：{1}"", name, age));
console.WriteLine(string.Format(""价格：{0}元，是学生：{1}"", price, isStudent));");

            // 实际运行代码，输出结果
            StringBuilder sb = new StringBuilder();
            int age = 25;
            double price = 99.9;
            bool isStudent = true;
            char grade = 'A';
            string name = "张三";
            decimal money = 1000.50m;
            
            sb.AppendLine(string.Format("姓名：{0}，年龄：{1}", name, age));
            sb.AppendLine(string.Format("价格：{0}元，是学生：{1}", price, isStudent));
            sb.AppendLine(string.Format("等级：{0}，余额：{1:C}", grade, money));
            
            console.WriteSubSection("运行结果：");
            console.WriteOutput(sb.ToString());
            console.WriteDivider();

            console.WriteSection("1.3 变量声明与类型转换");
            console.WriteParagraph("C# 2.0中必须显式声明变量类型，不支持var推断（var是C#3.0才引入的）。类型转换分为隐式转换和显式转换。");
            
            console.WriteCode(@"int i = 10;
// 隐式转换：int自动转double（无精度损失）
double d = i;
// 显式转换：double转int需要强制转换（会丢失小数部分）
double pi = 3.1415;
int piInt = (int)pi; // 结果为3
// 字符串转数值
string numStr = ""123"";
int num = int.Parse(numStr); // 转换成功为123");

            StringBuilder sb2 = new StringBuilder();
            int i = 10;
            double d = i;
            double pi = 3.1415;
            int piInt = (int)pi;
            string numStr = "123";
            int num = int.Parse(numStr);
            
            sb2.AppendLine(string.Format("隐式转换 int i={0} → double d={1}", i, d));
            sb2.AppendLine(string.Format("显式转换 double pi={0} → int piInt={1}（丢失小数部分）", pi, piInt));
            sb2.AppendLine(string.Format("字符串\"{0}\"解析为int：{1}", numStr, num));
            
            console.WriteSubSection("运行结果：");
            console.WriteOutput(sb2.ToString());

            console.WriteNote("注意：使用Parse转换时如果字符串格式不正确会抛出异常，更安全的做法是使用TryParse方法（第8章异常处理会详细讲解）。");
            
            console.WriteDivider();
            console.WriteSuccess("第1章完成！你已经学会了C# 2.0的基础程序结构、基本数据类型和类型转换。");
        }
    }
}
