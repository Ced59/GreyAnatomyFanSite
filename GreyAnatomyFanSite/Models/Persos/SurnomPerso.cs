namespace GreyAnatomyFanSite.Models.Persos
{
    public class SurnomPerso
    {
        private int id;
        private string surnom;
        private int idPerso;

        public string Surnom { get => surnom; set => surnom = value; }
        public int IdPerso { get => idPerso; set => idPerso = value; }
        public int Id { get => id; set => id = value; }
    }
}