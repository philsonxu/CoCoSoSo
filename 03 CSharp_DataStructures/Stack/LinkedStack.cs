using System;

namespace CSharpDataStructures.Stack
{
    /// <summary>
    /// 链栈节点
    /// </summary>
    internal class StackNode<T>
    {
        public T Value;
        public StackNode<T> Next;

        public StackNode(T value)
        {
            Value = value;
            Next = null;
        }
    }

    /// <summary>
    /// 链栈（基于链表实现）
    /// 后进先出(LIFO)数据结构，无需预设容量
    /// </summary>
    public class LinkedStack<T>
    {
        private StackNode<T> _top;
        private int _count;

        public LinkedStack()
        {
            _top = null;
            _count = 0;
        }

        /// <summary>
        /// 栈中元素个数
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// 栈是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return _top == null; }
        }

        /// <summary>
        /// 入栈
        /// </summary>
        public void Push(T item)
        {
            StackNode<T> newNode = new StackNode<T>(item);
            newNode.Next = _top;
            _top = newNode;
            _count++;
        }

        /// <summary>
        /// 出栈
        /// </summary>
        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("栈为空");

            T value = _top.Value;
            _top = _top.Next;
            _count--;
            return value;
        }

        /// <summary>
        /// 获取栈顶元素但不出栈
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("栈为空");

            return _top.Value;
        }

        /// <summary>
        /// 清空栈
        /// </summary>
        public void Clear()
        {
            _top = null;
            _count = 0;
        }
    }
}