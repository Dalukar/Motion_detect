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
        bool isCamera = false;
		bool pauseFlag = false;
		double slowCoef = 1;
		int backFramesCount = 1;
		VideoCapture capture = null;
		public MainForm()
		{
			InitializeComponent();
			VideoCapture capture = new VideoCapture(PathText.Text);
			
		}
		private void AvgMultiFrameDiffCallback()
		{
            int sleepTime = 50;
			try {
                if(isCamera)
                {
                    capture = new VideoCapture(0);
                    Thread.Sleep(1000);
                }
                else
                {
                    capture = new VideoCapture(PathText.Text);
                    sleepTime = (int)Math.Round(1000 / capture.Fps);
                }
				int imageCounter = 0;
				Mat image = new Mat();
				capture.Read(image);
				image = image.CvtColor(ColorConversion.BgrToGray);
				Mat diffImage = image.EmptyClone();
				Mat avgImage = image.EmptyClone();
				Mat[] prevImages = new Mat[backFramesCount];
				for (int i = 0; i < prevImages.Length; i++){	
					prevImages[i] = image.EmptyClone();
				}
        		while (true)
        		{
        			capture.Read(image);
                    if (image.Empty())
                        break;
                    image = image.CvtColor(ColorConversion.BgrToGray);
                    if(backFramesCount == 1)
                        avgImage = prevImages[0];
                    else
                        avgImage = AvgImgGray(prevImages);
                    Cv2.Absdiff(avgImage, image, diffImage);
                    //SimpleMotionDetect(AvgImg(prevImages),image,ref diffImage);
                    Cv2.Threshold(diffImage, diffImage, 50, 255, ThresholdType.Binary);
        			pctCvWindow.Image = image.ToBitmap();
            		pctDiff.Image = diffImage.ToBitmap();
            		prevImages[imageCounter] = image.Clone();
        			Scalar sum = diffImage.Sum();
        			SafeLog(Convert.ToString(sum.Val0 + sum.Val1 + sum.Val2 + sum.Val3) + "\n");
            		if(++imageCounter >= prevImages.Length)
            			imageCounter = 0;
            		while(pauseFlag)
            			Thread.Sleep(100);
            		Thread.Sleep(Convert.ToInt32(sleepTime*slowCoef));
        		}		 		
			}
			catch(Exception ex){
			 	SafeLog(ex.ToString());
			 	return;
			 }
		}
		
		long SimpleMotionDetect(Mat img1, Mat img2, ref Mat diffImage)
		{
			//Можно сделать раза в 3 быстрее http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
			int diff = 0;
			unsafe {
    			byte* ptr1 = (byte*)img1.Data;
    			byte* ptr2 = (byte*)img2.Data;
    			byte* ptr3 = (byte*)diffImage.Data;
    			for (int y = 0; y < img1.Height; y++) {
        			for (int x = 0; x < img1.Width; x++) {
    					int offset = (img1.Width * img1.Channels() * y) + (x * 3);
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

		Mat AvgImg(Mat[] inImg)
		{
			var avgImg = inImg[0].EmptyClone();
			unsafe {
				byte* ptrAvg = (byte*)avgImg.Data;
				byte*[] ptrPct = new byte*[inImg.Length];
				for (int i = 0; i < inImg.Length; i++)
				{
					ptrPct[i] = (byte*)inImg[i].Data;
				}
				
    			for (int y = 0; y < avgImg.Height; y++) {
        			for (int x = 0; x < avgImg.Width; x++) {
						int offset = (avgImg.Channels() * avgImg.Width * y) + (x * 3);
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
		
		Mat AvgImgGray(Mat[] inImg)
		{
			unsafe {
                Mat avgImg = inImg[0].EmptyClone();
				byte* ptrAvg = (byte*)avgImg.Data;
				byte*[] ptrPct = new byte*[inImg.Length];
				for (int i = 0; i < inImg.Length; i++)
				{
					ptrPct[i] = (byte*)inImg[i].Data;
				}
				
    			for (int y = 0; y < avgImg.Height; y++) {
        			for (int x = 0; x < avgImg.Width; x++) {
						int offset = (avgImg.Width * y) + (x * 3);
						int Sum = 0;
						for (int i = 0; i < inImg.Length; i++)
						{
            				Sum += ptrPct[i][offset];
						}
                        ptrAvg[offset] = Convert.ToByte(Sum / inImg.Length);    // B
        			}
    			}
                return avgImg;
			}
		}
		
		private void SafeLog(string text) {
            Action chTxt = new Action(() => {
                LogBox.Text = text;
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
			 if (_videoThread == null || !_videoThread.IsAlive){
			 	_videoThread = new Thread(new ThreadStart(AvgMultiFrameDiffCallback));
    			_videoThread.Start();
    		}
    		else{
    			_videoThread.Abort();
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
		void BackFramesCountTextTextChanged(object sender, EventArgs e)
		{
			try{
				backFramesCount = Convert.ToInt32(BackFramesCountText.Text);
			}
			catch(Exception ex){
				SafeLog("Wrong parameter");
			}
		}
		void SlowCoefTextTextChanged(object sender, EventArgs e)
		{
			try{
				slowCoef = Convert.ToDouble(SlowCoefText.Text);
			}
			catch(Exception ex){
				SafeLog("Wrong parameter");
			}
		}
		void BtnPauseClick(object sender, EventArgs e)
		{
			pauseFlag = !pauseFlag;
		}

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            PathText.Enabled = !checkBox1.Checked;
            BrowseButton.Enabled = !checkBox1.Checked;
            isCamera = checkBox1.Checked;
        }
	}
}
