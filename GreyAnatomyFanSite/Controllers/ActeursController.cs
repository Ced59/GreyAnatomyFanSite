using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Persos;
using GreyAnatomyFanSite.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Controllers
{
    public class ActeursController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();

            UserConnect(ViewBag);
            ConsentCookie(ViewBag);


            return View("ViewActeur");
        }

        public IActionResult ViewActeurID(int id)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();

            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Acteur a = new Acteur { IdActeur = id };
            a = a.GetActeurById();
            Personnage p = new Personnage { Id = a.IdPerso };
            p = p.GetPersoID(a.IdPerso);

            ActeurViewModel model = new ActeurViewModel { Acteur = a, Perso = p };

            return View("ViewActeur", model);
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
