using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20DataStructures.DataStructures
{
    /// <summary>
    /// 常用查找算法集合
    /// </summary>
    public class SearchAlgorithms
    {
        /// <summary>
        /// 顺序查找：遍历整个数组
        /// 时间复杂度 O(n)，无需排序
        /// </summary>
        public static int LinearSearch(int[] arr, int target)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == target)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 二分查找：针对有序数组
        /// 时间复杂度 O(logn)，必须先排序
        /// </summary>
        public static int BinarySearch(int[] sortedArr, int target)
        {
            int low = 0;
            int high = sortedArr.Length - 1;
            while (low <= high)
            {
                int mid = low + (high - low) / 2; // 防止溢出
                if (sortedArr[mid] == target)
                    return mid;
                else if (sortedArr[mid] < target)
                    low = mid + 1;
                else
                    high = mid - 1;
            }
            return -1;
        }

        /// <summary>
        /// 二分查找递归版本
        /// </summary>
        public static int BinarySearchRecursive(int[] sortedArr, int target)
        {
            return BinarySearchRecursive(sortedArr, target, 0, sortedArr.Length - 1);
        }

        private static int BinarySearchRecursive(int[] arr, int target, int low, int high)
        {
            if (low > high)
                return -1;
            int mid = low + (high - low) / 2;
            if (arr[mid] == target)
                return mid;
            else if (arr[mid] < target)
                return BinarySearchRecursive(arr, target, mid + 1, high);
            else
                return BinarySearchRecursive(arr, target, low, mid - 1);
        }
    }
}
