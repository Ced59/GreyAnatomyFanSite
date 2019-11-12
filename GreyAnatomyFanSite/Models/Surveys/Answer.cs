using GreyAnatomyFanSite.Tools.Bdds.BddSurveys;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.Models.Surveys
{
    public class Answer
    {
        private int id;
        private int idSurvey;
        private string label;
        private string image;
        private int nbreVote;
        private bool goodAnswer;

        public int Id { get => id; set => id = value; }
        public int IdSurvey { get => idSurvey; set => idSurvey = value; }
        public string Label { get => label; set => label = value; }
        public string Image { get => image; set => image = value; }
        public int NbreVote { get => nbreVote; set => nbreVote = value; }
        public bool GoodAnswer { get => goodAnswer; set => goodAnswer = value; }

        public List<Answer> AddAnswer()
        {
            return BddSurveys.Instance.AddAnswer(this);
        }

        public List<Answer> DeleteAnswer()
        {
            return BddSurveys.Instance.DeleteAnswer(this);
        }
    }
}
