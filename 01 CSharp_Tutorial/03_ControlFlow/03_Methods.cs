using System;

namespace CSharp20Tutorial.ControlFlow
{
    /// <summary>
    /// 方法与参数示例
    /// 演示方法定义、参数传递（值参数、ref参数、out参数、params参数）、方法重载
    /// </summary>
    class Methods
    {
        // 无返回值无参数方法
        static void SayHello()
        {
            Console.WriteLine("Hello from method!");
        }
        
        // 带参数有返回值方法
        static int Add(int a, int b)
        {
            return a + b;
        }
        
        // 值传递参数：方法内修改不影响外部变量
        static void PassByValue(int x)
        {
            x = 100;
            Console.WriteLine("方法内x = {0}", x);
        }
        
        // ref引用传递：方法内修改会影响外部变量，变量必须先初始化
        static void PassByRef(ref int x)
        {
            x = 200;
            Console.WriteLine("ref方法内x = {0}", x);
        }
        
        // out输出参数：用于返回多个值，方法内必须赋值
        static void Divide(int dividend, int divisor, out int quotient, out int remainder)
        {
            quotient = dividend / divisor;
            remainder = dividend % divisor;
        }
        
        // params可变参数：可以传递任意数量的同类型参数
        static int Sum(params int[] numbers)
        {
            int total = 0;
            foreach (int num in numbers)
            {
                total += num;
            }
            return total;
        }
        
        // 方法重载：方法名相同，参数列表不同
        static int Max(int a, int b)
        {
            return a > b ? a : b;
        }
        
        static int Max(int a, int b, int c)
        {
            return Max(Max(a, b), c);
        }
        
        static double Max(double a, double b)
        {
            return a > b ? a : b;
        }
        
        // 递归方法：计算阶乘
        static int Factorial(int n)
        {
            if (n <= 1)
                return 1;
            return n * Factorial(n - 1);
        }
        
        // 递归方法：斐波那契数列
        static int Fibonacci(int n)
        {
            if (n <= 2)
                return 1;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("===== 方法基本使用 =====");
            SayHello();
            int result = Add(10, 20);
            Console.WriteLine("10 + 20 = {0}", result);
            
            Console.WriteLine("\n===== 参数传递 =====");
            // 值传递
            int val = 10;
            Console.WriteLine("调用前val = {0}", val);
            PassByValue(val);
            Console.WriteLine("值传递后val = {0}", val);
            
            // ref引用传递
            int refVal = 10;
            Console.WriteLine("\n调用前refVal = {0}", refVal);
            PassByRef(ref refVal);
            Console.WriteLine("ref传递后refVal = {0}", refVal);
            
            // out输出参数
            Console.WriteLine("\nout参数示例：除法运算");
            int q, r;
            Divide(17, 5, out q, out r);
            Console.WriteLine("17 ÷ 5 = {0} 余 {1}", q, r);
            
            // params可变参数
            Console.WriteLine("\nparams可变参数示例：");
            Console.WriteLine("Sum(1,2,3) = {0}", Sum(1, 2, 3));
            Console.WriteLine("Sum(1,2,3,4,5) = {0}", Sum(1, 2, 3, 4, 5));
            int[] nums = { 10, 20, 30, 40 };
            Console.WriteLine("Sum(数组) = {0}", Sum(nums));
            
            Console.WriteLine("\n===== 方法重载 =====");
            Console.WriteLine("Max(10,20) = {0}", Max(10, 20));
            Console.WriteLine("Max(10,20,30) = {0}", Max(10, 20, 30));
            Console.WriteLine("Max(3.14, 2.78) = {0}", Max(3.14, 2.78));
            
            Console.WriteLine("\n===== 递归方法 =====");
            Console.WriteLine("5的阶乘：{0}", Factorial(5));
            Console.WriteLine("斐波那契数列第10项：{0}", Fibonacci(10));
            
            Console.Write("斐波那契数列前10项：");
            for (int i = 1; i <= 10; i++)
            {
                Console.Write("{0} ", Fibonacci(i));
            }
            Console.WriteLine();
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
