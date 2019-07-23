
using GreyAnatomyFanSite.Tools;
using System;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models
{
    public class Membres : Visiteur
    {

        private int idMembre;
        private string noUnique;
        private string pseudo;
        private string mail;
        private string password;
        private string statut;
        private string avatar;
        private string nom;
        private string prenom;
        private DateTime dateNaissance;
        private string genre;
        private DateTime dateInscription;
        private int nbreVisite;
        private int nbrePageViews;
        

        public Membres()
        {

        }

        public Membres (string Pseudo, string Avatar, string Statut) 
        {
            pseudo = Pseudo;
            avatar = Avatar;
            statut = Statut;
        }

        public Membres(string Ip, string Pseudo, string Mail, string Password) : base(Ip)
        {
            pseudo = Pseudo;
            mail = Mail;
            password = Password;
        }

        public string Pseudo { get => pseudo; set => pseudo = value; }
        public string Mail { get => mail; set => mail = value; }
        public string Password { get => password; set => password = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Genre { get => genre; set => genre = value; }
        public DateTime DateNaissance { get => dateNaissance; set => dateNaissance = value; }
        public string Avatar { get => avatar; set => avatar = value; }
        public string Statut { get => statut; set => statut = value; }
        public string NoUnique { get => noUnique; set => noUnique = value; }
        public int IdMembre { get => idMembre; set => idMembre = value; }
        public DateTime DateInscription { get => dateInscription; set => dateInscription = value; }
        public int NbreVisite { get => nbreVisite; set => nbreVisite = value; }
        public int NbrePageViews { get => nbrePageViews; set => nbrePageViews = value; }

        public string VerifStatut()
        {
            return BddUtilisateurs.Instance.VerifStatut(this);
        }

        public bool VerifPseudoExist()
        {
            return BddUtilisateurs.Instance.PseudoExist(this);
        }

        public bool VerifMailExist()
        {
            return BddUtilisateurs.Instance.MailExist(this);
        }

        public void AddMembre()
        {
            BddUtilisateurs.Instance.RegisterMembre(this);
        }

        public Membres GetMembreByNoUnique()
        {
            return BddUtilisateurs.Instance.GetMembreByNo(this);
        }

        public void UpdatePassword()
        {
            BddUtilisateurs.Instance.UpdatePassword(this);
        }

        public Membres Login()
        {
            string HashPassword = Crypto.HashMdp(this.Password);
            return BddUtilisateurs.Instance.ComparePassword(this.Mail, HashPassword);
        }

        public void UpdateAvatar()
        {
            BddUtilisateurs.Instance.UpdateAvatar(this);
        }

        public string GetNoUniqueMembre()
        {
            return BddUtilisateurs.Instance.GetNoUniqueMembre(this);
        }

        public Membres GetMembreById(int id)
        {
            return BddUtilisateurs.Instance.GetMembreById(id);
        }

        public List<Membres> GetAllMembres()
        {
            return BddUtilisateurs.Instance.GetAllMembres();
        }

        public Membres GetMembreByPseudo(string pseudo)
        {
            return BddUtilisateurs.Instance.GetMembreByPseudo(pseudo);
        }

        public Membres GetVisitMembres()
        {
            this.NbreVisite = BddUtilisateurs.Instance.GetVisitMembres(this);
            this.NbrePageViews = BddUtilisateurs.Instance.GetNbrePagesViewMembres(this);

            return this;
        }
    }
}
