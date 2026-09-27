using System;

namespace CSharpDataStructures.Stack
{
    /// <summary>
    /// 顺序栈（基于数组实现）
    /// 后进先出(LIFO)数据结构
    /// </summary>
    public class ArrayStack<T>
    {
        private T[] _items;
        private int _top; // 栈顶指针，-1表示空栈
        private const int DefaultCapacity = 10;

        public ArrayStack()
        {
            _items = new T[DefaultCapacity];
            _top = -1;
        }

        public ArrayStack(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            _items = new T[capacity];
            _top = -1;
        }

        /// <summary>
        /// 栈中元素个数
        /// </summary>
        public int Count
        {
            get { return _top + 1; }
        }

        /// <summary>
        /// 栈是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return _top == -1; }
        }

        /// <summary>
        /// 入栈
        /// </summary>
        public void Push(T item)
        {
            if (_top == _items.Length - 1)
                throw new InvalidOperationException("栈已满");

            _top++;
            _items[_top] = item;
        }

        /// <summary>
        /// 出栈
        /// </summary>
        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("栈为空");

            T item = _items[_top];
            _items[_top] = default(T);
            _top--;
            return item;
        }

        /// <summary>
        /// 获取栈顶元素但不出栈
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("栈为空");

            return _items[_top];
        }

        /// <summary>
        /// 清空栈
        /// </summary>
        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            _top = -1;
        }
    }
}