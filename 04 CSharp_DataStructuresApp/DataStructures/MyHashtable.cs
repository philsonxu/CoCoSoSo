using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20DataStructures.DataStructures
{
    /// <summary>
    /// 哈希表节点（键值对）
    /// </summary>
    public class HashNode<K, V>
    {
        public K Key;
        public V Value;
        public HashNode<K, V> Next;

        public HashNode(K key, V value)
        {
            Key = key;
            Value = value;
            Next = null;
        }
    }

    /// <summary>
    /// 哈希表实现：链地址法解决冲突
    /// C# 2.0 泛型实现
    /// </summary>
    public class MyHashtable<K, V>
    {
        private HashNode<K, V>[] _buckets;
        private int _count;
        private const int _defaultCapacity = 16;
        private const double _loadFactor = 0.75;

        public MyHashtable()
        {
            _buckets = new HashNode<K, V>[_defaultCapacity];
            _count = 0;
        }

        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// 添加键值对
        /// </summary>
        public void Add(K key, V value)
        {
            if ((double)_count / _buckets.Length > _loadFactor)
            {
                Resize();
            }

            int index = GetIndex(key);
            HashNode<K, V> newNode = new HashNode<K, V>(key, value);

            if (_buckets[index] == null)
            {
                _buckets[index] = newNode;
            }
            else
            {
                // 头插法添加到链表
                newNode.Next = _buckets[index];
                _buckets[index] = newNode;
            }
            _count++;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        public V Get(K key)
        {
            int index = GetIndex(key);
            HashNode<K, V> current = _buckets[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                    return current.Value;
                current = current.Next;
            }
            throw new KeyNotFoundException("键不存在: " + key);
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        public bool TryGet(K key, out V value)
        {
            int index = GetIndex(key);
            HashNode<K, V> current = _buckets[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    value = current.Value;
                    return true;
                }
                current = current.Next;
            }
            value = default(V);
            return false;
        }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool Remove(K key)
        {
            int index = GetIndex(key);
            HashNode<K, V> current = _buckets[index];
            HashNode<K, V> previous = null;

            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    if (previous == null)
                        _buckets[index] = current.Next;
                    else
                        previous.Next = current.Next;
                    _count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// 检查键是否存在
        /// </summary>
        public bool ContainsKey(K key)
        {
            int index = GetIndex(key);
            HashNode<K, V> current = _buckets[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                    return true;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// 获取所有键
        /// </summary>
        public K[] GetKeys()
        {
            List<K> keys = new List<K>();
            foreach (HashNode<K, V> bucket in _buckets)
            {
                HashNode<K, V> current = bucket;
                while (current != null)
                {
                    keys.Add(current.Key);
                    current = current.Next;
                }
            }
            return keys.ToArray();
        }

        /// <summary>
        /// 计算哈希索引
        /// </summary>
        private int GetIndex(K key)
        {
            int hash = key.GetHashCode();
            return Math.Abs(hash % _buckets.Length);
        }

        /// <summary>
        /// 扩容
        /// </summary>
        private void Resize()
        {
            HashNode<K, V>[] oldBuckets = _buckets;
            _buckets = new HashNode<K, V>[oldBuckets.Length * 2];
            _count = 0;

            foreach (HashNode<K, V> bucket in oldBuckets)
            {
                HashNode<K, V> current = bucket;
                while (current != null)
                {
                    Add(current.Key, current.Value);
                    current = current.Next;
                }
            }
        }

        public void Clear()
        {
            Array.Clear(_buckets, 0, _buckets.Length);
            _count = 0;
        }
    }
}
