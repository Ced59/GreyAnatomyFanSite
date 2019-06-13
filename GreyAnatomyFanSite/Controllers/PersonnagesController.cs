using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Persos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Controllers
{
    public class PersonnagesController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage();

            List<Personnage> Persos = p.GetAllPersos();

            return View("ViewPersos", Persos);
        }



        public IActionResult ViewPersonnageID(int id)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage();

            p = p.GetPersoID(id);

            return View("ViewPersonnageID", p);
        }



        public IActionResult AddPerso(string name, string firstname)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            List<PrenomPerso> Prenoms = new List<PrenomPerso>();

            PrenomPerso prenomPerso = new PrenomPerso { Prenom = firstname };

            Prenoms.Add(prenomPerso);

            Personnage p = new Personnage { Nom = name, Prenoms = Prenoms };

            p = p.AddNewPerso();

            return View("AddPersos", p);
        }

        public IActionResult ModifPerso(int id)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { Id = id };

            p = p.GetPersoID(id);

            return View("AddPersos", p);
        }

        public IActionResult AddFirstName(string firstname, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);


            PrenomPerso prenom = new PrenomPerso { Prenom = firstname, IdPerso = idPerso };
            List<PrenomPerso> prenoms = new List<PrenomPerso>();
            prenoms.Add(prenom);
            Personnage p = new Personnage { Id = idPerso, Prenoms = prenoms };

            p = p.AddSurnom();

            return View("AddPersos", p);
        }

        public IActionResult DeletePrenom(int id, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { Id = idPerso };
            p.DeletePrenom(id);
            p = p.GetPersoID(idPerso);

            return View("AddPersos", p);
        }


        public IActionResult AddSurName(string surname, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);


            SurnomPerso surnom = new SurnomPerso { Surnom = surname, IdPerso = idPerso };
            List<SurnomPerso> surnoms = new List<SurnomPerso>();
            surnoms.Add(surnom);
            Personnage p = new Personnage { Id = idPerso, Surnoms = surnoms };

            p = p.AddSurnom();

            return View("AddPersos", p);
        }


        public IActionResult StillAlive(string statutVivant, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { Id = idPerso };

            if (statutVivant == "yes")
            {
                p.StatutVivant = true;
            }
            else
            {
                p.StatutVivant = false;
            }

            p = p.UpdateStatutVivant();

            return View("AddPersos", p);
        }


        public IActionResult StillPresent(string statutPresent, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { Id = idPerso };

            if (statutPresent == "yes")
            {
                p.StatutPresent = true;
            }
            else
            {
                p.StatutPresent = false;
            }

            p = p.UpdateStatutPresent();

            return View("AddPersos", p);
        }



            private int GetVisitIP()
        {
            string remoteIpAddress = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
            Visiteur v = new Visiteur(remoteIpAddress);
            return v.GetVisit(v);
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

        private int GetPageVues()
        {
            Visiteur v = new Visiteur();
            return v.GetNbrePagesVues();
        }

        private void ConsentCookie(dynamic c)
        {
            if (Request.Cookies["ConsentCookies"] == null)
            {
                CookieOptions option = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(365),
                    HttpOnly = true
                };
                Response.Cookies.Append("ConsentCookies", "ok", option);

                c.ConsentCookies = "non";
            }

            else
            {
                c.ConsentCookies = "ok";
            }
        }

    }
}