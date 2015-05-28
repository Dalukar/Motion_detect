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
		int framesPerSample = 1;
		int fpsCoef = 20;
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
				IplImage diffImage = cap.QueryFrame().Clone();
				imgs[0] = cap.QueryFrame().Clone(); //чтоб не ругалось на null
				imgs[1] = cap.QueryFrame().Clone(); //Затупа, надо сделать красивее
				int k = 0;
				int imageN = 0;
        		while (true)
        		{
        			IplImage image = cap.QueryFrame();
            		//bm.SetResolution(pctCvWindow.Width, pctCvWindow.Height);
            		pctCvWindow.Image = image.ToBitmap();
            		Thread.Sleep(fpsCoef); //Надо как-то правильно ставить фпс
            		if(k == framesPerSample)
            		{
                        imgs[imageN] = image.Clone();
                        string diff = Convert.ToString(SimpleMotionDetect(imgs[0], imgs[1], ref diffImage));
                        Action chTxt = new Action(() => {diffText.Text = diff;});
            			if (InvokeRequired)
                			this.BeginInvoke(chTxt);
           				else chTxt();
            			pctDiff.Image = diffImage.ToBitmap();
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
            if(_videoThread != null)
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
		
		long SimpleMotionDetect(IplImage img1, IplImage img2, ref IplImage diffImage)
		{
			//Можно сделать раза в 3 быстрее http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
			int diff = 0;
			unsafe {
    			byte* ptr1 = (byte*)img1.ImageData;
    			byte* ptr2 = (byte*)img2.ImageData;
    			byte* ptr3 = (byte*)diffImage.ImageData;
    			for (int y = 0; y < img1.Height; y++) {
        			for (int x = 0; x < img1.Width; x++) {
            			int offset = (img1.WidthStep * y) + (x * 3);
            			byte b1 = ptr1[offset + 0];    // B
            			byte g1 = ptr1[offset + 1];    // G
            			byte r1 = ptr1[offset + 2];    // R
            			 byte grayScale1 = 
               				(byte)((b1 * .11) + //B
               				(g1 * .59) +  //G
               				(r1 * .3)); //R
            			byte b2 = ptr2[offset + 0];    // B
            			byte g2 = ptr2[offset + 1];    // G
            			byte r2 = ptr2[offset + 2];    // R
            			 byte grayScale2 = 
               				(byte)((b2 * .11) + //B
               				(g2 * .59) +  //G
               				(r2 * .3)); //R
            			 ptr3[offset + 0] = (Byte)Math.Abs(b2 - b1);    // B
            			ptr3[offset + 1] = (Byte)Math.Abs(g2 - g1);    // G
            			ptr3[offset + 2] = (Byte)Math.Abs(r2 - r1);     // R

                         diff += Math.Abs(grayScale1 - grayScale2);
        			}
    			}
			}
			return diff;
		}

	}
}
