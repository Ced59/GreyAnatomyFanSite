using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class PrenomPerso
    {
        private int id;
        private string prenom;
        private int idPerso;

        public string Prenom { get => prenom; set => prenom = value; }
        public int IdPerso { get => idPerso; set => idPerso = value; }
        public int Id { get => id; set => id = value; }
    }
}
