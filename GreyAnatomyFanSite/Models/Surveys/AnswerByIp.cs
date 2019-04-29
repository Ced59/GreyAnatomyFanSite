using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Surveys
{
    public class AnswerByIp
    {
        private int id;
        private int idIp;
        private int idSurvey;
        private int idAnswer;

        public int Id { get => id; set => id = value; }
        public int IdIp { get => idIp; set => idIp = value; }
        public int IdSurvey { get => idSurvey; set => idSurvey = value; }
        public int IdAnswer { get => idAnswer; set => idAnswer = value; }
    }
}
