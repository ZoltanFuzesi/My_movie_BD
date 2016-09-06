using System;
using System.Collections;
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
    public partial class MoreDetails : Form
    {
        private string title;
        private string year;
        private string runTime;
        private string homePage;
        private string collection;
        private string overview;
        private string imgPath;
        private ArrayList casts;

        public MoreDetails(string imgPath, string title, string year, string runTime, string homePage, string collection, string casts, string overview, ArrayList a)
        {
            InitializeComponent();
            this.imgPath = imgPath;
            this.title = title;
            this.year = year;
            this.runTime = runTime;
            this.homePage = homePage;
            this.collection = collection;
            this.casts = a;
            this.overview = overview;
            this.Text = title;
            showForm();
            
        }
        private void showForm()
        {
            if(setFields())
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                this.Show();
            }
            else
            {

            }
        }
        private Boolean setFields()
        {
            Boolean check;
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please select a movie first");
                check = false;
            }
            else
            {
                textBox1.Text = title;
                textBox1.SelectionStart = 0;
                textBox1.ReadOnly = true;
                textBox2.Text = year;
                textBox2.ReadOnly = true;
                textBox3.Text = runTime;
                textBox3.ReadOnly = true;

                textBox7.ReadOnly = true;
                textBox4.Text = collection;
                textBox4.ReadOnly = true;
                textBox5.Text = getCastStr();
                textBox5.ReadOnly = true;
                textBox6.Text = overview;
                textBox6.ReadOnly = true;
                try
                {
                    textBox7.Text = homePage.Length <= 0 ? "N/A" : homePage;
                    
                }
                catch (Exception) { }
                // pictureBox1.ImageLocation = imgPath;
                if (string.IsNullOrEmpty(imgPath))
                {
                    pictureBox1.Image = LoadDefaultPicture.getDefaultPicture();
                }
                else if (imgPath.Length <= 30)
                {
                    pictureBox1.Image = LoadDefaultPicture.getDefaultPicture();
                }
                else
                {
                    pictureBox1.ImageLocation = imgPath;
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                check = true;
            }
            return check;
        }

        private string getCastStr()
        {
            string newStr = "";
            foreach(string str in casts)
            {
                newStr += str + " , ";
            }
            return newStr;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void MoreDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
