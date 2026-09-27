using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 队列演示
    /// </summary>
    public class QueueDemo
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第4章：队列（Queue）");
            
            console.WriteSection("4.1 基本概念");
            console.WriteLine("队列是一种先进先出（FIFO, First In First Out）的线性表，只允许在队尾插入元素，在队头删除元素。");
            console.WriteLine("队列在生活中随处可见：排队买票、打印任务队列、消息队列等。");
            console.WriteTip("核心操作：Enqueue（入队）、Dequeue（出队）、Peek（查看队头），时间复杂度均为O(1)");
            console.WriteNote("本实现采用循环数组，避免普通数组实现中队头移动导致的空间浪费。");

            console.WriteSection("4.2 核心操作演示");
            MyQueue<string> queue = new MyQueue<string>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("依次入队：张三、李四、王五、赵六：");
            queue.Enqueue("张三"); queue.Enqueue("李四"); queue.Enqueue("王五"); queue.Enqueue("赵六");
            sb.AppendLine("队列元素（队头→队尾）：" + ArrayToString(queue.ToArray()));
            sb.AppendLine("队头元素：" + queue.Peek() + "，元素个数：" + queue.Count);
            console.WriteResult(sb.ToString());

            console.WriteLine("执行两次出队：");
            string d1 = queue.Dequeue();
            string d2 = queue.Dequeue();
            console.WriteResult("第一次出队：" + d1 + "，第二次出队：" + d2 + "\n剩余元素：" + ArrayToString(queue.ToArray()));

            console.WriteLine("继续入队 孙七、周八（验证循环数组特性）：");
            queue.Enqueue("孙七"); queue.Enqueue("周八");
            console.WriteResult("入队后：" + ArrayToString(queue.ToArray()));

            console.WriteSection("4.3 循环数组原理");
            console.WriteLine("普通数组实现队列出队后，队头前的空间无法复用，造成浪费。");
            console.WriteLine("循环数组通过取模运算将数组首尾相连：");
            console.WriteCode(
@"// 入队：尾指针后移
_tail = (_tail + 1) % _data.Length;
// 出队：头指针后移
_head = (_head + 1) % _data.Length;");
            console.WriteTip("扩容时需要将元素重新排列为从头开始的连续数组。");

            console.WriteSection("4.4 常见应用场景");
            console.WriteLine("✅ 操作系统进程调度、作业排队");
            console.WriteLine("✅ 消息队列、任务排队系统");
            console.WriteLine("✅ 广度优先搜索（BFS）");
            console.WriteLine("✅ 打印任务缓冲队列");
            console.WriteLine("✅ 生产者消费者模式");

            console.WriteHr();
            console.Render();
        }

        private static string ArrayToString<T>(T[] arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[头] ");
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i]);
                if (i < arr.Length - 1) sb.Append(" → ");
            }
            sb.Append(" [尾]");
            return sb.ToString();
        }
    }
}
