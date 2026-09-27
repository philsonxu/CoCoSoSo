using System;
using System.Text;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第4章：数组与字符串
    /// </summary>
    public class Chapter04
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第4章 数组与字符串");

            console.WriteSection("4.1 一维数组");
            console.WriteParagraph("数组是存储多个相同类型数据的集合，长度固定，下标从0开始。C# 2.0中数组是引用类型。");
            
            console.WriteCode(@"// 声明并初始化数组的几种方式
int[] nums1 = new int[5]; // 长度为5，默认值都是0
int[] nums2 = new int[] { 10, 20, 30, 40, 50 }; // 初始化赋值
int[] nums3 = { 1, 2, 3, 4, 5 }; // 简写形式
int len = nums3.Length; // 获取数组长度：5
int first = nums3[0]; // 访问第一个元素：1
nums3[0] = 100; // 修改元素");

            StringBuilder sb = new StringBuilder();
            int[] scores = { 85, 92, 78, 90, 88, 76, 95 };
            sb.AppendLine("数组元素：");
            for (int i = 0; i < scores.Length; i++)
            {
                sb.AppendLine(string.Format("  scores[{0}] = {1}", i, scores[i]));
            }
            
            int sum = 0;
            int max = scores[0];
            int min = scores[0];
            foreach (int sc in scores)
            {
                sum += sc;
                if (sc > max) max = sc;
                if (sc < min) min = sc;
            }
            double avg = (double)sum / scores.Length;
            sb.AppendLine(string.Format("总分：{0}，平均分：{1:F1}", sum, avg));
            sb.AppendLine(string.Format("最高分：{0}，最低分：{1}", max, min));
            
            console.WriteOutput(sb.ToString());
            console.WriteTip("foreach是遍历数组最简洁的方式，不需要下标，但是不能在foreach中修改数组元素。");
            console.WriteDivider();

            console.WriteSection("4.2 二维数组");
            console.WriteParagraph("C#支持多维数组，最常用的是二维数组，类似表格，有行和列两个下标。");
            
            console.WriteCode(@"// 声明3行4列的二维数组
int[,] matrix = new int[3, 4];
// 初始化二维数组
int[,] table = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
int rows = table.GetLength(0); // 行数：3
int cols = table.GetLength(1); // 列数：3
int center = table[1, 1]; // 中间元素：5");

            StringBuilder sb2 = new StringBuilder();
            int[,] table = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);
            sb2.AppendLine("3×3二维数组：");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sb2.Append(string.Format("{0}\t", table[i, j]));
                }
                sb2.AppendLine();
            }
            int diagonalSum = 0;
            for (int i = 0; i < rows; i++)
            {
                diagonalSum += table[i, i];
            }
            sb2.AppendLine(string.Format("对角线元素和：{0}", diagonalSum));
            
            console.WriteOutput(sb2.ToString());
            console.WriteDivider();

            console.WriteSection("4.3 字符串常用操作");
            console.WriteParagraph("string是C#中最常用的引用类型，不可变（每次修改都会创建新字符串），提供了非常多的实用方法。");
            
            console.WriteCode(@"string s = ""Hello, C# 2.0"";
int len = s.Length; // 长度
string sub = s.Substring(7, 4); // 截取子串：""C# 2""
int idx = s.IndexOf('C'); // 查找字符位置：7
string rep = s.Replace(""Hello"", ""你好""); // 替换
string[] parts = s.Split(','); // 分割
string upper = s.ToUpper(); // 转大写
string lower = s.ToLower(); // 转小写
string trim = ""  abc  "".Trim(); // 去除首尾空格");

            StringBuilder sb3 = new StringBuilder();
            string s = "Hello, C# 2.0 World";
            sb3.AppendLine(string.Format("原字符串：\"{0}\"", s));
            sb3.AppendLine(string.Format("长度：{0}", s.Length));
            sb3.AppendLine(string.Format("是否包含\"C#\"：{0}", s.Contains("C#")));
            sb3.AppendLine(string.Format("'C'第一次出现的位置：{0}", s.IndexOf('C')));
            sb3.AppendLine(string.Format("截取从索引7开始的4个字符：\"{0}\"", s.Substring(7, 4)));
            sb3.AppendLine(string.Format("替换Hello为你好：\"{0}\"", s.Replace("Hello", "你好")));
            sb3.AppendLine(string.Format("转大写：\"{0}\"", s.ToUpper()));
            
            string csv = "苹果,香蕉,橘子,葡萄";
            string[] fruits = csv.Split(',');
            sb3.AppendLine("\n按逗号分割字符串结果：");
            foreach (string f in fruits)
            {
                sb3.AppendLine(string.Format("  - {0}", f));
            }
            
            console.WriteOutput(sb3.ToString());
            console.WriteNote("重点：字符串是不可变的，所有修改字符串的方法（Replace/ToUpper/Substring等）都不会改变原字符串，而是返回一个新的字符串。");
            console.WriteDivider();

            console.WriteSection("4.4 StringBuilder高效字符串拼接");
            console.WriteParagraph("如果需要频繁拼接字符串，直接用+运算符会产生大量临时字符串，效率很低。这时候应该用StringBuilder（位于System.Text命名空间）。");
            
            console.WriteCode(@"StringBuilder sb = new StringBuilder();
for (int i = 1; i <= 5; i++)
{
    sb.AppendFormat(""第{0}行\n"", i);
}
string result = sb.ToString(); // 最后一次性转成string");

            StringBuilder sb4 = new StringBuilder();
            StringBuilder testSb = new StringBuilder();
            DateTime start = DateTime.Now;
            for (int i = 0; i < 10000; i++)
            {
                testSb.Append(i.ToString());
            }
            DateTime end = DateTime.Now;
            TimeSpan ts1 = end - start;
            
            string testStr = "";
            start = DateTime.Now;
            for (int i = 0; i < 10000; i++)
            {
                testStr += i.ToString();
            }
            end = DateTime.Now;
            TimeSpan ts2 = end - start;
            
            sb4.AppendLine("拼接10000次数字耗时对比：");
            sb4.AppendLine(string.Format("  StringBuilder耗时：{0}毫秒", ts1.TotalMilliseconds));
            sb4.AppendLine(string.Format("  直接string+耗时：{0}毫秒", ts2.TotalMilliseconds));
            sb4.AppendLine("结论：大量拼接字符串一定要用StringBuilder！");
            
            console.WriteOutput(sb4.ToString());
            console.WriteSuccess("第4章完成！你已经掌握了C# 2.0中数组和字符串的用法。");
        }
    }
}
