using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIdeoLibray
{
    class Series
    {
        public List<Created> results { get; set; }
        public List<Genres> genres { get; set; }
        public List<Networks> network { get; set; }
        public List<Production> productions { get; set; }
        public List<Season> seasons { get; set; }
        public string backdrop_path { get; set; }
        public string first_air_date { get; set; }
        public string homepage { get; set; }
        public string id { get; set; }
        public string in_production { get; set; }
        public string last_air_date { get; set; }
        public string original_name { get; set; }
        public string number_of_episodes { get; set; }
        public string original_language { get; set; }
        public string overview { get; set; }
        public string popularity { get; set; }
        public string poster_path { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string vote_average { get; set; }
        public string vote_count { get; set; }
    }
}
