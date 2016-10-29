namespace Emzi0767.Gaming.Sto.Abt2
{
    partial class FormAbtMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbtMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rGround = new System.Windows.Forms.RadioButton();
            this.rSpace = new System.Windows.Forms.RadioButton();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btGo = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbFile = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbClipboard = new System.Windows.Forms.CheckBox();
            this.bwInit = new System.ComponentModel.BackgroundWorker();
            this.bwOperation = new System.ComponentModel.BackgroundWorker();
            this.sfdMarkdown = new System.Windows.Forms.SaveFileDialog();
            this.lVersion = new System.Windows.Forms.Label();
            this.lThread = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rGround);
            this.groupBox1.Controls.Add(this.rSpace);
            this.groupBox1.Controls.Add(this.tbURL);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 112);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Build Information";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Build type:";
            // 
            // rGround
            // 
            this.rGround.AutoSize = true;
            this.rGround.Location = new System.Drawing.Point(122, 89);
            this.rGround.Name = "rGround";
            this.rGround.Size = new System.Drawing.Size(60, 17);
            this.rGround.TabIndex = 3;
            this.rGround.Text = "Ground";
            this.rGround.UseVisualStyleBackColor = true;
            // 
            // rSpace
            // 
            this.rSpace.AutoSize = true;
            this.rSpace.Checked = true;
            this.rSpace.Location = new System.Drawing.Point(9, 89);
            this.rSpace.Name = "rSpace";
            this.rSpace.Size = new System.Drawing.Size(56, 17);
            this.rSpace.TabIndex = 2;
            this.rSpace.TabStop = true;
            this.rSpace.Text = "Space";
            this.rSpace.UseVisualStyleBackColor = true;
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(9, 32);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(235, 20);
            this.tbURL.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "STO Academy URL:";
            // 
            // btGo
            // 
            this.btGo.Location = new System.Drawing.Point(12, 130);
            this.btGo.Name = "btGo";
            this.btGo.Size = new System.Drawing.Size(462, 23);
            this.btGo.TabIndex = 1;
            this.btGo.Text = "Convert";
            this.btGo.UseVisualStyleBackColor = true;
            this.btGo.Click += new System.EventHandler(this.btGo_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbFile);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cbClipboard);
            this.groupBox2.Location = new System.Drawing.Point(268, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(206, 74);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output Options";
            // 
            // cbFile
            // 
            this.cbFile.AutoSize = true;
            this.cbFile.Location = new System.Drawing.Point(105, 34);
            this.cbFile.Name = "cbFile";
            this.cbFile.Size = new System.Drawing.Size(42, 17);
            this.cbFile.TabIndex = 2;
            this.cbFile.Text = "File";
            this.cbFile.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Output to:";
            // 
            // cbClipboard
            // 
            this.cbClipboard.AutoSize = true;
            this.cbClipboard.Checked = true;
            this.cbClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbClipboard.Location = new System.Drawing.Point(9, 34);
            this.cbClipboard.Name = "cbClipboard";
            this.cbClipboard.Size = new System.Drawing.Size(70, 17);
            this.cbClipboard.TabIndex = 0;
            this.cbClipboard.Text = "Clipboard";
            this.cbClipboard.UseVisualStyleBackColor = true;
            // 
            // lVersion
            // 
            this.lVersion.AutoSize = true;
            this.lVersion.Location = new System.Drawing.Point(268, 98);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(48, 13);
            this.lVersion.TabIndex = 3;
            this.lVersion.Text = "Version: ";
            // 
            // lThread
            // 
            this.lThread.AutoSize = true;
            this.lThread.Location = new System.Drawing.Point(268, 111);
            this.lThread.Name = "lThread";
            this.lThread.Size = new System.Drawing.Size(163, 13);
            this.lThread.TabIndex = 4;
            this.lThread.TabStop = true;
            this.lThread.Text = "Don\'t forget to check for updates";
            this.lThread.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lThread_LinkClicked);
            // 
            // FormAbtMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 165);
            this.Controls.Add(this.lThread);
            this.Controls.Add(this.lVersion);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btGo);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbtMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Academy Build Tool 2.0, by Emzi0767";
            this.Load += new System.EventHandler(this.FormAbtMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btGo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rGround;
        private System.Windows.Forms.RadioButton rSpace;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbClipboard;
        private System.Windows.Forms.CheckBox cbFile;
        private System.ComponentModel.BackgroundWorker bwInit;
        private System.ComponentModel.BackgroundWorker bwOperation;
        private System.Windows.Forms.SaveFileDialog sfdMarkdown;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.LinkLabel lThread;
    }
}

