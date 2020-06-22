using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ball
{
    public partial class FormEasy : Form
    {
        int x, y, speed;
        Ball singleBall;
        Ball[] myBalls;
        List<Ball> myThreadedBalls;
        Color[] colors = new Color[] { Color.Red, Color.Blue, Color.DarkGreen, Color.Yellow, Color.Cyan };

        private void FormEasy_Load(object sender, EventArgs e)
        {
            speed = 1;
            x = this.ClientSize.Width / 2;
            y = this.ClientSize.Height - 10;
            Random random = new Random();
            //myBall = new Ball(this, 20, x, 0, speed, speed, Color.BlueViolet);
            singleBall = new Ball(this);
            singleBall.radius = random.Next(1, 100);
            singleBall.x = random.Next(singleBall.radius, ClientSize.Width - singleBall.radius);
            singleBall.y = random.Next(singleBall.radius, ClientSize.Height - singleBall.radius);
            singleBall.xspeed = random.Next(-5, 5);
            singleBall.yspeed = random.Next(-5, 5);
            singleBall.color = colors[random.Next(0, 4)];

            myBalls = new Ball[random.Next(10, 100)];
            myThreadedBalls = new List<Ball>();

            for (int i = 0; i < myBalls.Length; i++)
            {
                myBalls[i] = new Ball(this);
                myBalls[i].radius = random.Next(1, 100);
                myBalls[i].x = random.Next(myBalls[i].radius, ClientSize.Width - myBalls[i].radius);
                myBalls[i].y = random.Next(myBalls[i].radius, ClientSize.Height - myBalls[i].radius);
                myBalls[i].xspeed = random.Next(-5, 5);
                myBalls[i].yspeed = random.Next(-5, 5);
                myBalls[i].color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                myThreadedBalls.Add(myBalls[i]);
                // Threaded Ball
                Thread tid1 = new Thread(new ThreadStart(myBalls[i].move));
                tid1.Start();
            }

            timer1.Interval = 100;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            y = y + speed;
            if (y < 0)
                speed = -speed;
            ///if y is less than 0 then we change direction}
            else if (y + 10 > this.ClientSize.Height)
                speed = -speed;
            //            singleBall.move();
            //            foreach (Ball myBall in myBalls)
            //            foreach (Ball myBall in myThreadedBalls)
            //                myBall.move();
            this.Invalidate();
        }

        public FormEasy()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void FormEasy_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.Blue, x, y, 10, 10);
            SolidBrush colorBrush = new SolidBrush(singleBall.color);
            e.Graphics.FillEllipse(colorBrush, singleBall.x, singleBall.y, singleBall.radius, singleBall.radius);
            //foreach (Ball myBall in myBalls) {
            foreach (Ball myBall in myThreadedBalls)
            {
                colorBrush = new SolidBrush(myBall.color);
                e.Graphics.FillEllipse(colorBrush, myBall.x, myBall.y, myBall.radius, myBall.radius);
            }
        }
    }

    class Ball
    {

        public int radius;
        public int x, y;
        public int xspeed, yspeed;
        public Color color;
        public Boolean threadStop = false;
        public Boolean threadPause = false;
        Control form;

        public Ball(Control form)
        {
            this.form = form;
        }

        public void setForm(Control form)
        {
            this.form = form;
        }

        public Ball(Control form, int radius, int x, int y, int xspeed, int yspeed, Color color)
        {
            this.radius = radius;
            this.x = x;
            this.y = y;
            this.xspeed = xspeed;
            this.yspeed = yspeed;
            this.color = color;
            this.form = form;
        }

        public void move()
        {
            threadStop = false;
            threadPause = false;
            while (!threadStop)
            {
                if (!threadPause)
                {
                    y = y + yspeed;
                    if (y < 0)
                        yspeed = -yspeed;
                    ///if y is less than 0 then we change direction}
                    else if (y + radius > form.ClientSize.Height)
                        yspeed = -yspeed;

                    x = x + xspeed;

                    if (x < 0)
                        xspeed = -xspeed;
                    ///if y is less than 0 then we change direction}
                    else if (x + radius > form.ClientSize.Width)
                        xspeed = -xspeed;
                }
                Thread.Sleep(50);
            }
        }
    }
}
