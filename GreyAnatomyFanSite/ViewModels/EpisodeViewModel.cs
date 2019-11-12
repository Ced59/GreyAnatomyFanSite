using GreyAnatomyFanSite.Models.Serie;

namespace GreyAnatomyFanSite.ViewModels
{
    public class EpisodeViewModel
    {
        private Saison saison;
        private int episodeNumber;

        public Saison Saison { get => saison; set => saison = value; }
        public int EpisodeNumber { get => episodeNumber; set => episodeNumber = value; }
    }
}
