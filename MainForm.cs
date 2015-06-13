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
using OpenCvSharp.CPlusPlus;

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
        int bkgMode = 0;
        byte[,,] gaussDistr;
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
                if (bkgMode == 2)
                {
                    gaussDistr = new byte[image.Width, image.Height, 16];
                }
        		while (true)
        		{
                    capture.Read(image);
                    if (image.Empty())
                        break;
                    image = image.CvtColor(ColorConversion.BgrToGray);
                    if(backFramesCount == 1 && bkgMode != 2)
                        avgImage = prevImages[0];
                    else
                        avgImage = BkgCalc(prevImages, bkgMode);
                    Cv2.Absdiff(avgImage, image, diffImage);
                    Cv2.Blur(diffImage, diffImage, new OpenCvSharp.CPlusPlus.Size(8,8));
                    Cv2.Threshold(diffImage, diffImage, 30, 255, ThresholdType.Binary);
                    RectDetect(diffImage);
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
		
		Mat BkgCalc(Mat[] inImg, int mode)
		{
			unsafe {
                Mat retImg = inImg[0].EmptyClone();
				byte* ptrRet = (byte*)retImg.Data;
				byte*[] ptrPct = new byte*[inImg.Length];
				for (int i = 0; i < inImg.Length; i++)
				{
					ptrPct[i] = (byte*)inImg[i].Data;
				}
				
    			for (int y = 0; y < retImg.Height; y++) {
        			for (int x = 0; x < retImg.Width; x++) {
                        int offset = (retImg.Width * y) + x;
                        switch(mode)
                        {
                            case 0:
						        int Sum = 0;
						        for (int i = 0; i < inImg.Length; i++)
						        {
            				        Sum += ptrPct[i][offset];
						        }
                                ptrRet[offset] = Convert.ToByte(Sum / inImg.Length);
                                break;
                            case 1:
                                byte[] arr = new byte[inImg.Length];
                                for (int i = 0; i < inImg.Length; i++)
                                {
                                    arr[i] = ptrPct[i][offset];
                                }
                                Array.Sort(arr);
                                ptrRet[offset] = arr[Convert.ToInt32((arr.Length - 1) / 2)];
                                break;
                            case 2:
                                int maxIndex = 0;
                                int maxValue = 0;
                                double alpha = 0.5;
                                double aaa = 0;
                                for (int i = 0; i < 16; i++)
                                {
                                    gaussDistr[x, y, i] = Convert.ToByte(gaussDistr[x, y, i] * (1 - alpha));
                                    if (i == Convert.ToInt32(ptrPct[0][offset] / 16))
                                        gaussDistr[x, y, i] += Convert.ToByte(alpha*254);
                                    if (gaussDistr[x, y, i] > maxValue)
                                    {
                                        maxIndex = i;
                                        maxValue = gaussDistr[x, y, i];
                                    }
                                    ptrRet[offset] = Convert.ToByte(maxIndex * 16 + 8);
                                }
                                break;
                        }
        			}
    			}
                return retImg;
			}
		}

        void RectDetect(Mat inImg)
        {
            int[] p1 = new int[2];
            p1[0] = inImg.Width;
            int[] p2 = new int[2];
            p2[0] = 0;
            unsafe
            {
                byte* ptrPct = (byte*)inImg.Data;
                for (int y = 0; y < inImg.Height; y++)
                {
                    for (int x = 0; x < inImg.Width; x++)
                    {
                        int offset = (inImg.Width * y) + x;
                        if (ptrPct[offset] > 0)
                        {
                            if (x > p2[0])
                                p2[0] = x;
                            p2[1] = y;

                        }
                    }
                }
                for (int y = inImg.Height-1; y >= 0; y--)
                {
                    for (int x = inImg.Width-1; x >= 0; x--)
                    {
                        int offset = (inImg.Width * y) + x;
                        if (ptrPct[offset] > 0)
                        {
                            if (x < p1[0])
                                p1[0] = x;
                            p1[1] = y;
                        }
                    }
                }
            }
            Rect r1 = new Rect(p1[0], p1[1], p2[0]-p1[0], p2[1]-p1[1]);
            Cv2.Rectangle(inImg, r1, Scalar.White, 2, LineType.Link8, 0);
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

        private void BkgModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bkgMode = BkgModeBox.SelectedIndex;
        }
	}
}
