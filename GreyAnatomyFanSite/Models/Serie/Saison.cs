using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Serie
{
    public class Saison
    {
        private List<Episode> episodes;
        private int id;

        public List<Episode> Episodes { get => episodes; set => episodes = value; }
        public int Id { get => id; set => id = value; }
    }
}
