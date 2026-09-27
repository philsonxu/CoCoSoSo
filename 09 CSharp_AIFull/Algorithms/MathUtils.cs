using System;
using System.Collections.Generic;

namespace CSharp20AIFull
{
    /// <summary>
    /// 数学工具类
    /// </summary>
    public class MathUtils
    {
        private static Random _rand = new Random(42);

        public static double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public static double ReLU(double x)
        {
            return Math.Max(0, x);
        }

        public static double Tanh(double x)
        {
            return Math.Tanh(x);
        }

        public static double RandomDouble()
        {
            return _rand.NextDouble();
        }

        public static double RandomDouble(double min, double max)
        {
            return min + _rand.NextDouble() * (max - min);
        }

        public static int RandomInt(int max)
        {
            return _rand.Next(max);
        }

        public static int RandomInt(int min, int max)
        {
            return _rand.Next(min, max);
        }

        public static double Mean(double[] arr)
        {
            double sum = 0;
            for (int i = 0; i < arr.Length; i++) sum += arr[i];
            return sum / arr.Length;
        }

        public static double StdDev(double[] arr)
        {
            double mean = Mean(arr);
            double var = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                var += (arr[i] - mean) * (arr[i] - mean);
            }
            return Math.Sqrt(var / arr.Length);
        }

        public static double EuclideanDistance(double[] a, double[] b)
        {
            double sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += (a[i] - b[i]) * (a[i] - b[i]);
            }
            return Math.Sqrt(sum);
        }

        public static void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = _rand.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        public static Matrix OneHot(int label, int numClasses)
        {
            Matrix m = Matrix.Zeros(1, numClasses);
            m.Data[0][label] = 1.0;
            return m;
        }

        public static int ArgMax(double[] arr)
        {
            int idx = 0;
            double max = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                    idx = i;
                }
            }
            return idx;
        }

        public static int ArgMax(Matrix m, int row)
        {
            int idx = 0;
            double max = m.Data[row][0];
            for (int j = 1; j < m.Cols; j++)
            {
                if (m.Data[row][j] > max)
                {
                    max = m.Data[row][j];
                    idx = j;
                }
            }
            return idx;
        }
    }
}
