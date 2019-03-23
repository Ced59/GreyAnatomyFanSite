using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using GreyAnatomyFanSite.Tools;

namespace GreyAnatomyFanSite.Models
{
    public class Visiteur
    {
        private string ip;
        private DateTime date;

        public string Ip { get => ip; set => ip = value; }
        public DateTime Date { get => date; set => date = value; }

        public Visiteur()
        {

        }


        public Visiteur (string Ip) : this()
        {
            date = DateTime.Today;
            ip = Ip;

        }


        public int GetVisit(Visiteur v)
        {
           return BddUtilisateurs.Instance.GetVisit(v);
        }




        //public void Login ()

        //{
        //    Membres m = null;

        //    do
        //    {
        //        Console.WriteLine("Veuillez vous identifier");
        //        Console.WriteLine("Entrez votre pseudo: ");
        //        string Pseudo = VerifEntry.VerifPseudoExistLogin();
        //        Console.WriteLine("Entrez votre mot de passe");
        //        string Password = Console.ReadLine();
        //        string HashPassword = Crypto.HashMdp(Password);
        //        m = Bdd.Instance.ComparePassword(Pseudo, HashPassword);
        //        if (m == null)
        //        {
        //            Console.WriteLine("Identification erronée! Veuillez rééssayer!");
        //        }

        //    } while (m == null);

        //    if (m.Statut == "Modérateur")
        //    {
        //        Moderateur mod = new Moderateur(m.Pseudo, m.Avatar, m.Statut);
        //    }
        //    else if (m.Statut == "Administrateur")
        //    {
        //        Admin ad = new Admin(m.Pseudo, m.Avatar, m.Statut);
        //    }


        //    Console.WriteLine($"Identification réussie en tant que {m.Statut}");

            


        //}


    }

    
}
