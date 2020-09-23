namespace GreyAnatomyFanSite.Models.Persos
{
    public class PhotoPerso
    {
        private int id;
        private string url;
        private Membres membre;
        private int idPerso;
        private bool photoPrincipale;

        public int Id { get => id; set => id = value; }
        public string Url { get => url; set => url = value; }
        public Membres Membre { get => membre; set => membre = value; }
        public int IdPerso { get => idPerso; set => idPerso = value; }
        public bool PhotoPrincipale { get => photoPrincipale; set => photoPrincipale = value; }
    }
}