﻿/*
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
		private System.Windows.Forms.Label diffText;
		private System.Windows.Forms.TextBox BackFramesCountText;
		private System.Windows.Forms.Label label1;
		
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
			this.diffText = new System.Windows.Forms.Label();
			this.BackFramesCountText = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pctCvWindow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pctDiff)).BeginInit();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(762, 36);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(57, 23);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.Button1Click);
			// 
			// LogBox
			// 
			this.LogBox.Location = new System.Drawing.Point(506, 378);
			this.LogBox.Name = "LogBox";
			this.LogBox.ReadOnly = true;
			this.LogBox.Size = new System.Drawing.Size(295, 163);
			this.LogBox.TabIndex = 1;
			this.LogBox.Text = "";
			// 
			// PathText
			// 
			this.PathText.Location = new System.Drawing.Point(506, 12);
			this.PathText.Name = "PathText";
			this.PathText.Size = new System.Drawing.Size(250, 20);
			this.PathText.TabIndex = 2;
			this.PathText.Text = "C:\\Users\\Public\\Videos\\Sample Videos\\test.wmv";
			// 
			// BrowseButton
			// 
			this.BrowseButton.Location = new System.Drawing.Point(762, 10);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(57, 23);
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
			// diffText
			// 
			this.diffText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.diffText.Location = new System.Drawing.Point(506, 308);
			this.diffText.Name = "diffText";
			this.diffText.Size = new System.Drawing.Size(295, 53);
			this.diffText.TabIndex = 6;
			// 
			// BackFramesCountText
			// 
			this.BackFramesCountText.Location = new System.Drawing.Point(651, 38);
			this.BackFramesCountText.Name = "BackFramesCountText";
			this.BackFramesCountText.Size = new System.Drawing.Size(61, 20);
			this.BackFramesCountText.TabIndex = 7;
			this.BackFramesCountText.Text = "1";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(506, 41);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(139, 23);
			this.label1.TabIndex = 8;
			this.label1.Text = "Background frames count:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(831, 553);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BackFramesCountText);
			this.Controls.Add(this.diffText);
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
	}
}
