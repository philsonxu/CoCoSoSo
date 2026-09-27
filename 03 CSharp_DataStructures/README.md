# C# 数据结构与算法源程序库

本代码库基于 **C# 2.0** 语法实现，涵盖了计算机科学中常用的数据结构与经典算法，所有代码均带有详细中文注释，适合学习数据结构与算法的初学者参考使用。

## 📁 目录结构

```
CSharpDataStructures/
├── Linear/                 # 线性表
│   ├── ArrayList.cs        # 动态数组（顺序表）
│   └── SinglyLinkedList.cs # 单链表
├── Stack/                  # 栈
│   ├── ArrayStack.cs       # 顺序栈（数组实现）
│   └── LinkedStack.cs      # 链栈（链表实现）
├── Queue/                  # 队列
│   ├── ArrayQueue.cs       # 循环队列（数组实现）
│   └── LinkedQueue.cs      # 链队列（链表实现）
├── Tree/                   # 树结构
│   ├── BinaryTree.cs       # 二叉树（四种遍历）
│   └── BinarySearchTree.cs # 二叉搜索树
├── Graph/                  # 图结构
│   ├── AdjacencyMatrixGraph.cs  # 邻接矩阵图
│   └── AdjacencyListGraph.cs    # 邻接表图（带拓扑排序）
├── Hash/                   # 哈希表
│   └── HashTable.cs        # 链地址法哈希表
├── Heap/                   # 堆结构
│   ├── MaxHeap.cs          # 最大堆
│   └── MinHeap.cs          # 最小堆 + 优先队列
├── Sort/                   # 排序算法
│   └── SortAlgorithms.cs   # 8种经典排序算法
├── Search/                 # 查找算法
│   └── SearchAlgorithms.cs # 4种查找算法
├── String/                 # 字符串算法
│   └── StringAlgorithms.cs # KMP模式匹配
├── Program.cs              # 主程序（所有示例演示）
├── CSharpDataStructures.csproj # Visual Studio项目文件
└── README.md               # 说明文档
```

## 📚 包含知识点

### 线性结构
| 数据结构 | 实现方式 | 核心操作 | 时间复杂度 |
|---------|---------|---------|-----------|
| 动态数组 | 数组 | Add/Insert/Remove/IndexOf | 随机访问O(1)，增删O(n) |
| 单链表 | 节点指针 | AddFirst/AddLast/Remove/Reverse | 头插O(1)，查找O(n) |
| 顺序栈 | 数组 | Push/Pop/Peek | O(1) |
| 链栈 | 链表 | Push/Pop/Peek | O(1)，无需预设容量 |
| 循环队列 | 循环数组 | Enqueue/Dequeue/Peek | O(1)，解决假溢出 |
| 链队列 | 链表 | Enqueue/Dequeue/Peek | O(1)，双指针操作 |

### 树形结构
| 数据结构 | 核心功能 | 特点 |
|---------|---------|------|
| 普通二叉树 | 前/中/后/层序遍历（递归+非递归） | 深度/节点数/叶子数计算 |
| 二叉搜索树 | Insert/Remove/Contains/FindMin/FindMax | 中序遍历输出有序序列 |

> **遍历方式说明**：
> - 前序遍历：根 → 左 → 右
> - 中序遍历：左 → 根 → 右
> - 后序遍历：左 → 右 → 根
> - 层序遍历：广度优先(BFS)，按层遍历

### 图结构
| 实现方式 | 适用场景 | 核心算法 |
|---------|---------|---------|
| 邻接矩阵 | 稠密图 | DFS、BFS |
| 邻接表 | 稀疏图 | DFS、BFS、拓扑排序 |

### 其他数据结构
| 数据结构 | 说明 | 时间复杂度 |
|---------|------|-----------|
| 哈希表 | 链地址法解决冲突，支持动态扩容 | 增删查平均O(1) |
| 最大堆 | 大顶堆，实现堆排序 | 增删O(log n) |
| 最小堆 | 小顶堆，实现优先队列 | 增删O(log n) |
| 优先队列 | 基于最小堆，值越小优先级越高 | 入队出队O(log n) |

### 排序算法对比
| 算法 | 平均时间复杂度 | 最坏 | 最好 | 空间复杂度 | 稳定性 |
|-----|--------------|------|------|-----------|--------|
| 冒泡排序 | O(n²) | O(n²) | O(n) | O(1) | ✅ 稳定 |
| 选择排序 | O(n²) | O(n²) | O(n²) | O(1) | ❌ 不稳定 |
| 插入排序 | O(n²) | O(n²) | O(n) | O(1) | ✅ 稳定 |
| 希尔排序 | O(n^1.3) | O(n²) | O(n) | O(1) | ❌ 不稳定 |
| 快速排序 | O(n log n) | O(n²) | O(n log n) | O(log n) | ❌ 不稳定 |
| 归并排序 | O(n log n) | O(n log n) | O(n log n) | O(n) | ✅ 稳定 |
| 堆排序 | O(n log n) | O(n log n) | O(n log n) | O(1) | ❌ 不稳定 |

### 查找算法
| 算法 | 适用条件 | 时间复杂度 |
|-----|---------|-----------|
| 顺序查找 | 无序/有序数组 | O(n) |
| 二分查找 | 有序数组 | O(log n) |
| 插值查找 | 均匀分布的有序数组 | O(log log n) |
| KMP模式匹配 | 字符串匹配 | O(n+m) |

## 🚀 快速开始

### 环境要求
- Visual Studio 2005/2008/2010 或更高版本
- .NET Framework 2.0 运行时

### 运行方式
1. 打开 `CSharpDataStructures.csproj` 项目文件
2. 直接按 F5 运行 `Program.cs`，即可看到所有数据结构和算法的演示输出
3. 可将需要单独测试的文件设为启动项

### 示例代码片段

**链表使用示例**：
```csharp
SinglyLinkedList<string> list = new SinglyLinkedList<string>();
list.AddFirst("Hello");
list.AddLast("World");
list.Print(); // 输出: Hello -> World -> null
```

**快速排序示例**：
```csharp
int[] arr = { 64, 34, 25, 12, 22 };
SortAlgorithms.QuickSort(arr);
// 结果: 12 22 25 34 64
```

**二叉搜索树示例**：
```csharp
BinarySearchTree<int> bst = new BinarySearchTree<int>();
bst.Insert(5);
bst.Insert(3);
bst.Insert(8);
bst.InOrderTraversal(); // 中序输出: 3 5 8
```

## 💡 学习建议
1. **线性结构先行**：先掌握数组、链表、栈、队列这些基础线性结构
2. **理解复杂度**：重点理解每种数据结构操作的时间复杂度差异
3. **手动模拟**：对于树、图的遍历，建议手动画示意图模拟过程
4. **手写代码**：不要只看不写，尝试不看注释自己重新实现一遍
5. **对比学习**：比如栈和队列的差异、数组和链表的适用场景对比

## 📝 注意事项
- 所有代码严格遵循 C# 2.0 语法规范，未使用泛型集合（除自定义泛型外）、LINQ、var等后续版本特性
- 所有类和方法均配有XML文档注释，使用时Visual Studio会有智能提示
- 为保持代码简洁，部分边界情况做了简化处理，生产环境使用需增加更多异常判断

