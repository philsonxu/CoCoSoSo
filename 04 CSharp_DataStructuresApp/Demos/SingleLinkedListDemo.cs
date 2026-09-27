using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 单链表演示
    /// </summary>
    public class SingleLinkedListDemo
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第2章：单链表（SingleLinkedList）");
            
            console.WriteSection("2.1 基本概念");
            console.WriteLine("链表是一种物理存储单元上非连续、非顺序的存储结构，数据元素的逻辑顺序通过链表中的指针链接次序实现。");
            console.WriteLine("每个节点包含数据域和指针域（指向下一个节点），不需要预先分配内存大小，插入删除速度快。");
            console.WriteTip("时间复杂度：头插O(1)，尾插O(n)，插入删除O(n)（找到位置后O(1)），查找O(n)");

            console.WriteSection("2.2 节点结构");
            console.WriteCode(
@"public class LinkedListNode<T>
{
    public T Data;              // 数据域
    public LinkedListNode<T> Next; // 指针域，指向下一个节点
    public LinkedListNode(T data) { Data = data; Next = null; }
}");

            console.WriteSection("2.3 运行演示");
            SingleLinkedList<string> list = new SingleLinkedList<string>();

            // 尾部添加
            list.AddLast("苹果");
            list.AddLast("香蕉");
            list.AddLast("橙子");
            console.WriteResult("尾部添加3个水果后：" + ArrayToString(list.ToArray()) + "，长度：" + list.Count);

            // 头部添加
            list.AddFirst("西瓜");
            console.WriteResult("头部添加西瓜后：" + ArrayToString(list.ToArray()));

            // 指定位置插入
            list.Insert(2, "葡萄");
            console.WriteResult("索引2位置插入葡萄后：" + ArrayToString(list.ToArray()));

            // 删除元素
            list.Remove("香蕉");
            console.WriteResult("删除香蕉后：" + ArrayToString(list.ToArray()));

            // 查找
            bool hasOrange = list.Contains("橙子");
            bool hasPear = list.Contains("梨");
            console.WriteResult("是否包含橙子：" + hasOrange + "，是否包含梨：" + hasPear);

            // 反转链表
            console.WriteLine("反转链表：");
            list.Reverse();
            console.WriteResult("反转后：" + ArrayToString(list.ToArray()));

            console.WriteSection("2.4 顺序表 vs 链表对比");
            console.WriteLine("| 操作 | 顺序表 | 链表 |");
            console.WriteLine("|------|--------|------|");
            console.WriteLine("| 随机访问 | O(1) | O(n) |");
            console.WriteLine("| 头部插入 | O(n) | O(1) |");
            console.WriteLine("| 尾部插入 | O(1) 均摊 | O(n) |");
            console.WriteLine("| 中间插入 | O(n)（移动元素） | O(n)（找位置）+O(1)修改指针 |");
            console.WriteLine("| 内存占用 | 连续，可能有预留空间 | 分散，每个节点额外存指针 |");

            console.WriteHr();
            console.Render();
        }

        private static string ArrayToString<T>(T[] arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i]);
                if (i < arr.Length - 1) sb.Append(" → ");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
