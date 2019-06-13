using GreyAnatomyFanSite.Models.Serie;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Persos
{
    public class Personnage : Acteur
    {

        private int id;
        private string nom;
        private List<PrenomPerso> prenoms;
        private List<SurnomPerso> surnoms;
        private List<Episode> episodesApparition;
        private List<Relation> relations;
        private List<RelationFamille> famille;
        private List<PhotoPerso> photos;
        private bool statutVivant;
        private bool statutPresent;
        private string statutPerso;
        private List<Citation> citations;
        private string role;
        private string biographie;

        

        public string Nom { get => nom; set => nom = value; }
        public List<Episode> EpisodesApparition { get => episodesApparition; set => episodesApparition = value; }
        public List<Relation> Relations { get => relations; set => relations = value; }
        public List<RelationFamille> Famille { get => famille; set => famille = value; }
        public bool StatutVivant { get => statutVivant; set => statutVivant = value; }
        public bool StatutPresent { get => statutPresent; set => statutPresent = value; }
        public int Id { get => id; set => id = value; }
        public string StatutPerso { get => statutPerso; set => statutPerso = value; }
        public List<Citation> Citations { get => citations; set => citations = value; }
        public List<PrenomPerso> Prenoms { get => prenoms; set => prenoms = value; }
        public List<SurnomPerso> Surnoms { get => surnoms; set => surnoms = value; }
        public string Role { get => role; set => role = value; }
        public List<PhotoPerso> Photos { get => photos; set => photos = value; }
        public string Biographie { get => biographie; set => biographie = value; }

        public void AddPerso()
        {
            BddSerie.Instance.AddPerso(this);
        }

        public Personnage AddPrenom()
        {
            BddSerie.Instance.AddPrenom(this);

            return BddSerie.Instance.GetPersoByID(this.Id);
        }

        public Personnage AddSurnom()
        {
            BddSerie.Instance.AddSurnom(this);

            return BddSerie.Instance.GetPersoByID(this.Id);
        }

        public Personnage AddNewPerso()
        {
            return BddSerie.Instance.AddNewPerso(this);
        }

        public List<Personnage> GetAllPersos()
        {
            return BddSerie.Instance.GetAllPersos();
        }

        public Personnage GetPersoID(int id)
        {
            return BddSerie.Instance.GetPersoByID(id);
        }

        public void DeletePrenom(int id)
        {
            BddSerie.Instance.DeletePrenom(id);
        }

        public Personnage UpdateStatutVivant()
        {
            BddSerie.Instance.UpdateStatutVivant(this);

            return BddSerie.Instance.GetPersoByID(this.Id);
        }

        public Personnage UpdateStatutPresent()
        {
            BddSerie.Instance.UpdateStatutPresent(this);

            return BddSerie.Instance.GetPersoByID(this.Id);
        }
    }
}
