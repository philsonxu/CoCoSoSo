using System;

namespace CSharpDataStructures.Linear
{
    /// <summary>
    /// 动态数组（顺序表）实现
    /// 基于数组实现的线性表，支持动态扩容
    /// 时间复杂度：随机访问O(1)，插入删除O(n)
    /// </summary>
    public class ArrayList<T>
    {
        private T[] _items;
        private int _count;
        private const int DefaultCapacity = 4;

        public ArrayList()
        {
            _items = new T[DefaultCapacity];
            _count = 0;
        }

        public ArrayList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

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
        /// 数组容量
        /// </summary>
        public int Capacity
        {
            get { return _items.Length; }
        }

        /// <summary>
        /// 索引器
        /// </summary>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");
                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");
                _items[index] = value;
            }
        }

        /// <summary>
        /// 添加元素到末尾
        /// </summary>
        public void Add(T item)
        {
            if (_count == _items.Length)
                EnsureCapacity(_count + 1);

            _items[_count] = item;
            _count++;
        }

        /// <summary>
        /// 在指定位置插入元素
        /// </summary>
        public void Insert(int index, T item)
        {
            if (index < 0 || index > _count)
                throw new ArgumentOutOfRangeException("index");

            if (_count == _items.Length)
                EnsureCapacity(_count + 1);

            // 元素后移
            if (index < _count)
                Array.Copy(_items, index, _items, index + 1, _count - index);

            _items[index] = item;
            _count++;
        }

        /// <summary>
        /// 删除指定位置的元素
        /// </summary>
        public T RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException("index");

            T removed = _items[index];
            _count--;

            // 元素前移
            if (index < _count)
                Array.Copy(_items, index + 1, _items, index, _count - index);

            _items[_count] = default(T); // 释放引用
            return removed;
        }

        /// <summary>
        /// 查找元素索引
        /// </summary>
        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, _count);
        }

        /// <summary>
        /// 是否包含元素
        /// </summary>
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// 清空数组
        /// </summary>
        public void Clear()
        {
            if (_count > 0)
            {
                Array.Clear(_items, 0, _count);
                _count = 0;
            }
        }

        /// <summary>
        /// 扩容方法
        /// </summary>
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;
                if (newCapacity < min)
                    newCapacity = min;

                T[] newItems = new T[newCapacity];
                if (_count > 0)
                    Array.Copy(_items, 0, newItems, 0, _count);

                _items = newItems;
            }
        }

        /// <summary>
        /// 转换为数组
        /// </summary>
        public T[] ToArray()
        {
            T[] result = new T[_count];
            Array.Copy(_items, 0, result, 0, _count);
            return result;
        }
    }
}