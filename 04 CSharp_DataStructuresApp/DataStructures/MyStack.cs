using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20DataStructures.DataStructures
{
    /// <summary>
    /// 栈实现：后进先出(LIFO)
    /// 基于数组实现
    /// </summary>
    public class MyStack<T>
    {
        private T[] _data;
        private int _top; // 栈顶指针，指向栈顶元素下一个位置
        private const int _defaultCapacity = 4;

        public MyStack()
        {
            _data = new T[_defaultCapacity];
            _top = 0;
        }

        public MyStack(int capacity)
        {
            _data = new T[capacity];
            _top = 0;
        }

        /// <summary>
        /// 栈中元素个数
        /// </summary>
        public int Count
        {
            get { return _top; }
        }

        /// <summary>
        /// 栈是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return _top == 0; }
        }

        /// <summary>
        /// 入栈
        /// </summary>
        public void Push(T item)
        {
            if (_top == _data.Length)
            {
                EnsureCapacity(_top * 2);
            }
            _data[_top] = item;
            _top++;
        }

        /// <summary>
        /// 出栈
        /// </summary>
        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("栈为空");
            _top--;
            T item = _data[_top];
            _data[_top] = default(T);
            return item;
        }

        /// <summary>
        /// 获取栈顶元素但不出栈
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("栈为空");
            return _data[_top - 1];
        }

        /// <summary>
        /// 清空栈
        /// </summary>
        public void Clear()
        {
            Array.Clear(_data, 0, _top);
            _top = 0;
        }

        private void EnsureCapacity(int newCapacity)
        {
            if (newCapacity < _defaultCapacity)
                newCapacity = _defaultCapacity;
            T[] newData = new T[newCapacity];
            Array.Copy(_data, 0, newData, 0, _top);
            _data = newData;
        }

        /// <summary>
        /// 转换为数组（栈底到栈顶顺序）
        /// </summary>
        public T[] ToArray()
        {
            T[] result = new T[_top];
            Array.Copy(_data, 0, result, 0, _top);
            return result;
        }
    }
}
