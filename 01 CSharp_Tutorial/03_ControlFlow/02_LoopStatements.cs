using System;

namespace CSharp20Tutorial.ControlFlow
{
    /// <summary>
    /// 循环语句示例
    /// 演示for、while、do-while、foreach循环，以及break、continue跳转语句
    /// </summary>
    class LoopStatements
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== for循环：输出1-10 =====");
            for (int i = 1; i <= 10; i++)
            {
                Console.Write("{0} ", i);
            }
            Console.WriteLine();
            
            // for循环计算1-100的和
            int sum = 0;
            for (int i = 1; i <= 100; i++)
            {
                sum += i;
            }
            Console.WriteLine("1到100的和：{0}", sum);
            
            // for循环打印九九乘法表
            Console.WriteLine("\n===== for循环：九九乘法表 =====");
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    Console.Write("{0}x{1}={2}\t", j, i, j * i);
                }
                Console.WriteLine();
            }
            
            Console.WriteLine("\n===== while循环 =====");
            // while循环计算斐波那契数列前20项
            Console.WriteLine("斐波那契数列前20项：");
            int a = 1, b = 1;
            int count = 0;
            while (count < 20)
            {
                Console.Write("{0} ", a);
                int temp = a;
                a = b;
                b = temp + b;
                count++;
            }
            Console.WriteLine();
            
            // while循环猜数字游戏
            Console.WriteLine("\n===== while循环：猜数字游戏 =====");
            Random random = new Random();
            int target = random.Next(1, 101);  // 生成1-100的随机数
            int guess = 0;
            int guessCount = 0;
            
            Console.WriteLine("我已经想好了一个1-100之间的数字，请猜一猜：");
            while (guess != target)
            {
                Console.Write("请输入你的猜测：");
                guess = int.Parse(Console.ReadLine());
                guessCount++;
                
                if (guess > target)
                {
                    Console.WriteLine("太大了！再小一点。");
                }
                else if (guess < target)
                {
                    Console.WriteLine("太小了！再大一点。");
                }
                else
                {
                    Console.WriteLine("恭喜你猜对了！答案就是{0}，你共猜了{1}次。", target, guessCount);
                }
            }
            
            Console.WriteLine("\n===== do-while循环 =====");
            // do-while保证至少执行一次
            string input;
            do
            {
                Console.WriteLine("这是do-while循环，至少执行一次");
                Console.Write("是否继续？(y/n)：");
                input = Console.ReadLine().ToLower();
            } while (input == "y");
            
            Console.WriteLine("\n===== foreach循环 =====");
            // foreach用于遍历集合或数组
            string[] colors = { "红色", "橙色", "黄色", "绿色", "青色", "蓝色", "紫色" };
            Console.WriteLine("彩虹颜色：");
            foreach (string color in colors)
            {
                Console.WriteLine("  - {0}", color);
            }
            
            Console.WriteLine("\n===== break和continue =====");
            // break：跳出整个循环
            Console.WriteLine("break示例：找到第一个能被7整除的数(1-50)");
            for (int i = 1; i <= 50; i++)
            {
                if (i % 7 == 0)
                {
                    Console.WriteLine("找到：{0}", i);
                    break;  // 找到后立即跳出循环
                }
            }
            
            // continue：跳过本次循环，继续下一次
            Console.WriteLine("\ncontinue示例：输出1-20中不能被3整除的数");
            for (int i = 1; i <= 20; i++)
            {
                if (i % 3 == 0)
                {
                    continue;  // 跳过能被3整除的数
                }
                Console.Write("{0} ", i);
            }
            Console.WriteLine();
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
