namespace GreyAnatomyFanSite.Models.Persos
{
    public class PrenomActeur
    {
        private int id;
        private string prenom;
        private int idActeur;

        public int Id { get => id; set => id = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public int IdActeur { get => idActeur; set => idActeur = value; }
    }
}