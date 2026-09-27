using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20DataStructures.DataStructures
{
    /// <summary>
    /// 顺序表实现：基于数组的动态线性表
    /// C# 2.0 泛型实现
    /// </summary>
    public class SeqList<T>
    {
        private T[] _data;
        private int _count;
        private const int _defaultCapacity = 4;

        public SeqList()
        {
            _data = new T[_defaultCapacity];
            _count = 0;
        }

        public SeqList(int capacity)
        {
            _data = new T[capacity];
            _count = 0;
        }

        /// <summary>
        /// 获取元素个数
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// 获取容量
        /// </summary>
        public int Capacity
        {
            get { return _data.Length; }
        }

        /// <summary>
        /// 索引器访问
        /// </summary>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");
                return _data[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");
                _data[index] = value;
            }
        }

        /// <summary>
        /// 尾部添加元素
        /// </summary>
        public void Add(T item)
        {
            if (_count == _data.Length)
            {
                EnsureCapacity(_count * 2);
            }
            _data[_count] = item;
            _count++;
        }

        /// <summary>
        /// 在指定位置插入元素
        /// </summary>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException("index");
            
            if (_count == _data.Length)
            {
                EnsureCapacity(_count * 2);
            }

            // 元素后移
            for (int i = _count; i > index; i--)
            {
                _data[i] = _data[i - 1];
            }
            _data[index] = item;
            _count++;
        }

        /// <summary>
        /// 删除指定位置元素
        /// </summary>
        public T RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException("index");

            T removed = _data[index];
            // 元素前移
            for (int i = index; i < _count - 1; i++)
            {
                _data[i] = _data[i + 1];
            }
            _count--;
            _data[_count] = default(T); // 释放引用
            return removed;
        }

        /// <summary>
        /// 查找元素索引
        /// </summary>
        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_data[i].Equals(item))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 清空表
        /// </summary>
        public void Clear()
        {
            Array.Clear(_data, 0, _count);
            _count = 0;
        }

        /// <summary>
        /// 扩容
        /// </summary>
        private void EnsureCapacity(int newCapacity)
        {
            if (newCapacity < _defaultCapacity)
                newCapacity = _defaultCapacity;
            
            T[] newData = new T[newCapacity];
            Array.Copy(_data, 0, newData, 0, _count);
            _data = newData;
        }

        /// <summary>
        /// 转换为数组用于显示
        /// </summary>
        public T[] ToArray()
        {
            T[] result = new T[_count];
            Array.Copy(_data, 0, result, 0, _count);
            return result;
        }
    }
}
