using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;

namespace Emzi0767.Tools.ErrorView
{
    public partial class Form1 : Form
    {
        private const string ERRLINK = "https://www.reddit.com/message/compose/?to=eMZi0767&subject=ABT%20Bug%20Report&message=I%20experienced%20a%20crash%20with%20ABT.%20Attached%20below%20is%20the%20bug%20report.%0A%0A%3Cinsert%20link%20to%20.bug%20file%20here%3E";
        private FileInfo file = null;

        public Form1(FileInfo fi)
        {
            InitializeComponent();

            this.pictureBox1.Image = SystemIcons.Error.ToBitmap();
            file = fi;
            textBox2.Text = fi.FullName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(ERRLINK);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var pth = Path.GetDirectoryName(textBox2.Text);
                Process.Start(pth);
            }
            catch (Exception)
            { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            textBox1.Text = e.Result.ToString();
            backgroundWorker1.DoWork -= BackgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted -= BackgroundWorker1_RunWorkerCompleted;
        }

        private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            using (var fs = file.OpenRead())
            using (var gz = new GZipStream(fs, CompressionMode.Decompress))
            using (var sr = new StreamReader(gz, new UTF8Encoding(false)))
            {
                e.Result = sr.ReadToEnd();
            }
        }
    }
}
