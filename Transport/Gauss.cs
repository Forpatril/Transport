using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transport
{
    class Gauss
    {
        const double eps = 1e-5;

        private static int[] GetRow(int[,] c, int row_n)
        {
            int[] row = new int[c.GetLength(1)];
            for (int i = 0; i < c.GetLength(1); i++)
                row[i] = c[row_n, i];
            return row;
        }

        static private void SwitchRows(double[,] c, int r_i, int r_j)
        {
            for (int i = 0; i < c.GetLength(1); i++)
            {
                double p = c[r_i, i];
                c[r_i, i] = c[r_j, i];
                c[r_j, i] = p;
            }
        }

        static private bool normal(double[,] m, List<int> list)
        {
            bool res = true;
            for (int i = 0; i < m.GetLength(0); i++)
                if (Math.Abs(m[i, i]) < eps)
                {
                    list.Add(i);
                    res = false;
                    for (int j = 0; j < m.GetLength(0) && !res; j++)
                        if (Math.Abs(m[j, i]) > eps && !list.Contains(j))
                        {
                            SwitchRows(m, i, j);
                            res = normal(m, list);
                            if (!res)
                                SwitchRows(m, i, j);
                        }
                }
            if (!res)
                list.Remove(list.Last());
            return res;
        }

        static public double[] solve(double[,] m)
        {
            int n = m.GetLength(0);
            normal(m, new List<int>());
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n + 1; j++)
                    m[i, j] /= m[i, i];
                for (int j = i + 1; j < n; j++)
                    for (int k = i + 1; k < n + 1; k++)
                        m[j, k] -= m[i, k] * m[j, i];
            }
            for (int i = n - 1; i > -1; i--)
                for (int j = n - 1; j > i; j--)
                    m[i, n] -= m[i, j] * m[j, n];
            double[] res = new double[n];
            for (int i = 0; i < n; i++)
                res[i] = m[i, n];
            return res;
        }
    }
}
