using System;

namespace CSharpDataStructures.Queue
{
    /// <summary>
    /// 顺序队列（基于数组的循环队列实现）
    /// 先进先出(FIFO)数据结构
    /// 使用循环数组避免假溢出
    /// </summary>
    public class ArrayQueue<T>
    {
        private T[] _items;
        private int _head; // 队头索引
        private int _tail; // 队尾索引（指向下一个可插入位置）
        private int _count;
        private const int DefaultCapacity = 10;

        public ArrayQueue()
        {
            _items = new T[DefaultCapacity];
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        public ArrayQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            _items = new T[capacity];
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        /// <summary>
        /// 队列元素个数
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// 队列是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return _count == 0; }
        }

        /// <summary>
        /// 入队
        /// </summary>
        public void Enqueue(T item)
        {
            if (_count == _items.Length)
                throw new InvalidOperationException("队列已满");

            _items[_tail] = item;
            _tail = (_tail + 1) % _items.Length; // 循环
            _count++;
        }

        /// <summary>
        /// 出队
        /// </summary>
        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("队列为空");

            T item = _items[_head];
            _items[_head] = default(T);
            _head = (_head + 1) % _items.Length; // 循环
            _count--;
            return item;
        }

        /// <summary>
        /// 获取队头元素但不出队
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("队列为空");

            return _items[_head];
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            _head = 0;
            _tail = 0;
            _count = 0;
        }
    }
}