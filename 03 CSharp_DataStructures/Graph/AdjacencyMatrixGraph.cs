using System;
using System.Collections.Generic;

namespace CSharpDataStructures.Graph
{
    /// <summary>
    /// 邻接矩阵表示的无向图
    /// 适合稠密图，空间复杂度O(V²)
    /// </summary>
    public class AdjacencyMatrixGraph
    {
        private int _vertexCount; // 顶点数
        private int _edgeCount;   // 边数
        private int[,] _matrix;   // 邻接矩阵

        public AdjacencyMatrixGraph(int vertexCount)
        {
            if (vertexCount <= 0)
                throw new ArgumentException("顶点数必须大于0");

            _vertexCount = vertexCount;
            _edgeCount = 0;
            _matrix = new int[vertexCount, vertexCount];

            // 初始化矩阵
            for (int i = 0; i < vertexCount; i++)
                for (int j = 0; j < vertexCount; j++)
                    _matrix[i, j] = 0;
        }

        /// <summary>
        /// 顶点数
        /// </summary>
        public int VertexCount
        {
            get { return _vertexCount; }
        }

        /// <summary>
        /// 边数
        /// </summary>
        public int EdgeCount
        {
            get { return _edgeCount; }
        }

        /// <summary>
        /// 添加边
        /// </summary>
        public void AddEdge(int v, int w)
        {
            if (v < 0 || v >= _vertexCount || w < 0 || w >= _vertexCount)
                throw new ArgumentOutOfRangeException("顶点索引超出范围");

            if (_matrix[v, w] == 0)
                _edgeCount++;

            _matrix[v, w] = 1;
            _matrix[w, v] = 1; // 无向图对称
        }

        /// <summary>
        /// 删除边
        /// </summary>
        public void RemoveEdge(int v, int w)
        {
            if (v < 0 || v >= _vertexCount || w < 0 || w >= _vertexCount)
                throw new ArgumentOutOfRangeException("顶点索引超出范围");

            if (_matrix[v, w] == 1)
                _edgeCount--;

            _matrix[v, w] = 0;
            _matrix[w, v] = 0;
        }

        /// <summary>
        /// 判断是否有边
        /// </summary>
        public bool HasEdge(int v, int w)
        {
            return _matrix[v, w] == 1;
        }

        /// <summary>
        /// 获取顶点v的所有邻接点
        /// </summary>
        public List<int> GetNeighbors(int v)
        {
            List<int> neighbors = new List<int>();
            for (int i = 0; i < _vertexCount; i++)
            {
                if (_matrix[v, i] == 1)
                    neighbors.Add(i);
            }
            return neighbors;
        }

        /// <summary>
        /// 深度优先遍历DFS
        /// </summary>
        public void DFS(int start)
        {
            bool[] visited = new bool[_vertexCount];
            DFS(start, visited);
            Console.WriteLine();
        }

        private void DFS(int v, bool[] visited)
        {
            visited[v] = true;
            Console.Write(v + " ");

            List<int> neighbors = GetNeighbors(v);
            foreach (int w in neighbors)
            {
                if (!visited[w])
                    DFS(w, visited);
            }
        }

        /// <summary>
        /// 广度优先遍历BFS
        /// </summary>
        public void BFS(int start)
        {
            bool[] visited = new bool[_vertexCount];
            Queue<int> queue = new Queue<int>();

            visited[start] = true;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                Console.Write(v + " ");

                List<int> neighbors = GetNeighbors(v);
                foreach (int w in neighbors)
                {
                    if (!visited[w])
                    {
                        visited[w] = true;
                        queue.Enqueue(w);
                    }
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 打印邻接矩阵
        /// </summary>
        public void Print()
        {
            Console.Write("  ");
            for (int i = 0; i < _vertexCount; i++)
                Console.Write(i + " ");
            Console.WriteLine();

            for (int i = 0; i < _vertexCount; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < _vertexCount; j++)
                {
                    Console.Write(_matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}