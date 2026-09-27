using System;

namespace CSharp20Tutorial.BasicSyntax
{
    /// <summary>
    /// 变量与常量示例
    /// 演示C#中变量声明、赋值和常量定义
    /// </summary>
    class VariablesAndConstants
    {
        static void Main(string[] args)
        {
            // 变量声明与赋值
            int age = 25;                  // 整型变量
            double height = 1.75;          // 双精度浮点型
            string name = "张三";          // 字符串
            bool isStudent = true;         // 布尔型
            char gender = '男';            // 字符型
            
            // 常量定义（使用const关键字，值不可修改）
            const double PI = 3.1415926;
            const string COURSE = "C#程序设计";
            
            // 输出变量值
            Console.WriteLine("===== 个人信息 =====");
            Console.WriteLine("姓名：{0}", name);
            Console.WriteLine("年龄：{0}岁", age);
            Console.WriteLine("身高：{0}米", height);
            Console.WriteLine("性别：{0}", gender);
            Console.WriteLine("是否学生：{0}", isStudent ? "是" : "否");
            Console.WriteLine("\n课程名称：{0}", COURSE);
            Console.WriteLine("圆周率：{0}", PI);
            
            // 变量可以重新赋值
            age = 26;
            Console.WriteLine("\n一年后年龄：{0}岁", age);
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
