using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Surveys;
using GreyAnatomyFanSite.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Controllers
{
    public class SurveysController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);

            Survey s = new Survey();
            List<Survey> surveys = s.GetAllSurveys(true);

            return View("Index", surveys);
        }


        public IActionResult Admin()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();

            UserConnect(ViewBag);

            if (!ViewBag.Logged)
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            if ((ViewBag.Statut != "Administrateur") && (ViewBag.Statut != "Coeur"))
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }


            Survey s = new Survey();
            List<Survey> surveys = s.GetAllSurveys(null);

            return View("SurveysAdmin", surveys);
        }

        public IActionResult Add()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);

            if (!ViewBag.Logged)
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            if ((ViewBag.Statut != "Administrateur") && (ViewBag.Statut != "Coeur"))
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            return View("AddSurvey");
        }



        [HttpPost]
        public IActionResult AddAsk(string ask)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);

            if (!ViewBag.Logged)
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            if ((ViewBag.Statut != "Administrateur") && (ViewBag.Statut != "Coeur"))
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }



            Survey s = new Survey { Titre = ask, IdCreateur = Convert.ToInt32(ViewBag.Id) };

            s = s.AddAsk();

            return View("AddSurvey", s);
        }


        [HttpPost]
        public IActionResult AddAnswer(string answer, int idSurvey)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);

            if (!ViewBag.Logged)
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            if ((ViewBag.Statut != "Administrateur") && (ViewBag.Statut != "Coeur"))
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }


            Answer a = new Answer { IdSurvey = idSurvey, Label = answer };
            List<Answer> answers = a.AddAnswer();
            Survey s = new Survey { Id = idSurvey };
            s = s.GetSurvey();
            s.Answers = answers;
            return View("AddSurvey", s);
        }

        public IActionResult ModifSurvey(int id)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);

            if (!ViewBag.Logged)
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            if ((ViewBag.Statut != "Administrateur") && (ViewBag.Statut != "Coeur"))
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }


            Survey s = new Survey { Id = id };
            s = s.GetSurvey();

            return View("AddSurvey", s);
        }

        public IActionResult ValidSurvey(int idSurvey)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);

            if (!ViewBag.Logged)
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            if ((ViewBag.Statut != "Administrateur") && (ViewBag.Statut != "Coeur"))
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            Survey s = new Survey { Id = idSurvey };
            s.ValidSurvey();
            s = s.GetSurvey();


            Membres m = new Membres(); //Récupérer le membre auteur du sondage
            m = m.GetMembreById(s.IdCreateur);

            SurveyViewModel viewModel = new SurveyViewModel { Survey = s, Membre = m };

            return View("ViewSurvey", viewModel);
        }

        public IActionResult DeleteAnswer(int id, int idSurvey)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);

            if (!ViewBag.Logged)
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            if ((ViewBag.Statut != "Administrateur") && (ViewBag.Statut != "Coeur"))
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }

            Survey s = new Survey { Id = idSurvey };
            Answer a = new Answer { Id = id, IdSurvey = idSurvey };
            List<Answer> answers = a.DeleteAnswer();
            s = s.GetSurvey();
            s.Answers = answers;
            return View("AddSurvey", s);
        }

        public IActionResult DisplaySurvey(int id)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);


            Survey s = new Survey { Id = id };
            s = s.GetSurvey();

            Membres m = new Membres(); //Récupérer le membre auteur du sondage
            m = m.GetMembreById(s.IdCreateur);

            SurveyViewModel viewModel = new SurveyViewModel { Survey = s, Membre = m };


            bool DejaVote; // Vérifier si déjà voté
            if (ViewBag.Logged) 
            {
                AnswerByMembre answer = new AnswerByMembre();
                int IdMembre = Convert.ToInt32(ViewBag.Id);
                DejaVote = answer.VerifVoteMembre(IdMembre, id);
            }
            else
            {
                Visiteur v = new Visiteur();
                string remoteIpAddress = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
                AnswerByIp answer = new AnswerByIp();
                DejaVote = answer.VerifVoteIp(v.GetIdIp(remoteIpAddress), id);
            }

            if (DejaVote)
            {
                List<Survey> surveys = s.GetAllSurveys(true);
                SurveyResultViewModel result = new SurveyResultViewModel { Survey = s, Surveys = surveys, Membre = m};
                return View("ResultSurvey", result);
            }

            return View("ViewSurvey", viewModel);
        }


        [HttpPost]
        public IActionResult Vote(int? vote, int idSurvey)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            ViewBag.NbrePagesVues = GetPageVues();
            UserConnect(ViewBag);


            List<string> errors = new List<string>();

            if (vote == null)
            {
                errors.Add("Vous devez cocher une case pour voter!");
                ViewBag.errors = errors;
                Survey survey = new Survey { Id = idSurvey };
                survey = survey.GetSurvey();

                Membres membre = new Membres(); //Récupérer le membre auteur du sondage
                membre = membre.GetMembreById(survey.IdCreateur);
                SurveyViewModel viewModel = new SurveyViewModel { Survey = survey, Membre = membre };

                return View("ViewSurvey", viewModel);
            }

            if (ViewBag.Logged)
            {
                AnswerByMembre answer = new AnswerByMembre { IdMembre = Convert.ToInt32(ViewBag.Id), IdSurvey = idSurvey, IdAnswer = (int)vote };
                answer.SaveVoteMembre();
            }
            else
            {
                Visiteur v = new Visiteur();
                string remoteIpAddress = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
                AnswerByIp answer = new AnswerByIp { IdIp = v.GetIdIp(remoteIpAddress), IdAnswer = (int)vote, IdSurvey = idSurvey };
                answer.SaveVoteIdIp();
            }

            Survey s = new Survey { Id = idSurvey };
            s = s.GetSurvey();

            Membres m = new Membres(); //Récupérer le membre auteur du sondage
            m = m.GetMembreById(s.IdCreateur);
            List<Survey> surveys = s.GetAllSurveys(true);
            SurveyResultViewModel result = new SurveyResultViewModel { Survey = s, Surveys = surveys, Membre = m };
            return View("ResultSurvey", result);
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