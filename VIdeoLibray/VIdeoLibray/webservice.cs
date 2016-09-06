using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VIdeoLibray
{
    class webservice
    {
        private Progress prog;
        public webservice()
        {

        }
		
        public string serviceAction(string action, string dbName, string postOne, string ID, string postTwo,  string name, string postThree, string release, string postFour, string genres, string postFive, string poster, string postSix, string otherLanguage, string url)
        {
            string respond = "Check your internet connection";
            string urlAddress = "???" + url +".php";

            using (WebClient client = new WebClient())
            {
                NameValueCollection postData = new NameValueCollection()
                     {
                    
                        { action, dbName },
                        { postOne, ID }, 
                        { postTwo , name},
                        { postThree , release},
                        { postFour , genres},
                        { postFive , poster},
                        { postSix , otherLanguage}
                    };


                try
                {
                    respond = Encoding.UTF8.GetString(client.UploadValues(urlAddress, postData));

                }
                catch (Exception) { MessageBox.Show("Check the internet connection"); }

            }
            return respond;
            
        }

        public string serviceRegister(string action, string dbName, string postOne, string ID, string postTwo, string name, string url,string macOneAction,string macOne, string macTwoAction, string macTwo, string macThreeAction, string macThree)
        {

            string respond = "Check your internet connection";
            string urlAddress = "???" + url + ".php";

            using (WebClient client = new WebClient())
            {
                NameValueCollection postData = new NameValueCollection()
                     {
                        { action, dbName },
                        { postOne, ID }, //at login use for password
                        { postTwo , name}//at login use for table ???
                    };


                try
                {
                    respond = Encoding.UTF8.GetString(client.UploadValues(urlAddress, postData));

                }
                catch (Exception) { MessageBox.Show("Check the internet connection"); }

            }
            return respond;

        }


    }
}
