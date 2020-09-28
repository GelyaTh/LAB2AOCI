using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV; 
using Emgu.CV.CvEnum; 
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace AOCI_Lab2
{
    public partial class Form1 : Form
    {

        
        private PhotoShop photoshop;

        public Form1()
        {
            InitializeComponent();
        }

        public Image<Bgr, byte> LoadImage()
        {
            Image<Bgr, byte> loadedImage;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();  

            if (result == DialogResult.OK) 
            {
                string fileName = openFileDialog.FileName;
                loadedImage = new Image<Bgr, byte>(fileName);
                return loadedImage.Resize(640, 480, Inter.Linear);
            }
            else
                return LoadImage().Resize(640, 480, Inter.Linear);
        }
        //Загрузка
        private void button1_Click(object sender, EventArgs e)
        {
            photoshop = new PhotoShop(LoadImage());
            imageBox1.Image = photoshop.GetSourceImage().Resize(640, 480, Inter.Linear);
        }

        //Канал
        private void button2_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetChannels(checkBox1.Checked, checkBox2.Checked, checkBox3.Checked).Resize(640, 480, Inter.Linear);
        }

        //Чёрно-белый
        private void button3_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetGrayImage().Resize(640, 480, Inter.Linear);
        }

        //Сепия
        private void button4_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetSepiaImage().Resize(640, 480, Inter.Linear);
        }

        
        //Яркость
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetBrightImage(trackBar1.Value).Resize(640, 480, Inter.Linear);
        }

        //Контраст
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetContrastImage(trackBar2.Value).Resize(640, 480, Inter.Linear);
        }

        //Дополнение
        private void button7_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetAddedImage(LoadImage());
        }

        //Исключение
        private void button8_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetSubtractedImage(LoadImage());
        }

        //Пересечение
        private void button9_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetIntersectedImage(LoadImage());
        }

        //HSV
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetHSVImage(trackBar3.Value).Resize(640, 480, Inter.Linear);
        }

        //Медианное размытие
        private void button5_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.GetMedianBlur().Resize(640, 480, Inter.Linear);
        }

        //Оконный фильтр
        private void button6_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            int[,] w = new int[3, 3]
             {
                   {(int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value},
                   {(int)numericUpDown4.Value, (int)numericUpDown5.Value, (int)numericUpDown6.Value},
                   {(int)numericUpDown7.Value, (int)numericUpDown8.Value, (int)numericUpDown9.Value}
            };
            imageBox2.Image = photoshop.GetWindowFilter(w).Resize(640, 480, Inter.Linear);
        }

        //Акварельный фильтр
        private void button10_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.AquarelFilter(trackBar1.Value, trackBar2.Value, LoadImage(), trackBar4.Value, trackBar5.Value).Resize(640, 480, Inter.Linear);
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            trackBar5.Value = 10 - trackBar4.Value;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            trackBar4.Value = 10 - trackBar5.Value;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (photoshop is null) return;
            imageBox2.Image = photoshop.CartoonFilter();
        }
    }
}
