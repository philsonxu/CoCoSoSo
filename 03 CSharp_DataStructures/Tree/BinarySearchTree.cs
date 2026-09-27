using System;

namespace CSharpDataStructures.Tree
{
    /// <summary>
    /// 二叉搜索树（二叉排序树）实现
    /// 性质：左子树所有节点值 < 根节点值 < 右子树所有节点值
    /// 时间复杂度：平均O(log n)，最坏O(n)
    /// </summary>
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        private BinaryTreeNode<T> _root;
        private int _count;

        public BinarySearchTree()
        {
            _root = null;
            _count = 0;
        }

        /// <summary>
        /// 节点总数
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
            get { return _root == null; }
        }

        /// <summary>
        /// 插入节点
        /// </summary>
        public void Insert(T value)
        {
            _root = Insert(_root, value);
            _count++;
        }

        private BinaryTreeNode<T> Insert(BinaryTreeNode<T> node, T value)
        {
            if (node == null)
                return new BinaryTreeNode<T>(value);

            int cmp = value.CompareTo(node.Value);
            if (cmp < 0)
                node.Left = Insert(node.Left, value);
            else if (cmp > 0)
                node.Right = Insert(node.Right, value);
            // 相等则不插入重复值

            return node;
        }

        /// <summary>
        /// 查找节点是否存在
        /// </summary>
        public bool Contains(T value)
        {
            return Contains(_root, value);
        }

        private bool Contains(BinaryTreeNode<T> node, T value)
        {
            if (node == null)
                return false;

            int cmp = value.CompareTo(node.Value);
            if (cmp < 0)
                return Contains(node.Left, value);
            else if (cmp > 0)
                return Contains(node.Right, value);
            else
                return true;
        }

        /// <summary>
        /// 查找最小值
        /// </summary>
        public T FindMin()
        {
            if (IsEmpty)
                throw new InvalidOperationException("树为空");

            return FindMin(_root).Value;
        }

        private BinaryTreeNode<T> FindMin(BinaryTreeNode<T> node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        /// <summary>
        /// 查找最大值
        /// </summary>
        public T FindMax()
        {
            if (IsEmpty)
                throw new InvalidOperationException("树为空");

            return FindMax(_root).Value;
        }

        private BinaryTreeNode<T> FindMax(BinaryTreeNode<T> node)
        {
            while (node.Right != null)
                node = node.Right;
            return node;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public bool Remove(T value)
        {
            if (!Contains(value))
                return false;

            _root = Remove(_root, value);
            _count--;
            return true;
        }

        private BinaryTreeNode<T> Remove(BinaryTreeNode<T> node, T value)
        {
            if (node == null)
                return null;

            int cmp = value.CompareTo(node.Value);
            if (cmp < 0)
            {
                node.Left = Remove(node.Left, value);
            }
            else if (cmp > 0)
            {
                node.Right = Remove(node.Right, value);
            }
            else
            {
                // 找到要删除的节点
                // 情况1：叶子节点
                if (node.Left == null && node.Right == null)
                    return null;
                // 情况2：只有一个子节点
                else if (node.Left == null)
                    return node.Right;
                else if (node.Right == null)
                    return node.Left;
                // 情况3：有两个子节点，用右子树最小值替换
                else
                {
                    BinaryTreeNode<T> minNode = FindMin(node.Right);
                    node.Value = minNode.Value;
                    node.Right = Remove(node.Right, minNode.Value);
                }
            }
            return node;
        }

        /// <summary>
        /// 中序遍历（结果为有序序列）
        /// </summary>
        public void InOrderTraversal()
        {
            InOrder(_root);
            Console.WriteLine();
        }

        private void InOrder(BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                InOrder(node.Left);
                Console.Write(node.Value + " ");
                InOrder(node.Right);
            }
        }

        /// <summary>
        /// 清空树
        /// </summary>
        public void Clear()
        {
            _root = null;
            _count = 0;
        }
    }
}