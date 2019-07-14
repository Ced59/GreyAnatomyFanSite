using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Persos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Controllers
{
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Serie()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();

            UserConnect(ViewBag);

            if (!ViewBag.logged)
            {
                return View("Login");
            }

            return View("Serie");
        }


        public IActionResult Membres()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();

            UserConnect(ViewBag);

            if (!ViewBag.logged)
            {
                return View("Login");
            }


            return View("AdminMembres");
        }


        public IActionResult Persos()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();

            UserConnect(ViewBag);

            if (!ViewBag.logged)
            {
                return View("Login");
            }

            Personnage p = new Personnage();

            List<Personnage> Persos = p.GetAllPersos();
            

            return View("AdminPersos", Persos);
        }

        public IActionResult AddPerso()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();

            UserConnect(ViewBag);

            if (!ViewBag.logged)
            {
                return View("Login");
            }

            return View("AddPersos");
        }


        private int GetVisitIP()
        {
            string remoteIpAddress = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
            Visiteur v = new Visiteur(remoteIpAddress);
            return v.GetVisit(v);
        }

        private int GetPageVues()
        {
            Visiteur v = new Visiteur();
            return v.GetNbrePagesVues();
        }


        private void CookieUserExist(dynamic v)
        {
            if (Request.Cookies["User"] != null)
            {
                string valCookie = Request.Cookies["User"];
                Membres m = new Membres { NoUnique = valCookie };
                m = m.GetMembreByNoUnique();
                v.Mail = m.Mail;

            }
            else
            {
                Membres m = new Membres();
                v.Mail = "Votre Adresse Mail";

            }
        }

        private void UserConnect(dynamic v)
        {
            bool? logged = Convert.ToBoolean(HttpContext.Session.GetString("logged"));
            if (logged == true)
            {
                v.Logged = logged;
                v.Pseudo = HttpContext.Session.GetString("pseudo");
                v.Id = HttpContext.Session.GetString("idMembre");
                v.Mail = HttpContext.Session.GetString("mail");
                v.Avatar = HttpContext.Session.GetString("avatar");
                v.Statut = HttpContext.Session.GetString("statut");


                if (HttpContext.Session.GetString("statut") == "Coeur")
                {
                    v.MessageBonjour = "mon Coeur";
                }
                else
                {
                    v.MessageBonjour = HttpContext.Session.GetString("pseudo");
                }

            }

            else
            {
                v.Logged = false;
            }
        }
    }

}