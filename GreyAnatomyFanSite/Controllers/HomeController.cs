using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GreyAnatomyFanSite.Models;
using Microsoft.AspNetCore.Http;
using GreyAnatomyFanSite.Models.Persos;
using GreyAnatomyFanSite.ViewModels;
using GreyAnatomyFanSite.Models.Site;
using System.IO;


namespace GreyAnatomyFanSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int pagination, int category)
        {
             
            ViewBag.NbreVisitUnique = GetVisitIP();
            UserConnect(ViewBag);

            Personnage p = new Personnage();
            List<Personnage> Acteurs = p.GetAllPersos();
            Article a = new Article();
            CategoryArticle ca = new CategoryArticle();

            int NbreArticles = a.GetNbreArticles(category);
            int nbrePagesPagination;

            if (NbreArticles == 0)
            {
                nbrePagesPagination = 1;
            }
            else
            {
                nbrePagesPagination = 0;

                while ((NbreArticles) >0)
                {
                    nbrePagesPagination++;
                    NbreArticles = NbreArticles - 10;
                }
                
            }

            int? paginationGetArticles = null;

            if (pagination != 0)
            {
                paginationGetArticles = pagination - 1;
            }

            HomeViewModel viewModel = new HomeViewModel { BirthDatesActeurs = Acteurs, Articles = a.GetAllArticles(paginationGetArticles, category), NbrePagePagination = nbrePagesPagination, PagePagination = pagination, CategoryArticles = ca.GetAllCategory(), ActiveCategory = category};  



            string remoteIpAddress = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
            Visiteur v = new Visiteur(remoteIpAddress);
            ViewBag.NbreVisitUnique = v.GetVisit(v);
            return View("Index", viewModel);
        }



        public IActionResult ViewArticle(int id)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            UserConnect(ViewBag);

            Article a = new Article { Id = id };

            a = a.GetArticle();
            
            a.Texte = a.Texte.Replace(Environment.NewLine, "<br/>");

            ViewBag.MetaDescription = a.Texte;

            Personnage perso = new Personnage();
            List<Personnage> Persos = perso.GetAllPersos();

            foreach (Personnage p in Persos)
            {
                a.Texte = a.Texte.Replace(p.Prenoms[0].Prenom, "<a href=\"/Personnages/ViewPersonnageID/" + p.Id + "\">" + p.Prenoms[0].Prenom + "</a>");
                a.Texte = a.Texte.Replace(p.Nom, "<a href=\"/Personnages/ViewPersonnageID/" + p.Id + "\">" + p.Nom + "</a>");
            }

            a.Texte = a.Texte.Replace("Grey Sloan Memorial", "<a href=\"/Serie/Hospital/\">" + "Grey Sloan Memorial Hospital" + "</a>");
            a.Texte = a.Texte.Replace("Grey Sloan Memorial Hospital", "<a href=\"/Serie/Hospital/\">" + "Grey Sloan Memorial Hospital" + "</a>");
            a.Texte = a.Texte.Replace("Seattle Grace", "<a href=\"/Serie/Hospital/\">" + "Seattle Grace" + "</a>");

            Membres m = new Membres();

            ArticleMembreViewModel viewModel = new ArticleMembreViewModel {Article = a, Membre = m};


            return View("ViewArticle", viewModel);
        }




        public IActionResult Articles(int? pagination)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            UserConnect(ViewBag);

            List<CategoryArticle> categories = new List<CategoryArticle>();


            if ((ViewBag.Statut == "Coeur") || (ViewBag.Statut == "Administrateur"))
            {
                CategoryArticle c = new CategoryArticle();
                Article a = new Article();
                List<Article> articles = new List<Article>();

                ArticlesCategoriesViewModel viewModel = new ArticlesCategoriesViewModel { Categories = c.GetAllCategory(), Articles = a.GetAllArticles(pagination, null) }; // Ajouter liste articles plus tard

                return View("ListArticles", viewModel);
            }

            else
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }
        }


        public IActionResult AddArticle()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            UserConnect(ViewBag);

            if ((ViewBag.Statut == "Coeur") || (ViewBag.Statut == "Administrateur"))
            {
                CategoryArticle c = new CategoryArticle();
                List<CategoryArticle> categories = c.GetAllCategory();

                return View("AddArticle", categories);
            }

            else
            {
                return RedirectToRoute(new { controller = "Membres", action = "Login" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddArticlePost(string titre, string text, int idCategory, IFormFile media)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            UserConnect(ViewBag);

            CategoryArticle c = new CategoryArticle { Id = idCategory };

            Membres m = new Membres { IdMembre = Convert.ToInt32(ViewBag.Id) };

            Article a = new Article { Titre = titre, Texte = text, Categorie = c, Auteur = m, Date = DateTime.Now };

            a = a.AddArticle();

            string NumeroUnique = Guid.NewGuid().ToString("N");


            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/MediasArticles", a.Id.ToString() + "-" + NumeroUnique + media.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await media.CopyToAsync(stream);
            MediaArticle mediaArticle = new MediaArticle
            {
                Url = "images/MediasArticles/" + a.Id.ToString() + "-" + NumeroUnique + media.FileName,
                IdArticle = a.Id

            };

            mediaArticle.AddMediaArticle();


            ArticlesCategoriesViewModel viewModel = new ArticlesCategoriesViewModel { Categories = c.GetAllCategory(), Articles = a.GetAllArticles(null, null) };

            return View("ListArticles", viewModel);

        }


        public IActionResult AddCategory()
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            UserConnect(ViewBag);

            return View("AddCategory");
        }

        [HttpPost]
        public IActionResult AddCategoryPost(string nom)
        {
            ViewBag.NbreVisitUnique = GetVisitIP();
            UserConnect(ViewBag);

            if (nom == null)
            {
                ViewBag.errors = "Veuillez entrer un nom valide";
                return View("AddCategory");
            }

            else
            {
                CategoryArticle c = new CategoryArticle { TitreCategory = nom };

                List<Article> articles = new List<Article>();

                ArticlesCategoriesViewModel a = new ArticlesCategoriesViewModel { Categories = c.AddCategory(), Articles = articles };

                return View("ListArticles", a);
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

        private int GetVisitIP()
        {
            string remoteIpAddress = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
            Visiteur v = new Visiteur(remoteIpAddress);
            return v.GetVisit(v);
        }
    }
}

