using System;

namespace CSharp20Tutorial.DataTypes
{
    /// <summary>
    /// 值类型示例
    /// 演示C#中的基本值类型：整型、浮点型、布尔型、字符型、结构体
    /// </summary>
    class ValueTypes
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 整型类型 =====");
            sbyte sb = -128;                 // 8位有符号整数 -128~127
            byte b = 255;                    // 8位无符号整数 0~255
            short s = -32768;                // 16位有符号整数
            ushort us = 65535;               // 16位无符号整数
            int i = 2147483647;              // 32位有符号整数
            uint ui = 4294967295;            // 32位无符号整数
            long l = 9223372036854775807;    // 64位有符号整数
            ulong ul = 18446744073709551615; // 64位无符号整数
            
            Console.WriteLine("sbyte: {0} ~ {1}", sbyte.MinValue, sbyte.MaxValue);
            Console.WriteLine("byte: {0} ~ {1}", byte.MinValue, byte.MaxValue);
            Console.WriteLine("short: {0} ~ {1}", short.MinValue, short.MaxValue);
            Console.WriteLine("int: {0} ~ {1}", int.MinValue, int.MaxValue);
            Console.WriteLine("long: {0} ~ {1}", long.MinValue, long.MaxValue);
            
            Console.WriteLine("\n===== 浮点类型 =====");
            float f = 3.14f;                 // 单精度浮点，需加f后缀
            double d = 3.1415926535;         // 双精度浮点
            decimal dec = 123.456m;          // 高精度十进制，需加m后缀
            
            Console.WriteLine("float示例：{0}", f);
            Console.WriteLine("double示例：{0}", d);
            Console.WriteLine("decimal示例：{0} (适用于金融计算)", dec);
            Console.WriteLine("float精度：约6-9位有效数字");
            Console.WriteLine("double精度：约15-17位有效数字");
            Console.WriteLine("decimal精度：约28-29位有效数字");
            
            Console.WriteLine("\n===== 布尔与字符类型 =====");
            bool flag = true;                // 布尔值 true/false
            char ch = 'A';                   // Unicode字符
            char chineseChar = '中';
            
            Console.WriteLine("布尔值：{0}", flag);
            Console.WriteLine("字符：{0}，ASCII码：{1}", ch, (int)ch);
            Console.WriteLine("中文字符：{0}，Unicode码：{1}", chineseChar, (int)chineseChar);
            
            Console.WriteLine("\n===== 可空类型（Nullable）=====");
            // C# 2.0引入可空类型，值类型可以赋值为null
            int? nullableInt = null;
            Console.WriteLine("可空int初始值：{0}", nullableInt.HasValue ? nullableInt.Value.ToString() : "空值");
            nullableInt = 100;
            Console.WriteLine("赋值后可空int：{0}", nullableInt.Value);
            Console.WriteLine("使用??运算符：{0}", nullableInt ?? -1);
            
            // 结构体示例
            Console.WriteLine("\n===== 结构体示例 =====");
            Point p1 = new Point();
            p1.X = 10;
            p1.Y = 20;
            Console.WriteLine("点坐标：({0}, {1})", p1.X, p1.Y);
            
            Point p2 = new Point(5, 15);
            Console.WriteLine("点p2到原点距离：{0:F2}", p2.DistanceToOrigin());
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
    
    /// <summary>
    /// 自定义结构体（值类型）
    /// </summary>
    struct Point
    {
        public int X;
        public int Y;
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public double DistanceToOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
    }
}
