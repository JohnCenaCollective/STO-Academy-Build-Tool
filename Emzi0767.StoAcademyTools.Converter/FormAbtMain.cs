using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Emzi0767.StoAcademyTools.Converter.Utility;
using Emzi0767.StoAcademyTools.Library;
using Emzi0767.StoAcademyTools.Library.Data;

namespace Emzi0767.StoAcademyTools.Converter
{
    public partial class FormAbtMain : Form
    {
        private StoAcademyInterface stoat;

        public FormAbtMain()
        {
            InitializeComponent();
        }

        private void FormAbtMain_Load(object sender, EventArgs e)
        {
            var a = Assembly.GetExecutingAssembly();
            var n = a.GetName();
            var v = n.Version;

            lVersion.Text = string.Concat("Version: ", v.ToString(), " by Emzi0767, \nmaintained by JohnCenaPTF");
            lThread.Links.Add(0, lThread.Text.Length, "https://github.com/JohnCenaCollective/STO-Academy-Build-Tool/releases/latest");

            bwInit.DoWork += BwInit_DoWork;
            bwInit.RunWorkerCompleted += BwInit_RunWorkerCompleted;

            bwOperation.DoWork += BwOperation_DoWork;
            bwOperation.RunWorkerCompleted += BwOperation_RunWorkerCompleted;

            btGo.Enabled = false;
            cbClipboard.Enabled = false;
            cbFile.Enabled = false;
            rSpace.Enabled = false;
            rGround.Enabled = false;
            tbURL.Enabled = false;
            tbURL.Text = "Loading academy data...";

            bwInit.RunWorkerAsync();
        }

        private void BwInit_DoWork(object sender, DoWorkEventArgs e)
        {
            this.stoat = new StoAcademyInterface();
            this.stoat.Initialize();
        }

        private void BwInit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bwInit.DoWork -= BwInit_DoWork;
            bwInit.RunWorkerCompleted -= BwInit_RunWorkerCompleted;

            btGo.Enabled = true;
            cbClipboard.Enabled = true;
            cbFile.Enabled = true;
            rSpace.Enabled = true;
            rGround.Enabled = true;
            tbURL.Enabled = true;
            tbURL.Text = "";
        }

        private void btGo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbURL.Text))
            {
                MessageBox.Show(this, "You need enter a build ID.", "No build ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!cbClipboard.Checked && !cbFile.Checked)
            {
                MessageBox.Show(this, "You need to select at least one output.", "Output warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btGo.Enabled = false;
            cbClipboard.Enabled = false;
            cbFile.Enabled = false;
            rSpace.Enabled = false;
            rGround.Enabled = false;
            tbURL.Enabled = false;
            Program.buildid = tbURL.Text;
            bwOperation.RunWorkerAsync(tbURL.Text);
        }

        private void BwOperation_DoWork(object sender, DoWorkEventArgs e)
        {
            var buildid = e.Argument.ToString();
            var builduri = (Uri)null;

            if (buildid.StartsWith("http://") && Uri.TryCreate(buildid, UriKind.RelativeOrAbsolute, out builduri))
                buildid = builduri.AbsolutePath.Substring(1);

            var build = stoat.GetBuild(buildid);
            e.Result = build;
        }

        private void BwOperation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw e.Error;

            var saveto = new bool[]
            {
                cbClipboard.Checked,  // [0]: Cliboard
                cbFile.Checked,  // [1]: File
            };

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var mdw = new MarkdownWriter(sw))
            using (var bw = new BuildWriter(mdw))
            {
                bw.WriteBuild((StoAcademyBuild)e.Result, stoat, rSpace.Checked ? BuildType.Space : BuildType.Ground);
            }
            var build = sb.ToString();

            // save to clipboard
            if (saveto[0] && MessageBox.Show(this, "Are you sure you want to replace the contents of the clipboard with your build?", "Save to clipboard", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Clipboard.SetText(build);
            }

            // save to file
            if (saveto[1])
            {
                sfdMarkdown.FileName = string.Concat("build_", ((StoAcademyBuild)e.Result).ID, ".txt");
                if (sfdMarkdown.ShowDialog() == DialogResult.OK)
                {
                    var fi = new FileInfo(sfdMarkdown.FileName);
                    using (var fs = fi.Create())
                    using (var sw = new StreamWriter(fs, new UTF8Encoding(false)))
                    {
                        sw.Write(build);
                    }
                }
            }
            
            var msg = "Build conversion completed. Put the contents of ";
            if (saveto[1])
                msg = string.Concat(msg, "the saved file");
            if (saveto[1] && saveto[0])
                msg = string.Concat(msg, " or ");
            if (saveto[0])
                msg = string.Concat(msg, "your clipboard");
            msg = string.Concat(msg, " in your /r/stobuilds post.");
            MessageBox.Show(this, msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btGo.Enabled = true;
            cbClipboard.Enabled = true;
            cbFile.Enabled = true;
            rSpace.Enabled = true;
            rGround.Enabled = true;
            tbURL.Enabled = true;
        }

        private void lThread_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var lnk = e.Link.LinkData.ToString();
            Process.Start(lnk);
        }
    }
}
