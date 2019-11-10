using GreyAnatomyFanSite.Models.Persos;

namespace GreyAnatomyFanSite.ViewModels
{
    public class ActeurViewModel
    {
        private Acteur acteur;
        private Personnage perso;

        public Acteur Acteur { get => acteur; set => acteur = value; }
        public Personnage Perso { get => perso; set => perso = value; }
    }
}
