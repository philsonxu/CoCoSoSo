using System;

namespace CSharp20Tutorial.BasicSyntax
{
    /// <summary>
    /// 第一个C#程序：Hello World示例
    /// 演示C#程序的基本结构
    /// </summary>
    class HelloWorld
    {
        // 程序入口点：Main方法
        static void Main(string[] args)
        {
            // 输出文本到控制台
            Console.WriteLine("Hello, World!");
            Console.WriteLine("欢迎学习C# 2.0程序设计！");
            
            // 等待用户输入，防止控制台窗口立即关闭
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
