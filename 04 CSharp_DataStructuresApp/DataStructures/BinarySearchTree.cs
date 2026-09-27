using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20DataStructures.DataStructures
{
    /// <summary>
    /// 二叉树节点
    /// </summary>
    public class BinaryTreeNode<T> where T : IComparable<T>
    {
        public T Data;
        public BinaryTreeNode<T> Left;
        public BinaryTreeNode<T> Right;

        public BinaryTreeNode(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }

    /// <summary>
    /// 二叉搜索树实现
    /// 满足：左子树所有节点 < 根节点 < 右子树所有节点
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

        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// 插入节点
        /// </summary>
        public void Insert(T data)
        {
            BinaryTreeNode<T> newNode = new BinaryTreeNode<T>(data);
            if (_root == null)
            {
                _root = newNode;
                _count++;
                return;
            }

            BinaryTreeNode<T> current = _root;
            BinaryTreeNode<T> parent = null;
            while (current != null)
            {
                parent = current;
                if (data.CompareTo(current.Data) < 0)
                {
                    current = current.Left;
                }
                else if (data.CompareTo(current.Data) > 0)
                {
                    current = current.Right;
                }
                else
                {
                    return; // 重复元素不插入
                }
            }

            if (data.CompareTo(parent.Data) < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;
            _count++;
        }

        /// <summary>
        /// 查找节点是否存在
        /// </summary>
        public bool Contains(T data)
        {
            BinaryTreeNode<T> current = _root;
            while (current != null)
            {
                if (data.CompareTo(current.Data) < 0)
                    current = current.Left;
                else if (data.CompareTo(current.Data) > 0)
                    current = current.Right;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 先序遍历：根-左-右
        /// </summary>
        public T[] PreOrderTraversal()
        {
            List<T> result = new List<T>();
            PreOrder(_root, result);
            return result.ToArray();
        }

        private void PreOrder(BinaryTreeNode<T> node, List<T> result)
        {
            if (node != null)
            {
                result.Add(node.Data);
                PreOrder(node.Left, result);
                PreOrder(node.Right, result);
            }
        }

        /// <summary>
        /// 中序遍历：左-根-右（结果为升序）
        /// </summary>
        public T[] InOrderTraversal()
        {
            List<T> result = new List<T>();
            InOrder(_root, result);
            return result.ToArray();
        }

        private void InOrder(BinaryTreeNode<T> node, List<T> result)
        {
            if (node != null)
            {
                InOrder(node.Left, result);
                result.Add(node.Data);
                InOrder(node.Right, result);
            }
        }

        /// <summary>
        /// 后序遍历：左-右-根
        /// </summary>
        public T[] PostOrderTraversal()
        {
            List<T> result = new List<T>();
            PostOrder(_root, result);
            return result.ToArray();
        }

        private void PostOrder(BinaryTreeNode<T> node, List<T> result)
        {
            if (node != null)
            {
                PostOrder(node.Left, result);
                PostOrder(node.Right, result);
                result.Add(node.Data);
            }
        }

        /// <summary>
        /// 层序遍历（广度优先）
        /// </summary>
        public T[] LevelOrderTraversal()
        {
            List<T> result = new List<T>();
            if (_root == null)
                return result.ToArray();

            Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(_root);
            while (queue.Count > 0)
            {
                BinaryTreeNode<T> node = queue.Dequeue();
                result.Add(node.Data);
                if (node.Left != null)
                    queue.Enqueue(node.Left);
                if (node.Right != null)
                    queue.Enqueue(node.Right);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        public T Min()
        {
            if (_root == null)
                throw new InvalidOperationException("树为空");
            BinaryTreeNode<T> current = _root;
            while (current.Left != null)
                current = current.Left;
            return current.Data;
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        public T Max()
        {
            if (_root == null)
                throw new InvalidOperationException("树为空");
            BinaryTreeNode<T> current = _root;
            while (current.Right != null)
                current = current.Right;
            return current.Data;
        }

        /// <summary>
        /// 获取树的高度
        /// </summary>
        public int Height()
        {
            return GetHeight(_root);
        }

        private int GetHeight(BinaryTreeNode<T> node)
        {
            if (node == null)
                return 0;
            int leftHeight = GetHeight(node.Left);
            int rightHeight = GetHeight(node.Right);
            return Math.Max(leftHeight, rightHeight) + 1;
        }

        public void Clear()
        {
            _root = null;
            _count = 0;
        }
    }
}
