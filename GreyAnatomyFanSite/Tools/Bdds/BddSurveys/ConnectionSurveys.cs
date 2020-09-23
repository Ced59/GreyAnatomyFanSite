using System.Data.SqlClient;

namespace GreyAnatomyFanSite.Tools.Bdds.BddSurveys
{
    public class ConnectionSurveys
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
                        _instance = new SqlConnection(PassConnection.ConnectionBddSurveys());

                        try
                        {
                            _instance.Open();
                            _instance.Close();
                        }
                        catch (System.Data.SqlClient.SqlException)
                        {
                            _instance = new SqlConnection(@"Data Source=226114-18021;Initial Catalog=db782547998;Integrated Security=True");
                        }
                    }
                    return _instance;
                }
            }
        }

        private ConnectionSurveys()
        {
        }
    }
}