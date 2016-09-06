using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIdeoLibray
{
    class SingleMovie
    {
        public int adult { get; set; }
        public string backdrop_path { get; set; }
        public int belongs_to_collection { get; set; }
        public int budget { get; set; }
        public List<Genres> genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }//movie id
        public int imdb_id { get; set; }//imdb id
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }//what about the movie
        public double popularity { get; set; }//imdb id
        public string poster_path { get; set; }//poster to movie
        public List<Companies> production_companies { get; set; }
        public List<Countries> production_countries { get; set; }
        public string release_date { get; set; }
        public string revenue { get; set; }
        public string runtime { get; set; }
        public List<Languages> spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string video { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }

    }
}
