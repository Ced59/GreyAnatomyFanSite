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

        public int GetNbrePagesVues()
        {
            return BddUtilisateurs.Instance.GetNbrePagesVues();
        }

        public int GetIdIp(string remoteIpAddress)
        {
            return BddUtilisateurs.Instance.GetIdIpByIp(remoteIpAddress);
        }




        


    }

    
}
