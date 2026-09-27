using System;

namespace CSharpDataStructures.Sort
{
    /// <summary>
    /// 排序算法集合类
    /// 包含8种经典排序算法的C# 2.0实现
    /// </summary>
    public static class SortAlgorithms
    {
        #region 冒泡排序 O(n²) 稳定

        /// <summary>
        /// 冒泡排序
        /// 相邻元素两两比较，大的往后冒
        /// 最好O(n)，最坏O(n²)，稳定
        /// </summary>
        public static void BubbleSort<T>(T[] arr) where T : IComparable<T>
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        Swap(arr, j, j + 1);
                        swapped = true;
                    }
                }
                if (!swapped) break; // 无交换则已有序
            }
        }

        #endregion

        #region 选择排序 O(n²) 不稳定

        /// <summary>
        /// 选择排序
        /// 每次选择最小元素放到前面
        /// O(n²)，不稳定
        /// </summary>
        public static void SelectionSort<T>(T[] arr) where T : IComparable<T>
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j].CompareTo(arr[minIndex]) < 0)
                        minIndex = j;
                }
                if (minIndex != i)
                    Swap(arr, i, minIndex);
            }
        }

        #endregion

        #region 插入排序 O(n²) 稳定

        /// <summary>
        /// 插入排序
        /// 将元素插入到已排序部分的正确位置
        /// 最好O(n)，最坏O(n²)，稳定
        /// </summary>
        public static void InsertionSort<T>(T[] arr) where T : IComparable<T>
        {
            int n = arr.Length;
            for (int i = 1; i < n; i++)
            {
                T current = arr[i];
                int j = i - 1;
                while (j >= 0 && arr[j].CompareTo(current) > 0)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = current;
            }
        }

        #endregion

        #region 希尔排序 O(n^1.3) 不稳定

        /// <summary>
        /// 希尔排序（缩小增量排序）
        /// 插入排序的改进版
        /// O(n^1.3)，不稳定
        /// </summary>
        public static void ShellSort<T>(T[] arr) where T : IComparable<T>
        {
            int n = arr.Length;
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    T temp = arr[i];
                    int j;
                    for (j = i; j >= gap && arr[j - gap].CompareTo(temp) > 0; j -= gap)
                    {
                        arr[j] = arr[j - gap];
                    }
                    arr[j] = temp;
                }
            }
        }

        #endregion

        #region 快速排序 O(n log n) 不稳定

        /// <summary>
        /// 快速排序
        /// 分治思想，选基准分区
        /// 平均O(n log n)，最坏O(n²)，不稳定
        /// </summary>
        public static void QuickSort<T>(T[] arr) where T : IComparable<T>
        {
            QuickSort(arr, 0, arr.Length - 1);
        }

        private static void QuickSort<T>(T[] arr, int left, int right) where T : IComparable<T>
        {
            if (left < right)
            {
                int pivotIndex = Partition(arr, left, right);
                QuickSort(arr, left, pivotIndex - 1);
                QuickSort(arr, pivotIndex + 1, right);
            }
        }

        private static int Partition<T>(T[] arr, int left, int right) where T : IComparable<T>
        {
            T pivot = arr[right]; // 选最右为基准
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (arr[j].CompareTo(pivot) < 0)
                {
                    i++;
                    Swap(arr, i, j);
                }
            }
            Swap(arr, i + 1, right);
            return i + 1;
        }

        #endregion

        #region 归并排序 O(n log n) 稳定

        /// <summary>
        /// 归并排序
        /// 分治思想，先分后合
        /// O(n log n)，稳定，需要额外空间O(n)
        /// </summary>
        public static void MergeSort<T>(T[] arr) where T : IComparable<T>
        {
            T[] temp = new T[arr.Length];
            MergeSort(arr, 0, arr.Length - 1, temp);
        }

        private static void MergeSort<T>(T[] arr, int left, int right, T[] temp) where T : IComparable<T>
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSort(arr, left, mid, temp);
                MergeSort(arr, mid + 1, right, temp);
                Merge(arr, left, mid, right, temp);
            }
        }

        private static void Merge<T>(T[] arr, int left, int mid, int right, T[] temp) where T : IComparable<T>
        {
            int i = left;    // 左序列指针
            int j = mid + 1; // 右序列指针
            int k = 0;       // 临时数组指针

            while (i <= mid && j <= right)
            {
                if (arr[i].CompareTo(arr[j]) <= 0)
                    temp[k++] = arr[i++];
                else
                    temp[k++] = arr[j++];
            }

            while (i <= mid)
                temp[k++] = arr[i++];
            while (j <= right)
                temp[k++] = arr[j++];

            k = 0;
            while (left <= right)
                arr[left++] = temp[k++];
        }

        #endregion

        #region 堆排序 O(n log n) 不稳定

        /// <summary>
        /// 堆排序
        /// 利用最大堆实现升序排序
        /// O(n log n)，不稳定，原地排序
        /// </summary>
        public static void HeapSort<T>(T[] arr) where T : IComparable<T>
        {
            int n = arr.Length;

            // 构建最大堆
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            // 依次取出堆顶
            for (int i = n - 1; i > 0; i--)
            {
                Swap(arr, 0, i);
                Heapify(arr, i, 0);
            }
        }

        private static void Heapify<T>(T[] arr, int n, int i) where T : IComparable<T>
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && arr[left].CompareTo(arr[largest]) > 0)
                largest = left;
            if (right < n && arr[right].CompareTo(arr[largest]) > 0)
                largest = right;

            if (largest != i)
            {
                Swap(arr, i, largest);
                Heapify(arr, n, largest);
            }
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 交换数组元素
        /// </summary>
        private static void Swap<T>(T[] arr, int i, int j)
        {
            T temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        /// <summary>
        /// 打印数组
        /// </summary>
        public static void Print<T>(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();
        }

        #endregion
    }
}