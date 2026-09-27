using System;

namespace CSharpDataStructures.Queue
{
    /// <summary>
    /// 链队列节点
    /// </summary>
    internal class QueueNode<T>
    {
        public T Value;
        public QueueNode<T> Next;

        public QueueNode(T value)
        {
            Value = value;
            Next = null;
        }
    }

    /// <summary>
    /// 链队列（基于链表实现）
    /// 先进先出(FIFO)数据结构，无需预设容量
    /// </summary>
    public class LinkedQueue<T>
    {
        private QueueNode<T> _head; // 队头
        private QueueNode<T> _tail; // 队尾
        private int _count;

        public LinkedQueue()
        {
            _head = null;
            _tail = null;
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
        /// 入队（在尾部添加）
        /// </summary>
        public void Enqueue(T item)
        {
            QueueNode<T> newNode = new QueueNode<T>(item);

            if (_tail == null)
            {
                _head = newNode;
                _tail = newNode;
            }
            else
            {
                _tail.Next = newNode;
                _tail = newNode;
            }
            _count++;
        }

        /// <summary>
        /// 出队（从头部移除）
        /// </summary>
        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("队列为空");

            T value = _head.Value;
            _head = _head.Next;
            _count--;

            if (_head == null)
                _tail = null;

            return value;
        }

        /// <summary>
        /// 获取队头元素但不出队
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("队列为空");

            return _head.Value;
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }
    }
}