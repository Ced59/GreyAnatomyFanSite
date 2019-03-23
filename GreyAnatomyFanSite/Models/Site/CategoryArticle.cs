using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Site
{
    public class CategoryArticle
    {
        private int id;
        private string titreCategory;

        public int Id { get => id; set => id = value; }
        public string TitreCategory { get => titreCategory; set => titreCategory = value; }

        public List<CategoryArticle> AddCategory()
        {
            return BddSerie.Instance.AddCategory(this);
        }

        public List<CategoryArticle> GetAllCategory()
        {
            return BddSerie.Instance.GetCategories();
        }
    }
}
