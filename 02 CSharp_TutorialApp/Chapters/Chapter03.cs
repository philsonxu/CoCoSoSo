using System;
using System.Text;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第3章：条件判断与循环语句
    /// </summary>
    public class Chapter03
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第3章 条件判断与循环语句");

            console.WriteSection("3.1 if-else条件判断");
            console.WriteParagraph("if语句是最基础的分支结构，根据条件真假执行不同代码块，支持多分支else if。");
            
            console.WriteCode(@"int score = 82;
if (score >= 90)
{
    Console.WriteLine(""优秀"");
}
else if (score >= 80)
{
    Console.WriteLine(""良好"");
}
else if (score >= 60)
{
    Console.WriteLine(""及格"");
}
else
{
    Console.WriteLine(""不及格"");
}");

            StringBuilder sb = new StringBuilder();
            int[] scores = new int[] { 95, 82, 65, 58 };
            foreach (int sc in scores)
            {
                string level;
                if (sc >= 90) level = "优秀";
                else if (sc >= 80) level = "良好";
                else if (sc >= 60) level = "及格";
                else level = "不及格";
                sb.AppendLine(string.Format("分数{0} → {1}", sc, level));
            }
            console.WriteOutput(sb.ToString());
            console.WriteDivider();

            console.WriteSection("3.2 switch多分支判断");
            console.WriteParagraph("switch用于一个变量对应多个固定值的分支判断，C# 2.0中只支持整数、char、字符串、枚举类型。注意每个case必须有break（C#不支持case穿透）。");
            
            console.WriteCode(@"int weekday = 3;
switch (weekday)
{
    case 1: Console.WriteLine(""星期一""); break;
    case 2: Console.WriteLine(""星期二""); break;
    case 3: Console.WriteLine(""星期三""); break;
    case 4: Console.WriteLine(""星期四""); break;
    case 5: Console.WriteLine(""星期五""); break;
    case 6:
    case 7: Console.WriteLine(""周末""); break;
    default: Console.WriteLine(""输入错误""); break;
}");

            StringBuilder sb2 = new StringBuilder();
            for (int day = 1; day <= 7; day++)
            {
                string text;
                switch (day)
                {
                    case 1: text = "星期一"; break;
                    case 2: text = "星期二"; break;
                    case 3: text = "星期三"; break;
                    case 4: text = "星期四"; break;
                    case 5: text = "星期五"; break;
                    case 6:
                    case 7: text = "周末"; break;
                    default: text = "错误"; break;
                }
                sb2.AppendLine(string.Format("{0} → {1}", day, text));
            }
            console.WriteOutput(sb2.ToString());
            console.WriteDivider();

            console.WriteSection("3.3 for循环");
            console.WriteParagraph("for循环用于知道循环次数的场景，是最常用的循环结构。经典案例：打印九九乘法表。");
            
            console.WriteCode(@"// 计算1到100的和
int sum = 0;
for (int i = 1; i <= 100; i++)
{
    sum += i;
}
Console.WriteLine(""1到100的和是："" + sum); // 结果5050");

            StringBuilder sb3 = new StringBuilder();
            int sum = 0;
            for (int i = 1; i <= 100; i++)
            {
                sum += i;
            }
            sb3.AppendLine(string.Format("1到100的和：{0}", sum));
            
            sb3.AppendLine("\n九九乘法表：");
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    sb3.Append(string.Format("{0}×{1}={2}\t", j, i, j * i));
                }
                sb3.AppendLine();
            }
            console.WriteOutput(sb3.ToString());
            console.WriteDivider();

            console.WriteSection("3.4 while与do-while循环");
            console.WriteParagraph("while先判断条件再执行，可能一次都不执行；do-while先执行一次再判断条件，保证循环体至少执行一次。");
            
            console.WriteCode(@"// while循环：计算1+2+...+10
int n = 1, s = 0;
while (n <= 10)
{
    s += n;
    n++;
}
// do-while循环：即使条件一开始不满足也会执行一次
int count = 100;
do
{
    Console.WriteLine(count);
    count++;
} while (count < 5); // 只输出100一次");

            StringBuilder sb4 = new StringBuilder();
            int n = 1, s = 0;
            while (n <= 10)
            {
                s += n;
                n++;
            }
            sb4.AppendLine(string.Format("while循环计算1-10的和：{0}", s));
            
            sb4.AppendLine("do-while循环演示（条件count<5初始为false）：");
            int count = 100;
            do
            {
                sb4.AppendLine(string.Format("  执行了循环，count={0}", count));
                count++;
            } while (count < 5);
            sb4.AppendLine("循环结束，仅执行了一次。");
            
            console.WriteOutput(sb4.ToString());
            console.WriteDivider();

            console.WriteSection("3.5 break与continue");
            console.WriteParagraph("break立即跳出整个循环；continue跳过本次循环剩余代码，直接进入下一次循环。");
            
            console.WriteCode(@"// break：找到第一个能被7整除的数就停止
for (int i = 1; i <= 100; i++)
{
    if (i % 7 == 0) 
    {
        Console.WriteLine(""第一个被7整除的数："" + i); // 7
        break;
    }
}
// continue：跳过偶数，只输出奇数
for (int i = 1; i <= 10; i++)
{
    if (i % 2 == 0) continue;
    Console.Write(i + "" ""); // 1 3 5 7 9
}");

            StringBuilder sb5 = new StringBuilder();
            for (int i = 1; i <= 100; i++)
            {
                if (i % 7 == 0)
                {
                    sb5.AppendLine(string.Format("break示例：1-100中第一个被7整除的数是{0}", i));
                    break;
                }
            }
            
            sb5.Append("continue示例：1-10中的奇数：");
            for (int i = 1; i <= 10; i++)
            {
                if (i % 2 == 0) continue;
                sb5.Append(i + " ");
            }
            sb5.AppendLine();
            
            console.WriteOutput(sb5.ToString());
            console.WriteSuccess("第3章完成！你已经掌握了C#中的所有流程控制语句，可以编写逻辑复杂的程序了。");
        }
    }
}
