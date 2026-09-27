using System;

namespace CSharp20Tutorial.DataTypes
{
    /// <summary>
    /// 引用类型示例
    /// 演示C#中的string字符串、object类型、类型转换
    /// </summary>
    class ReferenceTypes
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 字符串（string）类型 =====");
            string str1 = "Hello";
            string str2 = "World";
            string str3 = str1 + " " + str2;  // 字符串拼接
            
            Console.WriteLine("str1: {0}", str1);
            Console.WriteLine("str2: {0}", str2);
            Console.WriteLine("str3: {0}", str3);
            
            // 字符串常用方法
            Console.WriteLine("\n字符串常用方法：");
            Console.WriteLine("长度：{0}", str3.Length);
            Console.WriteLine("转为大写：{0}", str3.ToUpper());
            Console.WriteLine("转为小写：{0}", str3.ToLower());
            Console.WriteLine("包含'll'：{0}", str3.Contains("ll"));
            Console.WriteLine("以'He'开头：{0}", str3.StartsWith("He"));
            Console.WriteLine("以'ld'结尾：{0}", str3.EndsWith("ld"));
            Console.WriteLine("索引位置：{0}", str3.IndexOf("o"));
            Console.WriteLine("子字符串：{0}", str3.Substring(6, 3));
            Console.WriteLine("替换：{0}", str3.Replace("World", "C#"));
            
            // 字符串分割与连接
            Console.WriteLine("\n字符串分割与连接：");
            string names = "张三,李四,王五,赵六";
            string[] nameArray = names.Split(',');
            Console.WriteLine("分割后共{0}个姓名：", nameArray.Length);
            foreach (string name in nameArray)
            {
                Console.WriteLine("  - {0}", name);
            }
            string joinedNames = string.Join(" | ", nameArray);
            Console.WriteLine("连接后：{0}", joinedNames);
            
            // 逐字字符串
            Console.WriteLine("\n逐字字符串（@前缀）：");
            string path = @"C:\Program Files\Microsoft Visual Studio";
            Console.WriteLine("文件路径：{0}", path);
            string multiLine = @"这是第一行
这是第二行
这是第三行";
            Console.WriteLine(multiLine);
            
            Console.WriteLine("\n===== object类型 =====");
            // object是所有类型的基类
            object obj1 = 100;              // 装箱：值类型转引用类型
            object obj2 = "这是字符串";
            object obj3 = 3.14;
            
            Console.WriteLine("obj1类型：{0}，值：{1}", obj1.GetType(), obj1);
            Console.WriteLine("obj2类型：{0}，值：{1}", obj2.GetType(), obj2);
            
            int num = (int)obj1;            // 拆箱：引用类型转值类型
            Console.WriteLine("拆箱后值：{0}", num);
            
            Console.WriteLine("\n===== 类型转换 =====");
            // 隐式转换（自动类型转换，无精度丢失）
            int intValue = 123;
            double doubleValue = intValue;
            Console.WriteLine("隐式转换 int->double：{0} -> {1}", intValue, doubleValue);
            
            // 显式转换（强制转换，可能丢失精度）
            double pi = 3.14159;
            int intPi = (int)pi;
            Console.WriteLine("强制转换 double->int：{0} -> {1}", pi, intPi);
            
            // Convert类转换
            Console.WriteLine("\n使用Convert类转换：");
            string strNum = "456";
            int convertedInt = Convert.ToInt32(strNum);
            Console.WriteLine("字符串转int：{0} -> {1}", strNum, convertedInt);
            
            string strDouble = "789.12";
            double convertedDouble = Convert.ToDouble(strDouble);
            Console.WriteLine("字符串转double：{0} -> {1}", strDouble, convertedDouble);
            
            // Parse方法
            int parsedInt = int.Parse("1000");
            Console.WriteLine("int.Parse转换：{0}", parsedInt);
            
            // TryParse安全转换
            Console.WriteLine("\n使用TryParse安全转换：");
            string input = "abc123";
            int result;
            bool parseSuccess = int.TryParse(input, out result);
            if (parseSuccess)
            {
                Console.WriteLine("转换成功：{0}", result);
            }
            else
            {
                Console.WriteLine("转换失败，输入不是有效的整数");
            }
            
            input = "12345";
            parseSuccess = int.TryParse(input, out result);
            if (parseSuccess)
            {
                Console.WriteLine("转换成功：{0}", result);
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
