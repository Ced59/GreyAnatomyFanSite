using GreyAnatomyFanSite.Models.Persos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
