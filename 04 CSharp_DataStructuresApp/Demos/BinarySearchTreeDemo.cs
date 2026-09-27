using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 二叉搜索树演示
    /// </summary>
    public class BinarySearchTreeDemo
    {
        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第5章：二叉搜索树（Binary Search Tree）");
            
            console.WriteSection("5.1 基本概念");
            console.WriteLine("二叉搜索树是一种特殊的二叉树，满足以下性质：");
            console.WriteLine("• 若左子树不为空，则左子树上所有节点的值均小于根节点的值");
            console.WriteLine("• 若右子树不为空，则右子树上所有节点的值均大于根节点的值");
            console.WriteLine("• 左右子树也都是二叉搜索树");
            console.WriteTip("中序遍历二叉搜索树可以得到一个升序序列，这是BST的重要特性。");
            console.WriteLine("平均时间复杂度：查找、插入、删除均为O(logn)；最坏情况（斜树）退化为O(n)。");

            console.WriteSection("5.2 构建二叉搜索树");
            console.WriteLine("按顺序插入元素：50, 30, 70, 20, 40, 60, 80");
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            int[] nums = new int[] { 50, 30, 70, 20, 40, 60, 80 };
            foreach (int n in nums)
            {
                bst.Insert(n);
            }
            console.WriteResult("节点总数：" + bst.Count + "，树的高度：" + bst.Height());
            console.WriteResult("最小值：" + bst.Min() + "，最大值：" + bst.Max());

            console.WriteSection("5.3 四种遍历方式");
            console.WriteLine("先序遍历（根→左→右）：");
            console.WriteResult(ArrayToString(bst.PreOrderTraversal()));
            console.WriteLine("中序遍历（左→根→右）[升序]：");
            console.WriteResult(ArrayToString(bst.InOrderTraversal()));
            console.WriteLine("后序遍历（左→右→根）：");
            console.WriteResult(ArrayToString(bst.PostOrderTraversal()));
            console.WriteLine("层序遍历（广度优先，按层输出）：");
            console.WriteResult(ArrayToString(bst.LevelOrderTraversal()));

            console.WriteSection("5.4 查找操作");
            console.WriteResult(
                "查找40：" + (bst.Contains(40) ? "存在" : "不存在") + "\n" +
                "查找90：" + (bst.Contains(90) ? "存在" : "不存在")
            );

            console.WriteSection("5.5 树结构示意图");
            console.WriteCode(
@"        50
      /    \
    30      70
   /  \    /  \
 20   40  60   80");

            console.WriteSection("5.6 适用场景与优缺点");
            console.WriteLine("✅ 优点：有序，插入删除查找效率高，支持范围查询");
            console.WriteLine("❌ 缺点：最坏情况退化为链表；没有随机访问");
            console.WriteLine("✅ 应用场景：有序映射、集合、数据库索引基础");
            console.WriteNote("实际工程中常用平衡二叉树（AVL、红黑树）解决BST最坏情况退化问题。");

            console.WriteHr();
            console.Render();
        }

        private static string ArrayToString<T>(T[] arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i]);
                if (i < arr.Length - 1) sb.Append(", ");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
