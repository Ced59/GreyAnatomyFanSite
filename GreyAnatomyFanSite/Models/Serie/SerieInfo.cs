using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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



        public string Original_name { get => original_name; set => original_name = value; }
        public string Overview { get => overview; set => overview = value; }
        public DateTime Last_air_date { get => last_air_date; set => last_air_date = value; }
        public string Homepage { get => homepage; set => homepage = value; }
        public int Number_of_seasons { get => number_of_seasons; set => number_of_seasons = value; }
        public int Number_of_episodes { get => number_of_episodes; set => number_of_episodes = value; }
        public int IdSerie { get => idSerie; set => idSerie = value; }
        public bool In_production { get => in_production; set => in_production = value; }
        public int Id { get => id; set => id = value; }

        internal SerieInfo updateWithTheMovieDB()
        {
            var client = new RestClient("https://api.themoviedb.org/3/tv/1416?append_to_response=JSON&language=fr-FR&api_key=d9f3e64a71289b0ce3eb8e272b1836d6");
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            var response2 = JsonConvert.DeserializeObject<SerieInfo>(response.Content);


            return response2;
        }

        public void updateSerieInfosInBdd()
        {
            BddSerie.Instance.UpdateSerieInfos(this);
        }
    }

}
