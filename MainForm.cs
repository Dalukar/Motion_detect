/*
 * Created by SharpDevelop.
 * User: VKirgintcev
 * Date: 27.05.2015
 * Time: 8:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;

namespace Motion_detect
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Thread _videoThread;
		int framesPerSample = 10;
		public MainForm()
		{
			InitializeComponent();
			
		}
		void Button1Click(object sender, EventArgs e)
		{
			 if (btnStart.Text.Equals("Start"))
    		{
        		CaptureCamera();
        		btnStart.Text = "Stop";
    		}
    		else
    		{
    			_videoThread.Abort();
        		btnStart.Text = "Start";
    		}	
					
		}
		void BrowseButtonClick(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
   			openFileDialog1.Title = "Выбрать файл";
    		if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
   			{
    			PathText.Text = openFileDialog1.FileName;
   			}
	
		}
		
		private void CaptureCamera()
		{
    		_videoThread = new Thread(new ThreadStart(CaptureVideoCallback));
    		_videoThread.Start();
		}
		
		private void CaptureVideoCallback()
		{
			using (CvCapture cap = new CvCapture(PathText.Text))
    		{
				IplImage[] imgs = new IplImage[2];
				imgs[0] = cap.QueryFrame(); //чтоб не ругалось на null
				imgs[1] = cap.QueryFrame(); //Затупа, надо сделать красивее
				int k = 0;
				int imageN = 0;
        		while (true)
        		{
        			IplImage image = cap.QueryFrame();
        			Bitmap bm = image.ToBitmap();
            		//bm.SetResolution(pctCvWindow.Width, pctCvWindow.Height);
            		pctCvWindow.Image = bm;
            		Thread.Sleep(20); //Надо как-то правильно ставить фпс
            		if(k == framesPerSample)
            		{
            			SafeLog("Diff: " +SimpleMotionDetect(imgs[0], imgs[1]) + "\n");
        				imageN = (imageN == 1) ? 0 : 1;
        				k = 0;
            		}
            		else
            		{
            			k++;
            		}
        		}
    		}
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			_videoThread.Abort();
		}
		
		private void SafeLog(string text) {
            Action chTxt = new Action(() => {
                LogBox.Text += text;
            });
 
            if (InvokeRequired)
                this.BeginInvoke(chTxt);
            else chTxt();
        }
		
		int SimpleMotionDetect(IplImage img1, IplImage img2)
		{
			//Можно сделать раза в 3 быстрее http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
			int diff = 0;
			unsafe {
    			byte* ptr1 = (byte*)img1.ImageData;
    			byte* ptr2 = (byte*)img1.ImageData;
    			for (int y = 0; y < img1.Height; y++) {
        			for (int x = 0; x < img1.Width; x++) {
            			int offset = (img1.WidthStep * y) + (x * 3);
            			byte b1 = ptr1[offset + 0];    // B
            			byte g1 = ptr1[offset + 1];    // G
            			byte r1 = ptr1[offset + 2];    // R
            			ptr1[offset + 0] = r1;
            			ptr1[offset + 1] = g1;
            			ptr1[offset + 2] = b1;
            			 byte grayScale1 = 
               				(byte)((b1 * .11) + //B
               				(g1 * .59) +  //G
               				(r1 * .3)); //R
            			 
            			byte b2 = ptr2[offset + 0];    // B
            			byte g2 = ptr2[offset + 1];    // G
            			byte r2 = ptr2[offset + 2];    // R
            			ptr2[offset + 0] = r2;
            			ptr2[offset + 1] = g2;
            			ptr2[offset + 2] = b2;
            			 byte grayScale2 = 
               				(byte)((b2 * .11) + //B
               				(g2 * .59) +  //G
               				(r2 * .3)); //R
            			 
            			 diff += Math.Abs(grayScale1 - grayScale2)/20;
        			}
    			}
			}
			return diff;
		}

	}
}
