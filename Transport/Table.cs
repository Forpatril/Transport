using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transport
{
    class Table
    {
        public Table(int[,] matr, int[] S, int[] D)
        {
            m = matr.GetLength(0);
            n = matr.GetLength(1);
            this.matrix = new Element[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    this.matrix[i, j] = new Element();
                    this.matrix[i, j].weight = matr[i, j];
                    this.matrix[i, j].value = 0;
                }
            }
            this.S = S;
            this.D = D;
        }

        public double[,] getMatrix()
        {
            double[,] matr = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matr[i, j] = this.matrix[i, j].value;
                }
            }
            return matr;
        }

        public Element[,] matrix { get; set; }
        public int[] S { get; set; }
        public int[] D { get; set; }
        public int m { get; set; }
        public int n { get; set; }
        public int iteration { get; set; }
        public List<Pair> path { get; set; }

    }

    class Element
    {
        public Element()
        {
            bas = false;
        }
        public double value { get; set; }
        public int weight { get; set; }
        public bool bas { get; set; }
    }

    class Pair
    {
        public Pair(int i, int j, double value = 0)
        {
            this.i = i;
            this.j = j;
            this.value = value;
        }

        public override string ToString()
        {
            return "(" + i.ToString() + ", " + j.ToString() + ")";
        }

        public double value { get; set; }
        public int i { get; set; }
        public int j { get; set; }
    }
}
