using System;
using System.Collections.Generic;
using System.Text;
using CSharp20DataStructures.DataStructures;

namespace CSharp20DataStructures.Demos
{
    /// <summary>
    /// 图与图遍历演示
    /// </summary>
    public class GraphDemo
    {
        private int _vertexCount;
        private List<int>[] _adj; // 邻接表

        public GraphDemo(int v)
        {
            _vertexCount = v;
            _adj = new List<int>[v];
            for (int i = 0; i < v; i++)
            {
                _adj[i] = new List<int>();
            }
        }

        /// <summary>
        /// 添加边（无向图）
        /// </summary>
        public void AddEdge(int v, int w)
        {
            _adj[v].Add(w);
            _adj[w].Add(v);
        }

        public static void Run(HtmlConsole console)
        {
            console.WriteTitle("第10章：图（Graph）与遍历算法");
            
            console.WriteSection("10.1 图的基本概念");
            console.WriteLine("图是由顶点集合和边集合组成的非线性数据结构，用于表示多对多的关系。");
            console.WriteLine("存储方式：邻接矩阵、邻接表；本示例使用邻接表，适合稀疏图。");
            console.WriteLine("两种核心遍历算法：深度优先搜索（DFS，栈/递归）、广度优先搜索（BFS，队列）。");

            console.WriteSection("10.2 构建示例图");
            console.WriteCode(
@"        0 ------ 1
        |        |
        |        |
        2 ------ 3
         \      /
          \    /
            4
            |
            5");
            GraphDemo graph = new GraphDemo(6);
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(2, 3);
            graph.AddEdge(2, 4);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            console.WriteResult("已创建6个顶点7条边的无向图");

            console.WriteSection("10.3 广度优先搜索（BFS）");
            console.WriteLine("BFS类似于层序遍历，从起点开始先访问所有邻接点，再逐层向外扩散，使用队列实现。");
            console.WriteTip("BFS可以找到无权图的最短路径！");
            string bfsResult = graph.BFS(0);
            console.WriteResult("从顶点0开始BFS遍历顺序：\n" + bfsResult);

            console.WriteSection("10.4 深度优先搜索（DFS）");
            console.WriteLine("DFS沿着一条路径走到头，再回溯走其他路径，使用递归或栈实现。");
            string dfsResult = graph.DFS(0);
            console.WriteResult("从顶点0开始DFS遍历顺序：\n" + dfsResult);

            console.WriteSection("10.5 BFS与DFS对比");
            console.WriteLine("| 算法 | 数据结构 | 特点 | 适用场景 |");
            console.WriteLine("|------|----------|------|----------|");
            console.WriteLine("| BFS | 队列 | 按层遍历，先近后远 | 最短路径、层序遍历、找最近点 |");
            console.WriteLine("| DFS | 栈/递归 | 一条路走到底再回溯 | 连通分量、拓扑排序、回溯搜索 |");

            console.WriteSection("10.6 图算法应用场景");
            console.WriteLine("✅ 社交网络好友推荐");
            console.WriteLine("✅ 地图导航路径规划");
            console.WriteLine("✅ 网页爬虫、搜索引擎");
            console.WriteLine("✅ 依赖管理、拓扑排序");
            console.WriteLine("✅ 网络路由、最短路径算法（Dijkstra、Floyd）");

            console.WriteHr();
            console.Render();
        }

        /// <summary>
        /// 广度优先搜索
        /// </summary>
        private string BFS(int start)
        {
            bool[] visited = new bool[_vertexCount];
            MyQueue<int> queue = new MyQueue<int>();
            StringBuilder sb = new StringBuilder();

            visited[start] = true;
            queue.Enqueue(start);

            while (!queue.IsEmpty)
            {
                int v = queue.Dequeue();
                sb.Append(v + " → ");

                foreach (int w in _adj[v])
                {
                    if (!visited[w])
                    {
                        visited[w] = true;
                        queue.Enqueue(w);
                    }
                }
            }
            sb.Length -= 4; // 去掉最后一个箭头
            return sb.ToString();
        }

        /// <summary>
        /// 深度优先搜索
        /// </summary>
        private string DFS(int start)
        {
            bool[] visited = new bool[_vertexCount];
            StringBuilder sb = new StringBuilder();
            DFSUtil(start, visited, sb);
            sb.Length -= 4;
            return sb.ToString();
        }

        private void DFSUtil(int v, bool[] visited, StringBuilder sb)
        {
            visited[v] = true;
            sb.Append(v + " → ");
            foreach (int w in _adj[v])
            {
                if (!visited[w])
                {
                    DFSUtil(w, visited, sb);
                }
            }
        }
    }
}
