using System;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models.Site
{
    public class Commentaire
    {
        private int id;
        private string titre;
        private string text;
        private string typePubli;
        private int idPubli;
        private DateTime date;
        private Membres membre;
        private int idMembre;

        public int Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }
        public string Text { get => text; set => text = value; }
        public string TypePubli { get => typePubli; set => typePubli = value; }
        public int IdPubli { get => idPubli; set => idPubli = value; }
        public DateTime Date { get => date; set => date = value; }
        public Membres Membre { get => membre; set => membre = value; }
        public int IdMembre { get => idMembre; set => idMembre = value; }

        public void AddComment()
        {
            BddSerie.Instance.AddComment(this);
        }

        public List<Commentaire> GetComments()
        {
            List<Commentaire> commentaires = BddSerie.Instance.GetComments(this);
            if (commentaires != null)
            {
                foreach (Commentaire c in commentaires)
                {
                    c.Membre = BddUtilisateurs.Instance.GetMembreById(c.IdMembre);
                }
            }
         
            return commentaires;
        }
    }
}
