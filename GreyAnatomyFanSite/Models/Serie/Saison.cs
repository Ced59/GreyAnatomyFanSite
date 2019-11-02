using GreyAnatomyFanSite.Tools;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Serie
{
    public class Saison
    {
        private int id;
        private int idSerie;
        private DateTime air_date;
        private string name;
        private string overview;
        private string poster_path;
        private int season_number;
        private int idTMDB;
        private List<Episode> episodes;

        public int Id { get => id; set => id = value; }
        public int IdSerie { get => idSerie; set => idSerie = value; }
        public DateTime Air_date { get => air_date; set => air_date = value; }
        public string Name { get => name; set => name = value; }
        public string Overview { get => overview; set => overview = value; }
        public string Poster_path { get => poster_path; set => poster_path = value; }
        public int Season_number { get => season_number; set => season_number = value; }
        public int IdTMDB { get => idTMDB; set => idTMDB = value; }
        public List<Episode> Episodes { get => episodes; set => episodes = value; }

        internal Saison updateSaisonWithMovieDB(int i, int idSerie)
        {
            var client = new RestClient(PassConnection.connectionTheMovieDBSeasons(i, idSerie));
            var request = new RestRequest(Method.GET);
            request.AddParameter("undefined", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                return null;
            }

            var responseObject = JsonConvert.DeserializeObject<Saison>(response.Content);

            responseObject.IdSerie = idSerie;

            return responseObject;
        }

        internal static void updateSeasonsInDatabase(List<Saison> saisons)
        {
            BddSerie.Instance.UpdateSeasonsSerie(saisons);
        }
    }
}
