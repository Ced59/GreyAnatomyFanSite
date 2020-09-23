using GreyAnatomyFanSite.Tools;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models.Serie
{
    public class SerieInfo
    {
        private int id;
        private int idSerie;
        private string original_name;
        private string overview;
        private DateTime last_air_date;
        private string homepage;
        private int number_of_seasons;
        private int number_of_episodes;
        private bool in_production;
        private string poster_path;
        private List<Saison> saisons;

        public string Original_name { get => original_name; set => original_name = value; }
        public string Overview { get => overview; set => overview = value; }
        public DateTime Last_air_date { get => last_air_date; set => last_air_date = value; }
        public string Homepage { get => homepage; set => homepage = value; }
        public int Number_of_seasons { get => number_of_seasons; set => number_of_seasons = value; }
        public int Number_of_episodes { get => number_of_episodes; set => number_of_episodes = value; }
        public int IdSerie { get => idSerie; set => idSerie = value; }
        public bool In_production { get => in_production; set => in_production = value; }
        public int Id { get => id; set => id = value; }
        public string Poster_path { get => poster_path; set => poster_path = value; }
        public List<Saison> Saisons { get => saisons; set => saisons = value; }

        internal SerieInfo updateWithTheMovieDB()
        {
            var client = new RestClient(PassConnection.connectionTheMovieDB());
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                return null;
            }

            var responseObject = JsonConvert.DeserializeObject<SerieInfo>(response.Content);

            return responseObject;
        }

        public void updateSerieInfosInBdd()
        {
            BddSerie.Instance.UpdateSerieInfos(this);
        }

        internal SerieInfo updatePrivatePracticeWithTheMovieDB()
        {
            var client = new RestClient(PassConnection.connectionTheMovieDBPrivatePractice());
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                return null;
            }

            var responseObject = JsonConvert.DeserializeObject<SerieInfo>(response.Content);

            return responseObject;
        }

        internal SerieInfo updateStationWithTheMovieDB()
        {
            var client = new RestClient(PassConnection.connectionTheMovieDBStation());
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                return null;
            }

            var responseObject = JsonConvert.DeserializeObject<SerieInfo>(response.Content);

            return responseObject;
        }

        internal SerieInfo getSerie(int idSerie)
        {
            return BddSerie.Instance.GetSerie(idSerie);
        }

        internal List<Saison> getSaisons()
        {
            return BddSerie.Instance.GetSaisons(this);
        }
    }
}