using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.ViewModels
{
    public class MembreLoginViewModel
    {
        private Membres membre;
        private Article article;
        private List<Commentaire> commentaires;
        private List<Article> articles;

        public Membres Membre { get => membre; set => membre = value; }
        public Article Article { get => article; set => article = value; }
        public List<Commentaire> Commentaires { get => commentaires; set => commentaires = value; }
        public List<Article> Articles { get => articles; set => articles = value; }
    }
}
