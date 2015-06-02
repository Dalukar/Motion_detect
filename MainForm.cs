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
using  OpenCvSharp.CPlusPlus;

namespace Motion_detect
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Thread _videoThread;
		float slowCoef = 1;
		int backFramesCount = 1;
		public MainForm()
		{
			InitializeComponent();
			
		}
			
		private void SingleFrameDiffCallback()
		{
			CvCapture capture = new CvCapture(PathText.Text);
			int sleepTime = (int)Math.Round(1000 / capture.Fps* slowCoef);
			IplImage image = capture.QueryFrame();
			IplImage prevImage = image.EmptyClone();
			IplImage diffImage = image.EmptyClone();
			using (CvCapture cap = new CvCapture(PathText.Text))
    		{
        		while (true)
        		{
        			image = capture.QueryFrame();
        			if(image == null)
            			break;   
        			//Cv.AbsDiff(prevImage,image,diffImage);
           			SimpleMotionDetect(prevImage,image,ref diffImage);
        			pctCvWindow.Image = image.ToBitmap();
            		pctDiff.Image = diffImage.ToBitmap();
            		prevImage = image.Clone();
        			Scalar sum = diffImage.Sum();
        			Action chTxt = new Action(() => 
        			{diffText.Text = Convert.ToString(sum.Val0 + sum.Val1 + sum.Val2 + sum.Val3) + "\n" + sum.ToString();});
            		if (InvokeRequired)
               			this.BeginInvoke(chTxt);
            		else chTxt();
        			Thread.Sleep(sleepTime);
        		}
    		}
		}
			
		private void AvgMultiFrameDiffCallback()
		{
			CvCapture capture = new CvCapture(PathText.Text);
			int sleepTime = (int)Math.Round(1000 / capture.Fps* slowCoef);
			IplImage image = capture.QueryFrame();
			IplImage diffImage = image.EmptyClone();
			IplImage[] prevImages = new IplImage[backFramesCount];
			int imageCounter = 0;
			image = capture.QueryFrame();
			for (int i = 0; i < prevImages.Length; i++)
				{	
				prevImages[i] = image.EmptyClone();
				}
			using (CvCapture cap = new CvCapture(PathText.Text))
    		{
        		while (true)
        		{
        			image = capture.QueryFrame();
        			if(image == null)
            			break;   
        			Cv.AbsDiff(AvgImg(prevImages),image,diffImage);
        			//SimpleMotionDetect(AvgImg(prevImages),image,ref diffImage);
        			pctCvWindow.Image = image.ToBitmap();
            		pctDiff.Image = diffImage.ToBitmap();
            		prevImages[imageCounter] = image.Clone();
        			Scalar sum = diffImage.Sum();
        			Action chTxt = new Action(() => 
        			{diffText.Text = Convert.ToString(sum.Val0 + sum.Val1 + sum.Val2 + sum.Val3) + "\n" + sum.ToString();});
            		if (InvokeRequired)
               			this.BeginInvoke(chTxt);
            		else chTxt();
            		if(++imageCounter >= prevImages.Length)
            			imageCounter = 0;
            		
        			Thread.Sleep(sleepTime);
        		}
    		}
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

		IplImage AvgImg(IplImage[] inImg)
		{
			IplImage avgImg = inImg[0].EmptyClone();
			unsafe {
				byte* ptrAvg = (byte*)avgImg.ImageData;
				byte*[] ptrPct = new byte*[inImg.Length];
				for (int i = 0; i < inImg.Length; i++)
				{
					ptrPct[i] = (byte*)inImg[i].ImageData;
				}
				
    			for (int y = 0; y < avgImg.Height; y++) {
        			for (int x = 0; x < avgImg.Width; x++) {
						int offset = (avgImg.WidthStep * y) + (x * 3);
						int bSum = 0;
						int gSum = 0;
						int rSum = 0;
						for (int i = 0; i < inImg.Length; i++)
						{
            				bSum += ptrPct[i][offset + 0];    // B
            				gSum += ptrPct[i][offset + 1];    // G
            				rSum += ptrPct[i][offset + 2];    // R
						}
						ptrAvg[offset + 0] = Convert.ToByte(bSum/inImg.Length);    // B
						ptrAvg[offset + 1] = Convert.ToByte(gSum/inImg.Length);    // G
						ptrAvg[offset + 2] = Convert.ToByte(rSum/inImg.Length);     // R
        			}
    			}
			}
			return avgImg;
		}
		
		private void SafeLog(string text) {
            Action chTxt = new Action(() => {
                LogBox.Text += text;
            });
 
            if (InvokeRequired)
                this.BeginInvoke(chTxt);
            else chTxt();
        }
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
            if(_videoThread != null)
			_videoThread.Abort();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			 if (btnStart.Text.Equals("Start"))
    		{
			 	try {
			 		backFramesCount=Convert.ToInt32(BackFramesCountText.Text);
			 	}
			 	catch(Exception ex){
			 		return;
			 	}
			 	if(backFramesCount == 1)
			 	{
			 		_videoThread = new Thread(new ThreadStart(SingleFrameDiffCallback));
    				_videoThread.Start();
    				btnStart.Text = "Stop";
			 	}
			 	if(backFramesCount > 1)
			 	{
			 		_videoThread = new Thread(new ThreadStart(AvgMultiFrameDiffCallback));
    				_videoThread.Start();
    				btnStart.Text = "Stop";
			 	}  
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
	}
}
