using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;


namespace Fractals
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Pen pen;

        private const byte N = 255; //num of iterations
        //public double xMin, xMax, yMin, yMax;//coordinates min and max

        /*  void setBitmapProperties(int resW, int resH)
          {
               // this.resW = resW;                                   
              // this.resH = resH;                                    
             //  bm = new Bitmap(resW, resH);                                    

               }*/
        /*public struct Complex
        {
            public double Re, Im;//real part, imaginary part
        }
        public Complex CSum(Complex A, Complex B)
        {
            Complex sum = new Complex();
            sum.Re = A.Re + B.Re;
            sum.Im = A.Im + B.Im;
            return sum;
        }

        public Complex CRatio(Complex A, Complex B)
        {
            Complex ratio = new Complex();
            ratio.Re = A.Re * B.Re - A.Im * B.Im;
            ratio.Im = A.Re * B.Im + A.Im * B.Re;
            return ratio;    
        }

        public Complex CSquare(Complex A)
        {
            return CRatio(A, A);
        }

        public double CModulo(Complex A)
        {
            return Math.Sqrt(A.Re * A.Re + A.Im * A.Im);
        }*/

    
        
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
                drawCantorSet(right - d , right-(right- d), n + 1, pb);// tu jest problem
            }
            
            pen.Dispose();
            g.Dispose();
        }

        void drawMandelbrotSet(PictureBox pb)
        {
            double xMin = -2; double xMax = 1;
            double yMin = -1.5; double yMax = 1.5;
            int I;
            double dX, dY;

            Complex c;
            Complex p;
           
            pb.Image = new Bitmap(pb.Width, pb.Height);
            g = pb.CreateGraphics();

            dX = (xMax - xMin) / pb.Width;
            dY = (yMax - yMin) / pb.Height;

            for (int i = 0; i < pb.Width; i++)
            {
                for (int j =0; j < pb.Height; j++)
                {
                    c = new Complex(xMin + i * dX, yMin + j * dY);
                    p = new Complex(0, 0);

                    I = 0;
                    do
                    {
                        p = Complex.Add(Complex.Pow(p,2), c);
                        I++;
                    } while((I>N) || (Complex.Abs(p) >= 2));
                   
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pictureBox1.Paint += pictureBox1_Paint;
            
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
        }

        private void resolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {/*
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

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Bitmap bm = new Bitmap(pictureBox1.BackgroundImage);
            SaveFileDialog dialog = new SaveFileDialog();
            PictureBox pb1 = new PictureBox();
            pb1.Image = new Bitmap(pb1.Width, pb1.Height);//aha...
            pb1.Width = 1366;
            pb1.Height = 768;
            DrawSierpinskisLane(0, 0, pb1.Width, 1, pb1);
            dialog.Filter = "bmp|*.bmp";
            dialog.ShowDialog();
            if (dialog.FileName != "")       // JAK TO ZROBIC???? ZEBY NIE BYLO NULL???
                                             // bm.Save(dialog.FileName);
                pb1.Image.Save(dialog.FileName);
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
    }
}
