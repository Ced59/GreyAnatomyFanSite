using GreyAnatomyFanSite.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.ViewModels
{
    public class ArticlesCategoriesViewModel
    {
        private List<CategoryArticle> categories;
        private List<Article> articles;

        public List<CategoryArticle> Categories { get => categories; set => categories = value; }
        public List<Article> Articles { get => articles; set => articles = value; }
    }
}
