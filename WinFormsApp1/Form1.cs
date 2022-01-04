using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var html = GetData("https://concung.com/tim-sieu-thi.html");
            TestData(html);
        }
        void TestData(string html)
        {
            File.WriteAllText("res.html", html);
            Process.Start("res.html");
        }

        string GetData(string url)
        {
            HttpRequest http = new HttpRequest();
            http.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
            string html = http.Get(url).ToString();
            return html;
        }
    }
}
