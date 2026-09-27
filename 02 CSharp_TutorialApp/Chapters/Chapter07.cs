using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace CSharp20Tutorial.Chapters
{
    /// <summary>
    /// 第7章：泛型（C# 2.0核心特性）
    /// </summary>
    public class Chapter07
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第7章 泛型（C# 2.0 核心新特性）");
            
            console.WriteSection("7.1 为什么需要泛型：非泛型ArrayList的问题");
            console.WriteParagraph("在C# 1.1中，集合类ArrayList存储的是object类型，任何类型都可以放进去，但是有两个严重问题：类型不安全（可以混入任意类型），以及值类型存取需要装箱拆箱，性能差。");
            
            console.WriteCode(@"// C# 1.1 非泛型ArrayList
ArrayList list = new ArrayList();
list.Add(10);      // int → object 装箱
list.Add(""hello""); // string → object
list.Add(DateTime.Now);
// 取出时必须强制转换，类型错误运行时才会报错
int num = (int)list[0]; // 没问题
// string s = (string)list[0]; // 运行时报错！编译时发现不了");

            StringBuilder sb = new StringBuilder();
            ArrayList list = new ArrayList();
            list.Add(10);
            list.Add(20);
            list.Add(30);
            sb.AppendLine("ArrayList可以存入任意类型，但取出需要强制转换：");
            int sum = 0;
            foreach (object item in list)
            {
                // 如果混入非int类型，这里会抛异常
                if (item is int)
                {
                    int val = (int)item;
                    sum += val;
                    sb.AppendLine(string.Format("  元素：{0}，累加和：{1}", val, sum));
                }
            }
            sb.AppendLine("\n问题：1. 没有编译时类型检查，容易出错；2. 值类型需要装箱拆箱，性能低。");
            
            console.WriteOutput(sb.ToString());
            console.WriteError("ArrayList演示：如果不小心往里面加了一个字符串，遍历转int的时候就会崩溃，编译器完全检查不出来！");
            console.WriteDivider();

            console.WriteSection("7.2 泛型集合List<T>");
            console.WriteParagraph("泛型使用<T>作为类型参数，创建集合时指定具体类型，编译器保证类型安全，不需要强制转换，值类型也不需要装箱拆箱。List<T>是最常用的泛型动态数组。");
            
            console.WriteCode(@"// 创建只能存int的List集合
List<int> intList = new List<int>();
intList.Add(10);
intList.Add(20);
intList.Add(30);
// intList.Add(""hello""); // 编译直接报错！类型安全
int first = intList[0]; // 不需要强转，直接就是int
intList.Sort(); // 内置排序");

            StringBuilder sb2 = new StringBuilder();
            List<string> names = new List<string>();
            names.Add("张三");
            names.Add("李四");
            names.Add("王五");
            names.Add("赵六");
            sb2.AppendLine("List<string> 字符串集合内容：");
            for (int i = 0; i < names.Count; i++)
            {
                sb2.AppendLine(string.Format("  [{0}] {1}", i, names[i]));
            }
            sb2.AppendLine(string.Format("集合元素个数：{0}", names.Count));
            names.Remove("李四");
            sb2.AppendLine(string.Format("移除\"李四\"后，个数：{0}", names.Count));
            sb2.AppendLine("\n泛型集合的优点：");
            sb2.AppendLine("✅ 编译时类型检查，不会混入错误类型");
            sb2.AppendLine("✅ 取出不需要强制转换，代码更简洁");
            sb2.AppendLine("✅ 值类型不需要装箱拆箱，性能大幅提升");
            
            console.WriteOutput(sb2.ToString());
            console.WriteDivider();

            console.WriteSection("7.3 泛型字典Dictionary<TKey, TValue>");
            console.WriteParagraph("Dictionary<TKey, TValue>是键值对集合，类似哈希表，通过键快速查找值，键必须唯一。是日常开发中最常用的泛型集合之一。");
            
            console.WriteCode(@"// 键是string（姓名），值是int（年龄）
Dictionary<string, int> ageDict = new Dictionary<string, int>();
ageDict.Add(""张三"", 25);
ageDict.Add(""李四"", 30);
ageDict[""王五""] = 28; // 另一种添加/赋值方式
int age = ageDict[""张三""]; // 通过键快速取值
bool has = ageDict.ContainsKey(""赵六""); // 判断键是否存在");

            StringBuilder sb3 = new StringBuilder();
            Dictionary<string, int> scoreDict = new Dictionary<string, int>();
            scoreDict.Add("张三", 85);
            scoreDict.Add("李四", 92);
            scoreDict.Add("王五", 78);
            scoreDict["赵六"] = 95;
            
            sb3.AppendLine("学生成绩字典（姓名→分数）：");
            console.WriteTableStart();
            console.WriteTableHeader("姓名", "分数");
            foreach (KeyValuePair<string, int> kv in scoreDict)
            {
                console.WriteTableRow(kv.Key, kv.Value.ToString());
            }
            console.WriteTableEnd();
            
            sb3.AppendLine(string.Format("张三的分数：{0}", scoreDict["张三"]));
            sb3.AppendLine(string.Format("是否存在\"孙七\"：{0}", scoreDict.ContainsKey("孙七") ? "存在" : "不存在"));
            
            console.WriteOutput(sb3.ToString());
            console.WriteTip("遍历Dictionary用foreach(KeyValuePair<K,V> kv in dict)，不要老版本的DictionaryEntry了。");
            console.WriteDivider();

            console.WriteSection("7.4 泛型方法");
            console.WriteParagraph("除了泛型类/泛型集合，还可以定义泛型方法，方法可以接受任意类型的参数，实现通用逻辑。");
            
            console.WriteCode(@"// 泛型方法：交换两个任意类型的变量
static void Swap<T>(ref T a, ref T b)
{
    T temp = a;
    a = b;
    b = temp;
}
// 调用时编译器自动推断类型
int x = 1, y = 2;
Swap(ref x, ref y); // T自动推断为int
string s1 = ""hello"", s2 = ""world"";
Swap(ref s1, ref s2); // T自动推断为string");

            StringBuilder sb4 = new StringBuilder();
            int ix = 1, iy = 2;
            sb4.AppendLine(string.Format("交换前：x={0}, y={1}", ix, iy));
            Swap<int>(ref ix, ref iy);
            sb4.AppendLine(string.Format("交换后：x={0}, y={1}", ix, iy));
            
            string sa = "hello", sbv = "world";
            sb4.AppendLine(string.Format("\n交换前：s1=\"{0}\", s2=\"{1}\"", sa, sbv));
            Swap<string>(ref sa, ref sbv);
            sb4.AppendLine(string.Format("交换后：s1=\"{0}\", s2=\"{1}\"", sa, sbv));
            
            console.WriteOutput(sb4.ToString());
            console.WriteSuccess("第7章完成！泛型是C# 2.0最重要的特性，后续所有.NET开发都离不开泛型集合。");
        }

        // 泛型方法示例
        private static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
