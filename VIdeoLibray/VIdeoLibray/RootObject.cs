using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIdeoLibray
{
    class RootObject
    {
        public int page { get; set; }
        public List<Result> results { get; set; }
        public List<Casts> cast { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
        public List<Genres> genres { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }//movie id
        public string actorId { get; set; }//movie id
        public string original_title { get; set; }
        public string first_air_date { get; set; }
        public string overview { get; set; }//what about the movie
        public string poster_path { get; set; }//poster to movie
        public string release_date { get; set; }
        public string revenue { get; set; }
        public string runtime { get; set; }
        public string biography { get; set; }
        public string birthday { get; set; }
        public string deathday { get; set; }
        public string place_of_birth { get; set; }
        public string profile_path { get; set; }
        public string other_language { get; set; }




    }
}
