using GreyAnatomyFanSite.Models.Persos;
using GreyAnatomyFanSite.Tools;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models.Serie
{
    public class Episode
    {
        private DateTime air_date;
        private int episode_number;
        private int id;
        private int idTheMovieDB;
        private string name;
        private string overview;
        private int season_number;
        private int show_id;
        private string still_path;
        private List<Personnage> personnages;
        private List<Patient> patients;
        private EpisodeImages photos;
        private int idEpisode;
        private int idSaison;
        private Realisateur realisateur;
        private Scenarist scenarist;
        private int noProduction;

        public List<Personnage> Personnages { get => personnages; set => personnages = value; }
        public List<Patient> Patients { get => patients; set => patients = value; }
        public int IdEpisode { get => idEpisode; set => idEpisode = value; }
        public int IdSaison { get => idSaison; set => idSaison = value; }
        public string Name { get => name; set => name = value; }
        public Realisateur Realisateur { get => realisateur; set => realisateur = value; }
        public Scenarist Scenarist { get => scenarist; set => scenarist = value; }
        public int NoProduction { get => noProduction; set => noProduction = value; }
        public string Overview { get => overview; set => overview = value; }
        public int Season_number { get => season_number; set => season_number = value; }
        public int Show_id { get => show_id; set => show_id = value; }
        public string Still_path { get => still_path; set => still_path = value; }
        public DateTime Air_date { get => air_date; set => air_date = value; }
        public int Episode_number { get => episode_number; set => episode_number = value; }
        public int Id { get => id; set => id = value; }
        public int IdTheMovieDB { get => idTheMovieDB; set => idTheMovieDB = value; }
        public EpisodeImages Photos { get => photos; set => photos = value; }

        internal EpisodeImages updatePhotosEpisodeWithMovieDB(int idSerie)
        {
            EpisodeImages episodeImages = new EpisodeImages();

            var client = new RestClient(PassConnection.connectionTheMovieDBPhotosEpisodes(idSerie, this));
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var responseObject = JsonConvert.DeserializeObject<EpisodeImages>(response.Content);

            return responseObject;
        }
    }
}