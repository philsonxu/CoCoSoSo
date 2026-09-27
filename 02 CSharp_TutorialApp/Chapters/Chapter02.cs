using System;
using System.Text;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第2章：运算符与表达式
    /// </summary>
    public class Chapter02
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第2章 运算符与表达式");

            console.WriteSection("2.1 算术运算符");
            console.WriteParagraph("C#支持标准的数学算术运算符，用于数值计算。");
            
            console.WriteCode(@"int a = 10;
int b = 3;
int add = a + b;    // 加法：13
int sub = a - b;    // 减法：7
int mul = a * b;    // 乘法：30
int div = a / b;    // 整数除法：3（注意不是3.333）
int mod = a % b;    // 取余：1（10除以3余1）
double divDouble = (double)a / b; // 浮点数除法：3.333...");

            StringBuilder sb = new StringBuilder();
            int a = 10;
            int b = 3;
            sb.AppendLine(string.Format("{0} + {1} = {2}", a, b, a + b));
            sb.AppendLine(string.Format("{0} - {1} = {2}", a, b, a - b));
            sb.AppendLine(string.Format("{0} * {1} = {2}", a, b, a * b));
            sb.AppendLine(string.Format("{0} / {1} = {2}（整数除法，截断小数）", a, b, a / b));
            sb.AppendLine(string.Format("{0} % {1} = {2}（取余数）", a, b, a % b));
            sb.AppendLine(string.Format("(double){0} / {1} = {2:F3}（浮点数除法）", a, b, (double)a / b));
            
            console.WriteOutput(sb.ToString());
            console.WriteNote("重点：两个整数相除，结果仍为整数，小数部分会被直接截断，不会四舍五入。要得到小数结果必须先把其中一个数转成double/float/decimal。");
            console.WriteDivider();

            console.WriteSection("2.2 自增自减运算符");
            console.WriteParagraph("++ 和 -- 是C#中非常常用的快捷运算符，分为前缀和后缀两种形式，效果不同。");
            
            console.WriteCode(@"int x = 5;
int y1 = x++; // 后缀：先赋值，后自增 → y1=5，执行后x=6
int m = 5;
int y2 = ++m; // 前缀：先自增，后赋值 → y2=6，执行后m=6");

            StringBuilder sb2 = new StringBuilder();
            int x = 5;
            int y1 = x++;
            sb2.AppendLine(string.Format("int x=5; int y1=x++; → y1={0}, x={1}（先赋值后加）", y1, x));
            int m = 5;
            int y2 = ++m;
            sb2.AppendLine(string.Format("int m=5; int y2=++m; → y2={0}, m={1}（先加后赋值）", y2, m));
            
            console.WriteOutput(sb2.ToString());
            console.WriteTip("经验：在复杂表达式中使用自增自减容易混淆，建议单独写成一行，避免出现在其他表达式内部。");
            console.WriteDivider();

            console.WriteSection("2.3 比较运算符与逻辑运算符");
            console.WriteParagraph("比较运算符返回bool类型结果，常用于if条件判断；逻辑运算符用于组合多个条件。");
            
            console.WriteTableStart();
            console.WriteTableHeader("运算符", "说明", "示例", "结果");
            console.WriteTableRow("==", "等于", "5 == 3", "false");
            console.WriteTableRow("!=", "不等于", "5 != 3", "true");
            console.WriteTableRow(">, <", "大于/小于", "5 > 3", "true");
            console.WriteTableRow(">=, <=", "大于等于/小于等于", "5 >= 5", "true");
            console.WriteTableRow("&&", "逻辑与（并且）", "true && false", "false");
            console.WriteTableRow("||", "逻辑或（或者）", "true || false", "true");
            console.WriteTableRow("!", "逻辑非（取反）", "!true", "false");
            console.WriteTableEnd();

            console.WriteCode(@"int score = 85;
bool pass = score >= 60; // true
bool excellent = score >= 90; // false
bool good = score >= 80 && score < 90; // true
bool failOrPerfect = score < 60 || score == 100; // false");

            StringBuilder sb3 = new StringBuilder();
            int score = 85;
            sb3.AppendLine(string.Format("分数：{0}", score));
            sb3.AppendLine(string.Format("是否及格（>=60）：{0}", score >= 60));
            sb3.AppendLine(string.Format("是否优秀（>=90）：{0}", score >= 90));
            sb3.AppendLine(string.Format("是否良好（80-89）：{0}", score >= 80 && score < 90));
            sb3.AppendLine(string.Format("不及格或满分：{0}", score < 60 || score == 100));
            
            console.WriteOutput(sb3.ToString());
            console.WriteDivider();

            console.WriteSection("2.4 三元运算符");
            console.WriteParagraph("三元运算符是if-else的简写形式，语法：条件 ? 真值 : 假值，是C#中唯一的三目运算符。");
            
            console.WriteCode(@"int s = 75;
string result = s >= 60 ? ""及格"" : ""不及格"";
// 等价于 if(s>=60) result=""及格""; else result=""不及格"";
int max = a > b ? a : b; // 取两个数的较大值");

            StringBuilder sb4 = new StringBuilder();
            int s = 75;
            string result = s >= 60 ? "及格" : "不及格";
            sb4.AppendLine(string.Format("分数{0} → 结果：{1}", s, result));
            int s2 = 55;
            string result2 = s2 >= 60 ? "及格" : "不及格";
            sb4.AppendLine(string.Format("分数{0} → 结果：{1}", s2, result2));
            
            console.WriteOutput(sb4.ToString());
            console.WriteSuccess("第2章完成！你已经掌握了C# 2.0中的常用运算符。");
        }
    }
}
