using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20DataStructures.DataStructures
{
    /// <summary>
    /// 单链表节点
    /// </summary>
    public class LinkedListNode<T>
    {
        public T Data;
        public LinkedListNode<T> Next;

        public LinkedListNode(T data)
        {
            Data = data;
            Next = null;
        }
    }

    /// <summary>
    /// 单链表实现
    /// </summary>
    public class SingleLinkedList<T>
    {
        private LinkedListNode<T> _head;
        private int _count;

        public SingleLinkedList()
        {
            _head = null;
            _count = 0;
        }

        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// 头部添加
        /// </summary>
        public void AddFirst(T item)
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(item);
            newNode.Next = _head;
            _head = newNode;
            _count++;
        }

        /// <summary>
        /// 尾部添加
        /// </summary>
        public void AddLast(T item)
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(item);
            if (_head == null)
            {
                _head = newNode;
            }
            else
            {
                LinkedListNode<T> current = _head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            _count++;
        }

        /// <summary>
        /// 指定位置插入
        /// </summary>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException("index");

            if (index == 0)
            {
                AddFirst(item);
                return;
            }

            LinkedListNode<T> current = _head;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }
            LinkedListNode<T> newNode = new LinkedListNode<T>(item);
            newNode.Next = current.Next;
            current.Next = newNode;
            _count++;
        }

        /// <summary>
        /// 删除第一个匹配元素
        /// </summary>
        public bool Remove(T item)
        {
            LinkedListNode<T> current = _head;
            LinkedListNode<T> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(item))
                {
                    if (previous == null)
                    {
                        _head = current.Next;
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }
                    _count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// 查找元素是否存在
        /// </summary>
        public bool Contains(T item)
        {
            LinkedListNode<T> current = _head;
            while (current != null)
            {
                if (current.Data.Equals(item))
                    return true;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// 反转链表
        /// </summary>
        public void Reverse()
        {
            LinkedListNode<T> previous = null;
            LinkedListNode<T> current = _head;
            LinkedListNode<T> next = null;

            while (current != null)
            {
                next = current.Next;
                current.Next = previous;
                previous = current;
                current = next;
            }
            _head = previous;
        }

        /// <summary>
        /// 转换为数组
        /// </summary>
        public T[] ToArray()
        {
            T[] result = new T[_count];
            LinkedListNode<T> current = _head;
            for (int i = 0; i < _count; i++)
            {
                result[i] = current.Data;
                current = current.Next;
            }
            return result;
        }

        /// <summary>
        /// 清空链表
        /// </summary>
        public void Clear()
        {
            _head = null;
            _count = 0;
        }
    }
}
