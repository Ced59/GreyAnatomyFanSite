﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class SurnomPerso
    {
        private string surnom;
        private int idPerso;

        public string Surnom { get => surnom; set => surnom = value; }
        public int IdPerso { get => idPerso; set => idPerso = value; }
    }
}