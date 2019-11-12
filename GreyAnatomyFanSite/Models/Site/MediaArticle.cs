namespace GreyAnatomyFanSite.Models.Site
{
    public class MediaArticle
    {
        private int id;
        private string url;
        private int idArticle;

        public int Id { get => id; set => id = value; }
        public string Url { get => url; set => url = value; }
        public int IdArticle { get => idArticle; set => idArticle = value; }

        internal void AddMediaArticle()
        {
            BddSerie.Instance.AddMediaArticle(this);
        }
    }
}
