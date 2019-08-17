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
        private string titreFr;
        private string synopsis;
        private List<Personnage> personnages;
        private List<Patient> patients;
        private List<string> photos;
        private int idEpisode;
        private int idSaison;
        private Realisateur realisateur;
        private Scenarist scenarist;
        private int noProduction;


        public string Titre { get => titre; set => titre = value; }
        public string Synopsis { get => synopsis; set => synopsis = value; }
        public List<Personnage> Personnages { get => personnages; set => personnages = value; }
        public List<Patient> Patients { get => patients; set => patients = value; }
        public List<string> Photos { get => photos; set => photos = value; }
        public int IdEpisode { get => idEpisode; set => idEpisode = value; }
        public int IdSaison { get => idSaison; set => idSaison = value; }
        public string TitreFr { get => titreFr; set => titreFr = value; }
        public Realisateur Realisateur { get => realisateur; set => realisateur = value; }
        public Scenarist Scenarist { get => scenarist; set => scenarist = value; }
        public int NoProduction { get => noProduction; set => noProduction = value; }
    }
}
