using GreyAnatomyFanSite.Models.Serie;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class Citation
    {
        private string citationIntro;
        private string citationOutro;
        private Episode episode;
        private Personnage perso;

        public string CitationIntro { get => citationIntro; set => citationIntro = value; }
        public string CitationOutro { get => citationOutro; set => citationOutro = value; }
        public Episode Episode { get => episode; set => episode = value; }
        public Personnage Perso { get => perso; set => perso = value; }
    }
}