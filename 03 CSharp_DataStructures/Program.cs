using System;

namespace CSharpDataStructures
{
    /// <summary>
    /// 程序主入口
    /// 演示所有数据结构和算法的使用
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# 数据结构与算法演示程序 ===\n");

            // 1. 动态数组演示
            DemoArrayList();

            // 2. 链表演示
            DemoLinkedList();

            // 3. 栈演示
            DemoStack();

            // 4. 队列演示
            DemoQueue();

            // 5. 二叉树演示
            DemoBinaryTree();

            // 6. 二叉搜索树演示
            DemoBST();

            // 7. 图演示
            DemoGraph();

            // 8. 哈希表演示
            DemoHashTable();

            // 9. 堆演示
            DemoHeap();

            // 10. 排序算法演示
            DemoSort();

            // 11. 查找算法演示
            DemoSearch();

            // 12. KMP算法演示
            DemoKMP();

            Console.WriteLine("\n=== 所有演示完成 ===");
            Console.ReadKey();
        }

        static void DemoArrayList()
        {
            Console.WriteLine("--- 1. 动态数组(ArrayList)演示 ---");
            Linear.ArrayList<int> list = new Linear.ArrayList<int>();
            list.Add(10);
            list.Add(20);
            list.Add(30);
            list.Insert(1, 15);
            Console.WriteLine("元素个数: " + list.Count);
            Console.Write("元素: ");
            for (int i = 0; i < list.Count; i++)
                Console.Write(list[i] + " ");
            Console.WriteLine();
            list.RemoveAt(2);
            Console.WriteLine("删除索引2后: " + string.Join(" ", list.ToArray()));
            Console.WriteLine("是否包含20: " + list.Contains(20));
            Console.WriteLine();
        }

        static void DemoLinkedList()
        {
            Console.WriteLine("--- 2. 单链表(SinglyLinkedList)演示 ---");
            Linear.SinglyLinkedList<string> linkedList = new Linear.SinglyLinkedList<string>();
            linkedList.AddFirst("First");
            linkedList.AddLast("Second");
            linkedList.AddLast("Third");
            linkedList.Insert(2, "Inserted");
            Console.WriteLine("节点个数: " + linkedList.Count);
            Console.Write("链表: ");
            linkedList.Print();
            linkedList.Remove("Second");
            Console.Write("删除Second后: ");
            linkedList.Print();
            linkedList.Reverse();
            Console.Write("反转后: ");
            linkedList.Print();
            Console.WriteLine();
        }

        static void DemoStack()
        {
            Console.WriteLine("--- 3. 栈(Stack)演示 ---");
            Stack.LinkedStack<string> stack = new Stack.LinkedStack<string>();
            stack.Push("A");
            stack.Push("B");
            stack.Push("C");
            Console.WriteLine("栈顶元素: " + stack.Peek());
            Console.Write("出栈顺序: ");
            while (!stack.IsEmpty)
                Console.Write(stack.Pop() + " ");
            Console.WriteLine("\n");
        }

        static void DemoQueue()
        {
            Console.WriteLine("--- 4. 队列(Queue)演示 ---");
            Queue.LinkedQueue<int> queue = new Queue.LinkedQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            Console.WriteLine("队头元素: " + queue.Peek());
            Console.Write("出队顺序: ");
            while (!queue.IsEmpty)
                Console.Write(queue.Dequeue() + " ");
            Console.WriteLine("\n");
        }

        static void DemoBinaryTree()
        {
            Console.WriteLine("--- 5. 二叉树遍历演示 ---");
            Tree.BinaryTree<char> tree = new Tree.BinaryTree<char>('A');
            tree.Root.Left = new Tree.BinaryTreeNode<char>('B');
            tree.Root.Right = new Tree.BinaryTreeNode<char>('C');
            tree.Root.Left.Left = new Tree.BinaryTreeNode<char>('D');
            tree.Root.Left.Right = new Tree.BinaryTreeNode<char>('E');
            tree.Root.Right.Left = new Tree.BinaryTreeNode<char>('F');

            Console.Write("前序遍历: ");
            tree.PreOrderTraversal();
            Console.Write("中序遍历: ");
            tree.InOrderTraversal();
            Console.Write("后序遍历: ");
            tree.PostOrderTraversal();
            Console.Write("层序遍历: ");
            tree.LevelOrderTraversal();
            Console.WriteLine("树的深度: " + tree.GetDepth());
            Console.WriteLine("节点总数: " + tree.GetNodeCount());
            Console.WriteLine("叶子节点数: " + tree.GetLeafCount());
            Console.WriteLine();
        }

        static void DemoBST()
        {
            Console.WriteLine("--- 6. 二叉搜索树(BST)演示 ---");
            Tree.BinarySearchTree<int> bst = new Tree.BinarySearchTree<int>();
            int[] nums = { 5, 3, 8, 2, 4, 7, 9 };
            foreach (int num in nums)
                bst.Insert(num);

            Console.Write("中序遍历(有序): ");
            bst.InOrderTraversal();
            Console.WriteLine("最小值: " + bst.FindMin());
            Console.WriteLine("最大值: " + bst.FindMax());
            Console.WriteLine("是否包含4: " + bst.Contains(4));
            bst.Remove(5);
            Console.Write("删除5后中序: ");
            bst.InOrderTraversal();
            Console.WriteLine();
        }

        static void DemoGraph()
        {
            Console.WriteLine("--- 7. 图(邻接表)演示 ---");
            Graph.AdjacencyListGraph graph = new Graph.AdjacencyListGraph();
            for (int i = 0; i < 6; i++)
                graph.AddVertex(i);
            graph.AddUndirectedEdge(0, 1);
            graph.AddUndirectedEdge(0, 2);
            graph.AddUndirectedEdge(1, 3);
            graph.AddUndirectedEdge(2, 4);
            graph.AddUndirectedEdge(3, 5);
            graph.AddUndirectedEdge(4, 5);

            Console.Write("DFS遍历: ");
            graph.DFS(0);
            Console.Write("BFS遍历: ");
            graph.BFS(0);
            Console.WriteLine();
        }

        static void DemoHashTable()
        {
            Console.WriteLine("--- 8. 哈希表演示 ---");
            Hash.HashTable<string, int> ht = new Hash.HashTable<string, int>();
            ht.Add("张三", 90);
            ht.Add("李四", 85);
            ht.Add("王五", 95);
            Console.WriteLine("李四分数: " + ht["李四"]);
            ht["张三"] = 92;
            Console.WriteLine("张三分数更新后: " + ht["张三"]);
            Console.WriteLine("是否包含王五: " + ht.ContainsKey("王五"));
            ht.Remove("王五");
            Console.WriteLine("删除王五后是否包含: " + ht.ContainsKey("王五"));
            Console.WriteLine("元素个数: " + ht.Count);
            Console.WriteLine();
        }

        static void DemoHeap()
        {
            Console.WriteLine("--- 9. 最大堆演示 ---");
            Heap.MaxHeap<int> heap = new Heap.MaxHeap<int>();
            int[] nums = { 3, 1, 4, 1, 5, 9, 2, 6 };
            foreach (int num in nums)
                heap.Insert(num);

            Console.Write("堆排序结果: ");
            int[] sorted = heap.HeapSort();
            Console.WriteLine(string.Join(" ", sorted));
            Console.WriteLine();
        }

        static void DemoSort()
        {
            Console.WriteLine("--- 10. 排序算法演示 ---");
            int[] arr = { 64, 34, 25, 12, 22, 11, 90 };
            Console.WriteLine("原数组: " + string.Join(" ", arr));

            int[] test1 = (int[])arr.Clone();
            Sort.SortAlgorithms.QuickSort(test1);
            Console.WriteLine("快速排序: " + string.Join(" ", test1));

            int[] test2 = (int[])arr.Clone();
            Sort.SortAlgorithms.MergeSort(test2);
            Console.WriteLine("归并排序: " + string.Join(" ", test2));

            int[] test3 = (int[])arr.Clone();
            Sort.SortAlgorithms.HeapSort(test3);
            Console.WriteLine("堆排序:   " + string.Join(" ", test3));

            int[] test4 = (int[])arr.Clone();
            Sort.SortAlgorithms.BubbleSort(test4);
            Console.WriteLine("冒泡排序: " + string.Join(" ", test4));
            Console.WriteLine();
        }

        static void DemoSearch()
        {
            Console.WriteLine("--- 11. 查找算法演示 ---");
            int[] sortedArr = { 2, 5, 8, 12, 16, 23, 38, 56, 72, 91 };
            int target = 23;
            Console.WriteLine("有序数组: " + string.Join(" ", sortedArr));
            Console.WriteLine("查找目标: " + target);
            Console.WriteLine("顺序查找位置: " + Search.SearchAlgorithms.SequentialSearch(sortedArr, target));
            Console.WriteLine("二分查找位置: " + Search.SearchAlgorithms.BinarySearch(sortedArr, target));
            Console.WriteLine();
        }

        static void DemoKMP()
        {
            Console.WriteLine("--- 12. KMP字符串匹配演示 ---");
            string text = "ABABDABACDABABCABAB";
            string pattern = "ABABCABAB";
            Console.WriteLine("文本串: " + text);
            Console.WriteLine("模式串: " + pattern);
            int pos = String.StringAlgorithms.KMPMatch(text, pattern);
            Console.WriteLine("匹配位置: " + pos);
            if (pos >= 0)
                Console.WriteLine("匹配片段: " + text.Substring(pos, pattern.Length));
            Console.WriteLine();
        }
    }
}