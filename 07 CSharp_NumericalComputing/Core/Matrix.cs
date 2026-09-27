using System;

namespace NumericalComputing.Core
{
    /// <summary>
    /// 矩阵类，纯C#2.0实现，零第三方依赖
    /// </summary>
    public class Matrix
    {
        private double[,] _data;
        private int _rows;
        private int _cols;

        public Matrix(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
            _data = new double[rows, cols];
        }

        public Matrix(double[,] data)
        {
            _rows = data.GetLength(0);
            _cols = data.GetLength(1);
            _data = new double[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _data[i, j] = data[i, j];
                }
            }
        }

        public int Rows
        {
            get { return _rows; }
        }

        public int Cols
        {
            get { return _cols; }
        }

        public double this[int i, int j]
        {
            get { return _data[i, j]; }
            set { _data[i, j] = value; }
        }

        public static Matrix Zero(int rows, int cols)
        {
            return new Matrix(rows, cols);
        }

        public static Matrix Identity(int n)
        {
            Matrix m = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                m[i, i] = 1.0;
            }
            return m;
        }

        public Matrix Copy()
        {
            return new Matrix(_data);
        }

        public static Matrix Add(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("矩阵维度不匹配");
            Matrix res = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    res[i, j] = a[i, j] + b[i, j];
                }
            }
            return res;
        }

        public static Matrix Sub(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("矩阵维度不匹配");
            Matrix res = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    res[i, j] = a[i, j] - b[i, j];
                }
            }
            return res;
        }

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if (a.Cols != b.Rows)
                throw new ArgumentException("矩阵维度不匹配，无法相乘");
            Matrix res = new Matrix(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int k = 0; k < a.Cols; k++)
                {
                    if (Math.Abs(a[i, k]) < 1e-12) continue;
                    for (int j = 0; j < b.Cols; j++)
                    {
                        res[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return res;
        }

        public static Matrix Multiply(Matrix a, double scalar)
        {
            Matrix res = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    res[i, j] = a[i, j] * scalar;
                }
            }
            return res;
        }

        public Matrix Transpose()
        {
            Matrix res = new Matrix(_cols, _rows);
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    res[j, i] = _data[i, j];
                }
            }
            return res;
        }

        public void SwapRows(int i, int j)
        {
            if (i == j) return;
            for (int k = 0; k < _cols; k++)
            {
                double tmp = _data[i, k];
                _data[i, k] = _data[j, k];
                _data[j, k] = tmp;
            }
        }

        public int Pivot(int row, int col)
        {
            int maxRow = row;
            double maxVal = Math.Abs(_data[row, col]);
            for (int i = row + 1; i < _rows; i++)
            {
                if (Math.Abs(_data[i, col]) > maxVal)
                {
                    maxVal = Math.Abs(_data[i, col]);
                    maxRow = i;
                }
            }
            return maxRow;
        }

        public double Determinant()
        {
            if (_rows != _cols)
                throw new InvalidOperationException("只有方阵可以计算行列式");
            Matrix m = this.Copy();
            double det = 1.0;
            for (int k = 0; k < _rows; k++)
            {
                int pivot = m.Pivot(k, k);
                if (pivot != k)
                {
                    m.SwapRows(k, pivot);
                    det = -det;
                }
                if (Math.Abs(m[k, k]) < 1e-12) return 0.0;
                det *= m[k, k];
                for (int i = k + 1; i < _rows; i++)
                {
                    double factor = m[i, k] / m[k, k];
                    for (int j = k; j < _cols; j++)
                    {
                        m[i, j] -= factor * m[k, j];
                    }
                }
            }
            return det;
        }

        public Matrix Inverse()
        {
            if (_rows != _cols)
                throw new InvalidOperationException("只有方阵可以求逆");
            int n = _rows;
            Matrix aug = new Matrix(n, 2 * n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    aug[i, j] = _data[i, j];
                }
                aug[i, i + n] = 1.0;
            }

            for (int k = 0; k < n; k++)
            {
                int pivot = aug.Pivot(k, k);
                if (pivot != k) aug.SwapRows(k, pivot);
                if (Math.Abs(aug[k, k]) < 1e-12)
                    throw new InvalidOperationException("矩阵奇异，不可逆");

                double div = aug[k, k];
                for (int j = 0; j < 2 * n; j++)
                {
                    aug[k, j] /= div;
                }

                for (int i = 0; i < n; i++)
                {
                    if (i != k && Math.Abs(aug[i, k]) > 1e-12)
                    {
                        double factor = aug[i, k];
                        for (int j = 0; j < 2 * n; j++)
                        {
                            aug[i, j] -= factor * aug[k, j];
                        }
                    }
                }
            }

            Matrix res = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    res[i, j] = aug[i, j + n];
                }
            }
            return res;
        }

        public string ToString(int precision = 4)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < _rows; i++)
            {
                sb.Append("[ ");
                for (int j = 0; j < _cols; j++)
                {
                    sb.Append(_data[i, j].ToString("F" + precision));
                    if (j < _cols - 1) sb.Append(",\t");
                }
                sb.AppendLine(" ]");
            }
            return sb.ToString();
        }

        public double[] GetColumn(int col)
        {
            double[] res = new double[_rows];
            for (int i = 0; i < _rows; i++) res[i] = _data[i, col];
            return res;
        }

        public double[] GetRow(int row)
        {
            double[] res = new double[_cols];
            for (int j = 0; j < _cols; j++) res[j] = _data[row, j];
            return res;
        }

        public static Matrix FromColumnVector(double[] vec)
        {
            Matrix m = new Matrix(vec.Length, 1);
            for (int i = 0; i < vec.Length; i++) m[i, 0] = vec[i];
            return m;
        }

        public static Matrix FromRowVector(double[] vec)
        {
            Matrix m = new Matrix(1, vec.Length);
            for (int i = 0; i < vec.Length; i++) m[0, i] = vec[i];
            return m;
        }
    }
}