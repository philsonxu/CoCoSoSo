using System;
using System.Collections.Generic;

namespace CSharpDataStructures.Graph
{
    /// <summary>
    /// 邻接表边节点
    /// </summary>
    internal class EdgeNode
    {
        public int Vertex; // 邻接点索引
        public int Weight; // 权重
        public EdgeNode Next;

        public EdgeNode(int vertex, int weight = 1)
        {
            Vertex = vertex;
            Weight = weight;
            Next = null;
        }
    }

    /// <summary>
    /// 邻接表顶点节点
    /// </summary>
    internal class VertexNode
    {
        public int Data;
        public EdgeNode FirstEdge;

        public VertexNode(int data)
        {
            Data = data;
            FirstEdge = null;
        }
    }

    /// <summary>
    /// 邻接表表示的有向带权图
    /// 适合稀疏图，空间复杂度O(V+E)
    /// </summary>
    public class AdjacencyListGraph
    {
        private List<VertexNode> _vertexs;
        private int _edgeCount;

        public AdjacencyListGraph()
        {
            _vertexs = new List<VertexNode>();
            _edgeCount = 0;
        }

        /// <summary>
        /// 顶点数
        /// </summary>
        public int VertexCount
        {
            get { return _vertexs.Count; }
        }

        /// <summary>
        /// 边数
        /// </summary>
        public int EdgeCount
        {
            get { return _edgeCount; }
        }

        /// <summary>
        /// 添加顶点
        /// </summary>
        public void AddVertex(int data)
        {
            _vertexs.Add(new VertexNode(data));
        }

        /// <summary>
        /// 添加有向边
        /// </summary>
        public void AddEdge(int from, int to, int weight = 1)
        {
            if (from < 0 || from >= _vertexs.Count || to < 0 || to >= _vertexs.Count)
                throw new ArgumentOutOfRangeException("顶点索引超出范围");

            EdgeNode newEdge = new EdgeNode(to, weight);
            newEdge.Next = _vertexs[from].FirstEdge;
            _vertexs[from].FirstEdge = newEdge;
            _edgeCount++;
        }

        /// <summary>
        /// 添加无向边
        /// </summary>
        public void AddUndirectedEdge(int v, int w, int weight = 1)
        {
            AddEdge(v, w, weight);
            AddEdge(w, v, weight);
        }

        /// <summary>
        /// 深度优先遍历DFS
        /// </summary>
        public void DFS(int start)
        {
            bool[] visited = new bool[_vertexs.Count];
            DFS(start, visited);
            Console.WriteLine();
        }

        private void DFS(int v, bool[] visited)
        {
            visited[v] = true;
            Console.Write(v + " ");

            EdgeNode edge = _vertexs[v].FirstEdge;
            while (edge != null)
            {
                if (!visited[edge.Vertex])
                    DFS(edge.Vertex, visited);
                edge = edge.Next;
            }
        }

        /// <summary>
        /// 广度优先遍历BFS
        /// </summary>
        public void BFS(int start)
        {
            bool[] visited = new bool[_vertexs.Count];
            Queue<int> queue = new Queue<int>();

            visited[start] = true;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                Console.Write(v + " ");

                EdgeNode edge = _vertexs[v].FirstEdge;
                while (edge != null)
                {
                    if (!visited[edge.Vertex])
                    {
                        visited[edge.Vertex] = true;
                        queue.Enqueue(edge.Vertex);
                    }
                    edge = edge.Next;
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 拓扑排序（针对有向无环图DAG）
        /// </summary>
        public List<int> TopologicalSort()
        {
            int[] inDegree = new int[_vertexs.Count];
            // 计算入度
            for (int i = 0; i < _vertexs.Count; i++)
            {
                EdgeNode edge = _vertexs[i].FirstEdge;
                while (edge != null)
                {
                    inDegree[edge.Vertex]++;
                    edge = edge.Next;
                }
            }

            // 入度为0的顶点入队
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < _vertexs.Count; i++)
            {
                if (inDegree[i] == 0)
                    queue.Enqueue(i);
            }

            List<int> result = new List<int>();
            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                result.Add(v);

                EdgeNode edge = _vertexs[v].FirstEdge;
                while (edge != null)
                {
                    inDegree[edge.Vertex]--;
                    if (inDegree[edge.Vertex] == 0)
                        queue.Enqueue(edge.Vertex);
                    edge = edge.Next;
                }
            }

            // 如果结果数量不等于顶点数，说明有环
            if (result.Count != _vertexs.Count)
                throw new InvalidOperationException("图中存在环，无法进行拓扑排序");

            return result;
        }

        /// <summary>
        /// 打印邻接表
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < _vertexs.Count; i++)
            {
                Console.Write(i + ": ");
                EdgeNode edge = _vertexs[i].FirstEdge;
                while (edge != null)
                {
                    Console.Write(edge.Vertex + " -> ");
                    edge = edge.Next;
                }
                Console.WriteLine("null");
            }
        }
    }
}