﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models.Persos;
using GreyAnatomyFanSite.Models.Site;
using GreyAnatomyFanSite.Tools.Bdds.BddSerie;

namespace GreyAnatomyFanSite.Models
{
    public class BddSerie
    {
        private static BddSerie _instance = null;
        private static readonly object _lock = new object();

        public static BddSerie Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BddSerie();
                    }
                    return _instance;
                }
            }
        }


        #region Add Category

        public List<CategoryArticle> AddCategory(CategoryArticle categoryArticle)
        {

            IDbCommand command = new SqlCommand("INSERT INTO Categories (Titre) VALUES (@Titre)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Titre", SqlDbType.VarChar) { Value = categoryArticle.TitreCategory });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();

            return GetCategories();
        }

        #endregion


        public List<Article> GetArticlesByCategorie()
        {
            throw new NotImplementedException();
        }

        public Article GetArticle(Article article, out int IdAuteur)
        {
            CategoryArticle c = new CategoryArticle();

            IDbCommand command = new SqlCommand("SELECT * FROM Articles WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = article.Id });
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            if (reader.Read())
            {
                article.Titre = reader.GetString(1);
                article.Texte = reader.GetString(2);
                article.Date = reader.GetDateTime(5);
                IdAuteur = reader.GetInt32(4);
                int IdCategory = reader.GetInt32(3);
                reader.Close();
                command.Dispose();



                command = new SqlCommand("SELECT * FROM Categories WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = IdCategory });
                reader = (SqlDataReader)command.ExecuteReader();
                reader.Read();
                c.TitreCategory = reader.GetString(1);
                article.Categorie = c;
                reader.Close();
                command.Dispose();


                command = new SqlCommand("SELECT * FROM MediasArticles WHERE IdArticle = @Id", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = article.Id });
                reader = (SqlDataReader)command.ExecuteReader();
                reader.Read();
                article.Photo = reader.GetString(1);
                reader.Close();
                command.Dispose();

                ConnectionSerie.Instance.Close();
                return article;
            }

            else
            {
                IdAuteur = 0;
                return null;
            }

        }


        #region GetNbreArticle

        public int GetNbreArticles()
        {
            IDbCommand command = new SqlCommand("SELECT COUNT (*) FROM Articles", (SqlConnection)ConnectionSerie.Instance);
            ConnectionSerie.Instance.Open();
            int NbreArticles = (int)command.ExecuteScalar();
            command.Dispose();
            ConnectionSerie.Instance.Close();
            return NbreArticles;
        }

        #endregion

        #region Get All Articles

        public List<Article> GetAllArticles(int? pagination)
        {
            if (pagination == null)
            {
                pagination = 0;
            }

            
            List<Article> articles = new List<Article>();

            IDbCommand command = new SqlCommand("SELECT * FROM Articles AS a INNER JOIN MediasArticles AS m ON a.Id = m.IdArticle ORDER BY DatePubli DESC OFFSET @Pagination ROWS FETCH NEXT 10 ROWS ONLY", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Pagination", SqlDbType.Int) { Value = (pagination * 10) });
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            while (reader.Read())
            {
                CategoryArticle c = new CategoryArticle { Id = reader.GetInt32(3) };
                Membres m = new Membres { IdMembre = reader.GetInt32(4) };

                Article a = new Article { Id = reader.GetInt32(0), Titre = reader.GetString(1), Texte = reader.GetString(2), Categorie = c, Auteur = m, Date = reader.GetDateTime(5), Photo = reader.GetString(7) };

                articles.Add(a);
            }

            reader.Close();
            command.Dispose();
            ConnectionSerie.Instance.Close();

            return articles;
        }


        #endregion

        #region AddArticle

        public Article AddArticle(Article article)
        {
            IDbCommand command = new SqlCommand("INSERT INTO Articles (Titre, Texte, IdCategorie, IdAuteur, DatePubli) OUTPUT INSERTED.ID VALUES (@Titre, @Texte, @IdCategorie, @IdAuteur, @DatePubli)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Titre", SqlDbType.VarChar) { Value = article.Titre });
            command.Parameters.Add(new SqlParameter("@Texte", SqlDbType.Text) { Value = article.Texte });
            command.Parameters.Add(new SqlParameter("@IdCategorie", SqlDbType.Int) { Value = article.Categorie.Id });
            command.Parameters.Add(new SqlParameter("@IdAuteur", SqlDbType.Int) { Value = article.Auteur.IdMembre });
            command.Parameters.Add(new SqlParameter("@DatePubli", SqlDbType.DateTime) { Value = article.Date });

            ConnectionSerie.Instance.Open();
            article.Id = (int)command.ExecuteScalar();
            command.Dispose();
            ConnectionSerie.Instance.Close();
            return article;

        }

        public void AddMediaArticle(MediaArticle m)
        {
            IDbCommand command = new SqlCommand("INSERT INTO MediasArticles (Url, IdArticle) VALUES (@Url, @IdArticle)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Url", SqlDbType.VarChar) { Value = m.Url });
            command.Parameters.Add(new SqlParameter("@IdArticle", SqlDbType.Int) { Value = m.IdArticle });

            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();

        }


        #endregion

        #region Récuperer toutes les catégories d'articles

        public List<CategoryArticle> GetCategories()
        {
            List<CategoryArticle> categories = new List<CategoryArticle>();

            IDbCommand command = new SqlCommand("SELECT * FROM Categories", (SqlConnection)ConnectionSerie.Instance);
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            while (reader.Read())
            {
                CategoryArticle c = new CategoryArticle { Id = reader.GetInt32(0), TitreCategory = reader.GetString(1) };
                categories.Add(c);
            }

            reader.Close();
            command.Dispose();
            ConnectionSerie.Instance.Close();
            return categories;

        }

        #endregion


        #region GetAllActeursBirthDate

        public List<Personnage> GetAllActeursBirthDate()
        {
            List<Personnage> BirthDates = new List<Personnage>();

            IDbCommand command = new SqlCommand("SELECT a.DateNaissance, a.Nom, a.Prenom, p.Nom, a.Id, p.Id FROM Acteurs AS a INNER JOIN Persos AS p ON a.IdPerso = p.Id", (SqlConnection)ConnectionSerie.Instance);
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            while (reader.Read())
            {
                Personnage p = new Personnage { DateNaissance = reader.GetDateTime(0), NomActeur = reader.GetString(1), PrenomActeur = reader.GetString(2), Nom = reader.GetString(3), IdActeur = reader.GetInt32(4), IdPerso = reader.GetInt32(5) };
                BirthDates.Add(p);
            }
            reader.Close();
            command.Dispose();

            foreach (Personnage p in BirthDates)
            {
                p.Prenoms = GetPrenomPersos(p);
            }

            ConnectionSerie.Instance.Close();

            return BirthDates;
        }

        #endregion


        #region Ajouter Perso

        public void AddPerso(Personnage p)
        {
            int present = ConvertBoolToInt(p.StatutPresent);
            int vivant = ConvertBoolToInt(p.StatutVivant);

            IDbCommand command = new SqlCommand("INSERT INTO Persos (Nom, Role, Biographie, StatutVivant, StatutPresent) OUTPUT INSERTED.ID VALUES (@Nom, @Role, @Biographie, @StatutVivant, @StatutPresent)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Nom", SqlDbType.VarChar) { Value = p.Nom });
            command.Parameters.Add(new SqlParameter("@Role", SqlDbType.VarChar) { Value = p.Role });
            command.Parameters.Add(new SqlParameter("@StatutVivant", SqlDbType.Int) { Value = vivant });
            command.Parameters.Add(new SqlParameter("@StatutPresent", SqlDbType.Int) { Value = present });
            command.Parameters.Add(new SqlParameter("@Biographie", SqlDbType.VarChar) { Value = p.Biographie });


            ConnectionSerie.Instance.Open();
            int Id = (int)command.ExecuteScalar();
            command.Dispose();


            command = new SqlCommand("INSERT INTO Acteurs (IdPerso, DateNaissance, Nom, Prenom, Biographie) VALUES (@IdPerso, @DateNaissance, @Nom, @Prenom, @Biographie)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = Id });
            command.Parameters.Add(new SqlParameter("@Nom", SqlDbType.VarChar) { Value = p.NomActeur });
            command.Parameters.Add(new SqlParameter("@Prenom", SqlDbType.VarChar) { Value = p.PrenomActeur });
            command.Parameters.Add(new SqlParameter("@Biographie", SqlDbType.VarChar) { Value = p.BioActeur });
            command.Parameters.Add(new SqlParameter("@DateNaissance", SqlDbType.Date) { Value = p.DateNaissance });

            command.ExecuteNonQuery();
            command.Dispose();

            foreach (PrenomPerso prenom in p.Prenoms)
            {
                command = new SqlCommand("INSERT INTO PrenomsPersos (Prenom, IdPerso) VALUES (@Prenom, @IdPerso)", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Prenom", SqlDbType.VarChar) { Value = prenom.Prenom });
                command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = Id });
                command.ExecuteNonQuery();
                command.Dispose();
            }

            foreach (PhotoPerso photo in p.Photos)
            {
                command = new SqlCommand("INSERT INTO PhotosPersos (Image, IdPerso, PhotoPrincipale) VALUES (@Image, @IdPerso, @PhotoPrincipale)", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarChar) { Value = photo.Url });
                command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = Id });
                command.Parameters.Add(new SqlParameter("@PhotoPrincipale", SqlDbType.Int) { Value = 1 });

                command.ExecuteNonQuery();
                command.Dispose();
            }

            if (p.Surnoms != null)
            {
                foreach (SurnomPerso s in p.Surnoms)
                {
                    command = new SqlCommand("INSERT INTO SurnomsPersos (Surnom, IdPerso) VALUES (@Surnom, @IdPerso)", (SqlConnection)ConnectionSerie.Instance);
                    command.Parameters.Add(new SqlParameter("@Surnom", SqlDbType.VarChar) { Value = s.Surnom });
                    command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = Id });
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }


            ConnectionSerie.Instance.Close();

        }

        #endregion

        #region Récupérer Perso Par Id

        public Personnage GetPersoByID(int id)
        {
            IDbCommand command = new SqlCommand("SELECT * FROM Persos WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            Personnage p = new Personnage();

            try
            {
                reader.Read();
                p.Id = id;
                p.Nom = reader.GetString(1);
                p.Role = reader.GetString(4);
                p.Biographie = reader.GetString(5);

                if (reader.GetInt32(2) == 1)
                {
                    p.StatutVivant = true;
                }
                else
                {
                    p.StatutVivant = false;
                }

                if (reader.GetInt32(3) == 1)
                {
                    p.StatutPresent = true;
                }
                else
                {
                    p.StatutPresent = false;
                }
            }
            catch
            {
                reader.Close();
                command.Dispose();
                ConnectionSerie.Instance.Close();

                return null;
            }

            reader.Close();
            command.Dispose();

            p.Prenoms = GetPrenomPersos(p);
            p.Photos = GetPhotosPerso(p);
            p.Surnoms = GetSurnomsPersos(p);

            try
            {
                command = new SqlCommand("SELECT * FROM Acteurs WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = id });

                reader = (SqlDataReader)command.ExecuteReader();
                reader.Read();

                p.DateNaissance = reader.GetDateTime(2);
                p.NomActeur = reader.GetString(3);
                p.PrenomActeur = reader.GetString(4);
                p.BioActeur = reader.GetString(5);
                p.IdActeur = reader.GetInt32(0);
            }
            catch
            {
                p.NomActeur = null;
                p.PrenomActeur = null;
                p.BioActeur = null;
            }

            reader.Close();
            command.Dispose();


            ConnectionSerie.Instance.Close();
            return p;
        }
        #endregion


        #region Récupérer liste des acteurs

        public List<Personnage> GetAllPersos()
        {
            List<Personnage> l = new List<Personnage>();

            #region Recuperer tous les persos

            IDbCommand command = new SqlCommand("SELECT * FROM Persos", (SqlConnection)ConnectionSerie.Instance);
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();


            try
            {
                while (reader.Read())
                {
                    Personnage p = new Personnage { Id = reader.GetInt32(0), Nom = reader.GetString(1), Role = reader.GetString(4), Biographie = reader.GetString(5) };
                    if (reader.GetInt32(2) == 1)
                    {
                        p.StatutVivant = true;
                    }
                    else
                    {
                        p.StatutVivant = false;
                    }

                    if (reader.GetInt32(3) == 1)
                    {
                        p.StatutPresent = true;
                    }
                    else
                    {
                        p.StatutPresent = false;
                    }


                    l.Add(p);
                }

                reader.Close();
                command.Dispose();
            }
            catch
            {
                reader.Close();
                command.Dispose();
                ConnectionSerie.Instance.Close();

                return null;
            }




            #endregion


            foreach (Personnage p in l)
            {
                #region Recuperer les prenoms des persos


                p.Prenoms = GetPrenomPersos(p);

                #endregion


                #region Recuperer les surnoms des persos


                p.Surnoms = GetSurnomsPersos(p);

                #endregion

                #region Recuperer les photos des persos


                p.Photos = GetPhotosPerso(p);


                #endregion

                #region Récupérer infos Acteurs

                try
                {
                    command = new SqlCommand("SELECT * FROM Acteurs WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
                    command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
                    reader = (SqlDataReader)command.ExecuteReader();
                    reader.Read();
                    p.DateNaissance = reader.GetDateTime(2);
                    p.NomActeur = reader.GetString(3);
                    p.PrenomActeur = reader.GetString(4);
                    p.BioActeur = reader.GetString(5);
                    p.IdActeur = reader.GetInt32(0);
                }
                catch
                {
                    p.NomActeur = null;
                    p.PrenomActeur = null;
                    p.BioActeur = null;
                }

                reader.Close();
                command.Dispose();

                #endregion
            }


            ConnectionSerie.Instance.Close();
            return l;


        }

        #endregion


        #region Récuperer Photos Persos


        private List<PhotoPerso> GetPhotosPerso(Personnage p)
        {
            List<PhotoPerso> photos = new List<PhotoPerso>();

            IDbCommand command = new SqlCommand("SELECT Image, PhotoPrincipale FROM PhotosPersos WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            try
            {
                while (reader.Read())
                {

                    PhotoPerso photo = new PhotoPerso { Url = reader.GetString(0) };
                    if (reader.GetInt32(1) == 1)
                    {
                        photo.PhotoPrincipale = true;
                    }
                    else
                    {
                        photo.PhotoPrincipale = false;
                    }

                    photos.Add(photo);
                }
            }
            catch
            {
                photos = null;
            }

            reader.Close();
            command.Dispose();

            return photos;
        }

        #endregion

        #region Récupérer Surnoms Persos

        private List<SurnomPerso> GetSurnomsPersos(Personnage p)
        {
            List<SurnomPerso> surnoms = new List<SurnomPerso>();
            IDbCommand command = new SqlCommand("SELECT Surnom FROM SurnomsPersos WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            try
            {
                while (reader.Read())
                {

                    SurnomPerso surnom = new SurnomPerso { Surnom = reader.GetString(0) };
                    surnoms.Add(surnom);
                }
            }
            catch
            {
                surnoms = null;
            }


            reader.Close();
            command.Dispose();

            return surnoms;
        }

        #endregion


        #region Récupérer Prénoms Persos

        private List<PrenomPerso> GetPrenomPersos(Personnage p)
        {
            List<PrenomPerso> prenoms = new List<PrenomPerso>();
            IDbCommand command = new SqlCommand("SELECT Prenom FROM PrenomsPersos WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();


            while (reader.Read())
            {

                PrenomPerso prenom = new PrenomPerso { Prenom = reader.GetString(0) };
                prenoms.Add(prenom);
            }

            reader.Close();
            command.Dispose();

            return prenoms;
        }


        #endregion


        #region Conversion Booléen en Int

        private int ConvertBoolToInt(bool b)
        {
            if (b)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}