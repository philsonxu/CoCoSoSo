using System;

namespace CSharpDataStructures.Heap
{
    /// <summary>
    /// 最大堆实现（大顶堆）
    /// 性质：父节点值 >= 子节点值，堆顶为最大值
    /// 基于数组实现的完全二叉树
    /// 时间复杂度：插入删除O(log n)，获取最大值O(1)
    /// </summary>
    public class MaxHeap<T> where T : IComparable<T>
    {
        private T[] _items;
        private int _count;
        private const int DefaultCapacity = 10;

        public MaxHeap()
        {
            _items = new T[DefaultCapacity];
            _count = 0;
        }

        public MaxHeap(int capacity)
        {
            _items = new T[capacity];
            _count = 0;
        }

        /// <summary>
        /// 元素个数
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return _count == 0; }
        }

        /// <summary>
        /// 获取堆顶（最大值）
        /// </summary>
        public T Max
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException("堆为空");
                return _items[0];
            }
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        public void Insert(T value)
        {
            if (_count == _items.Length)
                EnsureCapacity();

            // 插入到末尾，然后上浮
            _items[_count] = value;
            _count++;
            SiftUp(_count - 1);
        }

        /// <summary>
        /// 弹出堆顶（删除并返回最大值）
        /// </summary>
        public T ExtractMax()
        {
            if (IsEmpty)
                throw new InvalidOperationException("堆为空");

            T max = _items[0];
            _count--;

            if (_count > 0)
            {
                _items[0] = _items[_count];
                SiftDown(0);
            }

            _items[_count] = default(T);
            return max;
        }

        /// <summary>
        /// 上浮操作（从子节点向上调整）
        /// </summary>
        private void SiftUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                // 如果当前节点大于父节点，交换
                if (_items[index].CompareTo(_items[parent]) > 0)
                {
                    Swap(index, parent);
                    index = parent;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 下沉操作（从父节点向下调整）
        /// </summary>
        private void SiftDown(int index)
        {
            while (true)
            {
                int left = 2 * index + 1;  // 左子节点
                int right = 2 * index + 2; // 右子节点
                int largest = index;

                // 找到三个节点中的最大值
                if (left < _count && _items[left].CompareTo(_items[largest]) > 0)
                    largest = left;
                if (right < _count && _items[right].CompareTo(_items[largest]) > 0)
                    largest = right;

                if (largest != index)
                {
                    Swap(index, largest);
                    index = largest;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 交换两个位置的元素
        /// </summary>
        private void Swap(int i, int j)
        {
            T temp = _items[i];
            _items[i] = _items[j];
            _items[j] = temp;
        }

        /// <summary>
        /// 扩容
        /// </summary>
        private void EnsureCapacity()
        {
            T[] newItems = new T[_items.Length * 2];
            Array.Copy(_items, 0, newItems, 0, _count);
            _items = newItems;
        }

        /// <summary>
        /// 堆排序（返回降序数组）
        /// </summary>
        public T[] HeapSort()
        {
            T[] result = new T[_count];
            int tempCount = _count;

            // 依次弹出最大值
            for (int i = 0; i < tempCount; i++)
                result[i] = ExtractMax();

            // 重新插入恢复堆
            for (int i = 0; i < tempCount; i++)
                Insert(result[i]);

            return result;
        }

        /// <summary>
        /// 清空堆
        /// </summary>
        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            _count = 0;
        }

        /// <summary>
        /// 打印堆
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < _count; i++)
                Console.Write(_items[i] + " ");
            Console.WriteLine();
        }
    }
}