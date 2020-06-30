using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ball
{
    public partial class FormNormal : Form
    {
        int x, y, speed, timing, acc, disappear, timing2, score;
        Ball singleBall;
        Ball[] myBalls;
        Ball[] redBalls;
        List<Ball> myThreadedBalls;
        List<Ball> myThreadedRedBalls;
        Color[] colors = new Color[] { Color.Red, Color.Blue, Color.DarkGreen, Color.Yellow, Color.Cyan };

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text == (score).ToString())
                {
                    timer1.Enabled = false;
                    MessageBox.Show("成功! 耗時" + timing / 10 + "." + timing % 10 + "秒");
                    writeCSV();
                    this.Close();
                }
                else
                {
                    timing += 30;
                }

                textBox1.Focus();
            }
        }

        private void writeCSV()
        {
            string[] arr = new string[4];

            DateTime myDate = DateTime.Now;

            string myDateString = myDate.ToString("yyyy-MM-dd");
            string myTimeString = myDate.ToString("HH:mm:ss");

            arr[0] = myDateString;
            arr[1] = myTimeString;
            arr[2] = "普通";
            arr[3] = timing / 10 + "." + timing % 10;

            string fileName = "myRank.csv";
            try
            {
                FileInfo fi = new FileInfo(fileName);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                FileStream fsw = new FileStream(fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                StreamWriter sw = new StreamWriter(fsw, System.Text.Encoding.UTF8);
                string csvData = arr[0] + "," + arr[1] + "," + arr[2] + "," + arr[3];
                sw.WriteLine(csvData);
                sw.Close();
                fsw.Close();
            }
            catch
            {
            }
        }

        private void FormNormal_Load(object sender, EventArgs e)
        {
            score = 0;
            disappear = 0;
            timing = 0;
            timing2 = 1;
            acc = 0;
            speed = 1;
            x = this.ClientSize.Width / 2;
            y = this.ClientSize.Height - 10;
            Random random = new Random();

            singleBall = new Ball(this);
            singleBall.radius = 40;
            singleBall.x = ClientSize.Width / 2 - singleBall.radius;
            singleBall.y = 0;
            singleBall.color = Color.Black;

            myBalls = new Ball[Constants.BallNumber];
            myThreadedBalls = new List<Ball>();

            for (int i = 0; i < myBalls.Length; i++)
            {
                myBalls[i] = new Ball(this);
                if (i % 3 == 0)
                {
                    myBalls[i].radius = 10;
                    score += 1;
                }
                else if (i % 3 == 1)
                {
                    myBalls[i].radius = 20;
                    score += 2;
                }
                else
                {
                    myBalls[i].radius = 30;
                    score += 3;
                }
                myBalls[i].x = random.Next(myBalls[i].radius, ClientSize.Width - myBalls[i].radius);
                myBalls[i].y = random.Next(myBalls[i].radius, ClientSize.Height - myBalls[i].radius);
                int s = random.Next(-Constants.BallSpeed, Constants.BallSpeed),
                    d = random.Next(-Constants.BallSpeed, Constants.BallSpeed);
                if (s == 0) s = 3;
                if (d == 0) d = 3;
                myBalls[i].xspeed = s;
                myBalls[i].yspeed = d;
                myBalls[i].color = Color.Green;
            }

            int r = random.Next(3, 5);
            acc += r;
            for (int i = 0; i < r; i++)
            {
                myThreadedBalls.Add(myBalls[i]);
                Thread tid1 = new Thread(new ThreadStart(myBalls[i].move));
                tid1.Start();
            }


            redBalls = new Ball[Constants.BallNumber];//red
            myThreadedRedBalls = new List<Ball>();

            for (int i = 0; i < redBalls.Length; i++)
            {
                redBalls[i] = new Ball(this);
                if (i % 3 == 0)
                {
                    redBalls[i].radius = 10;
                }
                else if (i % 3 == 1)
                {
                    redBalls[i].radius = 20;
                }
                else
                {
                    redBalls[i].radius = 30;
                }
                redBalls[i].x = random.Next(redBalls[i].radius, ClientSize.Width - redBalls[i].radius);
                redBalls[i].y = random.Next(redBalls[i].radius, ClientSize.Height - redBalls[i].radius);
                int s = random.Next(-Constants.BallSpeed, Constants.BallSpeed),
                    d = random.Next(-Constants.BallSpeed, Constants.BallSpeed);
                if (s == 0) s = 3;
                if (d == 0) d = 3;
                redBalls[i].xspeed = s;
                redBalls[i].yspeed = d;
                if (i % 2 == 0)
                {
                    redBalls[i].color = Color.Red;
                }
                else
                {
                    redBalls[i].color = Color.Blue;
                }

            }

            int rs = random.Next(5, 8);
            for (int i = 0; i < rs; i++)
            {
                myThreadedRedBalls.Add(redBalls[i]);
                Thread tid2 = new Thread(new ThreadStart(redBalls[i].move));
                tid2.Start();
            }

            timer1.Interval = 100;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            y = y + speed;
            if (y < 0)
                speed = -speed;
            else if (y + 10 > this.ClientSize.Height)
                speed = -speed;


            if (timing++ % 10 == 0)
            {
                if (acc < Constants.BallNumber)
                {
                    myThreadedBalls.Add(myBalls[acc]);
                    Thread tid1 = new Thread(new ThreadStart(myBalls[acc].move));
                    tid1.Start();

                    myThreadedRedBalls.Add(redBalls[acc]);
                    Thread tid2 = new Thread(new ThreadStart(redBalls[acc].move));
                    tid2.Start();

                    acc++;
                }
            }

            if (timing2++ % 40 == 0)
            {
                if (disappear < Constants.BallNumber)
                {
                    myThreadedBalls.Remove(myBalls[disappear]);
                    disappear++;
                }
            }

            label1.Text = "時間 : " + timing / 10 + "." + timing % 10;

            this.Invalidate();
        }

        public FormNormal()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void FormEasy_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush colorBrush = new SolidBrush(singleBall.color);
            foreach (Ball myBall in myThreadedBalls)
            {
                colorBrush = new SolidBrush(myBall.color);
                e.Graphics.FillEllipse(colorBrush, myBall.x, myBall.y, myBall.radius, myBall.radius);
            }

            foreach (Ball redBall in myThreadedRedBalls)
            {
                colorBrush = new SolidBrush(redBall.color);
                e.Graphics.FillEllipse(colorBrush, redBall.x, redBall.y, redBall.radius, redBall.radius);
            }
        }
    }
}
