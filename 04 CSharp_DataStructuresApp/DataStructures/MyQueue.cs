using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20DataStructures.DataStructures
{
    /// <summary>
    /// 队列实现：先进先出(FIFO)
    /// 循环数组实现
    /// </summary>
    public class MyQueue<T>
    {
        private T[] _data;
        private int _head; // 队头索引
        private int _tail; // 队尾索引（下一个插入位置）
        private int _count;
        private const int _defaultCapacity = 4;

        public MyQueue()
        {
            _data = new T[_defaultCapacity];
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        public MyQueue(int capacity)
        {
            _data = new T[capacity];
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        public int Count
        {
            get { return _count; }
        }

        public bool IsEmpty
        {
            get { return _count == 0; }
        }

        /// <summary>
        /// 入队
        /// </summary>
        public void Enqueue(T item)
        {
            if (_count == _data.Length)
            {
                EnsureCapacity(_count * 2);
            }
            _data[_tail] = item;
            _tail = (_tail + 1) % _data.Length;
            _count++;
        }

        /// <summary>
        /// 出队
        /// </summary>
        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("队列为空");
            T item = _data[_head];
            _data[_head] = default(T);
            _head = (_head + 1) % _data.Length;
            _count--;
            return item;
        }

        /// <summary>
        /// 查看队头元素
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("队列为空");
            return _data[_head];
        }

        public void Clear()
        {
            Array.Clear(_data, 0, _data.Length);
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        private void EnsureCapacity(int newCapacity)
        {
            if (newCapacity < _defaultCapacity)
                newCapacity = _defaultCapacity;
            T[] newData = new T[newCapacity];
            if (_count > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_data, _head, newData, 0, _count);
                }
                else
                {
                    Array.Copy(_data, _head, newData, 0, _data.Length - _head);
                    Array.Copy(_data, 0, newData, _data.Length - _head, _tail);
                }
            }
            _data = newData;
            _head = 0;
            _tail = _count;
        }

        /// <summary>
        /// 转换为数组（队头到队尾顺序）
        /// </summary>
        public T[] ToArray()
        {
            T[] result = new T[_count];
            if (_count > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_data, _head, result, 0, _count);
                }
                else
                {
                    Array.Copy(_data, _head, result, 0, _data.Length - _head);
                    Array.Copy(_data, 0, result, _data.Length - _head, _tail);
                }
            }
            return result;
        }
    }
}
