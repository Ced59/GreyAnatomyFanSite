using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.ViewModels
{
    public class ArticleMembreViewModel
    {
        private Membres membre;
        private Article article;

        public Membres Membre { get => membre; set => membre = value; }
        public Article Article { get => article; set => article = value; }
    }
}
