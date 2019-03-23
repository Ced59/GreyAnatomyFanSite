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
        private string prenomActeur;
        private DateTime dateNaissance;
        private string nationalite;
        private string villeNaissance;
        private string bioActeur;

        public int IdActeur { get => idActeur; set => idActeur = value; }
        public int IdPerso { get => idPerso; set => idPerso = value; }
        public DateTime DateNaissance { get => dateNaissance; set => dateNaissance = value; }
        public string NomActeur { get => nomActeur; set => nomActeur = value; }
        public string PrenomActeur { get => prenomActeur; set => prenomActeur = value; }
        public string BioActeur { get => bioActeur; set => bioActeur = value; }
        public string Nationalite { get => nationalite; set => nationalite = value; }
        public string VilleNaissance { get => villeNaissance; set => villeNaissance = value; }




    }
}


