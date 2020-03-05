﻿using System.Windows;
using Emgu.CV.Structure;
using Emgu.CV;
using System.ComponentModel;

namespace CameraEmguCV
{
    /// <summary>
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        public double rho = 0.5;
        public int lowThreshold = 30, highThreshold = 210, minLineLength = 90, maxLineGap = 50, houghThreshold = 50;
        MainWindow parent;
        TemplateImage t = TemplateImage.Instance;

        public DebugWindow()
        {
            InitializeComponent();
            this.parent = (MainWindow)Application.Current.MainWindow;
        }

        private void BtnDefineTemplate_Click(object sender, RoutedEventArgs e)
        {
            if(parent.selectedScreen != null)
            {
                Mat img = parent.selectedScreen;//CvInvoke.Imread("test_save.jpg", Emgu.CV.CvEnum.LoadImageType.Grayscale);
                Mat warp = new Mat(img.Size, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
                img.CopyTo(warp);
                img = ImageProcessor.ApplyBlur(img, 0, 3);

                img = ImageProcessor.CannyEdgeDetection(img, lowThreshold, highThreshold);
                img = ImageProcessor.ApplyDilation(img, 3);
                img = ImageProcessor.ApplyErosion(img, 3);

                LineSegment2D[] lines = ImageProcessor.HoughLines(img, rho, houghThreshold, minLineLength, maxLineGap);
                LineSegment2D[] singleLines = Utils.GetSingleLinesFromHoughLines(lines, 20);
                if(singleLines!= null)
                {
                    Mat mask = ImageProcessor.AddLines(singleLines, Utils.GetRatioOfSelectedArea(Utils.GetPoints(parent.markers)));
                    Mat templateImage = ImageProcessor.ApplyMask(warp, mask);
                    t.SetTemplateImageAndMask(templateImage.ToImage<Bgr, byte>(), mask.ToImage<Bgr, byte>());
                    tempImage.Source = Utils.ToBitmapSource(templateImage);
                    templateMask.Source = Utils.ToBitmapSource(mask);
                }
                else
                    MessageBox.Show("Couldn't find any lines with the specified parameters");
            }
            else
            {
                MessageBox.Show("You didn't put any markers on the screen! ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSingleLines_Click(object sender, RoutedEventArgs e)
        {
            if(parent.selectedScreen != null)
            {
                Mat img = parent.selectedScreen;//CvInvoke.Imread("test_save.jpg", Emgu.CV.CvEnum.LoadImageType.Grayscale);
                img = ImageProcessor.ApplyBlur(img, 0, 3);
                img = ImageProcessor.CannyEdgeDetection(img, lowThreshold, highThreshold);
                img = ImageProcessor.ApplyDilation(img, 3);
                img = ImageProcessor.ApplyErosion(img, 3);

                LineSegment2D[] lines = ImageProcessor.HoughLines(img, rho, houghThreshold, minLineLength, maxLineGap);
                LineSegment2D[] singleLines = Utils.GetSingleLinesFromHoughLines(lines, 20);//KMeans(lines, 5);
                if(singleLines != null)
                {
                    Mat output = ImageProcessor.AddLines(singleLines, Utils.GetRatioOfSelectedArea(Utils.GetPoints(parent.markers)));
                    CvInvoke.Imwrite("template_mask.jpg", output);
                    parent.image2.Source = Utils.ToBitmapSource(output.ToImage<Bgr, byte>());
                }
                else
                {
                    MessageBox.Show("Couldn't find any lines with the specified parameters");
                }  
            }
            else
            {
                MessageBox.Show("You didn't put any markers on the screen! ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }   
        }

        private void BtnWarp_Click(object sender, RoutedEventArgs e)
        {
            if(parent.selectedScreen != null)
            {
                Mat img = parent.selectedScreen;//CvInvoke.Imread("test_save.jpg", Emgu.CV.CvEnum.LoadImageType.Grayscale);         
                img = ImageProcessor.ApplyBlur(img, 0, 3);
                img = ImageProcessor.CannyEdgeDetection(img, lowThreshold, highThreshold);
                parent.image2.Source = Utils.ToBitmapSource(img.ToImage<Bgr, byte>());
            }
            else
            {
                MessageBox.Show("You didn't put any markers on the screen! ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           

        }

        private void SlRho_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rho = slRho.Value;
        }

        private void SlLowThresh_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lowThreshold = (int)slLowThresh.Value;
        }

        private void SlHighThresh_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            highThreshold = (int)slHighThresh.Value;
        }

        private void SlMinLineLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            minLineLength = (int)slMinLineLength.Value;
        }

        private void SlMaxLineGap_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            maxLineGap = (int)slMaxLineGap.Value;
        }

        private void SlHoughThresh_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            houghThreshold = (int)slHoughThresh.Value;
        }

    }
}
