using System.Data.SqlClient;

namespace GreyAnatomyFanSite.Tools.Bdds.BddSerie
{
    public class ConnectionSerie
    {
        private static SqlConnection _instance = null;
        private static readonly object _lock = new object();
        public static SqlConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {

                        _instance = new SqlConnection(PassConnection.ConnectionBddSerie());

                        try
                        {
                            _instance.Open();
                            _instance.Close();
                        }
                        catch (System.Data.SqlClient.SqlException)
                        {
                            _instance = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projets Visual Studio\GreyAnatomyFanSiteWebRelease\GreyAnatomyFanSite\BddsOffline\BddGreyFanSiteSerie.mdf;Integrated Security=True;Connect Timeout=30");

                        }


                    }
                    return _instance;
                }
            }
        }
        private ConnectionSerie()
        {

        }
    }
}
