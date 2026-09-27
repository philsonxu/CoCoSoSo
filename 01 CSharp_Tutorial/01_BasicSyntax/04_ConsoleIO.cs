using System;

namespace CSharp20Tutorial.BasicSyntax
{
    /// <summary>
    /// 控制台输入输出示例
    /// 演示Console类的常用输入输出方法
    /// </summary>
    class ConsoleIO
    {
        static void Main(string[] args)
        {
            // 格式化输出
            Console.WriteLine("===== 格式化输出示例 =====");
            string name = "李四";
            int age = 23;
            double score = 95.5;
            
            // 使用占位符
            Console.WriteLine("姓名：{0}，年龄：{1}岁，成绩：{2}分", name, age, score);
            
            // 使用字符串插值（C# 2.0不支持，这里用string.Format）
            string info = string.Format("学员信息：{0} - {1}岁 - {2:F1}分", name, age, score);
            Console.WriteLine(info);
            
            // 格式化数值
            Console.WriteLine("\n数值格式化：");
            double number = 12345.6789;
            Console.WriteLine("货币格式：{0:C}", number);      // 货币
            Console.WriteLine("固定小数位：{0:F2}", number);  // 保留2位小数
            Console.WriteLine("科学计数：{0:E}", number);     // 科学计数法
            Console.WriteLine("百分比：{0:P}", 0.856);        // 百分比
            
            // 控制台输入
            Console.WriteLine("\n===== 控制台输入示例 =====");
            Console.Write("请输入您的姓名：");
            string inputName = Console.ReadLine();
            
            Console.Write("请输入您的年龄：");
            string ageInput = Console.ReadLine();
            int inputAge = int.Parse(ageInput);  // 字符串转整数
            
            Console.Write("请输入您的身高(米)：");
            string heightInput = Console.ReadLine();
            double inputHeight = double.Parse(heightInput);
            
            Console.WriteLine("\n===== 您输入的信息 =====");
            Console.WriteLine("姓名：{0}", inputName);
            Console.WriteLine("年龄：{0}岁", inputAge);
            Console.WriteLine("身高：{0}米", inputHeight);
            Console.WriteLine("明年您将{0}岁", inputAge + 1);
            
            // ReadKey示例
            Console.WriteLine("\n按任意键继续...");
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine("\n您按下了：{0} 键", key.Key);
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
