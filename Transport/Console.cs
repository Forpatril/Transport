using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Transport
{
    class Console : Form1
    {

        public static void Write(params object[] s)
        {
            if (s.Length > 0)
            {
                int StartPosition = Edit1.Text.Length;
                if (StartPosition > 0 && Edit1.Text[StartPosition - 1] == '\n')
                    StartPosition++;
                if (s.Length == 1)
                    Edit1.Text += s[0] as string;
                else
                {
                    string s1 = String.Format(s[0] as string, s[1]);
                    Edit1.Text += s1;
                }
                if (ForegroundColor != ConsoleColor.Gray)
                    sel.Add(new Selection(StartPosition, Edit1.Text.Length - StartPosition));
            }
        }

        public static void WriteLine(params object[] s)
        {
            if (s.Length > 0)
            {
                int StartPosition = Edit1.Text.Length;
                if (StartPosition > 0 && Edit1.Text[StartPosition - 1] == '\n')
                    StartPosition++;
                if (s.Length == 1)
                    Edit1.Text += s[0] as string;
                else
                {
                    string s1 = String.Format(s[0] as string, s[1]);
                    Edit1.Text += s1;
                }
                if (ForegroundColor != ConsoleColor.Gray)
                    sel.Add(new Selection(StartPosition, Edit1.Text.Length - StartPosition));
            }
            Edit1.Text += '\n';
        }

        public static void Pnt()
        {
            foreach (Selection s in sel)
            {
                Edit1.Select(s.start, s.count);
                Edit1.SelectionColor = Color.Red;
                Edit1.Select(0, 0);
            }
            sel.RemoveRange(0, sel.Count);
        }

        public static System.ConsoleColor ForegroundColor;
        private struct Selection
        {
            public int start, count;
            public Selection(int s, int c)
            {
                start = s;
                count = c;
            }
        }
        private static List<Selection> sel = new List<Selection>();
    }
}
