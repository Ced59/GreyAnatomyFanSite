﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GreyAnatomyFanSite.Models.Persos;
using GreyAnatomyFanSite.Models.Serie;
using GreyAnatomyFanSite.Models.Site;
using GreyAnatomyFanSite.Tools;
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

        #region AddCommentaire

        public void AddComment(Commentaire commentaire)
        {
            IDbCommand command = new SqlCommand("INSERT INTO Commentaires (Titre, Texte, Date, IdMembre, TypePubli, IdPubli) VALUES (@Titre, @Texte, @Date, @IdMembre, @TypePubli, @IdPubli)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Titre", SqlDbType.VarChar) { Value = commentaire.Titre });
            command.Parameters.Add(new SqlParameter("@Texte", SqlDbType.Text) { Value = commentaire.Text });
            command.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime) { Value = commentaire.Date });
            command.Parameters.Add(new SqlParameter("@IdMembre", SqlDbType.Int) { Value = commentaire.IdMembre });
            command.Parameters.Add(new SqlParameter("@TypePubli", SqlDbType.VarChar) { Value = commentaire.TypePubli });
            command.Parameters.Add(new SqlParameter("@IdPubli", SqlDbType.Int) { Value = commentaire.IdPubli });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();

        }

        public void UpdateSeasonsSerie(List<Saison> saisons)
        {
            foreach (Saison s in saisons)
            {
                bool exist = seasonExist(s.Id);


                if (exist)
                {
                    IDbCommand command = new SqlCommand(
                    "UPDATE Saison SET " +
                    "DateDiffusion = @DateDiffusion, " +
                    "Nom = @Nom, " +
                    "Description = @Description, " +
                    "Affiche = @Affiche, " +
                    "NumeroSaison = @NumeroSaison " +
                    "WHERE IdTheMovieDB = @IdTheMovieDB",
                    (SqlConnection)ConnectionSerie.Instance);

                    command.Parameters.Add(new SqlParameter("@DateDiffusion", SqlDbType.Date) { Value = s.Air_date });
                    command.Parameters.Add(new SqlParameter("@Nom", SqlDbType.VarChar) { Value = s.Name });
                    command.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text) { Value = s.Overview });
                    command.Parameters.Add(new SqlParameter("@Affiche", SqlDbType.VarChar) { Value = s.Poster_path });
                    command.Parameters.Add(new SqlParameter("@NumeroSaison", SqlDbType.Int) { Value = s.Season_number });
                    command.Parameters.Add(new SqlParameter("@IdTheMovieDB", SqlDbType.Int) { Value = s.Id });

                    
                    ConnectionSerie.Instance.Open();
                    command.ExecuteNonQuery();
                    ConnectionSerie.Instance.Close();
                }
                else
                {
                    IDbCommand command = new SqlCommand(
                    "INSERT INTO Saison (IdSerie, DateDiffusion, Nom, Description, Affiche, NumeroSaison, IdTheMovieDB)" +
                    "VALUES (@IdSerie, @DateDiffusion, @Nom, @Description, @Affiche, @NumeroSaison, @IdTheMovieDB) ",
                    (SqlConnection)ConnectionSerie.Instance);

                    command.Parameters.Add(new SqlParameter("@IdSerie", SqlDbType.Int) { Value = s.IdSerie });
                    command.Parameters.Add(new SqlParameter("@DateDiffusion", SqlDbType.Date) { Value = s.Air_date });
                    command.Parameters.Add(new SqlParameter("@Nom", SqlDbType.VarChar) { Value = s.Name });
                    command.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text) { Value = s.Overview });
                    command.Parameters.Add(new SqlParameter("@Affiche", SqlDbType.VarChar) { Value = s.Poster_path });
                    command.Parameters.Add(new SqlParameter("@NumeroSaison", SqlDbType.Int) { Value = s.Season_number });
                    command.Parameters.Add(new SqlParameter("@IdTheMovieDB", SqlDbType.Int) { Value = s.Id });
                                       
                    ConnectionSerie.Instance.Open();
                    command.ExecuteNonQuery();
                    ConnectionSerie.Instance.Close();

                }
            }
        }

        private bool seasonExist(int id)
        {
            bool exist;
            IDbCommand command = new SqlCommand("SELECT * FROM Saison WHERE IdTheMovieDB = @IdTheMovieDB", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdTheMovieDB", SqlDbType.Int) { Value = id });
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();

            try
            {
                int test = reader.GetInt32(1);
                exist = true;
            }
            catch
            {
                exist = false;
            }

            reader.Close();
            command.Dispose();
            ConnectionSerie.Instance.Close();

            return exist;
        }

        public void UpdateSerieInfos(SerieInfo serieInfo)
        {

            bool exist = serieExist(serieInfo.Id);

            if (exist)
            {
                IDbCommand command = new SqlCommand(
                "UPDATE Serie SET " +
                "Titre = @Titre, " +
                "Homepage = @Homepage, " +
                "EnProduction = @EnProduction, " +
                "NbreSaisons = @NbreSaisons, " +
                "NbreEpisodes = @NbreEpisodes, " +
                "Description = @Description " +
                "WHERE IdMovieDatabase = @IdMovieDatabase",
                (SqlConnection)ConnectionSerie.Instance);

                command.Parameters.Add(new SqlParameter("@Titre", SqlDbType.VarChar) { Value = serieInfo.Original_name });
                command.Parameters.Add(new SqlParameter("@NbreSaisons", SqlDbType.Int) { Value = serieInfo.Number_of_seasons });
                command.Parameters.Add(new SqlParameter("@NbreEpisodes", SqlDbType.Int) { Value = serieInfo.Number_of_episodes });
                command.Parameters.Add(new SqlParameter("@Homepage", SqlDbType.VarChar) { Value = serieInfo.Homepage });
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text) { Value = serieInfo.Overview });
                command.Parameters.Add(new SqlParameter("@IdMovieDatabase", SqlDbType.Int) { Value = serieInfo.Id });

                if (serieInfo.In_production)
                {
                    command.Parameters.Add(new SqlParameter("@EnProduction", SqlDbType.Int) { Value = 1 });
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@EnProduction", SqlDbType.Int) { Value = 0 });
                }
                ConnectionSerie.Instance.Open();
                command.ExecuteNonQuery();
                ConnectionSerie.Instance.Close();
            }
            else
            {
                IDbCommand command = new SqlCommand(
                "INSERT INTO Serie (Titre, Homepage, EnProduction, NbreSaisons, NbreEpisodes, Description, IdMovieDatabase)" +
                "VALUES (@Titre, @Homepage, @EnProduction, @NbreSaisons, @NbreEpisodes, @Description, @IdMovieDatabase) ",
                (SqlConnection)ConnectionSerie.Instance);

                command.Parameters.Add(new SqlParameter("@Titre", SqlDbType.VarChar) { Value = serieInfo.Original_name });
                command.Parameters.Add(new SqlParameter("@NbreSaisons", SqlDbType.Int) { Value = serieInfo.Number_of_seasons });
                command.Parameters.Add(new SqlParameter("@NbreEpisodes", SqlDbType.Int) { Value = serieInfo.Number_of_episodes });
                command.Parameters.Add(new SqlParameter("@Homepage", SqlDbType.VarChar) { Value = serieInfo.Homepage });
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.Text) { Value = serieInfo.Overview });
                command.Parameters.Add(new SqlParameter("@IdMovieDatabase", SqlDbType.Int) { Value = serieInfo.Id });

                if (serieInfo.In_production)
                {
                    command.Parameters.Add(new SqlParameter("@EnProduction", SqlDbType.Int) { Value = 1 });
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@EnProduction", SqlDbType.Int) { Value = 0 });
                }
                ConnectionSerie.Instance.Open();
                command.ExecuteNonQuery();
                ConnectionSerie.Instance.Close();
            }


        }

        private bool serieExist(int id)
        {
            bool exist;
            IDbCommand command = new SqlCommand("SELECT * FROM Serie WHERE IdMovieDatabase = @IdMovieDatabase", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdMovieDatabase", SqlDbType.Int) { Value = id });
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();

            try
            {
                string test = reader.GetString(1);
                exist = true;
            }
            catch
            {
                exist = false;
            }

            reader.Close();
            command.Dispose();
            ConnectionSerie.Instance.Close();

            return exist;
        }

        public Acteur GetActeurById(Acteur acteur)
        {
            IDbCommand command = new SqlCommand("SELECT * FROM Acteurs WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = acteur.IdActeur });
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();
            try
            {
                acteur.NomActeur = reader.GetString(3);
            }
            catch
            {
                acteur.NomActeur = "undefined";
            }

            acteur.IdPerso = reader.GetInt32(1);

            try
            {
                acteur.DateNaissance = reader.GetDateTime(2);
            }
            catch
            {
                acteur.DateNaissance = Convert.ToDateTime(01 / 01 / 2000);
            }

            try
            {
                acteur.BioActeur = reader.GetString(5);
            }
            catch
            {
                acteur.BioActeur = "undefined";
            }

            reader.Close();
            command.Dispose();
            acteur.PrenomsActeur = GetPrenomsActeurs(acteur);
            ConnectionSerie.Instance.Close();
            return acteur;

        }


        public void DeletePrenom(int id)
        {
            IDbCommand command = new SqlCommand("DELETE FROM PrenomsPersos WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }

        public void DeleteSurnom(int id)
        {
            IDbCommand command = new SqlCommand("DELETE FROM SurnomsPersos WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }


        public void DeletePrenomActeur(int id)
        {
            IDbCommand command = new SqlCommand("DELETE FROM PrenomsActeurs WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }

        public void AddBirthdate(Personnage p)
        {
            IDbCommand command = new SqlCommand("UPDATE Acteurs SET DateNaissance = @DateNaissance WHERE Id = @IdActeur", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@DateNaissance", SqlDbType.DateTime) { Value = p.DateNaissance });
            command.Parameters.Add(new SqlParameter("@IdActeur", SqlDbType.Int) { Value = p.IdActeur });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }

        public void ModifBirthDate(Personnage p)
        {
            IDbCommand command = new SqlCommand("UPDATE Acteurs SET DateNaissance = @DateNaissance WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@DateNaissance", SqlDbType.DateTime) { Value = p.DateNaissance });
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = p.IdActeur });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }

        public void AddPhotos(Personnage p)
        {
            IDbCommand command = new SqlCommand();
            ConnectionSerie.Instance.Open();

            foreach (PhotoPerso photo in p.Photos)
            {
                command = new SqlCommand("UPDATE PhotosPersos SET Image = @Image, PhotoPrincipale = @Photoprincipale, IdPerso = @IdPerso WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarChar) { Value = photo.Url });
                command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
                command.Parameters.Add(new SqlParameter("@PhotoPrincipale", SqlDbType.Int) { Value = ConvertIntBool.ConvertBoolToInt(photo.PhotoPrincipale) });
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = photo.Id });



                command.ExecuteNonQuery();
                command.Dispose();
            }

            foreach (PhotoPerso photo in p.Photos)
            {
                command = new SqlCommand("SELECT Id FROM PhotosPersos WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = photo.Id });

                SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

                if (!reader.Read())
                {
                    reader.Close();
                    command.Dispose();

                    command = new SqlCommand("INSERT INTO PhotosPersos (IdPerso, Image, PhotoPrincipale) VALUES (@IdPerso, @Image, @PhotoPrincipale)", (SqlConnection)ConnectionSerie.Instance);
                    command.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarChar) { Value = photo.Url });
                    command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
                    command.Parameters.Add(new SqlParameter("@PhotoPrincipale", SqlDbType.Int) { Value = ConvertIntBool.ConvertBoolToInt(photo.PhotoPrincipale) });

                    command.ExecuteNonQuery();
                    command.Dispose();
                }

                else
                {
                    reader.Close();
                    command.Dispose();
                }
            }


            ConnectionSerie.Instance.Close();
        }



        public void UpdateStatutPresent(Personnage p)
        {
            IDbCommand command = new SqlCommand("UPDATE Persos SET StatutPresent = @StatutPresent WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@StatutPresent", SqlDbType.Int) { Value = ConvertIntBool.ConvertBoolToInt(p.StatutPresent) });
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = p.Id });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }

        public void UpdateStatutVivant(Personnage p)
        {
            IDbCommand command = new SqlCommand("UPDATE Persos SET StatutVivant = @StatutVivant WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@StatutVivant", SqlDbType.Int) { Value = ConvertIntBool.ConvertBoolToInt(p.StatutVivant) });
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = p.Id });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();

        }

        public void AddPrenom(Personnage p)
        {
            IDbCommand command = new SqlCommand("INSERT INTO PrenomsPersos (Prenom, IdPerso) VALUES (@Prenom, @IdPerso)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Prenom", SqlDbType.VarChar) { Value = p.Prenoms[0].Prenom });
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }

        public void AddActorName(Acteur a)
        {
            IDbCommand command = new SqlCommand("INSERT INTO PrenomsActeurs (Prenom, IdActeur) VALUES (@Prenom, @IdActeur)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Prenom", SqlDbType.VarChar) { Value = a.PrenomsActeur[0].Prenom });
            command.Parameters.Add(new SqlParameter("@IdActeur", SqlDbType.Int) { Value = a.IdActeur });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }


        public void AddSurnom(Personnage p)
        {
            IDbCommand command = new SqlCommand("INSERT INTO SurnomsPersos (Surnom, IdPerso) VALUES (@Surnom, @IdPerso)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Surnom", SqlDbType.VarChar) { Value = p.Surnoms[0].Surnom });
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            ConnectionSerie.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            ConnectionSerie.Instance.Close();
        }


        #endregion


        #region GetAllActeursNameFirstName

        public List<Acteur> GetAllActeurs()
        {
            List<Acteur> acteurs = new List<Acteur>();
            IDbCommand command = new SqlCommand("SELECT * FROM Acteurs", (SqlConnection)ConnectionSerie.Instance);
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            while (reader.Read())
            {
                Acteur a = new Acteur();
                a.NomActeur = reader.GetString(3);
                a.IdActeur = reader.GetInt32(0);
                acteurs.Add(a);
            }
            reader.Close();
            command.Dispose();

            foreach (Acteur a in acteurs)
            {
                a.PrenomsActeur = GetPrenomsActeurs(a);
            }

            ConnectionSerie.Instance.Close();
            return acteurs;
        }


        #endregion


        #region GetComments

        public List<Commentaire> GetComments(Commentaire commentaire)
        {
            List<Commentaire> Commentaires = new List<Commentaire>();
            IDbCommand command = new SqlCommand("SELECT * FROM Commentaires WHERE TypePubli = @TypePubli AND IdPubli = @IdPubli ORDER BY Date DESC", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@TypePubli", SqlDbType.VarChar) { Value = commentaire.TypePubli });
            command.Parameters.Add(new SqlParameter("@IdPubli", SqlDbType.Int) { Value = commentaire.IdPubli });
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            try
            {


                while (reader.Read())
                {
                    Commentaire c = new Commentaire { TypePubli = commentaire.TypePubli, IdPubli = commentaire.IdPubli };
                    c.Titre = reader.GetString(1);
                    c.Text = reader.GetString(2);
                    c.Date = reader.GetDateTime(3);
                    c.IdMembre = reader.GetInt32(4);
                    Commentaires.Add(c);
                }
                reader.Close();
                command.Dispose();
                ConnectionSerie.Instance.Close();

                if (Commentaires.Count == 0)
                {
                    return null;
                }
                return Commentaires;

            }
            catch
            {
                reader.Close();
                command.Dispose();
                ConnectionSerie.Instance.Close();
                return null;
            }
        }

        #endregion


        #region GetCategory by Id

        public CategoryArticle GetCategorieById(int id)
        {
            IDbCommand command = new SqlCommand("SELECT * FROM Categories WHERE Id = @Id", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();
            CategoryArticle c = new CategoryArticle { Id = id, TitreCategory = reader.GetString(1) };
            reader.Close();
            command.Dispose();
            ConnectionSerie.Instance.Close();
            return c;
        }

        #endregion


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
                article.Media = reader.GetString(1);
                if ((reader.GetString(1).Contains(".png") || reader.GetString(1).Contains(".jpg") || reader.GetString(1).Contains(".jpeg")))
                {
                    article.TypeMedia = "image";
                }

                else if ((reader.GetString(1).Contains(".mp4") || reader.GetString(1).Contains(".avi") || reader.GetString(1).Contains(".mpg")))
                {
                    article.TypeMedia = "video";
                }

                else
                {
                    article.TypeMedia = "autre";
                }
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

        public int GetNbreArticles(int? IdCategory)
        {

            if ((IdCategory == 0) || (IdCategory == null))
            {
                IDbCommand command = new SqlCommand("SELECT COUNT (*) FROM Articles", (SqlConnection)ConnectionSerie.Instance);
                ConnectionSerie.Instance.Open();
                int NbreArticles = (int)command.ExecuteScalar();
                command.Dispose();
                ConnectionSerie.Instance.Close();
                return NbreArticles;
            }

            else
            {
                IDbCommand command = new SqlCommand("SELECT COUNT (*) FROM Articles WHERE IdCategorie = @Id", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = IdCategory });
                ConnectionSerie.Instance.Open();
                int NbreArticles = (int)command.ExecuteScalar();
                command.Dispose();
                ConnectionSerie.Instance.Close();
                return NbreArticles;
            }

        }

        #endregion

        #region Get All Articles

        public List<Article> GetAllArticles(int? pagination, int? category)
        {
            if (pagination == null)
            {
                pagination = 0;
            }

            string request;

            if ((category != null) && (category != 0))
            {
                request = " WHERE IdCategorie = @IdCategorie ";
            }
            else
            {
                request = " ";
            }

            List<Article> articles = new List<Article>();

            IDbCommand command = new SqlCommand("SELECT * FROM Articles AS a INNER JOIN MediasArticles AS m ON a.Id = m.IdArticle" + request + "ORDER BY DatePubli DESC OFFSET @Pagination ROWS FETCH NEXT 10 ROWS ONLY", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Pagination", SqlDbType.Int) { Value = (pagination * 10) });
            if ((category != null) && (category != 0))
            {
                command.Parameters.Add(new SqlParameter("@IdCategorie", SqlDbType.Int) { Value = category });
            }
            ConnectionSerie.Instance.Open();
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            while (reader.Read())
            {
                CategoryArticle c = new CategoryArticle { Id = reader.GetInt32(3) };
                Membres m = new Membres { IdMembre = reader.GetInt32(4) };

                Article a = new Article { Id = reader.GetInt32(0), Titre = reader.GetString(1), Texte = reader.GetString(2), Categorie = c, Auteur = m, Date = reader.GetDateTime(5), Media = reader.GetString(7) };


                if ((reader.GetString(7).Contains(".png") || reader.GetString(7).Contains(".jpg") || reader.GetString(7).Contains(".jpeg")))
                {
                    a.TypeMedia = "image";
                }

                else if ((reader.GetString(7).Contains(".mp4") || reader.GetString(7).Contains(".avi") || reader.GetString(7).Contains(".mpg")))
                {
                    a.TypeMedia = "video";
                }

                else
                {
                    a.TypeMedia = "autre";
                }

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
                Personnage p = new Personnage { DateNaissance = reader.GetDateTime(0), NomActeur = reader.GetString(1), Nom = reader.GetString(3), IdActeur = reader.GetInt32(4), IdPerso = reader.GetInt32(5) };
                BirthDates.Add(p);
            }
            reader.Close();
            command.Dispose();

            foreach (Personnage p in BirthDates)
            {
                p.Prenoms = GetPrenomPersos(p);
                p.PrenomsActeur = GetPrenomsActeurs(p);
            }

            ConnectionSerie.Instance.Close();

            return BirthDates;
        }

        #endregion


        #region Ajouter Perso

        public Personnage AddNewPerso(Personnage p)
        {
            IDbCommand command = new SqlCommand("INSERT INTO Persos (Nom) OUTPUT INSERTED.ID VALUES (@Nom)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Nom", SqlDbType.VarChar) { Value = p.Nom });
            ConnectionSerie.Instance.Open();
            p.Id = (int)command.ExecuteScalar();
            command.Dispose();

            foreach (PrenomPerso prenom in p.Prenoms)
            {
                command = new SqlCommand("INSERT INTO PrenomsPersos (Prenom, IdPerso) OUTPUT INSERTED.ID VALUES (@Prenom, @IdPerso)", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Prenom", SqlDbType.VarChar) { Value = prenom.Prenom });
                command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
                prenom.Id = (int)command.ExecuteScalar();
                command.Dispose();
            }
            ConnectionSerie.Instance.Close();

            return p;
        }


        public void AddNewActor(Personnage p)
        {
            IDbCommand command = new SqlCommand("INSERT INTO Acteurs (Nom, IdPerso) OUTPUT INSERTED.ID VALUES (@Nom, @IdPerso)", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@Nom", SqlDbType.VarChar) { Value = p.NomActeur });
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.IdPerso });
            ConnectionSerie.Instance.Open();
            p.IdActeur = (int)command.ExecuteScalar();
            command.Dispose();

            foreach (PrenomActeur prenom in p.PrenomsActeur)
            {
                command = new SqlCommand("INSERT INTO PrenomsActeurs (Prenom, IdActeur) VALUES (@Prenom, @IdActeur)", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@Prenom", SqlDbType.VarChar) { Value = prenom.Prenom });
                command.Parameters.Add(new SqlParameter("@IdActeur", SqlDbType.Int) { Value = p.IdActeur });
                command.ExecuteNonQuery();
                command.Dispose();
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

                try
                {
                    p.Role = reader.GetString(4);
                }
                catch
                {
                    p.Role = null;
                }

                try
                {
                    p.Biographie = reader.GetString(4);
                }
                catch
                {
                    p.Biographie = null;
                }

                try
                {
                    if (reader.GetInt32(2) == 1)
                    {
                        p.StatutVivant = true;
                    }
                    else
                    {
                        p.StatutVivant = false;
                    }
                }
                catch
                {
                    p.StatutVivant = false;
                }

                try
                {
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
            p.PrenomsActeur = GetPrenomsActeurs(p);


            command = new SqlCommand("SELECT * FROM Acteurs WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = id });

            reader = (SqlDataReader)command.ExecuteReader();
            reader.Read();

            try
            {
                p.DateNaissance = reader.GetDateTime(2);
            }
            catch
            {
                p.DateNaissance = Convert.ToDateTime("01/01/0001");
            }

            try
            {
                p.NomActeur = reader.GetString(3);
            }
            catch
            {
                p.NomActeur = null;
            }

            try
            {
                p.BioActeur = reader.GetString(5);
            }
            catch
            {
                p.BioActeur = null;
            }

            try
            {
                p.IdActeur = reader.GetInt32(0);
            }
            catch
            {
                p.IdActeur = 0;
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
                    Personnage p = new Personnage { Id = reader.GetInt32(0), Nom = reader.GetString(1) };

                    try
                    {
                        if (reader.GetInt32(2) == 1)
                        {
                            p.StatutVivant = true;
                        }
                        else
                        {
                            p.StatutVivant = false;
                        }
                    }
                    catch
                    {
                        p.StatutVivant = false;
                    }

                    try
                    {
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


                command = new SqlCommand("SELECT * FROM Acteurs WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
                command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
                reader = (SqlDataReader)command.ExecuteReader();
                reader.Read();

                try
                {
                    p.DateNaissance = reader.GetDateTime(2);
                }
                catch
                {
                    p.DateNaissance = Convert.ToDateTime("01/01/0001");
                }

                try
                {
                    p.NomActeur = reader.GetString(3);
                }
                catch
                {
                    p.NomActeur = null;
                }

                try
                {
                    p.BioActeur = reader.GetString(5);
                }
                catch
                {
                    p.BioActeur = null;
                }

                try
                {
                    p.IdActeur = reader.GetInt32(0);
                }
                catch
                {
                    p.IdActeur = 0;
                }


                reader.Close();
                command.Dispose();

                p.PrenomsActeur = GetPrenomsActeurs(p);
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

            IDbCommand command = new SqlCommand("SELECT * FROM PhotosPersos WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            try
            {
                while (reader.Read())
                {

                    PhotoPerso photo = new PhotoPerso { Url = reader.GetString(2), Id = reader.GetInt32(0), IdPerso = p.Id, PhotoPrincipale = ConvertIntBool.ConvertIntToBool(reader.GetInt32(3)) };

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
            IDbCommand command = new SqlCommand("SELECT * FROM SurnomsPersos WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();

            try
            {
                while (reader.Read())
                {

                    SurnomPerso surnom = new SurnomPerso { Surnom = reader.GetString(1), Id = reader.GetInt32(0) };
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
            IDbCommand command = new SqlCommand("SELECT * FROM PrenomsPersos WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();


            try
            {
                while (reader.Read())
                {

                    PrenomPerso prenom = new PrenomPerso { Prenom = reader.GetString(1), Id = reader.GetInt32(0) };
                    prenoms.Add(prenom);
                }
            }
            catch
            {
                prenoms = null;
            }


            reader.Close();
            command.Dispose();

            return prenoms;
        }

        #endregion


        #region Récupérer prénoms acteurs

        private List<PrenomActeur> GetPrenomsActeurs(Personnage p)
        {
            IDbCommand command = new SqlCommand("SELECT Id FROM Acteurs WHERE IdPerso = @IdPerso", (SqlConnection)ConnectionSerie.Instance);

            if ((p.Id != 0))
            {
                command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.Id });
            }
            else
            {
                command.Parameters.Add(new SqlParameter("@IdPerso", SqlDbType.Int) { Value = p.IdPerso });
            }

            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
            try
            {
                reader.Read();
                p.IdActeur = reader.GetInt32(0);
                reader.Close();
                command.Dispose();
            }
            catch
            {
                p.IdActeur = 0;
                reader.Close();
                command.Dispose();
            }


            List<PrenomActeur> prenoms = new List<PrenomActeur>();
            command = new SqlCommand("SELECT * FROM PrenomsActeurs WHERE IdActeur = @IdActeur", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdActeur", SqlDbType.Int) { Value = p.IdActeur });
            reader = (SqlDataReader)command.ExecuteReader();


            try
            {
                while (reader.Read())
                {

                    PrenomActeur prenom = new PrenomActeur { Prenom = reader.GetString(1), Id = reader.GetInt32(0) };
                    prenoms.Add(prenom);
                }
            }
            catch
            {
                prenoms = null;
            }


            reader.Close();
            command.Dispose();

            return prenoms;
        }

        private List<PrenomActeur> GetPrenomsActeurs(Acteur a)
        {
            List<PrenomActeur> prenoms = new List<PrenomActeur>();
            IDbCommand command = new SqlCommand("SELECT * FROM PrenomsActeurs WHERE IdActeur = @IdActeur", (SqlConnection)ConnectionSerie.Instance);
            command.Parameters.Add(new SqlParameter("@IdActeur", SqlDbType.Int) { Value = a.IdActeur });
            SqlDataReader reader = (SqlDataReader)command.ExecuteReader();


            try
            {
                while (reader.Read())
                {

                    PrenomActeur prenom = new PrenomActeur { Prenom = reader.GetString(1), Id = reader.GetInt32(0) };
                    prenoms.Add(prenom);
                }
            }
            catch
            {
                prenoms = null;
            }


            reader.Close();
            command.Dispose();

            return prenoms;
        }
        #endregion
    }
}
