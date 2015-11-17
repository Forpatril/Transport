using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transport;


namespace Transport
{
    class Simplex
    {
        public static void printMatrix(Table table)
        {
            Console.WriteLine("");
            Console.WriteLine("-->" + table.iteration++);
            if (table.path != null)
            {
                Console.WriteLine("Цикл: ");
                string str = "";
                foreach (Pair p in table.path)
                {
                    Console.Write(str + p);
                    str = "=>";
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            for (int i = 0; i < table.m; i++)
            {
                for (int j = 0; j < table.n; j++)
                {
                    if (table.matrix[i, j].bas)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("{0,5:N0} ", (int)table.matrix[i, j].value);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void printMatrix(double[,] M, int k = 6)
        {
            string str = "";
            for (int i = 0; i < k - 1; i++)
            {
                str += " ";
            }
            str += "i ";
            char b = Convert.ToChar(236);
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    if (M[i, j] != double.PositiveInfinity)
                        Console.Write("{0," + k + ":N0} ", M[i, j]);
                    else
                        Console.Write(str);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //class Simplex
        static public double[,] solve(int[,] matr, int[] S, int[] D, bool debug)
        {
            Table table = new Table(matr, S, D);
            Pair pair = null;
            List<Pair> bas = SWangle.setBas(table);
            if (debug)
                printMatrix(table);
            double[] varBas = Gauss.solve(createSystem(table, bas));
            pair = findNewBas(table, varBas);
            while (pair.value < 0)
            {
                newBas(table, pair, bas);
                varBas = Gauss.solve(createSystem(table, bas));
                pair = findNewBas(table, varBas);
                if (debug)
                    printMatrix(table);
            }
            return table.getMatrix();
        }

        static private double[,] createSystem(Table table, List<Pair> pairs)
        {
            int size = table.m + table.n;
            double[,] system = newSystem(size);
            List<int> list = new List<int>();
            for (int i = 0; i < size; i++)
                list.Add(i);
            int ind = 0;
            foreach (Pair p in pairs)
            {
                if (system[p.i, p.i] == 0)
                    ind = p.i;
                else
                    if (system[table.m + p.j, table.m + p.j] == 0)
                        ind = table.m + p.j;
                    else
                        ind = list.Last();
                system[ind, size] = table.matrix[p.i, p.j].weight;
                system[ind, p.i] = 1;
                system[ind, table.m + p.j] = 1;
                list.Remove(ind);
            }
            ind = list.Last();
            system[ind, ind] = 1;
            system[ind, size] = 0;
            return system;
        }

        static private int getIndex(double[][] system, int size)
        {
            int i = 0;
            for (i = 0; i < size; i++)
                if (system[i][i] == 0)
                    break;
            return i;
        }

        static private double[,] newSystem(int size)
        {
            double[,] system = new double[size, size + 1];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size + 1; j++)
                    system[i, j] = 0;
            return system;
        }

        static private Pair findNewBas(Table table, double[] bas)
        {
            Pair pair = new Pair(0, 0);
            double d;
            for (int i = 0; i < table.m; i++)
                for (int j = 0; j < table.n; j++)
                    if (!table.matrix[i, j].bas)
                    {
                        d = table.matrix[i, j].weight - bas[i] - bas[table.m + j];
                        if (d < pair.value)
                        {
                            pair.value = d;
                            pair.i = i;
                            pair.j = j;
                        }
                    }
            return pair;
        }

        static private void newBas(Table table, Pair pair, List<Pair> bas)
        {
            List<Pair> path = new List<Pair>();
            pair.value = 0;
            bas.Add(pair);
            path.Add(pair);
            table.matrix[pair.i, pair.j].bas = true;
            moveTable(table, path, true);
            Pair p = pump(table, path);
            bas.RemoveAll(x => x.i == p.i && x.j == p.j);
            table.path = path;
        }

        static private bool moveTable(Table table, List<Pair> path, bool direct)
        {
            int i = path.Last().i, j = path.Last().j, add = 1;
            bool succ = false;

            if (path.First().i == path.Last().i && path.First().j == path.Last().j && path.Count > 1)
            {
                succ = true;
                path.Remove(path.Last());
            }

            while (!succ && add > -2)
            {
                while (!succ && ((direct && i + add < table.m && i + add > -1) || (!direct && j + add < table.n && j + add > -1)))
                {
                    if (direct)
                        i += add;
                    else
                        j += add;
                    if (table.matrix[i, j].bas)
                    {
                        path.Add(new Pair(i, j, table.matrix[i, j].value));
                        succ = moveTable(table, path, !direct);
                        if (!succ)
                            path.RemoveAll(x => x.i == i && x.j == j);
                    }
                }
                add -= 2;
                i = path.Last().i;
                j = path.Last().j;
            }
            return succ;

        }

        static private Pair pump(Table table, List<Pair> path)
        {
            double min = path.Where((x, ind) => ind % 2 == 1).Min(x => x.value);
            foreach (Pair p in path)
            {
                table.matrix[p.i, p.j].value += min;
                min *= -1;
            }
            Pair pair = path.FindLast(x => x.value == min);
            table.matrix[pair.i, pair.j].bas = false;
            return pair;
        }
        //end class Simplex

    }
}
