using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private int idMembre;

        public int Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }
        public string Text { get => text; set => text = value; }
        public string TypePubli { get => typePubli; set => typePubli = value; }
        public int IdPubli { get => idPubli; set => idPubli = value; }
        public DateTime Date { get => date; set => date = value; }
        public int IdMembre { get => idMembre; set => idMembre = value; }

        public void AddComment()
        {
            BddSerie.Instance.AddComment(this);
        }
    }
}
