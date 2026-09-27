using System;

namespace CSharpDataStructures.Heap
{
    /// <summary>
    /// 最小堆实现（小顶堆）
    /// 性质：父节点值 <= 子节点值，堆顶为最小值
    /// 优先队列的底层实现
    /// </summary>
    public class MinHeap<T> where T : IComparable<T>
    {
        private T[] _items;
        private int _count;
        private const int DefaultCapacity = 10;

        public MinHeap()
        {
            _items = new T[DefaultCapacity];
            _count = 0;
        }

        public MinHeap(int capacity)
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
        /// 获取堆顶（最小值）
        /// </summary>
        public T Min
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

            _items[_count] = value;
            _count++;
            SiftUp(_count - 1);
        }

        /// <summary>
        /// 弹出堆顶（删除并返回最小值）
        /// </summary>
        public T ExtractMin()
        {
            if (IsEmpty)
                throw new InvalidOperationException("堆为空");

            T min = _items[0];
            _count--;

            if (_count > 0)
            {
                _items[0] = _items[_count];
                SiftDown(0);
            }

            _items[_count] = default(T);
            return min;
        }

        /// <summary>
        /// 上浮操作
        /// </summary>
        private void SiftUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_items[index].CompareTo(_items[parent]) < 0)
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
        /// 下沉操作
        /// </summary>
        private void SiftDown(int index)
        {
            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int smallest = index;

                if (left < _count && _items[left].CompareTo(_items[smallest]) < 0)
                    smallest = left;
                if (right < _count && _items[right].CompareTo(_items[smallest]) < 0)
                    smallest = right;

                if (smallest != index)
                {
                    Swap(index, smallest);
                    index = smallest;
                }
                else
                {
                    break;
                }
            }
        }

        private void Swap(int i, int j)
        {
            T temp = _items[i];
            _items[i] = _items[j];
            _items[j] = temp;
        }

        private void EnsureCapacity()
        {
            T[] newItems = new T[_items.Length * 2];
            Array.Copy(_items, 0, newItems, 0, _count);
            _items = newItems;
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            _count = 0;
        }
    }

    /// <summary>
    /// 优先队列（基于最小堆实现）
    /// 值越小优先级越高
    /// </summary>
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private MinHeap<T> _heap;

        public PriorityQueue()
        {
            _heap = new MinHeap<T>();
        }

        public int Count
        {
            get { return _heap.Count; }
        }

        public bool IsEmpty
        {
            get { return _heap.IsEmpty; }
        }

        /// <summary>
        /// 入队
        /// </summary>
        public void Enqueue(T item)
        {
            _heap.Insert(item);
        }

        /// <summary>
        /// 出队（取出优先级最高的元素）
        /// </summary>
        public T Dequeue()
        {
            return _heap.ExtractMin();
        }

        /// <summary>
        /// 查看队首元素
        /// </summary>
        public T Peek()
        {
            return _heap.Min;
        }

        public void Clear()
        {
            _heap.Clear();
        }
    }
}