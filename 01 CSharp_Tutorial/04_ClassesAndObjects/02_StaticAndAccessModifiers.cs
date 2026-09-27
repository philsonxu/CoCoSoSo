using System;

namespace CSharp20Tutorial.ClassesAndObjects
{
    /// <summary>
    /// 静态成员与访问修饰符示例
    /// 演示静态字段、静态方法、静态类，以及public/private/protected/internal访问修饰符
    /// </summary>
    
    // 计算器类，包含静态成员
    class Calculator
    {
        // 静态字段：属于类本身，所有实例共享
        public static string Version = "1.0.0";
        private static int _instanceCount = 0;
        
        // 实例字段
        private string _name;
        
        // 静态构造函数：在第一次使用类时调用，只执行一次
        static Calculator()
        {
            Console.WriteLine("静态构造函数被调用，Calculator类初始化");
        }
        
        // 实例构造函数
        public Calculator(string name)
        {
            _name = name;
            _instanceCount++;
        }
        
        // 静态方法：不需要实例化即可调用，不能访问实例成员
        public static int Add(int a, int b)
        {
            return a + b;
        }
        
        public static int Subtract(int a, int b)
        {
            return a - b;
        }
        
        public static int Multiply(int a, int b)
        {
            return a * b;
        }
        
        public static double Divide(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("除数不能为零");
            }
            return a / b;
        }
        
        // 静态方法获取实例数量
        public static int GetInstanceCount()
        {
            return _instanceCount;
        }
        
        // 实例方法
        public void PrintInfo()
        {
            Console.WriteLine("计算器{0}，版本{1}", _name, Version);
        }
    }
    
    // 静态类：不能实例化，只能包含静态成员
    static class MathUtils
    {
        public const double PI = 3.1415926535;
        
        public static double CircleArea(double radius)
        {
            return PI * radius * radius;
        }
        
        public static double CirclePerimeter(double radius)
        {
            return 2 * PI * radius;
        }
        
        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }
        
        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }
        
        public static int Abs(int x)
        {
            return x >= 0 ? x : -x;
        }
    }
    
    // 访问修饰符演示
    class AccessDemo
    {
        // public：公共的，任何地方都可以访问
        public int PublicField = 10;
        
        // private：私有的，只有当前类内部可以访问
        private int _privateField = 20;
        
        // protected：受保护的，当前类和派生类可以访问
        protected int ProtectedField = 30;
        
        // internal：内部的，同一程序集内可以访问
        internal int InternalField = 40;
        
        public void ShowPrivate()
        {
            Console.WriteLine("私有字段值：{0}", _privateField);
        }
    }
    
    class StaticAndAccessModifiers
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 静态成员示例 =====");
            // 静态方法不需要实例化，直接通过类名调用
            Console.WriteLine("计算器版本：{0}", Calculator.Version);
            Console.WriteLine("10 + 20 = {0}", Calculator.Add(10, 20));
            Console.WriteLine("10 - 5 = {0}", Calculator.Subtract(10, 5));
            Console.WriteLine("3 * 4 = {0}", Calculator.Multiply(3, 4));
            Console.WriteLine("10 / 3 = {0:F2}", Calculator.Divide(10, 3));
            
            Console.WriteLine("\n当前实例数量：{0}", Calculator.GetInstanceCount());
            
            // 创建实例
            Calculator calc1 = new Calculator("科学计算器");
            calc1.PrintInfo();
            Calculator calc2 = new Calculator("普通计算器");
            calc2.PrintInfo();
            
            Console.WriteLine("创建实例后数量：{0}", Calculator.GetInstanceCount());
            
            Console.WriteLine("\n===== 静态类示例 =====");
            // 静态类不能实例化，直接调用
            Console.WriteLine("PI = {0}", MathUtils.PI);
            Console.WriteLine("半径为5的圆面积：{0:F2}", MathUtils.CircleArea(5));
            Console.WriteLine("半径为5的圆周长：{0:F2}", MathUtils.CirclePerimeter(5));
            Console.WriteLine("Max(10,20) = {0}", MathUtils.Max(10, 20));
            Console.WriteLine("Abs(-15) = {0}", MathUtils.Abs(-15));
            
            Console.WriteLine("\n===== 访问修饰符示例 =====");
            AccessDemo demo = new AccessDemo();
            Console.WriteLine("public字段：{0}", demo.PublicField);
            Console.WriteLine("internal字段：{0}", demo.InternalField);
            // Console.WriteLine(demo._privateField);  // 错误！私有字段外部不能访问
            // Console.WriteLine(demo.ProtectedField); // 错误！保护字段外部不能访问
            demo.ShowPrivate();  // 通过公共方法访问私有字段
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
