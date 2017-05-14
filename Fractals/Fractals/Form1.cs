using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractals
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Pen pen;

        private const byte N = 255; //num of iterations (Mandel(Buddha)brot and Julia set)

        private double xMin = -2;
        private double xMax = 1;
        private double yMin = -1.5;
        private double yMax = 1.5;

        public struct complex_num//there's Sysytem.Numerics in .NET but it doesnt work for me as intended
        {
            public double real, imaginary;
        }
        /* public complex_num CSum(complex_num A, complex_num B)
         {
             complex_num sum = new complex_num();
             sum.real = A.real + B.real;
             sum.imaginary = A.imaginary + B.imaginary;
             return sum;
         }
         public complex_num CRatio(complex_num A, complex_num B)
         {
             complex_num ratio = new complex_num();
             ratio.real = A.real * B.real - A.imaginary * B.imaginary;
             ratio.imaginary = A.real * B.imaginary + A.imaginary * B.real;
             return ratio;
         }
         public complex_num CSquare(complex_num A)
         {
             return CRatio(A, A);
         }*/
        public double CModulo(complex_num A)//abstract of complex number
        {
            return Math.Sqrt((A.real * A.real) + (A.imaginary * A.imaginary));
        }

        public complex_num g_g(complex_num z, complex_num p)//z[n+1] = z[n]^2 + p
        {
            complex_num f = new complex_num();
            f.real = z.real * z.real - z.imaginary * z.imaginary + p.real;
            f.imaginary = 2 * z.real * z.imaginary + p.imaginary;
            return f;
        }



        void DrawSierpinskisLane(double X, double Y, double W, int N, PictureBox pb)
        {
            pen = new Pen(Color.Black);
            
            Rectangle r = new Rectangle((int)X, (int)Y, (int)W, (int)W);
            double D = W / 3;

            g = pb.CreateGraphics();
            g.DrawRectangle(pen, r);
            if (N < int.Parse(textBox1.Text))
            {
                DrawSierpinskisLane(X, Y, D, N + 1, pb);
                DrawSierpinskisLane(X + D, Y, D, N + 1, pb);
                DrawSierpinskisLane(X + 2 * D, Y, D, N + 1, pb);

                DrawSierpinskisLane(X, Y + D, D, N + 1, pb);
                DrawSierpinskisLane(X + 2 * D, Y + D, D, N + 1, pb);

                DrawSierpinskisLane(X, Y + 2 * D, D, N + 1, pb);
                DrawSierpinskisLane(X + D, Y + 2 * D, D, N + 1, pb);
                DrawSierpinskisLane(X + 2 * D, Y + 2 * D, D, N + 1, pb);
            }
            pen.Dispose();
            g.Dispose();
        }

        void drawSierpinskisTriangle(float x, float y, float d, int n, PictureBox pb)
        {

            pen = new Pen(Color.Black);
           
            g = pb.CreateGraphics();

            g.DrawLine(pen, x, y, x + d, y);
            g.DrawLine(pen, x + d, y, x + d / 2, (float)(y - d * Math.Sqrt(3) / 2));
            g.DrawLine(pen, x + d / 2, (float)(y - d * Math.Sqrt(3) / 2), x, y);

            if (n < int.Parse(textBox1.Text))
            {
                drawSierpinskisTriangle(x, y, d / 2, n + 1, pb);
                drawSierpinskisTriangle(x + d / 2, y, d / 2, n + 1, pb);
                drawSierpinskisTriangle(x + d / 4, (float)(y - Math.Sqrt(3) * d / 4), d / 2, n + 1, pb);
            }
            pen.Dispose();
            g.Dispose();
        }

        void drawKochsCurve(float x1, float y1, float x2, float y2, int n, PictureBox pb)
        {
            
            float dx = x2 - x1,
                  dy = y2 - y1;
            pen = new Pen(Color.Black);
            
            g = pb.CreateGraphics();

            if (n == int.Parse(textBox1.Text))
            {
                g.DrawLine(pen, x1, y1, x2, y2);
            }
            else
            {
                drawKochsCurve(x1, y1, x1 + dx / 3, y1 + dy / 3, n + 1, pb);
                drawKochsCurve(x1 + dx * 2 / 3, y1 + dy * 2 / 3, x2, y2, n + 1, pb);
                drawKochsCurve(x1 + dx / 3, y1 + dy / 3, (float)((x1 + x2) / 2 +
                    Math.Sqrt(3) / 6 * dy),
                    (float)((y1 + y2) / 2 - (Math.Sqrt(3) / 6) * dx), n + 1, pb);
                drawKochsCurve((float)((x1 + x2) / 2 + (Math.Sqrt(3) / 6) * dy),
                    (float)((y1 + y2) / 2 - Math.Sqrt(3) / 6 * dx),
                    x1 + dx * 2 / 3, y1 + dy * 2 / 3, n + 1, pb);
            }
            pen.Dispose();
            g.Dispose();
        }

        void drawCantorSet(int left, int right, int n, PictureBox pb)
        {
            pen = new Pen(Color.Black);
            g = pb.CreateGraphics();
            
            Rectangle r = new Rectangle(left, pb.Height - 10 - n * 5,
                                        right, 4);

            g.DrawRectangle(pen, r);
            int d;
            if (n < int.Parse(textBox1.Text) && (left < right))
            {
                d = (right - left) / 3;
                drawCantorSet(left, left + d, n + 1, pb);
                drawCantorSet(right - d, right - (right - d), n + 1, pb);// here's a problem
            }

            pen.Dispose();
            g.Dispose();
        }

        void drawMandelbrotSet(PictureBox pb)
        {
            int I;
            double dX, dY;

            complex_num p;
            complex_num z;

            pb.Image = new Bitmap(pb.Width, pb.Height);
            g = pb.CreateGraphics();

            dX = (xMax - xMin) / pb.Width;
            dY = (yMax - yMin) / pb.Height;

            for (int i = 0; i < pictureBox1.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Height; j++)
                {
                    p.real = xMin + i * dX;
                    p.imaginary = yMin + j * dY;

                    z.real = 0;
                    z.imaginary = 0;

                    I = 0;
                    do
                    {
                        z = g_g(z, p);
                        I++;
                    } while ((I < N) && (CModulo(z) < 2));

                    if (I >= N)
                        ((Bitmap)pb.Image).SetPixel(i, j, Color.White);
                    else
                        ((Bitmap)pb.Image).SetPixel(i, j, Color.Black);
                }
            }
            g.Dispose();
        }


        public complex_num c_julia_param;

        void drawJuliaSet(PictureBox pb)
        {  
            int I;
            double dX, dY;

            complex_num p;
            complex_num c;
            complex_num z;

            pb.Image = new Bitmap(pb.Width, pb.Height);
            g = pb.CreateGraphics();

            dX = (xMax - xMin) / pb.Width;
            dY = (yMax - yMin) / pb.Height;
            
            for (int i = 0; i < pictureBox1.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Height; j++)
                {
                    p.real = xMin + i * dX;
                    p.imaginary = yMin + j * dY;

                    c.real = c_julia_param.real;
                    c.imaginary = c_julia_param.imaginary;

                    z = p;

                    I = 0;
                    do
                    {
                        z = g_g(z, c);
                        I++;
                    } while ((I < N) && (CModulo(z) < 2));

                    if (I >= N)
                        ((Bitmap)pb.Image).SetPixel(i, j, Color.White);
                    else
                        ((Bitmap)pb.Image).SetPixel(i, j, Color.Black);
                }
            }
            g.Dispose();
        }

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            pictureBox1.Refresh();
            if (radioButton1.Checked)
                drawSierpinskisTriangle(0, (pictureBox1.Height - 10), pictureBox1.Width, 1, pictureBox1);
            if (radioButton2.Checked)
                DrawSierpinskisLane(0, 0, pictureBox1.Width-10, 1, pictureBox1);
            if (radioButton3.Checked)
                drawCantorSet(0, pictureBox1.Width - 10, 1, pictureBox1);
            if (radioButton4.Checked)
                drawKochsCurve(10, pictureBox1.Height - 10, pictureBox1.Width - 10, pictureBox1.Height - 10, 1, pictureBox1);
            if (radioButton5.Checked)
                drawMandelbrotSet(pictureBox1);
            if (radioButton7.Checked)
                drawJuliaSet(pictureBox1);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //button1_Click(sender, e);
            /*
            if (radioButton1.Checked)
                drawSierpinskisTriangle(0, (pictureBox1.Height - 10), pictureBox1.Width, 1, pictureBox1);
            if (radioButton2.Checked)
                DrawSierpinskisLane(0, 0, pictureBox1.Width - 10, 1, pictureBox1);
            if (radioButton3.Checked)
                drawCantorSet(0, pictureBox1.Width - 10, 1, pictureBox1);
            if (radioButton4.Checked)
                drawKochsCurve(10, pictureBox1.Height - 10, pictureBox1.Width - 10, pictureBox1.Height - 10, 1, pictureBox1);
            if (radioButton5.Checked)
                drawMandelbrotSet(pictureBox1);*/
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Close();
        }

        private void button2_Click(object sender, EventArgs e)//<--------
        {
            SaveFileDialog dialog = new SaveFileDialog();
           /* PictureBox pp = new PictureBox();
            pp.Width = 1366;
            pp.Height = 768;
            pp.Image = new Bitmap(pp.Width, pp.Height);//aha...
            drawMandelbrotSet(pp);*/

            dialog.Filter = "bmp|*.bmp";
            dialog.ShowDialog();
            if (dialog.FileName != "")
                // bm.Save(dialog.FileName);
                pictureBox1.Image.Save(dialog.FileName);
            /*SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "bmp|*.bmp";
            dialog.ShowDialog();
            if (dialog.FileName != "")
                // bm.Save(dialog.FileName);*/
            /*  PictureBox pb = new PictureBox();
              pb.Image = new Bitmap("C:\\Users\\pklos\\Desktop\\cos.bmp");
                  pb.Image.Save("C:\\Users\\pklos\\Desktop\\cos.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        */
        }

        private void radioButton7_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                c_julia_param.real = -0.123;
                c_julia_param.imaginary = 0.745;
            }else if (comboBox1.SelectedIndex == 1)
            {
                c_julia_param.real = -0.75;
                c_julia_param.imaginary = 0;
            }else if (comboBox1.SelectedIndex == 2)
            {
                c_julia_param.real = -0.390541;
                c_julia_param.imaginary = -0.586788;
            }else if (comboBox1.SelectedIndex == 3)
            {
                c_julia_param.real = 0;
                c_julia_param.imaginary = 1;
            }
        }
    }
}
