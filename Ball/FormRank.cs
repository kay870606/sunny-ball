using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ball
{
    public partial class FormRank : Form
    {
        public FormRank()
        {
            InitializeComponent();
        }

        private void FormRank_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            //Add items in the listview
            listView1.Columns.Add("日期", 100);
            listView1.Columns.Add("時間", 70);
            listView1.Columns.Add("難度", 70);
            listView1.Columns.Add("耗時(秒)", 70);


            string[] arr = new string[4];
            ListViewItem itm;

            string fileName = "myRank.csv";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("檔案不存在");
                return;
            }
            FileStream fsr = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fsr, Encoding.UTF8);
            string strLine = "";
            string[] arrStr = null;

            while ((strLine = sr.ReadLine()) != null)
            {
                arrStr = strLine.Split(',');
                itm = new ListViewItem(arrStr);
                listView1.Items.Add(itm);
            }
            sr.Close();
            fsr.Close();
        }
    }
}
