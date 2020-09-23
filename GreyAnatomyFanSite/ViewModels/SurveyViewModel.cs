using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Surveys;

namespace GreyAnatomyFanSite.ViewModels
{
    public class SurveyViewModel
    {
        private Survey survey;
        private Membres membre;

        public Survey Survey { get => survey; set => survey = value; }
        public Membres Membre { get => membre; set => membre = value; }
    }
}