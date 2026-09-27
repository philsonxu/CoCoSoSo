using System;

namespace CSharpDataStructures.Linear
{
    /// <summary>
    /// 单链表节点
    /// </summary>
    public class SinglyLinkedListNode<T>
    {
        public T Value;
        public SinglyLinkedListNode<T> Next;

        public SinglyLinkedListNode(T value)
        {
            Value = value;
            Next = null;
        }
    }

    /// <summary>
    /// 单链表实现
    /// 基于节点指针的线性表
    /// 时间复杂度：随机访问O(n)，头部插入删除O(1)
    /// </summary>
    public class SinglyLinkedList<T>
    {
        private SinglyLinkedListNode<T> _head;
        private int _count;

        public SinglyLinkedList()
        {
            _head = null;
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
            get { return _head == null; }
        }

        /// <summary>
        /// 获取头节点值
        /// </summary>
        public T First
        {
            get
            {
                if (_head == null)
                    throw new InvalidOperationException("链表为空");
                return _head.Value;
            }
        }

        /// <summary>
        /// 在头部添加节点
        /// </summary>
        public void AddFirst(T value)
        {
            SinglyLinkedListNode<T> newNode = new SinglyLinkedListNode<T>(value);
            newNode.Next = _head;
            _head = newNode;
            _count++;
        }

        /// <summary>
        /// 在尾部添加节点
        /// </summary>
        public void AddLast(T value)
        {
            SinglyLinkedListNode<T> newNode = new SinglyLinkedListNode<T>(value);

            if (_head == null)
            {
                _head = newNode;
            }
            else
            {
                SinglyLinkedListNode<T> current = _head;
                while (current.Next != null)
                    current = current.Next;
                current.Next = newNode;
            }
            _count++;
        }

        /// <summary>
        /// 在指定位置插入节点
        /// </summary>
        public void Insert(int index, T value)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException("index");

            if (index == 0)
            {
                AddFirst(value);
                return;
            }

            SinglyLinkedListNode<T> current = _head;
            for (int i = 0; i < index - 1; i++)
                current = current.Next;

            SinglyLinkedListNode<T> newNode = new SinglyLinkedListNode<T>(value);
            newNode.Next = current.Next;
            current.Next = newNode;
            _count++;
        }

        /// <summary>
        /// 删除头节点
        /// </summary>
        public T RemoveFirst()
        {
            if (_head == null)
                throw new InvalidOperationException("链表为空");

            T value = _head.Value;
            _head = _head.Next;
            _count--;
            return value;
        }

        /// <summary>
        /// 删除指定值的节点
        /// </summary>
        public bool Remove(T value)
        {
            if (_head == null)
                return false;

            // 处理头节点
            if (_head.Value.Equals(value))
            {
                _head = _head.Next;
                _count--;
                return true;
            }

            SinglyLinkedListNode<T> current = _head;
            while (current.Next != null && !current.Next.Value.Equals(value))
                current = current.Next;

            if (current.Next == null)
                return false;

            current.Next = current.Next.Next;
            _count--;
            return true;
        }

        /// <summary>
        /// 查找节点
        /// </summary>
        public bool Contains(T value)
        {
            SinglyLinkedListNode<T> current = _head;
            while (current != null)
            {
                if (current.Value.Equals(value))
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
            SinglyLinkedListNode<T> prev = null;
            SinglyLinkedListNode<T> current = _head;
            SinglyLinkedListNode<T> next = null;

            while (current != null)
            {
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }
            _head = prev;
        }

        /// <summary>
        /// 清空链表
        /// </summary>
        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        /// <summary>
        /// 遍历输出
        /// </summary>
        public void Print()
        {
            SinglyLinkedListNode<T> current = _head;
            while (current != null)
            {
                Console.Write(current.Value + " -> ");
                current = current.Next;
            }
            Console.WriteLine("null");
        }
    }
}