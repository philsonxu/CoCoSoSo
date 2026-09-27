using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 哈希表演示
    /// </summary>
    public class HashtableDemo
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第6章：哈希表（Hashtable/Dictionary）");
            
            console.WriteSection("6.1 基本概念");
            console.WriteLine("哈希表是根据键（Key）直接访问在内存存储位置的数据结构。通过哈希函数将键映射到数组的某个位置，实现O(1)时间复杂度的查找。");
            console.WriteLine("哈希冲突解决方法：链地址法（本实现）、开放地址法、再哈希法等。");
            console.WriteTip("核心操作：Add、Get、Remove，平均时间复杂度均为O(1)");
            console.WriteNote("负载因子=元素个数/桶数量，本实现默认0.75，超过自动扩容。");

            console.WriteSection("6.2 核心原理");
            console.WriteCode(
@"// 计算哈希索引
private int GetIndex(K key)
{
    int hash = key.GetHashCode();
    return Math.Abs(hash % _buckets.Length);
}

// 链地址法：每个桶是一个链表，冲突时添加到链表头
public void Add(K key, V value)
{
    int index = GetIndex(key);
    HashNode<K,V> newNode = new HashNode<K,V>(key, value);
    newNode.Next = _buckets[index];
    _buckets[index] = newNode;
}");

            console.WriteSection("6.3 运行演示：学生成绩管理");
            MyHashtable<string, int> scores = new MyHashtable<string, int>();
            scores.Add("张三", 85);
            scores.Add("李四", 92);
            scores.Add("王五", 78);
            scores.Add("赵六", 95);
            scores.Add("孙七", 88);

            console.WriteResult("已添加5名学生成绩，总人数：" + scores.Count);
            
            console.WriteLine("查询学生成绩：");
            int liScore = scores.Get("李四");
            int zhaoScore;
            bool found = scores.TryGet("赵六", out zhaoScore);
            int zhouScore;
            bool notFound = scores.TryGet("周八", out zhouScore);
            console.WriteResult(
                "李四成绩：" + liScore + "\n" +
                "赵六成绩：" + zhaoScore + "（查找结果：" + found + "）\n" +
                "周八成绩：" + (notFound ? zhouScore.ToString() : "未找到") + "（查找结果：" + notFound + "）"
            );

            console.WriteLine("删除王五的成绩：");
            scores.Remove("王五");
            console.WriteResult("删除后总人数：" + scores.Count + "，是否包含王五：" + scores.ContainsKey("王五"));

            console.WriteSection("6.4 哈希冲突说明");
            console.WriteLine("不同的键可能哈希到同一个桶（索引相同），这就是哈希冲突。");
            console.WriteLine("链地址法将冲突元素组成链表挂在同一个桶下，查找时遍历链表比较键。");
            console.WriteTip("好的哈希函数应该让键尽可能均匀分布，减少冲突。");

            console.WriteSection("6.5 适用场景");
            console.WriteLine("✅ 需要快速查找、插入、删除的键值对场景");
            console.WriteLine("✅ 缓存、字典、映射、计数");
            console.WriteLine("✅ 去重、频率统计");
            console.WriteLine("❌ 不支持有序遍历；不适合范围查询");

            console.WriteHr();
            console.Render();
        }
    }
}
