using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class Patient : Personnage
    {
        private string nomMaladie;

        public string NomMaladie { get => nomMaladie; set => nomMaladie = value; }
    }
}
