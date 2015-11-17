using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Transport
{
    public partial class Form1 : Form
    {
        private int step;
        private bool debug, preloaded;
        private string[] rbs = { "Ввести данные", "Использовать вариант 12" };
        private string[] btn = { "Начать", "Выбор", "Ввод размеров", "Ввод данных", "Расчёт", "Выход" };
        private string[] lbs = { "Источники", "Потребители" };
        public static System.Windows.Forms.RichTextBox Edit1;
        private int[,] matr;
        private double[,] res;
        private int[] S, D;
        private RadioButton[] rb;
        private TextBox[] tb;
        private Label[] lb;
        private CheckBox cb;
        private DataGridView dg;
        private Button bt;

        public Form1()
        {
            //Form fm =  ActiveForm;
            InitializeComponent();
            button1.index = 0;
            step = 0;
            button1.Text = btn[step];
            debug = false;
            preloaded = true;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void Selection()
        {
            button1.Left = 10;
            rb = new RadioButton[2];
            for (int i = 0; i < 2; i++)
            {
                rb[i] = new RadioButton();
                rb[i].Parent = ActiveForm;
                rb[i].Top = i * rb[i].Height;
                rb[i].Left = 20 + button1.Width;
                rb[i].Text = rbs[i];
                rb[i].Width = rbs[i].Length * 8;
                rb[i].index = i;
                rb[i].CheckedChanged += new EventHandler(PreloadedChanged);
            }
            ActiveForm.Width = 30 + button1.Width + rb[1].Width;
            button1.Text = btn[step + 1];
            step++;
        }

        private int VertBorder()
        {
            return ActiveForm.Width - ActiveForm.ClientSize.Width;
        }

        private int HorizBorder()
        {
            return ActiveForm.Height - ActiveForm.ClientSize.Height;
        }

        public void PreloadedChanged(object sender, System.EventArgs e)
        {
            RadioButton r = sender as RadioButton;
            preloaded = (r.index == 1);
        }

        private void EditKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !((int)e.KeyChar == 8) && !(e.KeyChar == ' '))
            {
                e.KeyChar = '\0';
            }
        }

        private void DataInput1()
        {
            foreach (RadioButton r in rb)
            {
                r.Dispose();
            }
            tb = new TextBox[2];
            lb = new Label[2];
            for (int i = 0; i < 2; i++)
            {
                tb[i] = new TextBox();
                tb[i].Parent = ActiveForm;
                tb[i].Left = 20 + button1.Width;
                tb[i].Height = button1.Height;
                tb[i].index = i;
                lb[i] = new Label();
                lb[i].Parent = ActiveForm;
                lb[i].Left = tb[i].Left;
                lb[i].Top = (i + 1) * 10 + (tb[i].Height + lb[i].Height) * i;
                tb[i].Top = (i + 1) * (10 + lb[i].Height) + tb[i].Height * i;
                lb[i].Text = lbs[i];
                tb[i].KeyPress += new KeyPressEventHandler(EditKeyPressed);
            }
            ActiveForm.Width = VertBorder() + 30 + button1.Width + tb[1].Width;
            ActiveForm.Height = HorizBorder() + 30 + 2 * (tb[1].Height + lb[1].Height);
            button1.Text = btn[step + 1];
            step++;
        }

        private int SplitString(ref int[] m, string s)
        {
            string[] ss = s.Split(' ');
            m = new int[ss.Length];
            if (s.Length == 0)
            {
                MessageBox.Show("Неверно введены данные");
                return 1;
            }
            for (int i = 0; i < ss.Length; i++)
            {
                    m[i] = int.Parse(ss[i]);
            }
            return 0;
        }

        private void DataInput2()
        {
            foreach (TextBox t in tb)
            {
                if (t.index == 1)
                    if (SplitString(ref D, t.Text) == 1)
                        return;
                    else
                        t.Dispose();
                else
                    if (SplitString(ref S, t.Text) == 1)
                        return;
                    else
                        t.Dispose();
            }
            foreach (Label l in lb)
                l.Dispose();
            dg = new DataGridView();
            int sl = S.Length, dl = D.Length;
            matr = new int[sl, dl];
            for (int i = 0; i < sl; i++)
            {
                for (int j = 0; j < dl; j++)
                {
                    matr[i, j] = 0;
                }
            }
            dg.Left = 10;
            dg.Top = 10;
            dg.ColumnHeadersVisible = false;
            dg.RowHeadersVisible = false;
            dg.RowCount = sl + 1;
            dg.ColumnCount = dl;
            for (int i = 0; i < dg.ColumnCount; i++)
            {
                dg.Columns[i].Width = 25;
            }

            for (int i = 0; i < dg.RowCount; i++)
            {
                dg.Rows[i].Height = 25;
            }
            dg.AllowUserToAddRows = false;
            dg.Width = D.Length * dg.Columns[1].Width + 5;
            dg.Height = S.Length * dg.Rows[1].Height + 5;
            button1.Left = dg.Left + dg.Width + 10;
            ActiveForm.Width = button1.Left + button1.Width + 10 + VertBorder();
            ActiveForm.Height = dg.Top + dg.Height + 10 + HorizBorder();
            dg.KeyPress += new KeyPressEventHandler(EditKeyPressed);
            dg.Parent = ActiveForm;
            step++;
        }

        private int DataInput3()
        {
            for (int i = 0; i < dg.RowCount; i++)
            {
                for (int j = 0; j < dg.ColumnCount; j++)
                {
                    try
                    {
                        matr[i, j] = int.Parse(dg.Rows[i].Cells[j].Value.ToString());
                    }
                    catch (NullReferenceException e)
                    {
                        MessageBox.Show(e.Message);
                        return 1;
                    }
                }
            }
            dg.Dispose();
            return 0;
        }

        private void Debug(object sender, System.EventArgs e)
        {
            debug = !debug;
        }

        private void Run()
        {
            if (dg != null)
            {
                if (DataInput3() == 1)
                    return;
            }
            else
                foreach (RadioButton r in rb)
                {
                    r.Dispose();
                }
            Edit1 = new System.Windows.Forms.RichTextBox();
            Edit1.Width = 80 * 7;
            Edit1.Height = 25 * Edit1.Font.Height;
            Edit1.Parent = ActiveForm;
            Edit1.Multiline = true;
            button1.Left = Edit1.Left + Edit1.Width + 10;
            ActiveForm.Width = button1.Left + button1.Width + 10 + VertBorder();
            ActiveForm.Height = HorizBorder() + Edit1.Top + Edit1.Height;
            button1.Text = btn[step + 1];
            bt = new Button();
            bt.Parent = ActiveForm;
            bt.Text = "Заново";
            bt.index = 1;
            bt.Width = button1.Width;
            bt.Height = button1.Height;
            bt.Top = button1.Top + button1.Height + 10;
            bt.Left = button1.Left;
            bt.Click +=new EventHandler(button_click);
            cb = new CheckBox();
            cb.Parent = ActiveForm;
            cb.Left = button1.Left;
            cb.Top = bt.Top + bt.Height + 10;
            cb.Text = "Отладка";
            cb.CheckedChanged += new EventHandler(Debug);
            step++;
        }

        private void var12()
        {
            S = new int[] { 55, 95, 45 };
            D = new int[] { 40, 30, 85, 40 };
            matr = new int[,] { { 7, 0, 8, 6 }, { 3, 5, 1, 11 }, { 2, 4, 7, 8 } };
        }

        public static double f(double[,] des, int[,] matr)
        {
            double res = 0;
            for (int i = 0; i < des.GetLength(0); i++)
            {
                for (int j = 0; j < des.GetLength(0); j++)
                {
                    res += des[i, j] * (matr[i, j] != double.PositiveInfinity ? matr[i, j] : 0);
                }
            }
            return res;
        }

        private void Solve()
        {
            res = Simplex.solve(matr, S, D, debug);
            for (int i = 0; i < 80; i++)
                Console.Write("=");
            Console.WriteLine();
            Console.WriteLine("Ответ: ");
            Console.WriteLine();
            Simplex.printMatrix(res, 5);
            Console.WriteLine("F= " + f(res, matr));
            Console.WriteLine();
            Console.Pnt();
            step++;
            button1.Text = btn[step];
        }

        private void mainButton()
        {
            switch (step)
            {
                case 0: Selection(); break;
                case 1:
                    if (preloaded)
                    {
                        step += 2;
                        var12();
                        mainButton();
                    }
                    else
                        DataInput1();
                    break;
                case 2: DataInput2(); break;
                case 3: Run(); break;
                case 4: Solve(); break;
                case 5: Close(); break;
            }
        }

        private void Reload()
        {
            step = 0;
            Edit1.Dispose();
            cb.Dispose();
            button1.Text = btn[step];
            debug = false;
            preloaded = true;
            Console.ForegroundColor = ConsoleColor.Gray;
            ActiveForm.Width = 258;
            ActiveForm.Height = 77;
            button1.Top = 12;
            button1.Left = 83;
            bt.Dispose();
        }

        private void button_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.index)
            {
                case 0: mainButton(); break;
                case 1: Reload(); break;
            }
        }
    }
}
