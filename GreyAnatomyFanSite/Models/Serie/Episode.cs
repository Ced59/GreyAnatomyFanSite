using GreyAnatomyFanSite.Models.Persos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Serie
{
    public class Episode
    {
        private string titre;
        private string synopsis;
        private List<Personnage> personnages;
        private DateTime premiereDiffusion;
        private DateTime premiereDiffusionFrance;
        private List<Patient> patients;
        private List<string> photos;
        private int idEpisode;
        private int idSaison;


        public string Titre { get => titre; set => titre = value; }
        public string Synopsis { get => synopsis; set => synopsis = value; }
        public List<Personnage> Personnages { get => personnages; set => personnages = value; }
        public DateTime PremiereDiffusion { get => premiereDiffusion; set => premiereDiffusion = value; }
        public DateTime PremiereDiffusionFrance { get => premiereDiffusionFrance; set => premiereDiffusionFrance = value; }
        public List<Patient> Patients { get => patients; set => patients = value; }
        public List<string> Photos { get => photos; set => photos = value; }
        public int IdEpisode { get => idEpisode; set => idEpisode = value; }
        public int IdSaison { get => idSaison; set => idSaison = value; }
    }
}
