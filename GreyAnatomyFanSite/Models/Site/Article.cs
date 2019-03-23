using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Site
{
    public class Article
    {
        private int id;
        private string titre;
        private string texte;
        private string photo;
        private Membres auteur;
        private List<Commentaire> commentaires;
        private DateTime date;
        private CategoryArticle categorie;
        
        

        public int Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }
        public string Texte { get => texte; set => texte = value; }
        public string Photo { get => photo; set => photo = value; }
        public Membres Auteur { get => auteur; set => auteur = value; }
        public List<Commentaire> Commentaires { get => commentaires; set => commentaires = value; }
        public DateTime Date { get => date; set => date = value; }
        public CategoryArticle Categorie { get => categorie; set => categorie = value; }
        

        public int GetNbreArticles()
        {
            return BddSerie.Instance.GetNbreArticles();
        }


        public Article AddArticle()
        {
            return BddSerie.Instance.AddArticle(this);
        }

        public List<Article> GetAllArticles(int? pagination)
        {
            return BddSerie.Instance.GetAllArticles(pagination);
        }

        public List<Article> GetArticlesByCategory()
        {
            return BddSerie.Instance.GetArticlesByCategorie();
        }

        public Article GetArticle()
        {
            BddSerie.Instance.GetArticle(this, out int IdAuteur);

            Membres m = BddUtilisateurs.Instance.GetMembreById(IdAuteur);

            this.Auteur = m;

            return this;
        }
    }
}
