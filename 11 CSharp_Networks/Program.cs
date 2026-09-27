using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace CSharpNetworkProgramming
{
    class Program
    {
        private static SortedList<string, ISample> samples;

        static void Main(string[] args)
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("     C# 网络编程示例库 (基于 .NET Framework 2.0 / C# 2.0)");
            Console.WriteLine("============================================================");
            Console.WriteLine();

            // 纯 C# 2.0 自动生成测试数据，不依赖任何外部脚本（Python/Bat 等）
            TestDataGenerator.EnsureTestData();
            Console.WriteLine();

            LoadSamples();

            if (args != null && args.Length > 0)
            {
                string key = args[0];
                if (samples.ContainsKey(key))
                {
                    RunSample(key);
                    return;
                }
            }

            while (true)
            {
                PrintMenu();
                Console.Write("请输入要运行的示例编号 (输入 q 退出, 输入 0 运行全部): ");
                string input = Console.ReadLine();
                if (input == null) continue;
                input = input.Trim().ToLower();
                if (input == "q" || input == "quit" || input == "exit") break;
                if (input == "0")
                {
                    RunAll();
                    continue;
                }
                if (samples.ContainsKey(input))
                {
                    RunSample(input);
                }
                else
                {
                    Console.WriteLine("未找到编号为 [{0}] 的示例，请重新输入。", input);
                }
            }
        }

        static void LoadSamples()
        {
            samples = new SortedList<string, ISample>();
            Assembly asm = Assembly.GetExecutingAssembly();
            Type[] types = asm.GetTypes();
            foreach (Type t in types)
            {
                if (t.IsClass && !t.IsAbstract && typeof(ISample).IsAssignableFrom(t))
                {
                    try
                    {
                        ISample s = (ISample)Activator.CreateInstance(t);
                        samples[s.Id] = s;
                    }
                    catch { }
                }
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("---- 示例菜单 ----");
            foreach (KeyValuePair<string, ISample> kv in samples)
            {
                Console.WriteLine("  {0,-8}{1}", kv.Key, kv.Value.Title);
            }
            Console.WriteLine("  {0,-8}退出", "q");
            Console.WriteLine("-------------------");
        }

        static void RunSample(string id)
        {
            ISample s = samples[id];
            Console.WriteLine();
            Console.WriteLine(">>> 运行示例 [{0}] {1}", id, s.Title);
            Console.WriteLine(">>> 知识点：{0}", s.Description);
            Console.WriteLine("------------------------------------------------------------");
            try
            {
                s.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("!!! 运行异常：{0}", ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("<<< 示例 [{0}] 结束，按回车键继续...", id);
            Console.ReadLine();
        }

        static void RunAll()
        {
            foreach (KeyValuePair<string, ISample> kv in samples)
            {
                RunSample(kv.Key);
            }
        }
    }

    public interface ISample
    {
        string Id { get; }
        string Title { get; }
        string Description { get; }
        void Run();
    }

    public static class TestData
    {
        public static string GetPath(string fileName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string p = Path.Combine(baseDir, "..", "..", "TestData", fileName);
            p = Path.GetFullPath(p);
            if (!File.Exists(p))
            {
                p = Path.Combine(baseDir, "TestData", fileName);
            }
            return p;
        }
    }
}
