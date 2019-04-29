using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
