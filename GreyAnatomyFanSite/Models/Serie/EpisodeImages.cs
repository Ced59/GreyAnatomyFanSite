using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models.Serie
{
    public class EpisodeImages
    {
        private int id;
        private List<EpisodeImg> stills;

        public int Id { get => id; set => id = value; }
        public List<EpisodeImg> Stills { get => stills; set => stills = value; }


    }
}