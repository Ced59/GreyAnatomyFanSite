using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Persos;
using GreyAnatomyFanSite.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.ViewModels
{
    public class HomeViewModel
    {
        private List<Personnage> birthDatesActeurs;
        private List<Article> articles;
        private int nbrePagePagination;
        private int? pagePagination;
        private List<CategoryArticle> categoryArticles;
        private int activeCategory;

        public List<Personnage> BirthDatesActeurs { get => birthDatesActeurs; set => birthDatesActeurs = value; }
        public List<Article> Articles { get => articles; set => articles = value; }
        public int NbrePagePagination { get => nbrePagePagination; set => nbrePagePagination = value; }
        public int? PagePagination { get => pagePagination; set => pagePagination = value; }
        public List<CategoryArticle> CategoryArticles { get => categoryArticles; set => categoryArticles = value; }
        public int ActiveCategory { get => activeCategory; set => activeCategory = value; }
    }
}
