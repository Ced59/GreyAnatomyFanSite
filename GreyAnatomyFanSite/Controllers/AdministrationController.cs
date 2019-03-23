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

            UserConnect(ViewBag);

            if (!ViewBag.logged)
            {
                return View("Login");
            }

            return View("AddPersos");
        }


        #region PostAddPerso

        [HttpPost]
        public async Task<IActionResult> AddPersoPost(string nom, string prenom, string prenom2, string prenom3, string surnom, string surnom2, string surnom3, string role,
            string bio, string statutPresent, string statutVivant, IFormFile image, string nomActeur, string prenomActeur, string bioActeur, DateTime dateNaissance)

        {

            ViewBag.NbreVisitUnique = GetVisitIP();

            UserConnect(ViewBag);

            PrenomPerso ListePrenom = new PrenomPerso { Prenom = prenom };


            List<PrenomPerso> l = new List<PrenomPerso>
            {
                ListePrenom
            };
            if (prenom2 != null)
            {
                PrenomPerso ListePrenom2 = new PrenomPerso { Prenom = prenom2 };
                l.Add(ListePrenom2);
            }
            if (prenom3 != null)
            {
                PrenomPerso ListePrenom3 = new PrenomPerso { Prenom = prenom3 };
                l.Add(ListePrenom3);
            }

            SurnomPerso ListeSurnom = new SurnomPerso { Surnom = surnom };

            List<SurnomPerso> s = new List<SurnomPerso>
            {
                ListeSurnom
            };

            if (surnom2 != null)
            {
                SurnomPerso ListeSurnom2 = new SurnomPerso { Surnom = surnom2 };
                s.Add(ListeSurnom2);
            }
            if (surnom3 != null)
            {

                SurnomPerso ListeSurnom3 = new SurnomPerso { Surnom = surnom3 };
                s.Add(ListeSurnom3);
            }




            Personnage p = new Personnage { Nom = nom, Prenoms = l, Surnoms = s, Role = role, Biographie = bio, BioActeur = bioActeur, NomActeur = nomActeur,
                                PrenomActeur = prenomActeur, DateNaissance = dateNaissance};

            if (statutPresent == "yes")
            {
                p.StatutPresent = true;
            }
            else
            {
                p.StatutPresent = false;
            }

            if (statutVivant == "yes")
            {
                p.StatutVivant = true;
            }
            else
            {
                p.StatutVivant = false;
            }

            List<PhotoPerso> photos = new List<PhotoPerso>();

            


            if (image.FileName.Contains(".png") || image.FileName.Contains(".jpg"))
            {
                if (image.Length > 1000000)
                {
                    ViewBag.errors = "Le fichier doit avoir une taille maximale de 1Mo.";
                    return View("AddPersos");
                }
                string NumeroUnique = Guid.NewGuid().ToString("N");

                if (image.FileName.Contains(".png"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/PhotosPersos", p.Nom.ToString() + "-" + NumeroUnique + ".png");
                    var stream = new FileStream(path, FileMode.Create);
                    await image.CopyToAsync(stream);
                    PhotoPerso photo = new PhotoPerso
                    {  
                        Url = "images/PhotosPersos/" + p.Nom.ToString() + "-" + NumeroUnique + ".png",
                        IdPerso = p.Id,
                        PhotoPrincipale = true
                    };

                    photos.Add(photo);
                }

                if (image.FileName.Contains(".jpg"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/PhotosPersos", p.Nom.ToString() + "-" + NumeroUnique + ".jpg");
                    var stream = new FileStream(path, FileMode.Create);
                    await image.CopyToAsync(stream);
                    PhotoPerso photo = new PhotoPerso
                    {
                        Url = "images/PhotosPersos/" + p.Nom.ToString() + "-" + NumeroUnique + ".jpg",
                        IdPerso = p.Id,
                        PhotoPrincipale = true
                    };

                    photos.Add(photo);                 
                }
            }
            else
            {
                ViewBag.errors = "Seuls les fichiers .jpg ou .png sont accceptés";
                return View("AddPersos");
            }

            p.Photos = photos;


            p.AddPerso();

            List<Personnage> Persos = p.GetAllPersos();

            return View("AdminPersos", Persos);
        }



#endregion

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
    }

}