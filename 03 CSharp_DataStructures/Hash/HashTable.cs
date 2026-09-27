using System;

namespace CSharpDataStructures.Hash
{
    /// <summary>
    /// 哈希表节点（键值对）
    /// </summary>
    internal class HashNode<K, V>
    {
        public K Key;
        public V Value;
        public HashNode<K, V> Next; // 链地址法解决冲突

        public HashNode(K key, V value)
        {
            Key = key;
            Value = value;
            Next = null;
        }
    }

    /// <summary>
    /// 哈希表实现（链地址法解决冲突）
    /// 时间复杂度：平均O(1)
    /// </summary>
    public class HashTable<K, V>
    {
        private HashNode<K, V>[] _buckets;
        private int _count;
        private const int DefaultCapacity = 16;
        private const double LoadFactor = 0.75; // 负载因子

        public HashTable()
        {
            _buckets = new HashNode<K, V>[DefaultCapacity];
            _count = 0;
        }

        public HashTable(int capacity)
        {
            _buckets = new HashNode<K, V>[capacity];
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
        /// 索引器
        /// </summary>
        public V this[K key]
        {
            get
            {
                V value;
                if (!TryGetValue(key, out value))
                    throw new KeyNotFoundException("键不存在");
                return value;
            }
            set { AddOrUpdate(key, value); }
        }

        /// <summary>
        /// 添加或更新键值对
        /// </summary>
        public void Add(K key, V value)
        {
            AddOrUpdate(key, value);
        }

        private void AddOrUpdate(K key, V value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            // 扩容检查
            if ((double)_count / _buckets.Length >= LoadFactor)
                Resize();

            int index = GetBucketIndex(key);

            // 检查是否已存在
            HashNode<K, V> current = _buckets[index];
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    current.Value = value; // 更新
                    return;
                }
                current = current.Next;
            }

            // 不存在则插入到链表头部
            HashNode<K, V> newNode = new HashNode<K, V>(key, value);
            newNode.Next = _buckets[index];
            _buckets[index] = newNode;
            _count++;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        public bool TryGetValue(K key, out V value)
        {
            value = default(V);
            if (key == null) return false;

            int index = GetBucketIndex(key);
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
            return false;
        }

        /// <summary>
        /// 是否包含键
        /// </summary>
        public bool ContainsKey(K key)
        {
            V value;
            return TryGetValue(key, out value);
        }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool Remove(K key)
        {
            if (key == null) return false;

            int index = GetBucketIndex(key);
            HashNode<K, V> current = _buckets[index];
            HashNode<K, V> prev = null;

            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    if (prev == null)
                        _buckets[index] = current.Next;
                    else
                        prev.Next = current.Next;

                    _count--;
                    return true;
                }
                prev = current;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// 清空哈希表
        /// </summary>
        public void Clear()
        {
            Array.Clear(_buckets, 0, _buckets.Length);
            _count = 0;
        }

        /// <summary>
        /// 计算桶索引（除留余数法）
        /// </summary>
        private int GetBucketIndex(K key)
        {
            int hash = key.GetHashCode();
            // 确保非负
            return Math.Abs(hash % _buckets.Length);
        }

        /// <summary>
        /// 扩容（容量翻倍）
        /// </summary>
        private void Resize()
        {
            int newCapacity = _buckets.Length * 2;
            HashNode<K, V>[] newBuckets = new HashNode<K, V>[newCapacity];

            // 重新哈希所有元素
            for (int i = 0; i < _buckets.Length; i++)
            {
                HashNode<K, V> current = _buckets[i];
                while (current != null)
                {
                    HashNode<K, V> next = current.Next;

                    int newIndex = Math.Abs(current.Key.GetHashCode() % newCapacity);
                    current.Next = newBuckets[newIndex];
                    newBuckets[newIndex] = current;

                    current = next;
                }
            }

            _buckets = newBuckets;
        }

        /// <summary>
        /// 打印哈希表
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < _buckets.Length; i++)
            {
                Console.Write("[" + i + "] ");
                HashNode<K, V> current = _buckets[i];
                while (current != null)
                {
                    Console.Write("(" + current.Key + ":" + current.Value + ") -> ");
                    current = current.Next;
                }
                Console.WriteLine("null");
            }
        }
    }
}