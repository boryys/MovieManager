using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager
{
    public enum Rating { Terrible, Bad, Ok, Good, Awesome };
    public enum Type { Thriller, Comedy, Drama, Horror };

    [Serializable()]
    public class Movie
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public Rating Rating { get; set; }
        public Type Type { get; set; }
    }
}
