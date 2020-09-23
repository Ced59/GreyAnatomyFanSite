namespace GreyAnatomyFanSite.Models
{
    public class Admin : Moderateur
    {
        public Admin()
        {
        }

        public Admin(string Pseudo, string Avatar, string Statut) : base(Pseudo, Avatar, Statut)
        {
        }

        public void ChangeStatutUserAdmin()
        {
        }

        public void DisplayAdministration()
        {
        }

        public void AddArticle()
        {
        }
    }
}