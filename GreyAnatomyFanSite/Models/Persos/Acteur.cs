using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class Acteur
    {
        private int idActeur;
        private int idPerso;
        private string nomActeur;
        private List<PrenomActeur> prenomsActeur;
        private DateTime dateNaissance;
        private string nationalite;
        private string villeNaissance;
        private string bioActeur;
        private string fluxFacebook;
        private string fluxInstagram;
        private string fluxTwitter;
        private List<string> photosActeur;

        public int IdActeur { get => idActeur; set => idActeur = value; }
        public int IdPerso { get => idPerso; set => idPerso = value; }
        public DateTime DateNaissance { get => dateNaissance; set => dateNaissance = value; }
        public string NomActeur { get => nomActeur; set => nomActeur = value; }
        public string BioActeur { get => bioActeur; set => bioActeur = value; }
        public string Nationalite { get => nationalite; set => nationalite = value; }
        public string VilleNaissance { get => villeNaissance; set => villeNaissance = value; }
        public string FluxFacebook { get => fluxFacebook; set => fluxFacebook = value; }
        public string FluxInstagram { get => fluxInstagram; set => fluxInstagram = value; }
        public string FluxTwitter { get => fluxTwitter; set => fluxTwitter = value; }
        public List<string> PhotosActeur { get => photosActeur; set => photosActeur = value; }
        public List<PrenomActeur> PrenomsActeur { get => prenomsActeur; set => prenomsActeur = value; }

        public List<Acteur> GetAllActeurs()
        {
            return BddSerie.Instance.GetAllActeurs();
        }

        public Acteur GetActeurById()
        {
            return BddSerie.Instance.GetActeurById(this);
        }
    }
}


