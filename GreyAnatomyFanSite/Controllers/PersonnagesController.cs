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

        public IActionResult AddNewPerso()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            return View("AddPersos");
        }


        [HttpPost]
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

        [HttpPost]
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

            p = p.AddPrenom();

            return View("AddPersos", p);
        }

        [HttpPost]
        public IActionResult AddActorFirstName(string firstname, int idActeur, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);


            PrenomActeur prenom = new PrenomActeur { Prenom = firstname, IdActeur = idActeur };
            List<PrenomActeur> prenoms = new List<PrenomActeur>();
            prenoms.Add(prenom);
            Acteur a = new Acteur { PrenomsActeur = prenoms, IdActeur = idActeur };
            Personnage p = new Personnage();

            p = p.AddPrenomActeur(a, idPerso);

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


        
        public IActionResult DeletePrenomActeur(int id, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { Id = idPerso };
            p.DeletePrenomActeur(id);
            p = p.GetPersoID(idPerso);

            return View("AddPersos", p);
        }


        [HttpPost]
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


        
        public IActionResult DeleteSurnom(int id, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { Id = idPerso };
            p.DeleteSurnom(id);
            p = p.GetPersoID(idPerso);

            return View("AddPersos", p);
        }


        [HttpPost]
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


        [HttpPost]
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


        [HttpPost]
        public async Task<IActionResult> AddPrincPhoto(IFormFile image, int idPerso, string photoPrincipale)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { Id = idPerso };
            p = p.GetPersoID(idPerso);

            if (photoPrincipale == "yes")
            {
                foreach (PhotoPerso photo in p.Photos)
                {
                    photo.PhotoPrincipale = false;
                }
            }


            if (image.FileName.Contains(".png") || image.FileName.Contains(".jpg"))
            {
                if (image.Length > 1000000)
                {
                    ViewBag.errors = "Le fichier doit avoir une taille maximale de 1Mo.";
                    return View("AddPersos", p);
                }
                string NumeroUnique = Guid.NewGuid().ToString("N").Substring(1, 10);

                if (image.FileName.Contains(".png"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/PhotosPersos", p.Id.ToString() + "-" + p.Nom.ToString() + "-" + p.Prenoms[0].Prenom.ToString() + "-" + NumeroUnique + ".png");
                    var stream = new FileStream(path, FileMode.Create);
                    await image.CopyToAsync(stream);
                    PhotoPerso photo = new PhotoPerso
                    {
                        Url = "images/PhotosPersos/" + p.Id.ToString() + "-" + p.Nom.ToString() + "-" + p.Prenoms[0].Prenom.ToString() + "-" + NumeroUnique + ".png",
                        IdPerso = p.Id,
                        PhotoPrincipale = true
                    };

                    p.Photos.Add(photo);
                }

                if (image.FileName.Contains(".jpg"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/PhotosPersos", p.Id.ToString() + "-" + p.Nom.ToString() + "-" + p.Prenoms[0].Prenom.ToString() + "-" + NumeroUnique + ".jpg");
                    var stream = new FileStream(path, FileMode.Create);
                    await image.CopyToAsync(stream);
                    PhotoPerso photo = new PhotoPerso
                    {
                        Url = "images/PhotosPersos/" + p.Id.ToString() + "-" + p.Nom.ToString() + "-" + p.Prenoms[0].Prenom.ToString() + "-" + NumeroUnique + ".jpg",
                        IdPerso = p.Id,
                        PhotoPrincipale = true
                    };

                    p.Photos.Add(photo);
                }
            }
            else
            {
                ViewBag.errors = "Seuls les fichiers .jpg ou .png sont accceptés";
                return View("AddPersos", p);
            }


            p = p.AddPhotos();

            return View("AddPersos", p);
        }


        [HttpPost]
        public IActionResult AddActor(string name, string firstname, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            List<PrenomActeur> Prenoms = new List<PrenomActeur>();

            PrenomActeur prenomPerso = new PrenomActeur { Prenom = firstname };

            Prenoms.Add(prenomPerso);

            Personnage p = new Personnage { NomActeur = name, PrenomsActeur = Prenoms, IdPerso = idPerso };

            p = p.AddNewActor();

            return View("AddPersos", p);
        }


        public IActionResult AddBirthDate(DateTime dateNaissance, int idActeur, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { IdActeur = idActeur, DateNaissance = dateNaissance, Id = idPerso };

            p = p.AddBirthdate();

            return View("AddPersos", p);
        }



        public IActionResult ModifBirthDate(DateTime dateNaissance, int idActeur, int idPerso)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();


            UserConnect(ViewBag);
            ConsentCookie(ViewBag);

            Personnage p = new Personnage { IdActeur = idActeur, DateNaissance = dateNaissance, Id = idPerso };

            p = p.ModifBirthdate();

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