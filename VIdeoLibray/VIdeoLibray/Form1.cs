using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Threading;
using System.Net.NetworkInformation;


namespace VIdeoLibray
{
    public partial class Form1 : Form
    {
        private string deleteMovieID;
        private ZoomPicture pic;
        private MoreDetails details;
        private string ActualID;
        private string poster_path;
        private string original_title;
        private string release_date;
        private string id;
        private string overview;
        private string runTime;
        private string homePage;
        private string collection;
        private string casts;
        private ArrayList allCast;
        private ArrayList allCastID;
        private ArrayList allCastName;
        private ArrayList singleActor;
        private ArrayList singleActorPoster;
        private string otherLanguageTitle;
        private string actName;
        private Progress prog;
        private string access = "NULL";
        private string dbName;
        private webservice serviceWeb;
        private ActorDataSheet actDat;
        private int tab;
        private string myLibraryActor;
        private string[,] myActorsDetails;
        private string[,] mymoviesDetails;
        private string[,] actorSearch;
        private string[,] movieSearch;
        private string singleID;
        private string apiK = "???";
        private string genres;
        private string posterPath;
        private string otherLanguage;
        private string updateMoviID;
       




        public Form1()
        {
            InitializeComponent();
            this.Text = "Video Library";
            StartPosition = FormStartPosition.CenterScreen;
            tabControl1.SelectedIndexChanged += new EventHandler(TabControl1_SelectedIndexChanged);
            serviceWeb = new webservice();
        }

        private void setAccess(string access)
        {
            this.access = access;
        }

        private string getAccess()
        {
            return access;
        }

        private void setDatabaseName(string dbName)
        {
            this.dbName = dbName;
        }

        private string getDatabaseName()
        {
            return dbName;
        }

        //set genres to moie
        private void setGenres(List<string>gen)
        {
            this.genres = "";
            foreach (string g in gen)
            {
                genres += "," + g;
            }

        }
        //get genres to selected movie to upload sql in string
        private string getGenres()
        {
            return genres;
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {

            }
            else
            {
                MovieSearch(textBox1.Text);//find movies for name(s)
            }
                
        }


        //search for movies for similar name(s)
        public async Task MovieSearch(string search)
        {
            prog = new Progress("Search movie : ",search);
            prog.progress();
            prog.Show();
            prog.progress();
            List<string> title = new List<string>();
            List<string> idss = new List<string>();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var newbaseAddress = new Uri("https://api.themoviedb.org/3/search/movie?api_key=" + apiK + "&query=");
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                // api_key can be requestred on TMDB website
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress + search))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<RootObject>(responseData);
                    foreach (var result in model.results)
                    {
                        // All movies build up movies drop down with the similar movie names
                        title.Add(result.title + " " + result.release_date);
                        idss.Add("" + result.id);
                        prog.updateLabel("Found : " + result.title);
                        Application.DoEvents();

                    }
                   
                }
            }
            fillComboBox(comboBox2 ,buildTwoDArray(title, idss));
            setMovieSearch(buildTwoDArray(title, idss));
            fillComboBox(comboBox1, buildTwoDArray(title, idss));
            prog.Close();
        }



        //Selected movie ID use for search
        private void setActualID(string id)
        {
            ActualID = id;
        }

        //Selected movie ID use for search
        private string getActualID()
        {
            return ActualID;
        }

        //Get details for movies by actor
        public async Task searchByActor(string search)
        {
 
            string castid = "";
            List<string> title = new List<string>();
            List<string> actList = new List<string>();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string url = "http://api.tmdb.org/3/search/person?api_key="+ apiK + "&query=";
            var newbaseAddress = new Uri(url);
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress + search))
                {
                    //Set the selected movie details
                    string responseData = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<RootObject>(responseData);
                    foreach (var result in model.results)
                    {
                        actList.Add(""+result.id);
                     }
                }
            }
            int count = actList.Count;
            if(count == 0)
            {
                MessageBox.Show("Can't find actor ! Please check you input !");
            }
            else if(count == 1)
            {
                foreach (var a in actList)
                {
                    castid = "" + a;
                }
                await searchByActorID(castid);
                
            }
            else 
            {
                prog = new Progress("Search for actors :",textBox20.Text);
                prog.progress();
                prog.Show();
                prog.progress();
                string act = getActName();
                List<string> names = new List<string>();
                List<string> idss = new List<string>();
                //search for a given actor details
                foreach (var a in actList)
                {
                    string ur = "http://api.themoviedb.org/3/person/" + a + "?api_key=" + apiK;
                   
                    var personUrl = new Uri(ur);
                    using (var httpClient = new HttpClient { BaseAddress = personUrl })
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                        using (var response = await httpClient.GetAsync(httpClient.BaseAddress))
                        {
                            //Set the selected movie details
                            string actorData = await response.Content.ReadAsStringAsync();
                            var actorDetails = JsonConvert.DeserializeObject<RootObject>(actorData);
                            names.Add(actorDetails.name);
                            idss.Add(a);
                                 prog.updateLabel("Found : " + actorDetails.name);
                                 Application.DoEvents();
                        }
                    }
                }
                setActorsSearch(buildTwoDArray(names, idss));
                fillComboBox(comboBox7, buildTwoDArray(names, idss));
                prog.Close();
            }
        }



        //get movies for actor ID 
        //Get details for movies by actor
        public async Task searchByActorID(string search)
        {
            prog = new Progress("Search movies for", comboBox7.Text);
            prog.progress();
            prog.Show();
            prog.progress();
            List<string> title = new List<string>();
            List<string> idss = new List<string>();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string url = "http://api.themoviedb.org/3/discover/movie?api_key="+ apiK + "&with_cast="+search;
            var newbaseAddress = new Uri(url);
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress))
                {
                    //Set the selected movie details
                    string responseData = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<RootObject>(responseData);
                    foreach (var result in model.results)
                    {
                        title.Add(result.original_title);
                        idss.Add("" + result.id);
                        prog.updateLabel("Found : " + result.original_title);
                        Application.DoEvents();
                    }
                }
            }
            //Fill combo box
            setMovieSearch(buildTwoDArray(title, idss));
            fillComboBox(comboBox1, buildTwoDArray(title, idss));
            prog.Close();
        }

        //Get details for single movie
        public async Task SingleMovieSearch(string search)
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var poster_Uri = "";
            List<string> list = new List<string>();
            
            string url = "http://api.themoviedb.org/3/movie/" +search +"?api_key=" + apiK;
            var newbaseAddress = new Uri(url);
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress))
                {
                    //Set the selected movie details
                    string responseData = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<RootObject>(responseData);
                    
                        poster_Uri = obj.poster_path;
                        setTitle(obj.original_title);
                        setRelease(obj.release_date);
                        setID("" + obj.id);
                        setOverview(obj.overview);
                        setRunTime(obj.runtime);
                        setHomePage(obj.homepage);
                        setCollection(obj.revenue);
                    
                   
                    foreach (var result in obj.genres)
                    {
                        list.Add(result.name);
                    }
                    await getCasts(search);
                    
                }
            }
            setGenres(list);
            if(string.IsNullOrEmpty(poster_Uri))
            {
                setPoster_path("/pictures/no_image.jpg");
            }
            else
            {
                setPoster_path("http://image.tmdb.org/t/p/w500" + poster_Uri);
            }
            

            if (getPoster_path().Length <= 30)
            {
                pictureBox1.Image = LoadDefaultPicture.getDefaultPicture();
                pictureBox2.Image = LoadDefaultPicture.getDefaultPicture();
                pictureBox3.Image = LoadDefaultPicture.getDefaultPicture();
                pictureBox4.Image = LoadDefaultPicture.getDefaultPicture();
            }
            else
            {
                if (getTab() == 0)
                {
                    pictureBox1.ImageLocation = getPoster_path();
                    pictureBox2.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox3.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox4.Image = LoadDefaultPicture.getDefaultPicture();
                }
                else if (getTab() == 1)
                {
                    
                    pictureBox1.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox2.ImageLocation = getPoster_path();
                    pictureBox3.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox4.Image = LoadDefaultPicture.getDefaultPicture();
                }
                else if (getTab() == 2)
                {
                    pictureBox1.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox2.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox3.ImageLocation = getPoster_path();
                    pictureBox4.Image = LoadDefaultPicture.getDefaultPicture();
                }
                else if (getTab() == 4)
                {
                    pictureBox1.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox2.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox3.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox4.ImageLocation = getPoster_path();
                }

            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            getDetails();

        }
       

        //Get details for single movie
        public async Task getCasts(string search)
        {
            ArrayList castName = new ArrayList();
            ArrayList actID = new ArrayList();
            ArrayList castList = new ArrayList();
            ArrayList actPoster = new ArrayList();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string url = "http://api.themoviedb.org/3/movie/" + search + "/casts?api_key=" + apiK;
            
            var newbaseAddress = new Uri(url);
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var res = await httpClient.GetAsync(httpClient.BaseAddress))
                {
                    //Set the selected movie details
                    string responseData = await res.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<RootObject>(responseData);
                    foreach (var result in model.cast)
                    {
                        actID.Add("" + result.id);
                        castName.Add("" + result.name);
                        if(string.IsNullOrEmpty(result.profile_path))
                        {                           
                                actPoster.Add("/pictures/no_image.jpg");
                        }
                        else
                        {
                            actPoster.Add("http://image.tmdb.org/t/p/w500" + result.profile_path);
                        }
                        
                        castList.Add("'" + result.character + "'" + " - " + result.name);
                    }
                    setSingleActorNames(castName);
                    setCastList(castList);
                    setActIDS(actID);
                    setSingleActorPoster(actPoster);
                }
            }
         
        }

        //set single actor names in selected movie
        private void setSingleActorPoster(ArrayList a)
        {
            singleActorPoster = a;
        }

        //get single actor names in selected movie
        private ArrayList getSingleActorPoster()
        {
            return singleActorPoster;
        }

        //set single actor names in selected movie
        private void setSingleActorNames(ArrayList a)
        {
            singleActor = a;
        }

        //get single actor names in selected movie
        private ArrayList getSingleActorNames()
        {
            return singleActor;
        }
        //set a single name
        private void setActorToShow(string id)
        {          
 
            singleID = id;
        }
        private string getActorToShow()
        {
            return singleID;
        }
  
        //set all cast id for a selected movie
        private void setActIDS(ArrayList allCastID)
        {
            this.allCastID = allCastID;
        }
        //get all castfor a selected movie
        private ArrayList getActIDS()
        {
            return allCastID;
        }

        //set all cast name for a selected movie
        private void setActorNames(ArrayList allCastName)
        {
            this.allCastName = allCastName;
        }
        //get all castfor a selected movie
        private ArrayList getActorNames()
        {
            return allCastName;
        }

        private void setCastList(ArrayList a)
        {
            allCast = a;
        }

        private ArrayList getCastList()
        {
            return allCast;
        }

        //Dropbox selecting single movie from list
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setTitle(textBox2.Text);
            ArrayList a = getActIDS();
            setActualID(id);
            SingleMovieSearch(movieSearch[comboBox1.SelectedIndex, 1]);
            setTab(0);
        }

        //search tab helper
        private void getDetails()
        {
            if (getTab() == 0)
            {
                textBox2.Text = getTitle();
                textBox11.Text = "";
                textBox3.Text = getRelease();
                textBox10.Text = "";
                textBox4.Text = getID();
                textBox9.Text = "";
                textBox6.Text = getOvewview();
                textBox8.Text = "";
                textBox14.Text = "";
                textBox13.Text = "";
                textBox12.Text = "";
                textBox5.Text = "";
                textBox25.Text = "";
                textBox24.Text = "";
                textBox23.Text = "";
                textBox22.Text = "";
            }
            else if (getTab() == 1)
            {
                textBox14.Text = "";
                textBox13.Text = "";
                textBox12.Text = "";
                textBox5.Text = getOvewview();
                textBox2.Text = "";
                textBox11.Text = getTitle();
                textBox3.Text = "";
                textBox10.Text = getRelease();
                textBox4.Text = "";
                textBox9.Text = getID();
                textBox6.Text = "";
                textBox8.Text = getOvewview();
                textBox25.Text = "";
                textBox24.Text = "";
                textBox23.Text = "";
                textBox22.Text = "";
            }
            else if (getTab() == 2)
            {
                textBox14.Text = getTitle();
                textBox13.Text = getRelease();
                textBox12.Text = getID();
                textBox5.Text = getOvewview();
                textBox2.Text = "";
                textBox11.Text = "";
                textBox3.Text = "";
                textBox10.Text = "";
                textBox4.Text = "";
                textBox9.Text = "";
                textBox6.Text = "";
                textBox8.Text = "";
                textBox25.Text = "";
                textBox24.Text = "";
                textBox23.Text = "";
                textBox22.Text = "";
            }
            else if (getTab() == 3)
            {
                textBox14.Text = "";
                textBox13.Text = "";
                textBox12.Text = "";
                textBox5.Text = getOvewview();
                textBox2.Text = "";
                textBox11.Text = "";
                textBox3.Text = "";
                textBox10.Text = "";
                textBox4.Text = "";
                textBox9.Text = "";
                textBox6.Text = "";
                textBox8.Text = "";
                textBox25.Text = getTitle();
                textBox24.Text = getRelease();
                textBox23.Text = getID();
                textBox22.Text = getOvewview();
            }
            else if (getTab() == 4)
            {
                textBox31.Text = getTitle();
                textBox28.Text = otherLanguageTitle;
            }
        }

        //belong to collection
        private void setCollection(string collection)
        {
            this.collection = collection;
        }

        private string getCollection()
        {
            return collection;
        }

        //run time of movie
        private void setRunTime(string runTime)
        {
            this.runTime = runTime;
        }

        private string getRunTime()
        {
            return runTime;
        }

        //Home page of movie
        private void setHomePage(string homePage)
        {
            this.homePage = homePage;
        }

        private string getHomePage()
        {
            return homePage;
        }

        //Castse of movie
        private void setCasts(string casts)
        {
            this.casts = casts;
        }

        private string getCasts()
        {
            return casts;
        }

        //Overview of movie
        private void setOverview(string overview)
        {
            this.overview = overview;
        }

        private string getOvewview()
        {
            return overview;
        }

        //Movie ID
        private void setID(string id)
        {
            this.id = id;
        }

        private string getID()
        {
            return id;
        }

        //Movie name
        private void setTitle(string original_title)
        {
            string newTile = original_title.Replace(@"'", @"");
            this.original_title = newTile;
        }

        private string getTitle()
        {
            return original_title;
        }

        //Release date
        private void setRelease(string release_date)
        {
            this.release_date = release_date;
        }

        private string getRelease()
        {
            return release_date;
        }


        //Poster
        private void setPoster_path(string poster_path)
        {
            this.poster_path = poster_path;
        }

        private String getPoster_path()
        {
            return poster_path;
        }


        //
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (getTab() == 0)
            {
                if (pic != null)
                {
                    pic.Close();
                }
                pic = new ZoomPicture(getPoster_path(), getTitle());
                pic.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MovieSearchActor(textBox5.Text);
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        //search tab more button
        private void button4_Click(object sender, EventArgs e)
        {
            if (getTab() == 0)
            {
                if(string.IsNullOrEmpty(textBox2.Text))
                {

                }
                else
                {
                    if (details != null)
                    {
                        details.Close();
                    }
                    details = new MoreDetails(getPoster_path(), getTitle(), getRelease(), getRunTime(), getHomePage(), getCollection(), getCasts(), getOvewview(), getCastList());
                    details.Show();
                }
                
            }
        }

        private void setMyLibrarySelectedActor(string id)
        {
            this.myLibraryActor = id;
        }

        private string getMyLibrarySelectedActor()
        {
            return myLibraryActor;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Search by actor button
        private void button15_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox20.Text))
            {

            }
            else
            {
                setActName(textBox20.Text);
                string name = getNameFormat(textBox20.Text);
                searchByActor(name);
            }
           
        }

        private void setActName(string name)
        {
            actName = name;
        }
        private string getActName()
        {
            return actName;
        }
        private string getNameFormat(string str)
        {
            string name = str.Replace(" ", "%20");
            return name;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchByActorID(actorSearch[comboBox7.SelectedIndex, 1]);
        }

        //read file
        private void button5_Click(object sender, EventArgs e)
        {
            string path = "";
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                path = file.FileName;
                textBox7.Text = path;
                List<string> lines = File.ReadLines(path).ToList();
                ArrayList lin = new ArrayList();
                foreach (string a in lines)
                {
                    lin.Add(a);
                }
                fillFromFile(lin);
            }         
        }

        //build library more button
        private void button6_Click(object sender, EventArgs e)
        {
            if (getTab() == 1)
            {
                if(string.IsNullOrEmpty(textBox11.Text))
                {

                }
                else
                {
                    if (details != null)
                    {
                        details.Close();
                    }
                    details = new MoreDetails(getPoster_path(), getTitle(), getRelease(), getRunTime(), getHomePage(), getCollection(), getCasts(), getOvewview(), getCastList());
                    details.Show();
                }
               
            }
        }

        //build library tab find movie
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            MovieSearch(comboBox3.Text);//find movies for name(s)
        }

        //build library combobox to show movies
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            setTitle(textBox11.Text);
            SingleMovieSearch(movieSearch[comboBox2.SelectedIndex, 1]);
            setTab(1);
        }
      
        //build library picture box click
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(getTab()==1)
            {
                if (pic != null)
                {
                    pic.Close();
                }
                pic = new ZoomPicture(getPoster_path(), getTitle());
                pic.Show();
            }
           
        }

       
        
        //delete movie from library
        private void button10_Click(object sender, EventArgs e)
        {
            if (dbName.Equals("???"))
            {
                MessageBox.Show("The 'movie' library is a sample library! \nYou can add movies but can't delete them!\nPlease create another library to try this function");
            }
            else
            {
                if (getAccess().Equals("SUCCESS"))
                {
                    //delete from library
                    DialogResult dialogResult = MessageBox.Show("Are you sure to delete movie from libaray?", "Delete movie", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        //do nothing

                    }
                    else if (dialogResult == DialogResult.Yes)
                    {
                        //do something
                        string ans = "";
                        string id = getMovieIDToDelete();

                        ans = serviceWeb.serviceAction("deleteMovie", dbName, "movieID", id, "", "", "", "", "", "", "", "", "", "", "Search");
                        clearFields();
                    }
                    else
                    {
                        MessageBox.Show("Please log into your library first!");
                    }
                }
            }
        }

        private void clearFields()
        {
            textBox14.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
            textBox5.Text = "";
            pictureBox3.Image = LoadDefaultPicture.getDefaultPicture();
        }

        //Log out button
        private void button12_Click(object sender, EventArgs e)
        {
            //delete from library
            if (getAccess().Equals("SUCCESS"))
            {
                DialogResult dialogResult = MessageBox.Show("Log out ?", "Library " + dbName, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    //do nothing
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    setAccess("NULL");
                    setDatabaseName("");
                    label17.Text = "You are logged out ";
                    textBox21.Text = "No library selected";
                    textBox14.Text = "";
                    textBox13.Text = "";
                    textBox12.Text = "";
                    textBox5.Text = "";
                    pictureBox3.Image = LoadDefaultPicture.getDefaultPicture();
                    pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                    string[,] emp = new string[0,1];
                    fillComboBox(comboBox4, emp);
                    fillComboBox(comboBox5, emp);
                }
            }
            
        }
        //DELETE
        //Actor search button
        private void button3_Click_1(object sender, EventArgs e)
        {
         
        }

        //More details button
        private void button9_Click(object sender, EventArgs e)
        {
            if (getAccess().Equals("SUCCESS"))
            {
                if(string.IsNullOrEmpty(textBox14.Text))
                {
                    //do nothing
                }
                else
                {
                    if (details != null)
                    {
                        details.Close();
                    }
                   
                        if (getTab() == 2)
                        {
                            if (details != null)
                            {
                                details.Close();
                            }
                            details = new MoreDetails(getPoster_path(), getTitle(), getRelease(), getRunTime(), getHomePage(), getCollection(), getCasts(), getOvewview(), getCastList());
                            details.Show();
                        }
                    
                }
               
            }
            else
            {
                MessageBox.Show("Please log into your library first!");
            }
        }

        //Clear actor search drop box
        private void button8_Click(object sender, EventArgs e)
        {
            if (getAccess().Equals("SUCCESS"))
            {
                //clear actor drop box
            }
            else
            {
                MessageBox.Show("Please log into your library first!");
            }
        }

     

        //********************* Webservice  ****************************************
        //Login to database
        private void button11_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(dbName))
            {
                callService("libraryName", "", "", "");
            }
            else
            {

            }

        }

        //Build library add to database button
        private void button7_Click(object sender, EventArgs e)
        {
            add();
        }

        //search page add to database button
        private void button2_Click(object sender, EventArgs e)
        {
            add();
        }

        private void add()
        {
            if(getTab()==0 && checkBox1.Checked)
            {
                otherLanguage = "Not Set";
                callService("", "addActors", "addmovie", "movieMerge");
            }
            else if (getTab() == 1 && checkBox2.Checked)
            {
                otherLanguage = "Not Set";
                callService("", "addActors", "addmovie", "movieMerge");
            }
            else if (getTab() == 3 && checkBox3.Checked)
            {
                otherLanguage = "Not Set";
                callService("", "", "addseries", "");
            }

            if ((getTab() == 0 && !checkBox1.Checked) && string.IsNullOrEmpty(otherLanguage))
            {
                otherLanguage = Prompt.ShowDialog(textBox2.Text, "Add title on other language");
                otherLanguage = RemoveAccents(otherLanguage);
            }
            else if ((getTab() == 1 && !checkBox2.Checked) && string.IsNullOrEmpty(otherLanguage))
            {
                otherLanguage = Prompt.ShowDialog(textBox11.Text, "Add title on other language");
                otherLanguage = RemoveAccents(otherLanguage);
            }
            else if ((getTab() == 3 && !checkBox3.Checked) && string.IsNullOrEmpty(otherLanguage))
            {
                otherLanguage = Prompt.ShowDialog(textBox25.Text, "Add title on other language");
                otherLanguage = RemoveAccents(otherLanguage);
            }


            if ((getTab() == 0 && !checkBox1.Checked) && !string.IsNullOrEmpty(otherLanguage))
            {
                callService("", "addActors", "addmovie", "movieMerge");
            }
            else if ((getTab() == 1 && !checkBox2.Checked) && !string.IsNullOrEmpty(otherLanguage))
            {
                callService("", "addActors", "addmovie", "movieMerge");
            }
            else if ((getTab() == 3 && !checkBox3.Checked) && !string.IsNullOrEmpty(otherLanguage))
            {
                callService("", "", "addseries", "");
            }
      
        }
        //remove accent from other language 
        private static string RemoveAccents(string s)
        {
            Encoding destEncoding = Encoding.GetEncoding("iso-8859-8");

            return destEncoding.GetString(
                Encoding.Convert(Encoding.UTF8, destEncoding, Encoding.UTF8.GetBytes(s)));
        }

        //upload details to MySql database 
        private void callService(string mainPost, string actor,string movie, string merge)
        {
           
            Boolean addMovie = false;
            if(prog!=null)
            {
                prog.Close();
            }
            prog = new Progress("Connecting to library : ", dbName);
            prog.progress();
            prog.Show();
            prog.progress();
            string ans = "";
            //Authentication !! - Log in
            if (mainPost.Equals("libraryName"))// string what, string whatName,
            {
                string logAns = "";
                prog.updateLabel("Checking credentials !");
                if (textBox15.Text.Length > 0 && textBox16.Text.Length > 0)
                {
                    logAns = serviceWeb.serviceAction(mainPost, textBox15.Text, "password", textBox16.Text, "table", textBox15.Text, "", "", "", "", "", "", "", "", "Login");
                    setAccess(logAns);
                    if (getAccess().Equals("SUCCESS"))
                    {
                        setDatabaseName(textBox15.Text);
                        textBox21.Text = dbName;
                        label17.Text = "You are logged in to \" " + dbName + " \"" +" video library";
                        textBox15.Text = "";
                        textBox16.Text = "";
                    }
                    else if (getAccess().Equals("FAILED"))
                    {
                        MessageBox.Show("Access Denied to \" " + textBox15.Text + " \"" +" video library");
                        label17.Text = "Access Denied to \" " + textBox15.Text + " \"" +" video library";
                    }
                    else
                    {
                        MessageBox.Show(getAccess());
                    }
                }
                else
                {
                    MessageBox.Show("Please fill up Library name & Password ! ");
                }
              
            }
            else if(getAccess().Equals("SUCCESS") && actor.Equals("addActors") && movie.Equals("addmovie") && merge.Equals("movieMerge"))//all other service call
            {
                if (movie.Equals("addmovie"))//adding movie
                {
                    prog.updateLabel("Adding movie : " + getTitle());
                    string id = getID();
                    ans = serviceWeb.serviceAction(movie, dbName, "movieID", id, "movieName", getTitle(), "release", getRelease(), "genres", getGenres(), "poster", getPoster_path(), "otherLanguage", getOtherLanguage(), "Search");
                    prog.updateLabel("Adding movie : " + getTitle());
                    Application.DoEvents();
  
                    if(ans.Equals("Success added new movie"))
                    {
                        addMovie = true;
                    }
                    else
                    {
                        addMovie = false;
                        MessageBox.Show(ans);
                    }
                }
                // prog.updateLabel("Adding actors");
                if (getTab() == 0 || getTab() == 1)
                {
                    if (actor.Equals("addActors") && addMovie)//adding actors
                    {
                        string[] ids = (string[])getActIDS().ToArray(typeof(string));
                        string[] nam = (string[])getSingleActorNames().ToArray(typeof(string));
                        string[] actPic = (string[])getSingleActorPoster().ToArray(typeof(string));
                        int len = ids.Length; ;
                        for (int i = 0; i < len; i++)
                        {
                            string tempID = ids[i];
                            string tempName = nam[i];
                            string tempPoster = actPic[i];
                            ans = serviceWeb.serviceAction(actor, dbName, "actorID", tempID, "actorName", tempName, "actorPoster", tempPoster, "", "", "", "", "", "", "Search");
                            prog.updateLabel(ans);
                            Application.DoEvents();
                        }

                    }

                    if (merge.Equals("movieMerge") && addMovie)//adding actors and movie to cross table
                    {
                        string id = getID();
                        string[] ids = (string[])getActIDS().ToArray(typeof(string));
                        int len = ids.Length; ;
                        for (int i = 0; i < len; i++)
                        {

                            prog.updateLabel("Updating library : " + dbName);
                            ans = serviceWeb.serviceAction(merge, dbName, "movieID", id, "actorID", ids[i], "", "", "", "", "", "", "", "", "Search");
                            prog.updateLabel(ans);
                            Application.DoEvents();
                        }
                        prog.updateLabel("Library updated");

                    }
                }

                


                prog.updateLabel("FINISH");
                prog.Close();
            }
            else if (getAccess().Equals("SUCCESS") && movie.Equals("addseries") && getTab() == 3)
            {
                string tit = getTitle();
                ans = serviceWeb.serviceAction(movie, dbName, "seriesID", id, "seriesName", tit, "release", getRelease(), "genres", getGenres(), "poster", getPoster_path(), "otherLanguage", getOtherLanguage(), "Search");
                MessageBox.Show(ans);
            }
            else //(!getAccess().Equals("SUCCESS"))//if logged in
            {
                MessageBox.Show("Please log into your library first!");
            }
            prog.Close();
            setOtherLanguage("");

        }

        //Read from folder button
        private void button16_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                textBox7.Text = fbd.SelectedPath;
                ArrayList lin = new ArrayList();
                DirectoryInfo d = new DirectoryInfo(fbd.SelectedPath);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles(); //Getting Text files
                string str = "";
                foreach (FileInfo file in Files)
                {
                    str = Path.GetFileNameWithoutExtension(file.Name);
                    lin.Add(str);
                }
                fillFromFile(lin);

            }
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {

        }

        //delete library button
        private void button14_Click(object sender, EventArgs e)
        {
            if (dbName.Equals("???")|| dbName.Equals("???"))
            {
                MessageBox.Show("The 'movie' library is a sample library! \nYou can't delete it!\nPlease create another library to try this function");
            }
            else
            { 
                if (getAccess().Equals("SUCCESS"))
                {

                    DialogResult dialogResult = MessageBox.Show("Are you sure to delete library : " + dbName, "Delete library", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        //do nothing
                    }
                    else if (dialogResult == DialogResult.Yes)
                    {
                        //do create
                        string logAns = "NOTHING";
                        logAns = serviceWeb.serviceAction("deleteLibrary", dbName, "", "", "", "", "", "", "", "", "", "", "", "", "Search");
                        MessageBox.Show(logAns);
                        setAccess("NULL");
                        label17.Text = "Library " + dbName + " deleted !!!";
                        textBox21.Text = "No library selected";
                        setDatabaseName("");
                        pictureBox3.Image = LoadDefaultPicture.getDefaultPicture();
                        pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                        string[,] emp = new string[0, 1];
                        fillComboBox(comboBox4, emp);
                        fillComboBox(comboBox5, emp);
                    }
                }
            }
        }

        //Tab control
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                setTab(0);
            else if (tabControl1.SelectedIndex == 1)
                setTab(1);
            else if (tabControl1.SelectedIndex == 2)
                setTab(2);
            else if (tabControl1.SelectedIndex == 3)
                setTab(3);
            else if (tabControl1.SelectedIndex == 4)
                setTab(4);
            else if (tabControl1.SelectedIndex == 5)
                setTab(5);
        }

        private void setTab(int n)
        {
            tab = n;
        }

        private int getTab()
        {
            return tab;
        }


        //CREATE library
        private void button13_Click(object sender, EventArgs e)
        {
            //do create
            string logAns = "NOTHING";
            logAns = serviceWeb.serviceAction("createLibrary", textBox18.Text, "userName", textBox18.Text, "PasswWord", textBox17.Text,"","", "", "", "", "", "", "", "Search");
            MessageBox.Show(logAns);
            textBox18.Text = "";
            textBox17.Text = "";
        }

        
        //Search for movies button
        private void button18_Click(object sender, EventArgs e)
        {
            //search for movie -> get name from textBox26 and -> send to api
            similarMovies(textBox26.Text);
        }
        //search for actors button
        private void button8_Click_1(object sender, EventArgs e)
        {
            //search for actors -> get name from textBox27 and -> send to api
            searchSimilarActors(textBox27.Text);


        }

        //load all cotors for selected library
        private async Task searchSimilarActors(string name)
        {
            List<string> names = new List<string>();
            List<string> idss = new List<string>();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var newbaseAddress = new Uri("???");
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress))
                {

                    string responseData = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<RootObject>(responseData);
                    foreach (var result in model.results)
                    {
                        names.Add(result.name);
                        idss.Add(result.actorId);
                        prog.updateLabel("Found : " + result.name);
                        Application.DoEvents();

                    }

                }
            }
            fillComboBox(comboBox4, buildTwoDArray(names, idss));
            setMyActors(buildTwoDArray(names, idss));
        }

        //load all movies for logged in library
        private async Task similarMovies(string name)
        {
            List<string> names = new List<string>();
            List<string> idss = new List<string>();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string murl = "???";
            var newbaseAddress = new Uri(murl);
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress))
                {

                    string responseData = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<RootObject>(responseData);
                    foreach (var result in model.results)
                    {
                        names.Add(result.name + " " + result.releaseDate);
                        idss.Add(result.movieid);
                    }

                }
            }
            if(getTab()==2)
                fillComboBox(comboBox5, buildTwoDArray(names, idss));
            if (getTab() == 4)
                fillComboBox(comboBox8, buildTwoDArray(names, idss));
            setMyMovies(buildTwoDArray(names, idss));

        }

        //DELETE
        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }
        //my library picture
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (getTab() == 2)
            {
                if (pic != null)
                {
                    pic.Close();
                }
                pic = new ZoomPicture(getPoster_path(), getTitle());
                pic.Show();
            }
        }

        //show selected actor data sheet
        private void button3_Click_2(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(comboBox4.Text))
            {

            }
            else
            {
                showActorDataSheet("Actor details", getActorToShow());
            }
            
        }

        //show selected actor data sheet
        private void showActorDataSheet(string name,string id)
        {
            if (actDat != null)
                    {
                         actDat.Close();
                    }
                     actDat = new ActorDataSheet(comboBox5.Text, id);
                     actDat.Show();
                
            
            
        }

        //show selected actor data sheet
        private void button17_Click_1(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(comboBox7.Text))
            {
                //do nothing
            }
            else
            {
                
                showActorDataSheet("Actor details", actorSearch[comboBox7.SelectedIndex, 1]);
            }
            
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

      
        //build 2D array
        private string[,] buildTwoDArray(List<string> names, List<string> idss)
        {
            string[,] details = new string[names.Count, idss.Count+1];

            for (int i = 0; i < names.Count; i++)
            {
                details[i, 0] = names[i];
                details[i, 1] = idss[i];
            }
            return details;
        }

        //serach movie from library
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            SingleMovieSearch(mymoviesDetails[comboBox5.SelectedIndex, 1]);
            setMovieIDToDelete(mymoviesDetails[comboBox5.SelectedIndex, 1]);
            setTab(2);
        }

        //set the movie id to delete
        private void setMovieIDToDelete(string id)
        {
            deleteMovieID = id;
        }
        //get movie id to delete
        private string getMovieIDToDelete()
        {
            return deleteMovieID;
        }

        //get the selected actor id
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            setActorToShow(myActorsDetails[comboBox4.SelectedIndex, 1]);
        }
      
        private void setMyActors(string[,] det)
        {
            this.myActorsDetails = det;
        }
        private string[,] getMyActors()
        {
            return myActorsDetails;
        }
        private void setMyMovies(string[,] det)
        {
            this.mymoviesDetails = det;
        }
        private string[,] getMyMovies()
        {
            return mymoviesDetails;
        }
        private void setActorsSearch(string[,] det)
        {
            this.actorSearch = det;
        }
        private string[,] getActorsSearch()
        {
            return actorSearch;
        }
        private void setMovieSearch(string[,] det)
        {
            this.movieSearch = det;
        }
        private string[,] getMovieSearch()
        {
            return movieSearch;
        }

        //fill dop box from txt file
        private void fillFromFile(ArrayList a)
        {
            string[] stringArray = (string[])a.ToArray(typeof(string));
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.Items.Clear();
            comboBox3.BeginUpdate(); // <- Stop painting
            try
            {
                foreach (string item in stringArray)
                {
                    comboBox3.Items.Add(item);
                }
            }
            finally
            {
                comboBox3.EndUpdate(); // <- Finally, repaint if required
            }
        }

 
        //fill comboBoxes
        private void fillComboBox(ComboBox comb, string[,] det)
        {
            comb.DropDownStyle = ComboBoxStyle.DropDownList;
            comb.Items.Clear();
            comb.BeginUpdate(); // <- Stop painting
            try
            {
                for (int i = 0; i < det.GetLength(0); i++)

                    comb.Items.Add(det[i, 0]);

            }
            finally
            {
                comb.EndUpdate(); // <- Finally, repaint if required
            }
        }

        //option to add titile in other language can use for indexing at search
        private void button19_Click(object sender, EventArgs e)
        {
            otherLanguage = Prompt.ShowDialog(textBox2.Text, "Add title on other language");

        }

       

        public void setOtherLanguage(string title)
        {
            otherLanguage = title;
        }

        private string getOtherLanguage()
        {
            return otherLanguage;
        }
        //add title in other language
        private void button20_Click(object sender, EventArgs e)
        {
            otherLanguage = Prompt.ShowDialog(textBox11.Text, "Add title on other language");
        }

        //Tv series serch button
        private void button21_Click(object sender, EventArgs e)
        {
            tvShowSearch(textBox19.Text);
        }

        //method for serach tv series
      
        public async Task tvShowSearch(string search)
        {
            prog = new Progress("Search TV show : ", search);
            prog.progress();
            prog.Show();
            prog.progress();
            List<string> title = new List<string>();
            List<string> idss = new List<string>();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var newbaseAddress = new Uri("https://api.themoviedb.org/3/search/tv?api_key=" + apiK + "&query=");
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                // api_key can be requestred on TMDB website
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress + search))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<RootObject>(responseData);
                    foreach (var result in model.results)
                    {
                        // All movies build up movies drop down with the similar movie names
                        title.Add(result.original_name + " " + result.first_air_date);
                        idss.Add("" + result.id);
                        prog.updateLabel("Found : " + result.original_name);
                        Application.DoEvents();

                    }

                }
            }
            fillComboBox(comboBox6, buildTwoDArray(title, idss));
            setMovieSearch(buildTwoDArray(title, idss));
            prog.Close();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            setTitle(textBox25.Text);
            ArrayList a = getActIDS();
            setActualID(id);
            singleTvShowDetails(movieSearch[comboBox6.SelectedIndex, 1]);
            setTab(3);
         
        }

        //Get details for single movie
        public async Task singleTvShowDetails(string id)
        {

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var poster_Uri = "";
            List<string> list = new List<string>();

            string url = "http://api.themoviedb.org/3/tv/" + id + "?api_key=" + apiK;
            var newbaseAddress = new Uri(url);
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress))
                {
                    //Set the selected movie details
                    string responseData = await response.Content.ReadAsStringAsync();

                    var obj = JsonConvert.DeserializeObject<RootObject>(responseData);
                    
                     poster_Uri = obj.poster_path;       
                     setTitle(obj.original_name);
                    setRelease(obj.first_air_date);
                    setID("" + obj.id);
                    setOverview(obj.overview);
                    setHomePage(obj.homepage);
                    list.Add("Series");
    
                }
            }
            setGenres(list);
            setPoster_path("http://image.tmdb.org/t/p/w500" + poster_Uri);

            if (getPoster_path().Length <= 30)
            {
                pictureBox4.Image = LoadDefaultPicture.getDefaultPicture();
            }
            else
            {
                    pictureBox4.ImageLocation = getPoster_path();

            }

            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            getDetails();

        }
        //series more details
        private void button23_Click(object sender, EventArgs e)
        {
            if (getTab() == 3)
            {
                if (string.IsNullOrEmpty(textBox25.Text))
                {

                }
                else
                {
                    if (details != null)
                    {
                        details.Close();
                    }
                    ArrayList l = new ArrayList();
                    details = new MoreDetails(getPoster_path(), getTitle(), getRelease(), getRunTime(), getHomePage(), getCollection(), getCasts(), getOvewview(), l);
                    details.Show();
                }

            }
        }

        //Add series to database
        private void button24_Click(object sender, EventArgs e)
        {
            add();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (getTab() == 3)
            {
                if (pic != null)
                {
                    pic.Close();
                }
                pic = new ZoomPicture(getPoster_path(), getTitle());
                pic.Show();
            }
        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        //change title  serach movie button
        private void button28_Click(object sender, EventArgs e)
        {
            similarMovies(textBox32.Text);
        }

        //change title tab combobox
        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
             updateDetails(mymoviesDetails[comboBox8.SelectedIndex, 1]);
        }

        //load all movies for logged in library
        private async Task updateDetails(string id)
        {
            List<string> names = new List<string>();
            List<string> idss = new List<string>();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string murl = "???";
            var newbaseAddress = new Uri(murl);
            using (var httpClient = new HttpClient { BaseAddress = newbaseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                using (var response = await httpClient.GetAsync(httpClient.BaseAddress))
                {

                    string responseData = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<RootObject>(responseData);
                      foreach (var result in obj.results)
                      {
                        textBox33.Text = result.original_title;
                        textBox34.Text = result.other_Language;
                        textBox31.Text = result.original_title;
                        textBox28.Text = result.other_Language;
                        updateMoviID   = result.movieid;
                        pictureBox5.ImageLocation = result.poster_path;
                    }

                }
            }
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;

        }
        //Update movie details
        private void button29_Click(object sender, EventArgs e)
        {
            string newTiltle = textBox33.Text;
            string newOtherTiltle = textBox34.Text;
            if (string.IsNullOrEmpty(textBox33.Text)|| string.IsNullOrEmpty(textBox34.Text))
            {
                MessageBox.Show("Fill up titles before updating");
            }
            else
            {
                string  ans = serviceWeb.serviceAction("update", dbName, "newTitle", textBox33.Text, "newOthertitle", textBox34.Text, "movieID", updateMoviID, "", "", "", "", "", "", "Search");
                textBox31.Text = textBox33.Text;
                textBox28.Text = textBox34.Text;
                MessageBox.Show(ans);
            }



        }
    }
}

