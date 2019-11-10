using GreyAnatomyFanSite.Models.Serie;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class RolePerso
    {
        private int idPerso;
        private string role;
        private Saison saison;
        private List<Episode> episode;

        public int IdPerso { get => idPerso; set => idPerso = value; }
        public string Role { get => role; set => role = value; }
        public Saison Saison { get => saison; set => saison = value; }
        public List<Episode> Episode { get => episode; set => episode = value; }
    }
}
