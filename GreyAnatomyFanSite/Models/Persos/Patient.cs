namespace GreyAnatomyFanSite.Models.Persos
{
    public class Patient : Personnage
    {
        private string nomMaladie;

        public string NomMaladie { get => nomMaladie; set => nomMaladie = value; }
    }
}