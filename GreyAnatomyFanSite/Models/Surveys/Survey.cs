using GreyAnatomyFanSite.Tools.Bdds.BddSurveys;
using System;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models.Surveys
{
    public class Survey
    {
        private int id;
        private string titre;
        private List<Answer> answers;
        private List<AnswerByIp> ipAnswers;
        private List<AnswerByMembre> membreAnswers;
        private string image;
        private int idCreateur;
        private bool online;
        private DateTime dateCreation;
        private int countVotes;


        public int Id { get => id; set => id = value; }

        public string Titre { get => titre; set => titre = value; }

        public List<Answer> Answers { get => answers; set => answers = value; }

        public List<AnswerByIp> IpAnswers { get => ipAnswers; set => ipAnswers = value; }

        public List<AnswerByMembre> MembreAnswers { get => membreAnswers; set => membreAnswers = value; }

        public string Image { get => image; set => image = value; }

        public int IdCreateur { get => idCreateur; set => idCreateur = value; }

        public bool Online { get => online; set => online = value; }

        public DateTime DateCreation { get => dateCreation; set => dateCreation = value; }

        public int CountVotes { get => countVotes; set => countVotes = value; }

        public List<Survey> GetAllSurveys(bool? online)
        {
            return BddSurveys.Instance.GetAllSurveys(online);
        }

        public Survey AddAsk()
        {
            return BddSurveys.Instance.AddSurvey(this);
        }

        public Survey GetSurvey()
        {
            return BddSurveys.Instance.GetSurvey(this);
        }

        public void ValidSurvey()
        {
            BddSurveys.Instance.Validation(this);
        }
    }
}
