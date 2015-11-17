using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transport
{
    class SWangle
    {
        public static List<Pair> setBas(Table table)
        {
            double[] S = new double[table.m], D = new double[table.n];
            List<Pair> pairs = new List<Pair>();
            int count = table.m + table.n - 1;

            table.S.CopyTo(S, 0);
            table.D.CopyTo(D, 0);

            for (int i = 0, j = 0; (i < table.m || j < table.n) && count > 0; count--)
            {
                pairs.Add(new Pair(i, j));
                table.matrix[i, j].bas = true;
                if ((table.matrix[i, j].value = (int)Math.Min(S[i], D[j])) == S[i])
                {
                    D[j] -= table.matrix[i, j].value;
                    i++;
                }
                else
                {
                    S[i] -= table.matrix[i, j].value;
                    j++;
                }
            }
            return pairs;

        }
    }
}
