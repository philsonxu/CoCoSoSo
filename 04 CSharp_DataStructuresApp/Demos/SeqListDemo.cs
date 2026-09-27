using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 顺序表演示
    /// </summary>
    public class SeqListDemo
    {
        public static void Run(HtmlConsole console)
        {
            //console.WriteHead();
            console.WriteTitle("第1章：顺序表（SeqList）");

            console.WriteSection("1.1 基本概念");
            console.WriteLine("顺序表是用一段地址连续的存储单元依次存储线性表的数据元素。");
            console.WriteLine("C#中通常使用数组来实现顺序表，支持随机访问，查找速度快，但插入删除需要移动元素。");
            console.WriteTip("时间复杂度：随机访问O(1)，插入删除O(n)，查找O(n)");

            console.WriteSection("1.2 核心实现代码");
            console.WriteCode(
@"public class SeqList<T>
{
    private T[] _data;
    private int _count;

    // 尾部添加元素 O(1) 均摊
    public void Add(T item)
    {
        if (_count == _data.Length) EnsureCapacity(_count * 2);
        _data[_count] = item;
        _count++;
    }

    // 指定位置插入 O(n)
    public void Insert(int index, T item) { ... }

    // 删除指定位置元素 O(n)
    public T RemoveAt(int index) { ... }
}");

            console.WriteSection("1.3 运行演示");
            SeqList<int> list = new SeqList<int>();

            // 添加元素
            StringBuilder addResult = new StringBuilder();
            addResult.AppendLine("依次添加元素 10, 20, 30, 40, 50：");
            list.Add(10); list.Add(20); list.Add(30); list.Add(40); list.Add(50);
            addResult.AppendLine("元素列表：" + ArrayToString(list.ToArray()));
            addResult.AppendLine("当前元素个数：" + list.Count + "，容量：" + list.Capacity);
            console.WriteResult(addResult.ToString());

            // 插入元素
            console.WriteLine("在索引2位置插入25：");
            list.Insert(2, 25);
            console.WriteResult("插入后：" + ArrayToString(list.ToArray()));

            // 删除元素
            console.WriteLine("删除索引0位置的元素：");
            int removed = list.RemoveAt(0);
            console.WriteResult("删除元素 " + removed + " 后：" + ArrayToString(list.ToArray()));

            // 查找元素
            console.WriteLine("查找元素30的索引：");
            int index = list.IndexOf(30);
            console.WriteResult("元素30的索引是：" + index);

            // 通过索引器访问
            console.WriteLine("通过索引器 list[2] 访问：");
            console.WriteResult("list[2] = " + list[2]);

            console.WriteSection("1.4 适用场景");
            console.WriteLine("✅ 需要频繁随机访问元素");
            console.WriteLine("✅ 元素数量变化不大，尾部操作多");
            console.WriteLine("❌ 频繁在中间插入/删除元素");
            console.WriteHr();
            console.Render();
        }

        private static string ArrayToString(Array arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr.GetValue(i));
                if (i < arr.Length - 1) sb.Append(", ");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
