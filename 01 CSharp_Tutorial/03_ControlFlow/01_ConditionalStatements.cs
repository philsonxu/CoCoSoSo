using System;

namespace CSharp20Tutorial.ControlFlow
{
    /// <summary>
    /// 条件语句示例
    /// 演示if-else、switch-case条件分支
    /// </summary>
    class ConditionalStatements
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== if-else语句 =====");
            
            // 简单if语句
            int score = 85;
            Console.WriteLine("考试分数：{0}", score);
            
            if (score >= 60)
            {
                Console.WriteLine("成绩及格！");
            }
            else
            {
                Console.WriteLine("成绩不及格！");
            }
            
            // 多分支if-else if-else
            Console.WriteLine("\n成绩等级判定：");
            if (score >= 90)
            {
                Console.WriteLine("优秀（A）");
            }
            else if (score >= 80)
            {
                Console.WriteLine("良好（B）");
            }
            else if (score >= 70)
            {
                Console.WriteLine("中等（C）");
            }
            else if (score >= 60)
            {
                Console.WriteLine("及格（D）");
            }
            else
            {
                Console.WriteLine("不及格（F）");
            }
            
            // 嵌套if
            Console.WriteLine("\n===== 嵌套if示例：闰年判断 =====");
            Console.Write("请输入年份：");
            int year = int.Parse(Console.ReadLine());
            
            bool isLeapYear = false;
            if (year % 4 == 0)
            {
                if (year % 100 == 0)
                {
                    if (year % 400 == 0)
                    {
                        isLeapYear = true;
                    }
                    else
                    {
                        isLeapYear = false;
                    }
                }
                else
                {
                    isLeapYear = true;
                }
            }
            else
            {
                isLeapYear = false;
            }
            
            Console.WriteLine("{0}年{1}闰年", year, isLeapYear ? "是" : "不是");
            
            // switch-case语句
            Console.WriteLine("\n===== switch-case语句 =====");
            Console.Write("请输入星期数字(1-7)：");
            int dayNum = int.Parse(Console.ReadLine());
            string dayName;
            
            switch (dayNum)
            {
                case 1:
                    dayName = "星期一";
                    break;
                case 2:
                    dayName = "星期二";
                    break;
                case 3:
                    dayName = "星期三";
                    break;
                case 4:
                    dayName = "星期四";
                    break;
                case 5:
                    dayName = "星期五";
                    break;
                case 6:
                    dayName = "星期六";
                    break;
                case 7:
                    dayName = "星期日";
                    break;
                default:
                    dayName = "无效输入";
                    break;
            }
            Console.WriteLine("今天是：{0}", dayName);
            
            // switch用于枚举
            Console.WriteLine("\n===== switch结合枚举 =====");
            Console.Write("请输入成绩等级(A/B/C/D/F)：");
            string gradeInput = Console.ReadLine().ToUpper();
            char grade = gradeInput[0];
            
            switch (grade)
            {
                case 'A':
                    Console.WriteLine("优秀，继续保持！");
                    break;
                case 'B':
                    Console.WriteLine("良好，还可以更好！");
                    break;
                case 'C':
                    Console.WriteLine("中等，加油！");
                    break;
                case 'D':
                    Console.WriteLine("及格，需要努力！");
                    break;
                case 'F':
                    Console.WriteLine("不及格，补考见！");
                    break;
                default:
                    Console.WriteLine("无效的等级输入");
                    break;
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
