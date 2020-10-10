using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCI_Lab2
{
    public class PhotoShop
    {
        private Image<Bgr, byte> sourceImage;
        public PhotoShop(Image<Bgr, byte> image)
        {
            sourceImage = image;
        }

        //Загрузка
        public Image<Bgr, byte> GetSourceImage()
        {
            return sourceImage;
        }

        //Канал
        public Image<Bgr, byte> GetChannels(bool red , bool green, bool blue)
        {
            //var channel = sourceImage.Split()[0]; //[] - channel index
            bool[] channels = new bool[3] { blue, green, red };
            Image<Bgr, byte> destImage = sourceImage.CopyBlank();

            VectorOfMat vm = new VectorOfMat();

            for (int i=0; i<3; i++)
            {
                if (channels[i] == true)
                {
                    vm.Push(sourceImage.Split()[i]); 
                }
                else
                {
                    vm.Push(sourceImage.Split()[i].CopyBlank());
                }
            }

            CvInvoke.Merge(vm, destImage);
            return destImage;
        }

        //ЧБ
        public Image<Gray, byte> GetGrayImage()
        {
            var grayImage = new Image<Gray, byte>(sourceImage.Size);

            for (int y = 0; y < grayImage.Height; y++)
                for (int x = 0; x < grayImage.Width; x++)
                {
                    grayImage.Data[y, x, 0] = Convert.ToByte(0.299 * sourceImage.Data[y, x, 2] + 0.587 * sourceImage.Data[y, x, 1] + 0.114 * sourceImage.Data[y, x, 0]);
                }

            return grayImage;
        }

        //Сепия
        public Image<Bgr, byte> GetSepiaImage()
        {
            var sepiaImage = new Image<Bgr, byte>(sourceImage.Size);

            for (int y = 0; y < sepiaImage.Height; y++)
                for (int x = 0; x < sepiaImage.Width; x++)
                {       
                    sepiaImage.Data[y, x, 0] = Convert.ToByte(AddColors(0.272 * sourceImage.Data[y, x, 2], 0.534 * sourceImage.Data[y, x, 1], 0.131 * sourceImage.Data[y, x, 0]));
                    sepiaImage.Data[y, x, 1] = Convert.ToByte(AddColors(0.349 * sourceImage.Data[y, x, 2], 0.686 * sourceImage.Data[y, x, 1], 0.168 * sourceImage.Data[y, x, 0]));
                    sepiaImage.Data[y, x, 2] = Convert.ToByte(AddColors(0.393 * sourceImage.Data[y, x, 2], 0.769 * sourceImage.Data[y, x, 1], 0.189 * sourceImage.Data[y, x, 0]));
                }

            return sepiaImage;
        }

        //Яркость
        public Image<Bgr, byte> GetBrightImage(int high)
        {
            return GetBrightImage(high, sourceImage);
        }
        public Image<Bgr, byte> GetBrightImage(int high, Image<Bgr, byte> image)
        {
            var brightImage = image.Copy();

            for (int ch = 0; ch < 3; ch++)
                for (int y = 0; y < brightImage.Height; y++)
                    for (int x = 0; x < brightImage.Width; x++)
                    {
                        if ((brightImage.Data[y, x, ch] + high) > 255)
                            brightImage.Data[y, x, ch] = 255;
                        else
                            brightImage.Data[y, x, ch] = Convert.ToByte(brightImage.Data[y, x, ch] + high);
                    }
            return brightImage;
        }

        //Контраст
        public Image<Bgr, byte> GetContrastImage(int highPlus)
        {
            return GetContrastImage(highPlus, sourceImage);
        }
        public Image<Bgr, byte> GetContrastImage(int highPlus, Image<Bgr, byte> image)
        {
            var contrastImage = image.Copy();
            double high = 1 + (double)highPlus / 10;
            for (int ch = 0; ch < 3; ch++)
                for (int y = 0; y < contrastImage.Height; y++)
                    for (int x = 0; x < contrastImage.Width; x++)
                    {
                        if ((contrastImage.Data[y, x, ch] * high) > 255)
                            contrastImage.Data[y, x, ch] = 255;
                        else
                            contrastImage.Data[y, x, ch] = Convert.ToByte(contrastImage.Data[y, x, ch] * high);
                    }
            return contrastImage;
        }

        //Дополнение
        public Image<Bgr, byte> GetAddedImage(Image<Bgr, byte> addedImage)
        {
            var resultImage = sourceImage.Copy();
            return resultImage.Add(addedImage);
            //return resultImage.AddWeighted(addedImage, 0.5, 0.5, 0);
        }

        public Image<Bgr, byte> GetAddedImage(Image<Bgr, byte> image1, Image<Bgr, byte> image2, int weight1, int weight2)
        {
            var resultImage = image1.Copy();
            return resultImage.AddWeighted(image2, (double)weight1/10, (double)weight2/10, 0);
        }

        //Исключение
        public Image<Bgr, byte> GetSubtractedImage(Image<Bgr, byte> subtractedImage)
        {
            var resultImage = sourceImage.Copy();
            return resultImage.Sub(subtractedImage);
        }

        //Пересечение
        public Image<Bgr, byte> GetIntersectedImage(Image<Bgr, byte> intersectedImage)
        {
            var resultImage = sourceImage.Copy();
            return resultImage.And(intersectedImage);
        }

        //HSV
        public Image<Hsv, byte> GetHSVImage(int trackbarValue)
        {
            Image<Hsv, byte> hsvImage = sourceImage.Convert<Hsv, byte>();

            for (int y = 0; y < hsvImage.Height; y++)
                for (int x = 0; x < hsvImage.Width; x++)
                {
                    hsvImage.Data[y, x, 0] = (byte)(trackbarValue);
                    //hsvImage.Data[y, x, 1] = (byte)(trackbarValue);
                    //hsvImage.Data[y, x, 2] = (byte)(trackbarValue);
                }
            return hsvImage;
        }

        //Медианное размытие
        public Image<Bgr, byte> GetMedianBlur(int level)
        {
            return GetMedianBlur(sourceImage, level);
        }
        public Image<Bgr, byte> GetMedianBlur(Image<Bgr, byte> source, int level)
        {
            Image<Bgr, byte> image = source.Copy();
            Image<Bgr, byte> result = image.CopyBlank();

            List<byte> window = new List<byte>();

            for (int ch = 0; ch < 3; ch ++)
            {
                for (int y = 0; y < image.Height; y++)
                    for (int x = 0; x < image.Width; x++)
                    {
                        window.Clear();

                        for (int i = -level; i < level + 1; i++)
                            for (int j = -level; j < level + 1; j++)
                            {
                                if ((i + y) >= 0 && (j + x) >= 0 && (i + y) < image.Height && (j + x) < image.Width)
                                    window.Add(image.Data[i + y, j + x, ch]);
                            }

                        window.Sort();

                        if (window.Count % 2 == 0)
                            result.Data[y, x, ch] = window[window.Count / 2];
                        else
                            result.Data[y, x, ch] = Convert.ToByte((window[window.Count / 2] + window[window.Count / 2 - 1]) / 2);
                    }
            }
                     
            return result;
        }

        private Image<Gray, byte> GetGrayMedianBlur(Image<Gray, byte> source)
        {
            Image<Gray, byte> image = source.Copy();
            Image<Gray, byte> result = image.CopyBlank();

            List<byte> window = new List<byte>();

            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    window.Clear();

                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            if ((i + y) >= 0 && (j + x) >= 0 && (i + y) < image.Height && (j + x) < image.Width)
                                window.Add(image.Data[i + y, j + x, 0]);
                        }

                    window.Sort();

                    if (window.Count % 2 == 0)
                        result.Data[y, x, 0] = window[window.Count / 2];
                    else
                        result.Data[y, x, 0] = Convert.ToByte((window[window.Count / 2] + window[window.Count / 2 - 1]) / 2);
                }

            return result;
        }

        //Оконный фильтр
        public Image<Bgr, byte> GetWindowFilter(int[,] matrix)
        {
            //Image<Gray, byte> gray = sourceImage.Convert<Gray, byte>();
            Image<Bgr, byte> result = sourceImage.CopyBlank();
            for (int ch= 0; ch<3; ch++)
            {
                for (int y = 1; y < sourceImage.Height - 1; y++)
                    for (int x = 1; x < sourceImage.Width - 1; x++)
                    {
                        int r = 0;

                        for (int i = -1; i < 2; i++)
                            for (int j = -1; j < 2; j++)
                            {
                                r += sourceImage.Data[i + y, j + x, ch] * matrix[i + 1, j + 1];
                            }

                        if (r > 255) r = 255;
                        if (r < 0) r = 0;

                        result.Data[y, x, ch] = (byte)r;
                    }
            }
            
            return result;

        }

        //Акварельный фильтр
        public Image<Bgr, byte> AquarelFilter(int bright, int contrast, Image<Bgr, byte> maskImage, int weight1, int weight2, int medianLevel)
        {
            Image<Bgr, byte> processedImage = sourceImage.Copy();
            processedImage = GetBrightImage(bright, processedImage);
            processedImage = GetContrastImage(contrast, processedImage);
            processedImage = GetMedianBlur(processedImage, medianLevel) ;
            processedImage = GetAddedImage(processedImage, maskImage, weight1, weight2);
            return processedImage;
        }

        //CartoonFilter
        public Image<Bgr, byte> CartoonFilter()
        {
            Image<Bgr, byte> processedImage = sourceImage.Copy();
            Image<Gray, byte> binarizingImage = GetGrayImage();
            binarizingImage = binarizingImage.SmoothMedian(7);
            var edges = binarizingImage.Convert<Gray, byte>(); 
            edges = edges.ThresholdBinary(new Gray(100), new Gray(255));

            return processedImage.And(edges.Convert<Bgr, byte>());
            //return edges;
        }
        private double AddColors(double color1, double color2)
        {
            if (color1 + color2 > 255) 
                return 255; 
            else if(color1 + color2 < 0) 
                return 0; 
            else
                return color1 + color2;
        }

        private double AddColors (double color1, double color2, double color3)
        {
            return AddColors(AddColors(color1, color2), color3);
        }

    }
}
