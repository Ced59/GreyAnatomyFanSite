using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class PrenomPerso
    {
        private string prenom;
        private int idPerso;

        public string Prenom { get => prenom; set => prenom = value; }
        public int IdPerso { get => idPerso; set => idPerso = value; }
    }
}
