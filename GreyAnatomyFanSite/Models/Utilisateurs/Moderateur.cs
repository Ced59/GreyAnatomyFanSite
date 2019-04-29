
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models
{
    public class Moderateur : Membres
    {
        

        public Moderateur()
        {

        }

        public Moderateur(string Pseudo, string Avatar, string Statut) : base(Pseudo, Avatar, Statut)
        {

        }



        public void EditAllCommentaire ()
        {

        }

        public void ChangeStatutMembre ()
        {
            
        }

        public void ChangeStatutVisiteur ()
        {
            
        }
    }
}
