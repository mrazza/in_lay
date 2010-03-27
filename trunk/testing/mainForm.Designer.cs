namespace testing
{
    partial class mainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblArtist = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblCurr = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblPer = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.libDisplay = new System.Windows.Forms.ListView();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.vol = new System.Windows.Forms.Label();
            this.rdLib = new System.Windows.Forms.RadioButton();
            this.rdPlay = new System.Windows.Forms.RadioButton();
            this.rdDyPlay = new System.Windows.Forms.RadioButton();
            this.stdPlay = new System.Windows.Forms.ListView();
            this.dynPlay = new System.Windows.Forms.ListView();
            this.cmdAddPlay = new System.Windows.Forms.Button();
            this.cmdAddDynPlay = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(410, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(92, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(312, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File URL/Path:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(410, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Open";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Title:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(70, 94);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 13);
            this.lblTitle.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(234, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Artist:";
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.Location = new System.Drawing.Point(273, 94);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(0, 13);
            this.lblArtist.TabIndex = 7;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(130, 164);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Play";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(211, 164);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "Pause";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(292, 164);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "Stop";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 135);
            this.progressBar1.Maximum = 100000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(477, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 11;
            // 
            // lblCurr
            // 
            this.lblCurr.AutoSize = true;
            this.lblCurr.Location = new System.Drawing.Point(8, 119);
            this.lblCurr.Name = "lblCurr";
            this.lblCurr.Size = new System.Drawing.Size(13, 13);
            this.lblCurr.TabIndex = 12;
            this.lblCurr.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "/";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(66, 119);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(13, 13);
            this.lblTotal.TabIndex = 14;
            this.lblTotal.Text = "0";
            // 
            // lblPer
            // 
            this.lblPer.AutoSize = true;
            this.lblPer.Location = new System.Drawing.Point(450, 119);
            this.lblPer.Name = "lblPer";
            this.lblPer.Size = new System.Drawing.Size(0, 13);
            this.lblPer.TabIndex = 15;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(41, 473);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(138, 23);
            this.button6.TabIndex = 17;
            this.button6.Text = "Build/Rebuild Database";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(289, 473);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(165, 20);
            this.txtSearch.TabIndex = 20;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(239, 476);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Search:";
            // 
            // libDisplay
            // 
            this.libDisplay.Location = new System.Drawing.Point(25, 277);
            this.libDisplay.Name = "libDisplay";
            this.libDisplay.Size = new System.Drawing.Size(448, 190);
            this.libDisplay.TabIndex = 22;
            this.libDisplay.UseCompatibleStateImageBehavior = false;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(15, 187);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(448, 45);
            this.trackBar1.SmallChange = 2;
            this.trackBar1.TabIndex = 23;
            this.trackBar1.TickFrequency = 5;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Volume:";
            // 
            // vol
            // 
            this.vol.AutoSize = true;
            this.vol.Location = new System.Drawing.Point(70, 174);
            this.vol.Name = "vol";
            this.vol.Size = new System.Drawing.Size(21, 13);
            this.vol.TabIndex = 25;
            this.vol.Text = "vol";
            // 
            // rdLib
            // 
            this.rdLib.Location = new System.Drawing.Point(25, 250);
            this.rdLib.Name = "rdLib";
            this.rdLib.Size = new System.Drawing.Size(64, 24);
            this.rdLib.TabIndex = 34;
            this.rdLib.Text = "Library";
            this.rdLib.CheckedChanged += new System.EventHandler(this.rdLib_CheckedChanged);
            // 
            // rdPlay
            // 
            this.rdPlay.AutoSize = true;
            this.rdPlay.Location = new System.Drawing.Point(519, 12);
            this.rdPlay.Name = "rdPlay";
            this.rdPlay.Size = new System.Drawing.Size(103, 17);
            this.rdPlay.TabIndex = 27;
            this.rdPlay.TabStop = true;
            this.rdPlay.Text = "Standard Playlist";
            this.rdPlay.UseVisualStyleBackColor = true;
            this.rdPlay.CheckedChanged += new System.EventHandler(this.rdPlay_CheckedChanged);
            // 
            // rdDyPlay
            // 
            this.rdDyPlay.AutoSize = true;
            this.rdDyPlay.Location = new System.Drawing.Point(519, 254);
            this.rdDyPlay.Name = "rdDyPlay";
            this.rdDyPlay.Size = new System.Drawing.Size(101, 17);
            this.rdDyPlay.TabIndex = 28;
            this.rdDyPlay.TabStop = true;
            this.rdDyPlay.Text = "Dynamic Playlist";
            this.rdDyPlay.UseVisualStyleBackColor = true;
            this.rdDyPlay.CheckedChanged += new System.EventHandler(this.rdDyPlay_CheckedChanged);
            // 
            // stdPlay
            // 
            this.stdPlay.Location = new System.Drawing.Point(519, 35);
            this.stdPlay.Name = "stdPlay";
            this.stdPlay.Size = new System.Drawing.Size(448, 190);
            this.stdPlay.TabIndex = 29;
            this.stdPlay.UseCompatibleStateImageBehavior = false;
            // 
            // dynPlay
            // 
            this.dynPlay.Location = new System.Drawing.Point(519, 277);
            this.dynPlay.Name = "dynPlay";
            this.dynPlay.Size = new System.Drawing.Size(448, 190);
            this.dynPlay.TabIndex = 30;
            this.dynPlay.UseCompatibleStateImageBehavior = false;
            // 
            // cmdAddPlay
            // 
            this.cmdAddPlay.Location = new System.Drawing.Point(628, 6);
            this.cmdAddPlay.Name = "cmdAddPlay";
            this.cmdAddPlay.Size = new System.Drawing.Size(75, 23);
            this.cmdAddPlay.TabIndex = 31;
            this.cmdAddPlay.Text = "Add New";
            this.cmdAddPlay.UseVisualStyleBackColor = true;
            this.cmdAddPlay.Click += new System.EventHandler(this.cmdAddPlay_Click);
            // 
            // cmdAddDynPlay
            // 
            this.cmdAddDynPlay.Location = new System.Drawing.Point(626, 248);
            this.cmdAddDynPlay.Name = "cmdAddDynPlay";
            this.cmdAddDynPlay.Size = new System.Drawing.Size(75, 23);
            this.cmdAddDynPlay.TabIndex = 32;
            this.cmdAddDynPlay.Text = "Add New";
            this.cmdAddDynPlay.UseVisualStyleBackColor = true;
            this.cmdAddDynPlay.Click += new System.EventHandler(this.cmdAddDynPlay_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.vol);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblPer);
            this.groupBox1.Controls.Add(this.lblTotal);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblCurr);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.lblArtist);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblTitle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Location = new System.Drawing.Point(10, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 239);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Test Player";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(709, 6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 35;
            this.button7.Text = "Add To...";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(101, 256);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 13);
            this.lblResult.TabIndex = 36;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(410, 74);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 37;
            this.button8.Text = "Stream Test";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 507);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdAddDynPlay);
            this.Controls.Add(this.cmdAddPlay);
            this.Controls.Add(this.dynPlay);
            this.Controls.Add(this.stdPlay);
            this.Controls.Add(this.rdDyPlay);
            this.Controls.Add(this.rdPlay);
            this.Controls.Add(this.rdLib);
            this.Controls.Add(this.libDisplay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.button6);
            this.Name = "mainForm";
            this.Text = "netAudio, netDiscographer Test Application";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblCurr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblPer;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView libDisplay;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label vol;
        private System.Windows.Forms.RadioButton rdLib;
        private System.Windows.Forms.RadioButton rdPlay;
        private System.Windows.Forms.RadioButton rdDyPlay;
        private System.Windows.Forms.ListView stdPlay;
        private System.Windows.Forms.ListView dynPlay;
        private System.Windows.Forms.Button cmdAddPlay;
        private System.Windows.Forms.Button cmdAddDynPlay;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button button8;
    }
}