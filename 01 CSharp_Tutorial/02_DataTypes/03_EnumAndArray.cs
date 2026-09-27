using System;

namespace CSharp20Tutorial.DataTypes
{
    /// <summary>
    /// 枚举与数组示例
    /// 演示C#中的枚举类型定义、一维数组、多维数组、交错数组
    /// </summary>
    class EnumAndArray
    {
        // 定义枚举类型
        enum Weekday
        {
            Monday = 1,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }
        
        enum Gender
        {
            男,
            女,
            保密
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("===== 枚举类型 =====");
            Weekday today = Weekday.Wednesday;
            Console.WriteLine("今天是：{0}，对应数值：{1}", today, (int)today);
            
            Gender userGender = Gender.男;
            Console.WriteLine("用户性别：{0}", userGender);
            
            // 枚举遍历
            Console.WriteLine("\n一周所有天：");
            foreach (Weekday day in Enum.GetValues(typeof(Weekday)))
            {
                Console.WriteLine("  {0}. {1}", (int)day, day);
            }
            
            Console.WriteLine("\n===== 一维数组 =====");
            // 数组声明与初始化
            int[] numbers = new int[5];           // 声明长度为5的int数组
            numbers[0] = 10;
            numbers[1] = 20;
            numbers[2] = 30;
            numbers[3] = 40;
            numbers[4] = 50;
            
            // 声明同时初始化
            int[] scores = { 85, 92, 78, 95, 88, 90 };
            string[] fruits = { "苹果", "香蕉", "橙子", "葡萄", "西瓜" };
            
            // 遍历数组
            Console.WriteLine("numbers数组元素：");
            for (int i = 0; i < numbers.Length; i++)
            {
                Console.WriteLine("  numbers[{0}] = {1}", i, numbers[i]);
            }
            
            Console.WriteLine("\n使用foreach遍历水果数组：");
            int index = 0;
            foreach (string fruit in fruits)
            {
                Console.WriteLine("  水果{0}：{1}", ++index, fruit);
            }
            
            // 数组常用操作
            Console.WriteLine("\n数组常用属性和方法：");
            Console.WriteLine("scores数组长度：{0}", scores.Length);
            Console.WriteLine("scores第一个元素：{0}", scores[0]);
            Console.WriteLine("scores最后一个元素：{0}", scores[scores.Length - 1]);
            
            // 排序
            Array.Sort(scores);
            Console.WriteLine("排序后scores：");
            foreach (int s in scores)
            {
                Console.Write("{0} ", s);
            }
            Console.WriteLine();
            
            // 求最大值、最小值、总和
            int max = scores[0];
            int min = scores[0];
            int sum = 0;
            foreach (int s in scores)
            {
                if (s > max) max = s;
                if (s < min) min = s;
                sum += s;
            }
            Console.WriteLine("最高分：{0}，最低分：{1}，总分：{2}，平均分：{3:F1}", 
                max, min, sum, (double)sum / scores.Length);
            
            Console.WriteLine("\n===== 二维数组 =====");
            // 二维数组声明
            int[,] matrix = new int[3, 3];  // 3行3列矩阵
            matrix[0, 0] = 1; matrix[0, 1] = 2; matrix[0, 2] = 3;
            matrix[1, 0] = 4; matrix[1, 1] = 5; matrix[1, 2] = 6;
            matrix[2, 0] = 7; matrix[2, 1] = 8; matrix[2, 2] = 9;
            
            // 初始化二维数组
            int[,] table = {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            
            Console.WriteLine("3x3矩阵：");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write("{0}\t", matrix[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("矩阵行数：{0}，列数：{1}", matrix.GetLength(0), matrix.GetLength(1));
            
            Console.WriteLine("\n===== 交错数组（数组的数组）=====");
            // 交错数组：每行长度可以不同
            int[][] jaggedArray = new int[3][];
            jaggedArray[0] = new int[] { 1, 2 };
            jaggedArray[1] = new int[] { 3, 4, 5 };
            jaggedArray[2] = new int[] { 6, 7, 8, 9 };
            
            Console.WriteLine("交错数组：");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                Console.Write("第{0}行：", i + 1);
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    Console.Write("{0} ", jaggedArray[i][j]);
                }
                Console.WriteLine();
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
