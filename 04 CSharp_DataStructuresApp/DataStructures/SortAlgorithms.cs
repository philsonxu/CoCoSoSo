using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20DataStructures.DataStructures
{
    /// <summary>
    /// 常用排序算法集合
    /// C# 2.0 实现，全部为原地排序
    /// </summary>
    public class SortAlgorithms
    {
        #region 冒泡排序 O(n²)
        /// <summary>
        /// 冒泡排序：相邻元素比较交换，大元素逐步"冒泡"到末尾
        /// </summary>
        public static void BubbleSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        // 交换
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swapped = true;
                    }
                }
                if (!swapped) break; // 已有序，提前退出
            }
        }
        #endregion

        #region 选择排序 O(n²)
        /// <summary>
        /// 选择排序：每次从未排序部分选择最小元素放到前面
        /// </summary>
        public static void SelectionSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                        minIndex = j;
                }
                if (minIndex != i)
                {
                    int temp = arr[i];
                    arr[i] = arr[minIndex];
                    arr[minIndex] = temp;
                }
            }
        }
        #endregion

        #region 插入排序 O(n²)
        /// <summary>
        /// 插入排序：将元素插入到已排序部分的正确位置
        /// 接近有序数组效率高
        /// </summary>
        public static void InsertionSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; i++)
            {
                int current = arr[i];
                int j = i - 1;
                while (j >= 0 && arr[j] > current)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = current;
            }
        }
        #endregion

        #region 快速排序 O(nlogn)
        /// <summary>
        /// 快速排序：分治法，选基准，分区，递归
        /// </summary>
        public static void QuickSort(int[] arr)
        {
            QuickSort(arr, 0, arr.Length - 1);
        }

        private static void QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(arr, low, high);
                QuickSort(arr, low, pivotIndex - 1);
                QuickSort(arr, pivotIndex + 1, high);
            }
        }

        private static int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high]; // 选最后一个元素为基准
            int i = low - 1;
            for (int j = low; j < high; j++)
            {
                if (arr[j] <= pivot)
                {
                    i++;
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
            // 基准放到正确位置
            int temp2 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp2;
            return i + 1;
        }
        #endregion

        #region 辅助方法
        public static void Swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        public static string ArrayToString(int[] arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i]);
                if (i < arr.Length - 1)
                    sb.Append(", ");
            }
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
