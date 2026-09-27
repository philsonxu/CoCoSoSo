using System;
using System.Collections.Generic;

namespace CSharp20Tutorial.Generics
{
    /// <summary>
    /// 泛型基础示例
    /// 演示泛型方法、泛型类、泛型集合List<T>、Dictionary<K,V>
    /// </summary>
    
    // 泛型类：通用的栈实现
    class MyStack<T>
    {
        private T[] _items;
        private int _count;
        private const int DefaultCapacity = 10;
        
        public MyStack()
        {
            _items = new T[DefaultCapacity];
            _count = 0;
        }
        
        public MyStack(int capacity)
        {
            _items = new T[capacity];
            _count = 0;
        }
        
        // 入栈
        public void Push(T item)
        {
            if (_count >= _items.Length)
            {
                // 扩容
                T[] newItems = new T[_items.Length * 2];
                Array.Copy(_items, newItems, _count);
                _items = newItems;
            }
            _items[_count++] = item;
        }
        
        // 出栈
        public T Pop()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("栈为空");
            }
            T item = _items[--_count];
            _items[_count] = default(T);  // 释放引用
            return item;
        }
        
        // 查看栈顶
        public T Peek()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("栈为空");
            }
            return _items[_count - 1];
        }
        
        public int Count
        {
            get { return _count; }
        }
        
        public bool IsEmpty
        {
            get { return _count == 0; }
        }
    }
    
    // 泛型方法示例类
    static class GenericUtils
    {
        // 泛型方法：交换两个变量
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        
        // 泛型方法：输出数组所有元素
        public static void PrintArray<T>(T[] array)
        {
            foreach (T item in array)
            {
                Console.Write("{0} ", item);
            }
            Console.WriteLine();
        }
        
        // 泛型方法：查找数组中最大值（需要实现IComparable<T>）
        public static T FindMax<T>(T[] array) where T : IComparable<T>
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("数组不能为空");
            }
            
            T max = array[0];
            foreach (T item in array)
            {
                if (item.CompareTo(max) > 0)
                {
                    max = item;
                }
            }
            return max;
        }
    }
    
    // 测试用的学生类，实现IComparable
    class Student : IComparable<Student>
    {
        public string Name { get; set; }
        public int Score { get; set; }
        
        public int CompareTo(Student other)
        {
            // 按成绩比较
            return this.Score.CompareTo(other.Score);
        }
        
        public override string ToString()
        {
            return string.Format("{0}:{1}分", Name, Score);
        }
    }
    
    class GenericsBasics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== 泛型集合：List<T> =====");
            // List<T>是类型安全的动态数组，不需要装箱拆箱
            List<int> intList = new List<int>();
            intList.Add(10);
            intList.Add(20);
            intList.Add(30);
            intList.Add(40);
            intList.Add(50);
            
            Console.WriteLine("intList元素个数：{0}", intList.Count);
            Console.Write("intList元素：");
            foreach (int num in intList)
            {
                Console.Write("{0} ", num);
            }
            Console.WriteLine();
            
            // List<string>存储字符串
            List<string> strList = new List<string>();
            strList.Add("苹果");
            strList.Add("香蕉");
            strList.Add("橙子");
            strList.AddRange(new string[] { "葡萄", "西瓜" });  // 批量添加
            
            Console.Write("strList元素：");
            foreach (string s in strList)
            {
                Console.Write("{0} ", s);
            }
            Console.WriteLine();
            
            Console.WriteLine("\n===== 泛型集合：Dictionary<K,V> =====");
            // 泛型哈希表，类型安全的键值对
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("001", "张三");
            dict.Add("002", "李四");
            dict.Add("003", "王五");
            dict["004"] = "赵六";
            
            Console.WriteLine("Dictionary元素个数：{0}", dict.Count);
            Console.WriteLine("学号002：{0}", dict["002"]);
            
            Console.WriteLine("所有学生：");
            foreach (KeyValuePair<string, string> kv in dict)
            {
                Console.WriteLine("  {0} : {1}", kv.Key, kv.Value);
            }
            
            // 键查找
            if (dict.ContainsKey("003"))
            {
                Console.WriteLine("存在学号003：{0}", dict["003"]);
            }
            
            Console.WriteLine("\n===== 自定义泛型类：MyStack<T> =====");
            // 整数栈
            MyStack<int> intStack = new MyStack<int>();
            intStack.Push(1);
            intStack.Push(2);
            intStack.Push(3);
            intStack.Push(4);
            intStack.Push(5);
            
            Console.WriteLine("intStack元素个数：{0}", intStack.Count);
            Console.Write("出栈顺序：");
            while (!intStack.IsEmpty)
            {
                Console.Write("{0} ", intStack.Pop());
            }
            Console.WriteLine();
            
            // 字符串栈
            MyStack<string> strStack = new MyStack<string>();
            strStack.Push("第一个");
            strStack.Push("第二个");
            strStack.Push("第三个");
            
            Console.WriteLine("strStack栈顶：{0}", strStack.Peek());
            Console.Write("字符串出栈：");
            while (!strStack.IsEmpty)
            {
                Console.Write("{0} ", strStack.Pop());
            }
            Console.WriteLine();
            
            Console.WriteLine("\n===== 泛型方法 =====");
            // 泛型交换方法
            int a = 10, b = 20;
            Console.WriteLine("交换前：a={0}, b={1}", a, b);
            GenericUtils.Swap(ref a, ref b);
            Console.WriteLine("交换后：a={0}, b={1}", a, b);
            
            string s1 = "Hello", s2 = "World";
            Console.WriteLine("交换前：s1={0}, s2={1}", s1, s2);
            GenericUtils.Swap(ref s1, ref s2);
            Console.WriteLine("交换后：s1={0}, s2={1}", s1, s2);
            
            // 泛型求最大值
            int[] numbers = { 23, 56, 12, 89, 45, 78 };
            Console.Write("数组：");
            GenericUtils.PrintArray(numbers);
            Console.WriteLine("最大值：{0}", GenericUtils.FindMax(numbers));
            
            Student[] students = {
                new Student{Name="张三", Score=85},
                new Student{Name="李四", Score=92},
                new Student{Name="王五", Score=78},
                new Student{Name="赵六", Score=95}
            };
            Student topStudent = GenericUtils.FindMax(students);
            Console.WriteLine("成绩最高的学生：{0}", topStudent);
            
            Console.WriteLine("\n===== 泛型的优势 =====");
            Console.WriteLine("1. 类型安全：编译时检查类型，避免运行时类型转换错误");
            Console.WriteLine("2. 性能提升：避免值类型的装箱拆箱操作");
            Console.WriteLine("3. 代码复用：一份代码支持多种数据类型");
            Console.WriteLine("4. 清晰易读：不需要频繁的类型转换");
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
