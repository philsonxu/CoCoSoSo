using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;
using System.Diagnostics;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 查找算法演示
    /// </summary>
    public class SearchDemo
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第8章：查找算法");
            
            console.WriteSection("8.1 查找算法概述");
            console.WriteLine("查找是在数据集合中寻找特定元素的过程，是计算机科学中最常用的操作之一。");
            console.WriteLine("本次演示两种基础查找算法：顺序查找和二分查找。");

            int[] arr = new int[] { 2, 5, 8, 12, 16, 23, 38, 56, 72, 91 };
            console.WriteResult("有序测试数组：" + SortAlgorithms.ArrayToString(arr));

            console.WriteSection("8.2 顺序查找（线性查找）");
            console.WriteLine("顺序查找就是从头到尾逐个遍历数组，比较每个元素是否等于目标值。");
            console.WriteCode(
@"public static int LinearSearch(int[] arr, int target)
{
    for (int i = 0; i < arr.Length; i++)
    {
        if (arr[i] == target) return i;
    }
    return -1;
}");
            int target1 = 23;
            int target2 = 99;
            int idx1 = SearchAlgorithms.LinearSearch(arr, target1);
            int idx2 = SearchAlgorithms.LinearSearch(arr, target2);
            console.WriteResult(
                "查找 " + target1 + "：索引 " + idx1 + (idx1 >= 0 ? "（找到）" : "（未找到）") + "\n" +
                "查找 " + target2 + "：索引 " + idx2 + (idx2 >= 0 ? "（找到）" : "（未找到）")
            );
            console.WriteTip("顺序查找时间复杂度O(n)，优点是不要求数组有序，实现简单；缺点是大数据量下慢。");

            console.WriteSection("8.3 二分查找（折半查找）");
            console.WriteLine("二分查找针对**有序数组**，每次取中间元素比较，将查找范围缩小一半，效率极高。");
            console.WriteCode(
@"public static int BinarySearch(int[] sortedArr, int target)
{
    int low = 0, high = sortedArr.Length - 1;
    while (low <= high)
    {
        int mid = low + (high - low) / 2; // 防止整数溢出
        if (sortedArr[mid] == target) return mid;
        else if (sortedArr[mid] < target) low = mid + 1;
        else high = mid - 1;
    }
    return -1;
}");
            int idx3 = SearchAlgorithms.BinarySearch(arr, target1);
            int idx4 = SearchAlgorithms.BinarySearch(arr, target2);
            console.WriteResult(
                "查找 " + target1 + "：索引 " + idx3 + (idx3 >= 0 ? "（找到）" : "（未找到）") + "\n" +
                "查找 " + target2 + "：索引 " + idx4 + (idx4 >= 0 ? "（未找到）" : "（未找到）")
            );
            console.WriteSuccess("二分查找时间复杂度O(logn)，例如100万元素最多只需要20次比较！");
            console.WriteNote("二分查找的前提条件：数组必须是有序的！");

            console.WriteSection("8.4 性能对比测试");
            int size = 1000000; // 100万元素
            int[] bigArr = new int[size];
            for (int i = 0; i < size; i++) bigArr[i] = i;
            int target = size - 1; // 查找最后一个元素（最坏情况）
            Stopwatch sw = Stopwatch.StartNew();

            SearchAlgorithms.LinearSearch(bigArr, target);
            sw.Stop();
            long linearTime = sw.ElapsedMilliseconds;

            sw.Reset(); sw.Start();
            SearchAlgorithms.BinarySearch(bigArr, target);
            sw.Stop();
            long binaryTime = sw.ElapsedTicks; // 微秒级

            console.WriteResult(
                "100万元素查找最后一个元素：\n" +
                "顺序查找耗时：" + linearTime + " ms\n" +
                "二分查找耗时：" + binaryTime + " ticks（约" + (binaryTime / 10000.0).ToString("F3") + " ms）"
            );
            console.WriteSuccess("可以看到有序数组上二分查找的性能优势是数量级的提升！");

            console.WriteSection("8.5 适用场景总结");
            console.WriteLine("| 算法 | 时间复杂度 | 要求 | 适用场景 |");
            console.WriteLine("|------|------------|------|----------|");
            console.WriteLine("| 顺序查找 | O(n) | 无要求 | 小数据量、无序数据 |");
            console.WriteLine("| 二分查找 | O(logn) | 数组有序 | 大数据量静态有序数据 |");

            console.WriteHr();
            console.Render();
        }
    }
}
