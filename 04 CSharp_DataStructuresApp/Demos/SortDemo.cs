using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;
using System.Diagnostics;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 排序算法演示
    /// </summary>
    public class SortDemo
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第7章：排序算法");

            console.WriteSection("7.1 排序算法概述");
            console.WriteLine("排序是将一组数据按照特定顺序（升序/降序）排列的过程，是最基础也是最重要的算法之一。");
            console.WriteLine("本次演示四种经典排序：冒泡排序、选择排序、插入排序、快速排序。");

            int[] original = new int[] { 64, 34, 25, 12, 22, 11, 90, 45, 78, 33 };
            console.WriteResult("原始待排序数组：" + SortAlgorithms.ArrayToString(original));

            console.WriteSection("7.2 冒泡排序 O(n²)");
            console.WriteLine("冒泡排序通过重复遍历数组，相邻元素两两比较交换，大元素像气泡一样\"冒\"到末尾。");
            int[] arr1 = (int[])original.Clone();
            Stopwatch sw = Stopwatch.StartNew();
            SortAlgorithms.BubbleSort(arr1);
            sw.Stop();
            console.WriteResult("冒泡排序结果：" + SortAlgorithms.ArrayToString(arr1) + "\n耗时：" + sw.ElapsedTicks + " ticks");
            console.WriteTip("优点：实现简单稳定；缺点：效率低，适合小数据量或教学演示。");

            console.WriteSection("7.3 选择排序 O(n²)");
            console.WriteLine("每次从未排序部分选出最小（大）元素，放到已排序部分末尾。");
            int[] arr2 = (int[])original.Clone();
            sw.Reset(); sw.Start();
            SortAlgorithms.SelectionSort(arr2);
            sw.Stop();
            console.WriteResult("选择排序结果：" + SortAlgorithms.ArrayToString(arr2) + "\n耗时：" + sw.ElapsedTicks + " ticks");
            console.WriteNote("选择排序是不稳定排序，交换次数比冒泡少（最多n-1次）。");

            console.WriteSection("7.4 插入排序 O(n²)");
            console.WriteLine("类似整理扑克牌：将未排序元素逐个插入到已排序部分的正确位置。");
            int[] arr3 = (int[])original.Clone();
            sw.Reset(); sw.Start();
            SortAlgorithms.InsertionSort(arr3);
            sw.Stop();
            console.WriteResult("插入排序结果：" + SortAlgorithms.ArrayToString(arr3) + "\n耗时：" + sw.ElapsedTicks + " ticks");
            console.WriteTip("插入排序对于接近有序的数组效率非常高，可以达到O(n)，是简单排序中实际性能最好的。");

            console.WriteSection("7.5 快速排序 O(nlogn)");
            console.WriteLine("分治思想：选择一个基准元素，将数组分为小于基准和大于基准两部分，递归排序两部分。");
            int[] arr4 = (int[])original.Clone();
            sw.Reset(); sw.Start();
            SortAlgorithms.QuickSort(arr4);
            sw.Stop();
            console.WriteResult("快速排序结果：" + SortAlgorithms.ArrayToString(arr4) + "\n耗时：" + sw.ElapsedTicks + " ticks");
            console.WriteSuccess("快速排序是实践中最快的通用排序算法之一，.NET框架内部Sort方法的基础就是快速排序。");

            console.WriteSection("7.6 算法对比");
            console.WriteLine("| 算法 | 平均时间 | 最坏时间 | 空间复杂度 | 稳定性 |");
            console.WriteLine("|------|----------|----------|------------|--------|");
            console.WriteLine("| 冒泡排序 | O(n²) | O(n²) | O(1) | 稳定 |");
            console.WriteLine("| 选择排序 | O(n²) | O(n²) | O(1) | 不稳定 |");
            console.WriteLine("| 插入排序 | O(n²) | O(n²) | O(1) | 稳定 |");
            console.WriteLine("| 快速排序 | O(nlogn) | O(n²) | O(logn) | 不稳定 |");

            console.WriteSection("7.7 性能测试：10000个随机数排序");
            int[] bigArr = GenerateRandomArray(10000);
            int[] testArr;

            testArr = (int[])bigArr.Clone();
            sw.Reset(); sw.Start();
            SortAlgorithms.BubbleSort(testArr);
            sw.Stop();
            long bubbleTime = sw.ElapsedMilliseconds;

            testArr = (int[])bigArr.Clone();
            sw.Reset(); sw.Start();
            SortAlgorithms.QuickSort(testArr);
            sw.Stop();
            long quickTime = sw.ElapsedMilliseconds;

            console.WriteResult(
                "冒泡排序耗时：" + bubbleTime + " ms\n" +
                "快速排序耗时：" + quickTime + " ms\n" +
                "性能差距：约 " + (bubbleTime / Math.Max(1, quickTime)) + " 倍"
            );

            console.WriteHr();
            console.Render();
        }

        private static int[] GenerateRandomArray(int length)
        {
            Random rand = new Random();
            int[] arr = new int[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = rand.Next(0, length * 10);
            }
            return arr;
        }
    }
}
