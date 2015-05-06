using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace OverworldGenerator
{
    public class Matrix : IEnumerable<double>
    {
        private double[,] elements;
        internal int cols { get; set; }
        internal int rows { get; set; }

        public Matrix(int cols, int rows) 
        {
            this.cols = cols;
            this.rows = rows;

            elements = new double[cols, rows];
        }

        public void Clear()
        {
            elements = new double[cols, rows];
        }

        private int Index(int x, int y)
        {
            return y * cols + x;
        }

        public double Get(int x, int y)
        {
            return elements[x,y];
        }

        public void Set(int x, int y, double v)
        {
            elements[x,y] = v;
        }

        public void Add(Matrix other)
        {
            Debug.Assert(cols == other.cols && rows == other.rows);
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; ++y)
                {
                    Set(x,y, Get(x,y) + other.Get(x,y));
                }
            }
        }

        public void Multiply(double m)
        {
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; ++y)
                {
                    Set(x, y, Get(x, y) * m);
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    yield return elements[i, j];
                }
            }
        }
    }
}
