using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace GraficulFunctiei
{
    public partial class Form1 : Form
    {

        int L, H;
        string functie_n = null;
        string functie_p = null;
        string[] operatori = { "+", "-", "*", "/", "^", "sqrt", "log", "sin", "cos", "tg", "ctg" };
        string[] operanzi = { "x", "e", "pi" };
        bool corect;
        double xmax0 = 0, ymax0 = 0;
        int cursor;
        Graphics axe, grafic;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            functie.SelectionStart = 1;
            functie.Select();
        }

        public Form1()
        {
            InitializeComponent();
            L = panel1.Width;
            H = panel1.Height;
            axe = panel1.CreateGraphics();
            grafic = panel1.CreateGraphics();
        }

        private double f(double x)
        {
            string[] termeni = functie_p.Split(' ');
            int varf = 0;
            double[] stiva = new double[1000];
            double operand1, operand2;
            foreach (string termen in termeni)
            {
                bool _operand = false;
                bool _operator = false;
                if (termen == "+" || termen == "-" || termen == "*" || termen == "/" || termen == "^" || termen == "sqrt" || termen == "log" || termen == "sin" || termen == "cos" || termen == "tg" || termen == "ctg" || termen == "(" || termen == ")")
                    _operator = true;
                else
                    if (termen != " " && termen != "")
                    _operand = true;
                if (_operand)
                {
                    if (termen == "x")
                        stiva[++varf] = x;
                    else
                    if (termen == "PI")
                        stiva[++varf] = Math.PI;
                    else
                    if (termen == "E")
                        stiva[++varf] = Math.E;
                    else
                        try
                        {
                            if (corect)
                                stiva[++varf] = Convert.ToDouble(termen);
                        }
                        catch (System.Exception)
                        {
                            MessageBox.Show("Functie scrisa incorect", "Eroare");
                            corect = false;
                            return 0;
                        }
                }
                if (_operator)
                {
                    if (termen != "sin" && termen != "cos" && termen != "tg" && termen != "ctg" && termen != "sqrt")
                    {
                        operand2 = stiva[varf];
                        varf--;
                        operand1 = stiva[varf];
                    }
                    else
                    {
                        operand1 = stiva[varf];
                        operand2 = 0;
                    }
                    switch (termen)
                    {
                        case "+":
                            stiva[varf] = operand1 + operand2;
                            break;
                        case "-":
                            stiva[varf] = operand1 - operand2;
                            break;
                        case "*":
                            stiva[varf] = operand1 * operand2;
                            break;
                        case "/":
                            stiva[varf] = operand1 / operand2;
                            break;
                        case "^":
                            stiva[varf] = Math.Pow(operand1, operand2);
                            break;
                        case "sqrt":
                            stiva[varf] = Math.Sqrt(operand1);
                            break;
                        case "log":
                            stiva[varf] = Math.Log10(operand2) / Math.Log10(operand1);
                            break;
                        case "sin":
                            stiva[varf] = Math.Sin(operand1);
                            break;
                        case "cos":
                            stiva[varf] = Math.Cos(operand1);
                            break;
                        case "tg":
                            stiva[varf] = Math.Sin(operand1) / Math.Cos(operand1);
                            break;
                        case "ctg":
                            stiva[varf] = Math.Cos(operand1) / Math.Sin(operand1);
                            break;

                    }
                }
            }
            return stiva[varf];
        }

        void desen_functie()
        {
            double x, y, y0;
            double xmax, ymax, u;
            int x1, y1, x2, y2;
            double prec = Convert.ToDouble(textBox1.Text);
            if (prec <= 0 || prec > 5000)
            {
                textBox1.Text = "1";
                prec = 1;
            }
            int patr = Convert.ToInt32(textBox2.Text);
            patr = 101 - patr;
            if (patr < 1 || patr > 100)
            {
                textBox2.Text = "100";
                patr = 1;
            }
            xmax = Convert.ToDouble(Xmax.Text) + 0.5 - (Math.Pow(Math.E, -1 * Convert.ToDouble(Xmax.Text))) / 2;
            ymax = Convert.ToDouble(Ymax.Text) + 0.5 - (Math.Pow(Math.E, -1 * Convert.ToDouble(Xmax.Text))) / 2;
            u = xmax * 2 / (L * prec);
            x = -xmax - u;
            y0 = f(x);
            Pen p = new Pen(Color.Blue,1);
            if(Convert.ToInt32(textBox3.Text)>=1 && Convert.ToInt32(textBox3.Text) <= 5)
            {
                p.Width = Convert.ToInt32(textBox3.Text);
            }
            switch(Convert.ToString(comboBox1.Text))
            {
                case "Black": 
                    p.Color = Color.Black;
                    break;
                case "Blue":
                    p.Color = Color.Blue;
                    break;
                case "Red":
                    p.Color = Color.Red;
                    break;
                case "Green":
                    p.Color = Color.Green;
                    break;
                case "Yellow":
                    p.Color = Color.Yellow;
                    break;
            }
            progressBar1.Value = 1;
            progressBar1.Maximum = (int)(L * prec);
            progressBar1.Minimum = 0;
            progressBar1.Step = 50;
            for (int i = 1; i <= L*prec; i++)
            {
                if (i % 50 == 0)
                    progressBar1.PerformStep();
                x = x + u;
                y = f(x);
                if (!Double.IsNaN(y) && !Double.IsNaN(y0))
                {
                    x1 = (int)Math.Round(Convert.ToDouble((i - 1) / prec));
                    y1 = ((int)Math.Round((L / 2 - ((L * y0) / (ymax * 2))))) / patr * patr;
                    x2 = (int)Math.Round(Convert.ToDouble(i / prec));
                    y2 = ((int)Math.Round((L / 2 - ((L * y) / (ymax * 2))))) / patr * patr;
                    Point p1 = new Point(x1, y1);
                    Point p2 = new Point(x2, y2);
                    grafic.DrawLine(p, p1, p2);
                }
                y0 = y;
            }
            progressBar1.PerformStep();
            /*
            double[] y = new double[501];
            double[] x = new double[501];
            double xmax, ymax, u;
            int V = 5 - Convert.ToInt32(viteza.Text);
            xmax = Convert.ToDouble(Xmax.Text) + 0.5 - (Math.Pow(Math.E, -1 * Convert.ToDouble(Xmax.Text))) / 2;
            ymax = Convert.ToDouble(Ymax.Text) + 0.5 - (Math.Pow(Math.E, -1 * Convert.ToDouble(Xmax.Text))) / 2;
            u = xmax * 2 / L;
            x[0] = -xmax - u;
            for (int i = 1; i <= L; i++)
            {
                x[i] = x[i - 1] + u;
                y[i] = f(x[i]);
            }
            double x1, y1, x2, y2;
            Pen p = new Pen(Color.Blue, 2);
            for (int i = 2; i <= L; i++)
            {
                if (!Double.IsNaN(y[i]) && !Double.IsNaN(y[i - 1]))
                {
                    System.Threading.Thread.Sleep(1);
                    x1 = i - 1;
                    y1 = L / 2 - ((L * y[i - 1]) / (ymax * 2));
                    x2 = i;
                    y2 = L / 2 - ((L * y[i]) / (ymax * 2));
                    Point p1 = new Point((int)x1, (int)y1);
                    Point p2 = new Point((int)x2, (int)y2);
                    grafic.DrawLine(p, p1, p2);
                }
            }

            */
        }

        int desen_unitati(Pen p, double xmax, double ymax)
        {
            Point p1, p2;
            int ux, uy, i, x, y;
            try
            {
                ux = Convert.ToInt32(L / 2 / xmax);
                uy = Convert.ToInt32(L / 2 / ymax);
            }
            catch(System.Exception)
            {
                return 0;
            }
            x = L / 2;
            y = H / 2;
            for (i = 1; i <= Math.Floor(xmax) + 1; i++)
            {
                x = x + ux;
                p1 = new Point(x, H / 2 - Convert.ToInt32(1 / xmax * 25));
                p2 = new Point(x, H / 2 + Convert.ToInt32(1 / xmax * 30));
                axe.DrawLine(p, p1, p2);
                p1 = new Point(L - x, H / 2 - Convert.ToInt32(1 / xmax * 25));
                p2 = new Point(L - x, H / 2 + Convert.ToInt32(1 / xmax * 30));
                axe.DrawLine(p, p1, p2);
            }
            for (i = 1; i <= Math.Floor(ymax) + 1; i++)
            {
                y = y + uy;
                p1 = new Point(L / 2 - Convert.ToInt32(1 / ymax * 25), y);
                p2 = new Point(L / 2 + Convert.ToInt32(1 / ymax * 25), y);
                axe.DrawLine(p, p1, p2);
                p1 = new Point(L / 2 - Convert.ToInt32(1 / ymax * 25), H - y);
                p2 = new Point(L / 2 + Convert.ToInt32(1 / ymax * 25), H - y);
                axe.DrawLine(p, p1, p2);
            }
            xmax0 = xmax;
            ymax0 = ymax;
            return 0;
        }

        void desen_axe(Pen p, double xmax, double ymax)
        {
            int x1, y1, x2, y2;
            x1 = 0; y1 = H / 2;
            x2 = L; y2 = H / 2;
            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y2);
            axe.DrawLine(p, p1, p2);
            x1 = L / 2; y1 = 0;
            x2 = L / 2; y2 = H;
            p1 = new Point(x1, y1);
            p2 = new Point(x2, y2);
            axe.DrawLine(p, p1, p2);
            desen_unitati(p,xmax,ymax);
        }

        void transformare1()
        {
            int i;
            int n = functie_n.Length;
            functie_n = functie_n.Insert(0, "("); n += 1;
            functie_n = functie_n.Insert(n, ")"); n += 1;
            char[] _functie = new char[1000];
            _functie = functie_n.ToCharArray();
            functie_n = new string(_functie);
            for (i = 1; i < n; i++)
            {
                if (_functie[i - 1] >= 48 && _functie[i - 1] <= 57)
                {
                    if (_functie[i] != ' ' && (_functie[i] < 41 || _functie[i] > 57) && _functie[i] != '.' && _functie[i] != '^')
                    {
                        functie_n = new string(_functie);
                        functie_n = functie_n.Insert(i, "*");
                        n++;
                        _functie = functie_n.ToCharArray();
                    }
                }
                if ((_functie[i - 1] == 'x' || (_functie[i - 1] == 'i' && _functie[i - 2] != 's') || _functie[i - 1] == 'e' || _functie[i - 1] == ')') && _functie[i - 1] != ' ')
                {
                    if (_functie[i] != ' ' && (_functie[i] < 41 || _functie[i] > 47) && _functie[i] != '.' && _functie[i] != '^')
                    {
                        functie_n = new string(_functie);
                        functie_n = functie_n.Insert(i, "*");
                        n++;
                        _functie = functie_n.ToCharArray();
                    }
                }
            }
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("sin",i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i+4, " ");
                    i += 5;
                    n += 2;
                }
            } // sin
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("cos", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 4, " ");
                    i += 5;
                    n += 2;
                }
            } // cos
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("tg", i);
                if (i>=1 && functie_n.IndexOf("ctg", i - 1) == i - 1)
                    i++;
                else
                    if (i != -1)
                    {
                        functie_n = functie_n.Insert(i, " ");
                        functie_n = functie_n.Insert(i + 3, " ");
                        i += 4;
                        n += 2;
                    }
            } // tg
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("ctg", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 4, " ");
                    i += 5;
                    n += 2;
                }
            } // ctg
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("log", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 4, " ");
                    i += 5;
                    n += 2;
                }
            } // log
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("sqrt", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 5, " ");
                    i += 6;
                    n += 2;
                }
            } // sqrt
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("^", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 2, " ");
                    i += 3;
                    n += 2;
                }
            } // ^
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("+", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 2, " ");
                    i += 3;
                    n += 2;
                }
            } // +
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("*", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 2, " ");
                    i += 3;
                    n += 2;
                }
            } // *
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("/", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 2, " ");
                    i += 3;
                    n += 2;
                }
            } // /
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("(", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 2, " ");
                    i += 3;
                    n += 2;
                }
            } // (
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf(")", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 2, " ");
                    i += 3;
                    n += 2;
                }
            } // )
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("x", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 2, " ");
                    i += 3;
                    n += 2;
                }
            } // x
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("pi", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 3, " ");
                    i += 4;
                    n += 2;
                }
            } // pi
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("e", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i, " ");
                    functie_n = functie_n.Insert(i + 2, " ");
                    i += 3;
                    n += 2;
                }
            } // e
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("-", i);
                if (i != -1)
                {
                    functie_n = functie_n.Insert(i + 1, "1 * ");
                    functie_n = functie_n.Insert(i, " ");
                    _functie = functie_n.ToCharArray();

                    if (_functie[i - 2] != '(')
                    {
                        functie_n = new string(_functie);
                        functie_n = functie_n.Insert(i + 2, " ");
                    }
                    i += 6;
                    n += 6;
                }
            } // -
            i = 0; while (i != -1)
            {
                i = functie_n.IndexOf("  ", 0);
                if (i != -1)
                {
                    functie_n = functie_n.Remove(i, 1);
                    n--;
                }
            } // "  "
        }

        void transformare2()
        {
            string[] termeni = functie_n.Split(' ');
            string[] stiva = new string[100];
            int varf = 0;
            foreach (string termen in termeni)
            {
                bool _operator = false;
                bool _operand = false;
                int i;
                if (termen == "+" || termen == "-" || termen == "*" || termen == "/" || termen == "^" || termen == "sqrt" || termen == "log" || termen == "sin" || termen == "cos" || termen == "tg" || termen == "ctg" || termen == "(" || termen == ")")
                    _operator = true;
                else
                    if (termen != " " && termen != "")
                        _operand = true;
                if (_operand)
                {
                    if (termen == "e" || termen == "E")
                        functie_p += "E";
                    else
                    if (termen == "pi" || termen == "PI")
                        functie_p += "PI";
                    else
                        functie_p += termen;
                    functie_p += " ";
                }
                if (_operator)
                {
                    stiva[++varf] = termen;

                    if (termen == ")")
                    {
                        varf--;
                        while (varf > 0 && stiva[varf] != "(")
                        {
                            functie_p += stiva[varf--];
                            functie_p += " ";
                        }
                        varf--;
                    }

                    if (termen == "sqrt" || termen == "log")
                    {
                        varf--;
                        i = varf;
                        while (i > 0 && (stiva[i] == "sin" || stiva[i] == "cos" || stiva[i] == "tg" || stiva[i] == "ctg" || stiva[i] == "^"))
                        {
                            functie_p += stiva[i--];
                            functie_p += " ";
                        }
                        varf = i + 1;
                        stiva[varf] = termen;
                    }

                    if (termen == "*" || termen == "/")
                    {
                        varf--;
                        i = varf;
                        while (i > 0 && (stiva[i] == "sin" || stiva[i] == "cos" || stiva[i] == "tg" || stiva[i] == "ctg" || stiva[i] == "^" || stiva[i] == "sqrt" || stiva[i] == "log"))
                        {
                            functie_p += stiva[i--];
                            functie_p += " ";
                        }
                        varf = i + 1;
                        stiva[varf] = termen;
                    }

                    if (termen == "+" || termen == "-")
                    {
                        varf--;
                        i = varf;
                        while (i > 0 && (stiva[i] == "sin" || stiva[i] == "cos" || stiva[i] == "tg" || stiva[i] == "ctg" || stiva[i] == "^" || stiva[i] == "sqrt" || stiva[i] == "log" || stiva[i] == "*" || stiva[i] == "/"))
                        {
                            functie_p += stiva[i--];
                            functie_p += " ";
                        }
                        varf = i + 1;
                        stiva[varf] = termen;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            functie_n = functie.Text;
            corect = true;
            functie_p = "";
            transformare1();
            transformare2();
            desen_functie();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double xmax = Convert.ToDouble(Xmax.Text) + 0.5 - (Math.Pow(Math.E, -1 * Convert.ToDouble(Xmax.Text))) / 2;
            double ymax = Convert.ToDouble(Ymax.Text) + 0.5 - (Math.Pow(Math.E, -1 * Convert.ToDouble(Xmax.Text))) / 2;
            Pen p = new Pen(Color.White);
            desen_axe(p, xmax0, ymax0);
            p = new Pen(Color.Black);
            desen_axe(p,xmax,ymax);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pen p = new Pen(Color.White);
            desen_axe(p,xmax0,ymax0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            progressBar1.Value=0;
            grafic.Clear(Color.White);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button5.Text + " ()");
            functie.SelectionStart = cursor + (button5.Text + " ()").Length-1;
            functie.Select();
        }// sin
        private void button6_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button6.Text + " ()");
            functie.SelectionStart = cursor + (button6.Text + " ()").Length-1;
            functie.Select();
        }// cos
        private void button7_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button7.Text + " ()");
            functie.SelectionStart = cursor + (button7.Text + " ()").Length-1;
            functie.Select();
        }// tg
        private void button8_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button8.Text + " ()");
            functie.SelectionStart = cursor + (button8.Text + " ()").Length-1;
            functie.Select();
        }// ctg
        private void button32_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, "pi");
            functie.SelectionStart = cursor + ("pi").Length;
            functie.Select();
        }// pi
        private void button10_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button10.Text + " () ()");
            functie.SelectionStart = cursor + (button10.Text + " ()").Length-1;
            functie.Select();
        }// log
        private void button9_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button9.Text + " ()");
            functie.SelectionStart = cursor + (button9.Text + " ()").Length - 1;
            functie.Select();
        }// sqrt
        private void button11_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button11.Text);
            functie.SelectionStart = cursor + (button11.Text).Length;
            functie.Select();
        }// ^
        private void button29_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button29.Text);
            functie.SelectionStart = cursor + (button29.Text).Length;
            functie.Select();
        }// (
        private void button30_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button30.Text);
            functie.SelectionStart = cursor + (button30.Text).Length;
            functie.Select();
        }// )
        private void button15_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button15.Text);
            functie.SelectionStart = cursor + (button15.Text).Length;
            functie.Select();
        }// x
        private void button12_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button12.Text);
            functie.SelectionStart = cursor + (button12.Text).Length;
            functie.Select();
        }// e
        private void button18_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button18.Text);
            functie.SelectionStart = cursor + (button18.Text).Length;
            functie.Select();
        }// /
        private void button27_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button27.Text);
            functie.SelectionStart = cursor + (button27.Text).Length;
            functie.Select();
        }// 9
        private void button26_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button26.Text);
            functie.SelectionStart = cursor + (button26.Text).Length;
            functie.Select();
        }// 8
        private void button25_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button25.Text);
            functie.SelectionStart = cursor + (button25.Text).Length;
            functie.Select();
        }// 7
        private void button24_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button24.Text);
            functie.SelectionStart = cursor + (button24.Text).Length;
            functie.Select();
        }// 4
        private void button23_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button23.Text);
            functie.SelectionStart = cursor + (button23.Text).Length;
            functie.Select();
        }// 5
        private void button22_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button22.Text);
            functie.SelectionStart = cursor + (button22.Text).Length;
            functie.Select();
        }// 6
        private void button17_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button17.Text);
            functie.SelectionStart = cursor + (button17.Text).Length;
            functie.Select();
        }// *
        private void button14_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button14.Text);
            functie.SelectionStart = cursor + (button14.Text).Length;
            functie.Select();
        }// -
        private void button19_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button19.Text);
            functie.SelectionStart = cursor + (button19.Text).Length;
            functie.Select();
        }// 3
        private void button20_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button20.Text);
            functie.SelectionStart = cursor + (button20.Text).Length;
            functie.Select();
        }// 2
        private void button21_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button21.Text);
            functie.SelectionStart = cursor + (button21.Text).Length;
            functie.Select();
        }// 1
        private void button28_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button28.Text);
            functie.SelectionStart = cursor + (button28.Text).Length;
            functie.Select();
        }// 0
        private void button33_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, " ");
            functie.SelectionStart = cursor + (" ").Length;
            functie.Select();
        }// ' '
        private void button34_Click(object sender, EventArgs e)
        {
            functie.Text = "";
            functie.SelectionStart = 0;
            functie.Select();
        }// c
        private void button36_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            int s = functie.SelectionLength;
            if (s > 0)
                functie.SelectionLength = 0;
            else
                if (cursor > 0)
                    cursor--;
            functie.SelectionStart = cursor;
            functie.Select();

        }// <-
        private void button35_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            int s = functie.SelectionLength;
            if (s > 0)
            {
                functie.SelectionLength = 0;
                cursor = cursor + s;
            }
            else
                if (cursor < functie.TextLength)
                    cursor++;
            functie.SelectionStart = cursor;
            functie.Select();
        }// ->
        private void button31_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button31.Text);
            functie.SelectionStart = cursor + (button31.Text).Length;
            functie.Select();
        }// .
        private void button13_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            functie.Text = functie.Text.Remove(cursor, functie.SelectionLength);
            functie.Text = functie.Text.Insert(cursor, button13.Text);
            functie.SelectionStart = cursor + (button13.Text).Length;
            functie.Select();
        }// +
        private void button16_Click(object sender, EventArgs e)
        {
            cursor = functie.SelectionStart;
            int s= functie.SelectionLength;
            if (s == 0 && cursor != 0)
                functie.Text = functie.Text.Remove(--cursor, 1);
            else
                functie.Text = functie.Text.Remove(cursor, s);
            functie.SelectionStart = cursor;
            functie.Select();
        }// <
    }
}
