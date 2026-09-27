using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp20AIFull
{
    /// <summary>
    /// 矩阵类 基础数据结构 C#2.0语法
    /// </summary>
    public class Matrix
    {
        public double[][] Data;
        public int Rows;
        public int Cols;

        public Matrix(int rows, int cols)
        {
            this.Rows = rows;
            this.Cols = cols;
            Data = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                Data[i] = new double[cols];
            }
        }

        public static Matrix Random(int rows, int cols, double scale)
        {
            Random rand = new Random();
            Matrix m = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    m.Data[i][j] = (rand.NextDouble() * 2 - 1) * scale;
                }
            }
            return m;
        }

        public static Matrix Random(int rows, int cols, double scale, int seed)
        {
            Random rand = new Random(seed);
            Matrix m = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    m.Data[i][j] = (rand.NextDouble() * 2 - 1) * scale;
                }
            }
            return m;
        }

        public static Matrix Zeros(int rows, int cols)
        {
            return new Matrix(rows, cols);
        }

        public double this[int i, int j]
        {
            get { return Data[i][j]; }
            set { Data[i][j] = value; }
        }

        public Matrix Copy()
        {
            Matrix m = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    m.Data[i][j] = Data[i][j];
                }
            }
            return m;
        }

        public Matrix T()
        {
            Matrix m = new Matrix(Cols, Rows);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    m.Data[j][i] = Data[i][j];
                }
            }
            return m;
        }

        public static Matrix Dot(Matrix a, Matrix b)
        {
            Matrix result = new Matrix(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.Cols; k++)
                    {
                        sum += a.Data[i][k] * b.Data[k][j];
                    }
                    result.Data[i][j] = sum;
                }
            }
            return result;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            Matrix m = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    m.Data[i][j] = a.Data[i][j] + b.Data[i][j];
                }
            }
            return m;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            Matrix m = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    m.Data[i][j] = a.Data[i][j] - b.Data[i][j];
                }
            }
            return m;
        }

        public static Matrix operator *(Matrix a, double s)
        {
            Matrix m = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    m.Data[i][j] = a.Data[i][j] * s;
                }
            }
            return m;
        }

        public static Matrix operator *(double s, Matrix a)
        {
            return a * s;
        }

        public static Matrix ElementMultiply(Matrix a, Matrix b)
        {
            Matrix m = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    m.Data[i][j] = a.Data[i][j] * b.Data[i][j];
                }
            }
            return m;
        }

        public void ApplySigmoid()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i][j] = MathUtils.Sigmoid(Data[i][j]);
                }
            }
        }

        public void ApplyReLU()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i][j] = Math.Max(0, Data[i][j]);
                }
            }
        }

        public void ApplyTanh()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i][j] = Math.Tanh(Data[i][j]);
                }
            }
        }

        public void Softmax()
        {
            for (int i = 0; i < Rows; i++)
            {
                double max = Data[i][0];
                for (int j = 1; j < Cols; j++)
                    if (Data[i][j] > max) max = Data[i][j];
                double sum = 0;
                for (int j = 0; j < Cols; j++)
                {
                    Data[i][j] = Math.Exp(Data[i][j] - max);
                    sum += Data[i][j];
                }
                for (int j = 0; j < Cols; j++)
                {
                    Data[i][j] /= sum;
                }
            }
        }

        public void Add(double v)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Data[i][j] += v;
                }
            }
        }
    }
}
