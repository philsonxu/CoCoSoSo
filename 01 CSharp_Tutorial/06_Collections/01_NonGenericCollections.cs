using System;
using System.Collections;

namespace CSharp20Tutorial.Collections
{
    /// <summary>
    /// 非泛型集合示例
    /// 演示ArrayList、Hashtable、Queue、Stack、SortedList等常用集合
    /// </summary>
    class NonGenericCollections
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== ArrayList：动态数组 =====");
            // ArrayList可以存储任意类型的对象，大小自动调整
            ArrayList list = new ArrayList();
            
            // 添加元素
            list.Add(100);
            list.Add("Hello");
            list.Add(3.14);
            list.Add(true);
            list.Add("World");
            
            Console.WriteLine("ArrayList元素个数：{0}", list.Count);
            Console.WriteLine("ArrayList容量：{0}", list.Capacity);
            
            // 遍历ArrayList
            Console.WriteLine("\n所有元素：");
            foreach (object item in list)
            {
                Console.WriteLine("  {0} (类型：{1})", item, item.GetType());
            }
            
            // 插入元素
            list.Insert(2, "插入的元素");
            Console.WriteLine("\n在索引2插入后：");
            foreach (object item in list)
            {
                Console.Write("{0} | ", item);
            }
            Console.WriteLine();
            
            // 删除元素
            list.Remove(3.14);  // 删除指定元素
            list.RemoveAt(0);   // 删除指定索引的元素
            Console.WriteLine("\n删除后元素个数：{0}", list.Count);
            
            // 查找元素
            Console.WriteLine("包含'Hello'：{0}", list.Contains("Hello"));
            Console.WriteLine("'World'的索引：{0}", list.IndexOf("World"));
            
            // 排序（注意：不同类型元素一起排序会出错！）
            ArrayList numList = new ArrayList();
            numList.Add(45);
            numList.Add(12);
            numList.Add(78);
            numList.Add(23);
            numList.Add(56);
            Console.WriteLine("\n排序前：");
            foreach (int num in numList)
            {
                Console.Write("{0} ", num);
            }
            numList.Sort();
            Console.WriteLine("\n排序后：");
            foreach (int num in numList)
            {
                Console.Write("{0} ", num);
            }
            Console.WriteLine();
            
            Console.WriteLine("\n===== Hashtable：哈希表（键值对）=====");
            Hashtable ht = new Hashtable();
            
            // 添加键值对
            ht.Add("001", "张三");
            ht.Add("002", "李四");
            ht.Add("003", "王五");
            ht.Add("004", "赵六");
            ht["005"] = "孙七";  // 另一种添加方式
            
            Console.WriteLine("Hashtable元素个数：{0}", ht.Count);
            
            // 通过键访问值
            Console.WriteLine("学号003的学生：{0}", ht["003"]);
            
            // 遍历Hashtable
            Console.WriteLine("\n所有学生信息：");
            foreach (DictionaryEntry entry in ht)
            {
                Console.WriteLine("  {0} : {1}", entry.Key, entry.Value);
            }
            
            // 判断键是否存在
            Console.WriteLine("\n包含键'001'：{0}", ht.ContainsKey("001"));
            Console.WriteLine("包含值'李四'：{0}", ht.ContainsValue("李四"));
            
            // 删除元素
            ht.Remove("004");
            Console.WriteLine("删除后元素个数：{0}", ht.Count);
            
            Console.WriteLine("\n===== Queue：队列（先进先出）=====");
            Queue queue = new Queue();
            queue.Enqueue("第一个顾客");
            queue.Enqueue("第二个顾客");
            queue.Enqueue("第三个顾客");
            queue.Enqueue("第四个顾客");
            
            Console.WriteLine("队列中人数：{0}", queue.Count);
            Console.WriteLine("队首元素：{0}", queue.Peek());  // 查看队首但不移除
            
            Console.WriteLine("\n开始叫号：");
            while (queue.Count > 0)
            {
                string customer = (string)queue.Dequeue();  // 出队
                Console.WriteLine("  {0} 办理业务，剩余等待人数：{1}", customer, queue.Count);
            }
            
            Console.WriteLine("\n===== Stack：栈（先进后出）=====");
            Stack stack = new Stack();
            stack.Push("底层盘子");
            stack.Push("中层盘子");
            stack.Push("上层盘子");
            stack.Push("最顶层盘子");
            
            Console.WriteLine("栈中盘子数：{0}", stack.Count);
            Console.WriteLine("栈顶元素：{0}", stack.Peek());
            
            Console.WriteLine("\n开始取盘子：");
            while (stack.Count > 0)
            {
                string plate = (string)stack.Pop();  // 弹出栈顶
                Console.WriteLine("  取出{0}，剩余盘子数：{1}", plate, stack.Count);
            }
            
            Console.WriteLine("\n===== SortedList：有序键值对 =====");
            SortedList sl = new SortedList();
            sl.Add("banana", "香蕉");
            sl.Add("apple", "苹果");
            sl.Add("orange", "橙子");
            sl.Add("grape", "葡萄");
            
            Console.WriteLine("SortedList会按键自动排序：");
            foreach (DictionaryEntry entry in sl)
            {
                Console.WriteLine("  {0,-10} : {1}", entry.Key, entry.Value);
            }
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
