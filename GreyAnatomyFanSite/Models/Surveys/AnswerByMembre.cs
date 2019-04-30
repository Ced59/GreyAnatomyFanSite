using GreyAnatomyFanSite.Tools.Bdds.BddSurveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreyAnatomyFanSite.Models.Surveys
{
    public class AnswerByMembre
    {
        private int id;
        private int idMembre;
        private int idSurvey;
        private int idAnswer;
        

        public int Id { get => id; set => id = value; }
        public int IdMembre { get => idMembre; set => idMembre = value; }
        public int IdSurvey { get => idSurvey; set => idSurvey = value; }
        public int IdAnswer { get => idAnswer; set => idAnswer = value; }
       

        public void SaveVoteMembre()
        {
            BddSurveys.Instance.SaveVoteMembre(this);
        }

        public bool VerifVoteMembre(int idMembre, int id)
        {
            return BddSurveys.Instance.VerifVoteMembre(idMembre, id);
        }
    }
}
