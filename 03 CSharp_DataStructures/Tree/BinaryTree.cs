using System;
using System.Collections.Generic;

namespace CSharpDataStructures.Tree
{
    /// <summary>
    /// 二叉树节点
    /// </summary>
    public class BinaryTreeNode<T>
    {
        public T Value;
        public BinaryTreeNode<T> Left;
        public BinaryTreeNode<T> Right;

        public BinaryTreeNode(T value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }

    /// <summary>
    /// 二叉树基本实现
    /// 包含四种遍历方式：前序、中序、后序、层序
    /// </summary>
    public class BinaryTree<T>
    {
        protected BinaryTreeNode<T> _root;

        public BinaryTree()
        {
            _root = null;
        }

        public BinaryTree(T value)
        {
            _root = new BinaryTreeNode<T>(value);
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public BinaryTreeNode<T> Root
        {
            get { return _root; }
            set { _root = value; }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return _root == null; }
        }

        #region 递归遍历

        /// <summary>
        /// 前序遍历（根-左-右）
        /// </summary>
        public void PreOrderTraversal()
        {
            PreOrder(_root);
            Console.WriteLine();
        }

        private void PreOrder(BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                Console.Write(node.Value + " ");
                PreOrder(node.Left);
                PreOrder(node.Right);
            }
        }

        /// <summary>
        /// 中序遍历（左-根-右）
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
        /// 后序遍历（左-右-根）
        /// </summary>
        public void PostOrderTraversal()
        {
            PostOrder(_root);
            Console.WriteLine();
        }

        private void PostOrder(BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                PostOrder(node.Left);
                PostOrder(node.Right);
                Console.Write(node.Value + " ");
            }
        }

        #endregion

        #region 非递归遍历（使用栈）

        /// <summary>
        /// 非递归前序遍历
        /// </summary>
        public void PreOrderNonRecursive()
        {
            if (_root == null) return;

            Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();
            stack.Push(_root);

            while (stack.Count > 0)
            {
                BinaryTreeNode<T> node = stack.Pop();
                Console.Write(node.Value + " ");

                if (node.Right != null)
                    stack.Push(node.Right);
                if (node.Left != null)
                    stack.Push(node.Left);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 非递归中序遍历
        /// </summary>
        public void InOrderNonRecursive()
        {
            if (_root == null) return;

            Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();
            BinaryTreeNode<T> current = _root;

            while (current != null || stack.Count > 0)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                current = stack.Pop();
                Console.Write(current.Value + " ");
                current = current.Right;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 非递归后序遍历（双栈法）
        /// </summary>
        public void PostOrderNonRecursive()
        {
            if (_root == null) return;

            Stack<BinaryTreeNode<T>> stack1 = new Stack<BinaryTreeNode<T>>();
            Stack<BinaryTreeNode<T>> stack2 = new Stack<BinaryTreeNode<T>>();
            stack1.Push(_root);

            while (stack1.Count > 0)
            {
                BinaryTreeNode<T> node = stack1.Pop();
                stack2.Push(node);

                if (node.Left != null)
                    stack1.Push(node.Left);
                if (node.Right != null)
                    stack1.Push(node.Right);
            }

            while (stack2.Count > 0)
            {
                Console.Write(stack2.Pop().Value + " ");
            }
            Console.WriteLine();
        }

        #endregion

        /// <summary>
        /// 层序遍历（广度优先BFS，使用队列）
        /// </summary>
        public void LevelOrderTraversal()
        {
            if (_root == null) return;

            Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(_root);

            while (queue.Count > 0)
            {
                BinaryTreeNode<T> node = queue.Dequeue();
                Console.Write(node.Value + " ");

                if (node.Left != null)
                    queue.Enqueue(node.Left);
                if (node.Right != null)
                    queue.Enqueue(node.Right);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 计算树的深度
        /// </summary>
        public int GetDepth()
        {
            return GetDepth(_root);
        }

        private int GetDepth(BinaryTreeNode<T> node)
        {
            if (node == null)
                return 0;

            int leftDepth = GetDepth(node.Left);
            int rightDepth = GetDepth(node.Right);
            return Math.Max(leftDepth, rightDepth) + 1;
        }

        /// <summary>
        /// 计算节点总数
        /// </summary>
        public int GetNodeCount()
        {
            return GetNodeCount(_root);
        }

        private int GetNodeCount(BinaryTreeNode<T> node)
        {
            if (node == null)
                return 0;
            return GetNodeCount(node.Left) + GetNodeCount(node.Right) + 1;
        }

        /// <summary>
        /// 计算叶子节点数
        /// </summary>
        public int GetLeafCount()
        {
            return GetLeafCount(_root);
        }

        private int GetLeafCount(BinaryTreeNode<T> node)
        {
            if (node == null)
                return 0;
            if (node.Left == null && node.Right == null)
                return 1;
            return GetLeafCount(node.Left) + GetLeafCount(node.Right);
        }
    }
}