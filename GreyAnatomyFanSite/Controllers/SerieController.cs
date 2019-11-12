using System;
using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Serie;
using GreyAnatomyFanSite.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Controllers
{
    public class SerieController : Controller
    {
        public IActionResult Index(int idSerie)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);
            ConsentCookie(ViewBag);


            SerieInfo serie = new SerieInfo();

            serie = serie.getSerie(idSerie);
            serie.Saisons = serie.getSaisons();

            return View("Index", serie);
        }

        public IActionResult ViewSeason(int idSerie, int saison)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Saison season = new Saison();

            season = season.getSeason(idSerie, saison);

            return View("ViewSeason", season);
        }

        public IActionResult ViewEpisode(int idSerie, int saison, int episode)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Saison season = new Saison();

            season = season.getSeasonById(idSerie, saison);

            EpisodeViewModel model = new EpisodeViewModel { Saison = season, EpisodeNumber = episode };

            return View("ViewEpisode", model);
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


        private int GetPageVues()
        {
            Visiteur v = new Visiteur();
            return v.GetNbrePagesVues();
        }

        private int GetVisitIP()
        {
            string remoteIpAddress = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
            Visiteur v = new Visiteur(remoteIpAddress);
            return v.GetVisit(v);
        }
    }
}
