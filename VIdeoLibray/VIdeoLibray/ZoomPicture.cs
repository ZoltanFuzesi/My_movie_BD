using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VIdeoLibray
{
    public partial class ZoomPicture : Form
    {
        private string img;
        public ZoomPicture(string img,string name)
        {
            InitializeComponent();
            this.img = img;
            SetImage(img);
            this.Text = name;
            StartPosition = FormStartPosition.CenterScreen;

        }

        private void SetImage(string i)
        {
            try
            {
            if (i.Length <= 30)
            {
                Console.WriteLine("No image " + i.Length);

                pictureBox1.Image = LoadDefaultPicture.getDefaultPicture();
            }
            else
                pictureBox1.ImageLocation = i;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception) { }
            
        }

        private void ZoomPicture_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
