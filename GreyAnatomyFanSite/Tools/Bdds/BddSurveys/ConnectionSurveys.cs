using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
                            _instance = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\GreyAnatomyFanSiteWebRelease\GreyAnatomyFanSite\BddsOffline\Survey.mdf;Integrated Security=True;Connect Timeout=30");

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
