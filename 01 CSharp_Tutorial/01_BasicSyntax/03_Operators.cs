using System;

namespace CSharp20Tutorial.BasicSyntax
{
    /// <summary>
    /// 运算符示例
    /// 演示C#中的算术、赋值、比较、逻辑等运算符
    /// </summary>
    class Operators
    {
        static void Main(string[] args)
        {
            int a = 10;
            int b = 3;
            
            Console.WriteLine("===== 算术运算符 =====");
            Console.WriteLine("a = {0}, b = {1}", a, b);
            Console.WriteLine("加法：a + b = {0}", a + b);
            Console.WriteLine("减法：a - b = {0}", a - b);
            Console.WriteLine("乘法：a * b = {0}", a * b);
            Console.WriteLine("除法：a / b = {0}", a / b);       // 整数除法
            Console.WriteLine("取余：a % b = {0}", a % b);       // 取模
            Console.WriteLine("自增：a++ = {0}", a++);           // 后置自增
            Console.WriteLine("自增后a = {0}", a);
            Console.WriteLine("前置自增：++b = {0}", ++b);
            
            Console.WriteLine("\n===== 赋值运算符 =====");
            int c = 5;
            Console.WriteLine("初始c = {0}", c);
            c += 3;  // c = c + 3
            Console.WriteLine("c += 3 后 c = {0}", c);
            c -= 2;  // c = c - 2
            Console.WriteLine("c -= 2 后 c = {0}", c);
            c *= 4;  // c = c * 4
            Console.WriteLine("c *= 4 后 c = {0}", c);
            c /= 2;  // c = c / 2
            Console.WriteLine("c /= 2 后 c = {0}", c);
            c %= 3;  // c = c % 3
            Console.WriteLine("c %= 3 后 c = {0}", c);
            
            Console.WriteLine("\n===== 比较运算符 =====");
            Console.WriteLine("a > b : {0}", a > b);
            Console.WriteLine("a < b : {0}", a < b);
            Console.WriteLine("a == b : {0}", a == b);
            Console.WriteLine("a != b : {0}", a != b);
            Console.WriteLine("a >= b : {0}", a >= b);
            Console.WriteLine("a <= b : {0}", a <= b);
            
            Console.WriteLine("\n===== 逻辑运算符 =====");
            bool x = true;
            bool y = false;
            Console.WriteLine("x = {0}, y = {1}", x, y);
            Console.WriteLine("逻辑与：x && y = {0}", x && y);
            Console.WriteLine("逻辑或：x || y = {0}", x || y);
            Console.WriteLine("逻辑非：!x = {0}", !x);
            
            Console.WriteLine("\n===== 三元运算符 =====");
            int max = (a > b) ? a : b;
            Console.WriteLine("a和b中的最大值是：{0}", max);
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
