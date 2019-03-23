using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Tools
{
    public static class PassConnection
    {

        public static string MailPassWord()
        {
            return "GreyFans59!";
        }

        public static string SmtpConfig()
        {
            return "smtp.ionos.fr";
        }

        public static string ConnectionBddSerie()
        {
            return @"Data Source=Provider=sqloledb; Data Source=db776017654.hosting-data.io,1433;Initial Catalog=db776017654;User Id=dbo776017654;Password=Corentinandme59!;";
        }

        public static string ConnectionBddMembres()
        {
            return @"Data Source=db775793599.hosting-data.io,1433;Initial Catalog=db775793599;User Id=dbo775793599;Password=Corentinandme59!;";
        }
    }
}
