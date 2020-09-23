using GreyAnatomyFanSite.Models.Serie;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class Relation
    {
        private int idPerso;
        private int idPersoRelation;
        private List<Episode> episodes;
        private string typeRelation;

        public int IdPerso { get => idPerso; set => idPerso = value; }
        public int IdPersoRelation { get => idPersoRelation; set => idPersoRelation = value; }
        public List<Episode> Episodes { get => episodes; set => episodes = value; }
        public string TypeRelation { get => typeRelation; set => typeRelation = value; }
    }
}