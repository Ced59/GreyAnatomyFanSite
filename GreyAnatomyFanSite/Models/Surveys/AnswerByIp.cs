using GreyAnatomyFanSite.Tools.Bdds.BddSurveys;
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

        public void SaveVoteIdIp()
        {
            BddSurveys.Instance.SaveVoteIdIp(this);
        }

        public bool VerifVoteIp(int idIp, int idSurvey)
        {
            return BddSurveys.Instance.VerifVoteIdIp(idIp, idSurvey);
        }
    }
}
