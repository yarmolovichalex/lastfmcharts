using System.Collections.Generic;

namespace LastFMCharts.Models
{
    public class ArtistViewModel
    {
        public string Name { get; set; }
        public int Listeners { get; set; }
        public int Plays { get; set; }
        public IEnumerable<string> Similar { get; set; }
    }
}