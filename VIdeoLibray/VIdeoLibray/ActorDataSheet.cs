using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VIdeoLibray
{
    public partial class ActorDataSheet : Form
    {

        private string id;
        private string name;
        private string biography;
        private string birthday;
        private string deathday = "N/A";
        private string place_of_birth;
        private string profile_path;
        private string homepage;


        public ActorDataSheet(string title, string id )
        {
            InitializeComponent();
            this.id = id;
            this.Text = title;
            searchDetails(id);
            StartPosition = FormStartPosition.CenterScreen;
            pictureBox1.Image = LoadDefaultPicture.getDefaultPicture();



        }

        private async Task searchDetails(string id)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var poster_Uri = "";
                      
            string url = "http://api.themoviedb.org/3/person/" + id + "?api_key=???";
            var newbaseAddress = new Uri(url);
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress))
                {
                    //Set the selected movie details
                    string responseData = await response.Content.ReadAsStringAsync();
                    // Console.WriteLine("Response " + responseData);
                    var obj = JsonConvert.DeserializeObject<RootObject>(responseData);
                    name = obj.name;
                    biography = obj.biography;
                    birthday = obj.birthday;
                    deathday = obj.deathday;
                    homepage = obj.homepage;
                    place_of_birth = obj.place_of_birth;
                    profile_path = obj.profile_path;



                }
            }
            setDetails();
        }

        private void setDetails()
        {

            
            string url = "http://image.tmdb.org/t/p/w500" + profile_path;
            textBox1.Text = name;
            textBox2.Text = birthday;
            textBox6.Text = deathday;
            textBox3.Text = place_of_birth;
            textBox4.Text = homepage;
            textBox5.Text = biography;
            pictureBox1.ImageLocation = url;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void ActorDataSheet_Load(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Boigraphy_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Sorry there is no homepage");
            }
            else
            {
                System.Diagnostics.Process.Start(textBox4.Text);
            }
            
        }
    }
}
