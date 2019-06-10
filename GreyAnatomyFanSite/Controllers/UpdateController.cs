using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Controllers
{
    public class UpdateController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            UserConnect(ViewBag);
            ViewBag.NbrePagesVues = GetPageVues();
            ConsentCookie(ViewBag);


            return View();
        }


        private void UserConnect(dynamic v)
        {
            bool? logged = Convert.ToBoolean(HttpContext.Session.GetString("logged"));
            if (logged == true)
            {
                v.Logged = logged;
                v.Pseudo = HttpContext.Session.GetString("pseudo");
                v.Id = HttpContext.Session.GetString("idMembre");
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
    }
}
