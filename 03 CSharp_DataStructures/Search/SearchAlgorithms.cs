using System;

namespace CSharpDataStructures.Search
{
    /// <summary>
    /// 查找算法集合类
    /// 包含4种经典查找算法
    /// </summary>
    public static class SearchAlgorithms
    {
        /// <summary>
        /// 顺序查找（线性查找）
        /// 适用于无序数组
        /// 时间复杂度O(n)
        /// </summary>
        /// <returns>找到返回索引，未找到返回-1</returns>
        public static int SequentialSearch<T>(T[] arr, T target) where T : IComparable<T>
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].CompareTo(target) == 0)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 二分查找（折半查找）
        /// 要求数组有序
        /// 时间复杂度O(log n)
        /// </summary>
        public static int BinarySearch<T>(T[] sortedArr, T target) where T : IComparable<T>
        {
            int left = 0;
            int right = sortedArr.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2; // 避免溢出
                int cmp = sortedArr[mid].CompareTo(target);

                if (cmp == 0)
                    return mid;
                else if (cmp < 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            return -1;
        }

        /// <summary>
        /// 二分查找递归版
        /// </summary>
        public static int BinarySearchRecursive<T>(T[] sortedArr, T target) where T : IComparable<T>
        {
            return BinarySearchRecursive(sortedArr, target, 0, sortedArr.Length - 1);
        }

        private static int BinarySearchRecursive<T>(T[] arr, T target, int left, int right) where T : IComparable<T>
        {
            if (left > right) return -1;

            int mid = left + (right - left) / 2;
            int cmp = arr[mid].CompareTo(target);

            if (cmp == 0)
                return mid;
            else if (cmp < 0)
                return BinarySearchRecursive(arr, target, mid + 1, right);
            else
                return BinarySearchRecursive(arr, target, left, mid - 1);
        }

        /// <summary>
        /// 插值查找
        /// 二分查找的改进，适合均匀分布的有序数组
        /// 时间复杂度平均O(log log n)
        /// </summary>
        public static int InterpolationSearch(int[] sortedArr, int target)
        {
            int left = 0;
            int right = sortedArr.Length - 1;

            while (left <= right && target >= sortedArr[left] && target <= sortedArr[right])
            {
                if (left == right)
                    return sortedArr[left] == target ? left : -1;

                // 插值公式计算mid位置
                int pos = left + (target - sortedArr[left]) * (right - left) / (sortedArr[right] - sortedArr[left]);

                if (sortedArr[pos] == target)
                    return pos;
                else if (sortedArr[pos] < target)
                    left = pos + 1;
                else
                    right = pos - 1;
            }
            return -1;
        }
    }
}