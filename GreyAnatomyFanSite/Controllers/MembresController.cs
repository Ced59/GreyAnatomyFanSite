using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace GreyAnatomyFanSite.Controllers
{
    public class MembresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Register()
        {

            ViewBag.NbreVisitUnique = GetVisitIP();


            Membres m = new Membres();
            return View(m);
        }

        public IActionResult EditMembre()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();

            UserConnect(ViewBag);

            if (!ViewBag.logged)
            {
                return View("Login");
            }

            return View("EditMembre");
        }


        public IActionResult ModifAvatar()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();

            UserConnect(ViewBag);

            if (!ViewBag.logged)
            {
                return View("Login");
            }

            return View("ModifAvatar");
        }

        [HttpPost]
        public async Task<IActionResult> ModifAvatarPost(IFormFile image)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();

            UserConnect(ViewBag);

            Membres m = new Membres
            {
                Pseudo = HttpContext.Session.GetString("pseudo"),
                Statut = HttpContext.Session.GetString("statut"),
                Mail = HttpContext.Session.GetString("mail"),
                IdMembre = Convert.ToInt32(HttpContext.Session.GetString("idMembre")),
                Avatar = HttpContext.Session.GetString("avatar")
            };



            if (image.FileName.Contains(".png") || image.FileName.Contains(".jpg"))
            {
                if (image.Length > 1000000)
                {
                    ViewBag.errors = "Le fichier doit avoir une taille maximale de 1Mo.";
                    return View("ModifAvatar");
                }
                string NumeroUnique = Guid.NewGuid().ToString("N");
                
                if (image.FileName.Contains(".png"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Avatars", m.IdMembre.ToString() + "-" + NumeroUnique + ".png");
                    var stream = new FileStream(path, FileMode.Create);
                    await image.CopyToAsync(stream);
                    m = new Membres
                    {
                        Pseudo = HttpContext.Session.GetString("pseudo"),
                        Statut = HttpContext.Session.GetString("statut"),
                        Mail = HttpContext.Session.GetString("mail"),
                        IdMembre = Convert.ToInt32(HttpContext.Session.GetString("idMembre")),
                        Avatar = "images/Avatars/" + m.IdMembre.ToString() + "-" + NumeroUnique + ".png"
                    };
                    m.UpdateAvatar();
                    HttpContext.Session.SetString("avatar", m.Avatar);
                }
                if (image.FileName.Contains(".jpg"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Avatars", m.IdMembre.ToString() + "-" + NumeroUnique + ".jpg");
                    var stream = new FileStream(path, FileMode.Create);
                    await image.CopyToAsync(stream);
                    m = new Membres
                    {
                        Pseudo = HttpContext.Session.GetString("pseudo"),
                        Statut = HttpContext.Session.GetString("statut"),
                        Mail = HttpContext.Session.GetString("mail"),
                        IdMembre = Convert.ToInt32(HttpContext.Session.GetString("idMembre")),
                        Avatar = "images/Avatars/" + m.IdMembre.ToString() + "-" + NumeroUnique + ".jpg"
                    };
                    m.UpdateAvatar();
                    HttpContext.Session.SetString("avatar", m.Avatar);
                }

                
                
            }

            else
            {
                ViewBag.errors = "Seuls les fichiers .jpg ou .png sont accceptés";
                return View("ModifAvatar");
            }



            

            UserConnect(ViewBag);

            HttpContext.Session.SetString("avatar", m.Avatar);

            ViewBag.ChangeAvatarOk = "Changement de photo de profil réussi";

            return View("ShowMembre");
        }


        public IActionResult Show()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();

            UserConnect(ViewBag);

            if (!ViewBag.logged)
            {
                return View("Login");
            }

            return View("ShowMembre");
        }

        [HttpPost]
        public IActionResult RegisterPost(string pseudo, string mail, string password, string cPassword)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();


            List<string> errors = new List<string>();

            string remoteIpAddress = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
            Membres m = new Membres { Pseudo = pseudo, Mail = mail, Password = password, Ip = remoteIpAddress };

            if (pseudo == null)
            {
                errors.Add("Merci de saisir un pseudo!");
            }
            if (mail == null)
            {
                errors.Add("Merci de saisir une adresse mail!");
            }
            if (password == null)
            {
                errors.Add("Merci de saisir un mot de passe!");
            }
            if (password != cPassword)
            {
                errors.Add("Merci de saisir le même mot de passe!");
            }

            if (m.Pseudo != null)
            {
                if (m.VerifPseudoExist())
                {
                    errors.Add("Ce pseudo existe déja, veuillez en choisir un autre.");
                }
            }

            if (m.Mail != null)
            {
                if (m.VerifMailExist())
                {
                    errors.Add("Cette adresse mail est déjà enregistrée!");
                }
            }

            if (errors.Count() > 0)
            {
                ViewBag.errors = errors;
                return View("Register", m);
            }
            else
            {
                string pattern = @"^[A-z0-9._-]+(@)[a-z-9_-]+\.[a-z]{2,5}$";
                Regex r = new Regex(pattern);

                if (!r.IsMatch(m.Mail))
                {
                    errors.Add("Cette adresse mail n'est pas valide!");
                }

                pattern = "[a-zA-Z0-9]{4,10}";
                r = new Regex(pattern);
                if (!r.IsMatch(m.Pseudo))
                {
                    errors.Add("Le pseudo doit contenir minimum 4 caractères!");
                }

                pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\\W)";
                r = new Regex(pattern);

                if (!r.IsMatch(m.Password))
                {
                    errors.Add("Mot de passe invalide! Il doit être compris entre 8 et 15 caractères et contenir " +
                        "au minimum un chiffre, une majuscule et un caractère spécial");
                }

                if (errors.Count() > 0)
                {
                    ViewBag.errors = errors;
                    return View("Register", m);
                }

            }


            string NumeroUnique = Guid.NewGuid().ToString("N"); /*Creation d'un ID unique pour le cookie*/

            string HashPassword = Crypto.HashMdp(password); /*Cryptage PassWord*/

            m = new Membres { Pseudo = pseudo, Mail = mail, Password = HashPassword, Ip = remoteIpAddress, NoUnique = NumeroUnique, DateInscription = DateTime.Now };

            SmtpClient client = new SmtpClient
            {
                Host = PassConnection.SmtpConfig(),
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("noreply@greys-anatomy-fans.com", PassConnection.MailPassWord())
            };

            MailMessage message = new MailMessage("noreply@greys-anatomy-fans.com", m.Mail)
            {
                IsBodyHtml = true,
                Body = "Merci de confirmer votre adresse mail en cliquant sur le lien suivant: <a href = \"http://www.greys-anatomy-fans.com/Membres/Confirm/" + m.NoUnique + "\">Lien</a>"
                ,


                Subject = "Confirmez votre adresse Mail"
            };



            client.Send(message);

            m.AddMembre();

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(30);
            option.HttpOnly = true;
            Response.Cookies.Append("User", m.NoUnique, option);

            var request = Request;

            return View("SendMailRegistration", m);
        }


        public IActionResult Confirm(string id)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();


            Membres m = new Membres { NoUnique = id };

            m = m.GetMembreByNoUnique();

            return View("Confirm", m);
        }

        public IActionResult ReinitializeLostPassWord(string id)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();

            Membres m = new Membres { NoUnique = id };

            m = m.GetMembreByNoUnique();

            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true
            };
            Response.Cookies.Append("User", id, option);

            return View("InitializeNewPassWord", m);
        }



        [HttpPost]
        public IActionResult InitializeNewPassWordPost(string password, string cPassword)
        {
            List<string> errors = new List<string>();


            if (password == null)
            {
                errors.Add("Merci de saisir un mot de passe!");
            }
            if (password != cPassword)
            {
                errors.Add("Merci de saisir le même mot de passe!");
            }

            if (errors.Count() > 0)
            {
                ViewBag.errors = errors;
                return View("InitializeNewPassWord");
            }
            else
            {

                string pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\\W)";

                Regex r = new Regex(pattern);

                if (!r.IsMatch(password))
                {
                    errors.Add("Mot de passe invalide! Il doit être compris entre 8 et 15 caractères et contenir " +
                        "au minimum un chiffre, une majuscule et un caractère spécial");
                }

                if (errors.Count() > 0)
                {
                    ViewBag.errors = errors;
                    return View("InitializeNewPassWord");
                }
            }


            string NoUnique = Request.Cookies["User"];


            string HashPassword = Crypto.HashMdp(password);

            Membres m = new Membres { Password = HashPassword, NoUnique = NoUnique };

            m.UpdatePassword();

            m = m.GetMembreByNoUnique();

            ViewBag.ChangePassOk = "Changement de mot de passe réussi. Merci de vous identifier.";

            return View("Login", m);
        }


        public IActionResult Login()
        {

            ViewBag.NbreVisitUnique = GetVisitIP();

            CookieUserExist(ViewBag);

            return View();

        }

        [HttpPost]
        public IActionResult LoginPost(string mail, string password)
        {
            List<string> errors = new List<string>();



            if (mail == null)
            {
                errors.Add("Merci de saisir une adresse mail.");
            }
            if (password == null)
            {
                errors.Add("Merci de saisir un mot de passe.");
            }

            if (errors.Count > 0)
            {
                ViewBag.errors = errors;
                ViewBag.Mail = mail;
                return View("Login");
            }
            else
            {
                Membres m = new Membres { Mail = mail, Password = password };

                if ((!m.VerifMailExist()))
                {
                    errors.Add("Il n'y a pas d'utilisateur avec cette adresse mail / mot de passe.");
                    ViewBag.errors = errors;
                    ViewBag.Mail = m.Mail;
                    return View("Login");
                }

                m = m.Login();

                if (m.Pseudo == null)
                {
                    errors.Add("Il n'y a pas d'utilisateur avec cette adresse mail / mot de passe.");
                    ViewBag.errors = errors;
                    ViewBag.Mail = m.Mail;
                    return View("Login");
                }

                if (m.VerifStatut() == "Inactif")
                {
                    errors.Add("Vous devez confirmer votre adresse Mail pour vous connecter.");
                    ViewBag.errors = errors;
                    ViewBag.Mail = m.Mail;
                    return View("Login");
                }

                else
                {
                    HttpContext.Session.SetString("logged", "true");
                    HttpContext.Session.SetString("pseudo", m.Pseudo);
                    HttpContext.Session.SetString("idMembre", m.IdMembre.ToString());
                    HttpContext.Session.SetString("statut", m.Statut);
                    HttpContext.Session.SetString("avatar", m.Avatar);
                    HttpContext.Session.SetString("mail", m.Mail);

                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(30);
                    option.HttpOnly = true;
                    Response.Cookies.Append("User", m.NoUnique, option);

                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }


        public IActionResult ChangePassword()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            CookieUserExist(ViewBag);

            return View("ForgotPassword");
        }


        [HttpPost]
        public IActionResult ChangePasswordPost(string mail)
        {

            List<string> errors = new List<string>();

            if (mail == null)
            {
                errors.Add("Merci de saisir une adresse mail.");
                ViewBag.errors = errors;
                return View("ForgotPassword");
            }

            Membres m = new Membres { Mail = mail };

            if ((!m.VerifMailExist()))
            {
                errors.Add("Il n'y a pas d'utilisateur avec cette adresse mail.");
                ViewBag.errors = errors;
                ViewBag.Mail = m.Mail;
                return View("ForgotPassword");
            }

            string NoUnique = m.GetNoUniqueMembre();

            SmtpClient client = new SmtpClient
            {
                Host = PassConnection.SmtpConfig(),
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("noreply@greys-anatomy-fans.com", PassConnection.MailPassWord())
            };

            MailMessage message = new MailMessage("noreply@greys-anatomy-fans.com", m.Mail)
            {
                IsBodyHtml = true,
                Body = "Merci de confirmer votre adresse mail en cliquant sur le lien suivant: <a href = \"http://www.greys-anatomy-fans.com/Membres/ReinitializeLostPassWord/" + NoUnique + "\">Lien</a>"
                ,


                Subject = "Réinitialisation du Mot de Passe"
            };



            client.Send(message);

            ViewBag.Message = $"Un mail vous permettant de réinitialiser votre mot de passe a été envoyé à {m.Mail}";

            return View("ForgotPassword");
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
    }
}