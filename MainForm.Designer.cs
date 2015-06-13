/*
 * Created by SharpDevelop.
 * User: VKirgintcev
 * Date: 27.05.2015
 * Time: 8:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Motion_detect
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.RichTextBox LogBox;
		private System.Windows.Forms.TextBox PathText;
		private System.Windows.Forms.Button BrowseButton;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.PictureBox pctCvWindow;
		private System.Windows.Forms.PictureBox pctDiff;
		private System.Windows.Forms.TextBox BackFramesCountText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox SlowCoefText;
		private System.Windows.Forms.Button btnPause;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnStart = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.PathText = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.pctCvWindow = new System.Windows.Forms.PictureBox();
            this.pctDiff = new System.Windows.Forms.PictureBox();
            this.BackFramesCountText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SlowCoefText = new System.Windows.Forms.TextBox();
            this.btnPause = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.BkgModeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pctCvWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctDiff)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(938, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start/Stop";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.Button1Click);
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(509, 378);
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(487, 163);
            this.LogBox.TabIndex = 1;
            this.LogBox.Text = "";
            // 
            // PathText
            // 
            this.PathText.Location = new System.Drawing.Point(577, 14);
            this.PathText.Name = "PathText";
            this.PathText.Size = new System.Drawing.Size(355, 20);
            this.PathText.TabIndex = 2;
            this.PathText.Text = "C:\\Users\\Public\\Videos\\Sample Videos\\test.wmv";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(938, 12);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(70, 23);
            this.BrowseButton.TabIndex = 3;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // pctCvWindow
            // 
            this.pctCvWindow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pctCvWindow.Location = new System.Drawing.Point(12, 12);
            this.pctCvWindow.Name = "pctCvWindow";
            this.pctCvWindow.Size = new System.Drawing.Size(488, 260);
            this.pctCvWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctCvWindow.TabIndex = 4;
            this.pctCvWindow.TabStop = false;
            // 
            // pctDiff
            // 
            this.pctDiff.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pctDiff.Location = new System.Drawing.Point(13, 281);
            this.pctDiff.Name = "pctDiff";
            this.pctDiff.Size = new System.Drawing.Size(487, 260);
            this.pctDiff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctDiff.TabIndex = 5;
            this.pctDiff.TabStop = false;
            // 
            // BackFramesCountText
            // 
            this.BackFramesCountText.Location = new System.Drawing.Point(651, 38);
            this.BackFramesCountText.Name = "BackFramesCountText";
            this.BackFramesCountText.Size = new System.Drawing.Size(42, 20);
            this.BackFramesCountText.TabIndex = 7;
            this.BackFramesCountText.Text = "1";
            this.BackFramesCountText.TextChanged += new System.EventHandler(this.BackFramesCountTextTextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(506, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Background frames count:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(506, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Frame time multipler:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SlowCoefText
            // 
            this.SlowCoefText.Location = new System.Drawing.Point(651, 70);
            this.SlowCoefText.Name = "SlowCoefText";
            this.SlowCoefText.Size = new System.Drawing.Size(42, 20);
            this.SlowCoefText.TabIndex = 9;
            this.SlowCoefText.Text = "1";
            this.SlowCoefText.TextChanged += new System.EventHandler(this.SlowCoefTextTextChanged);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(938, 67);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(70, 23);
            this.btnPause.TabIndex = 11;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.BtnPauseClick);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(509, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(62, 17);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Camera";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
            // 
            // BkgModeBox
            // 
            this.BkgModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BkgModeBox.FormattingEnabled = true;
            this.BkgModeBox.Items.AddRange(new object[] {
            "Average",
            "Median",
            "Gauss"});
            this.BkgModeBox.Location = new System.Drawing.Point(811, 40);
            this.BkgModeBox.Name = "BkgModeBox";
            this.BkgModeBox.Size = new System.Drawing.Size(121, 21);
            this.BkgModeBox.TabIndex = 13;
            this.BkgModeBox.SelectedIndexChanged += new System.EventHandler(this.BkgModeBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(699, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 23);
            this.label3.TabIndex = 14;
            this.label3.Text = "Mode";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 553);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BkgModeBox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SlowCoefText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BackFramesCountText);
            this.Controls.Add(this.pctDiff);
            this.Controls.Add(this.pctCvWindow);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.PathText);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.btnStart);
            this.Name = "MainForm";
            this.Text = "Motion_detect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pctCvWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctDiff)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox BkgModeBox;
        private System.Windows.Forms.Label label3;
	}
}
