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
    public partial class FormHard : Form
    {
        int x, y, speed, timing, acc;
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
                if (textBox1.Text == (acc).ToString())
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
            arr[2] = "困難";
            arr[3] = timing / 10 + "." + timing % 10;

            //            itm = new ListViewItem(arr);
            //            listView1.Items.Add(itm);

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

        private void FormHard_Load(object sender, EventArgs e)
        {
            timing = 0;
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

            myBalls = new Ball[Constants.BallNumber + 5];
            myThreadedBalls = new List<Ball>();

            for (int i = 0; i < myBalls.Length; i++)
            {
                myBalls[i] = new Ball(this);
                myBalls[i].radius = 20;
                myBalls[i].x = random.Next(myBalls[i].radius, ClientSize.Width - myBalls[i].radius);
                myBalls[i].y = random.Next(myBalls[i].radius, ClientSize.Height - myBalls[i].radius);
                int s = random.Next(-Constants.BallSpeed - 5, Constants.BallSpeed + 5),
                    d = random.Next(-Constants.BallSpeed - 5, Constants.BallSpeed + 5);
                if (s == 0) s = 3;
                if (d == 0) d = 3;
                myBalls[i].xspeed = s;
                myBalls[i].yspeed = d;
                myBalls[i].color = Color.Green;
            }

            int r = random.Next(5, 8);
            acc += r;
            for (int i = 0; i < r; i++)
            {
                myThreadedBalls.Add(myBalls[i]);
                Thread tid1 = new Thread(new ThreadStart(myBalls[i].move));
                tid1.Start();
            }


            redBalls = new Ball[Constants.BallNumber + 5];//red
            myThreadedRedBalls = new List<Ball>();

            for (int i = 0; i < redBalls.Length; i++)
            {
                redBalls[i] = new Ball(this);
                redBalls[i].radius = 20;
                redBalls[i].x = random.Next(redBalls[i].radius, ClientSize.Width - redBalls[i].radius);
                redBalls[i].y = random.Next(redBalls[i].radius, ClientSize.Height - redBalls[i].radius);
                int s = random.Next(-Constants.BallSpeed, Constants.BallSpeed),
                    d = random.Next(-Constants.BallSpeed, Constants.BallSpeed);
                if (s == 0) s = 3;
                if (d == 0) d = 3;
                redBalls[i].xspeed = s;
                redBalls[i].yspeed = d;
                redBalls[i].color = Color.Red;
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
                if (acc < Constants.BallNumber + 5)
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
            label1.Text = "時間 : " + timing / 10 + "." + timing % 10;

            this.Invalidate();
        }

        public FormHard()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void FormHard_Paint(object sender, PaintEventArgs e)
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

