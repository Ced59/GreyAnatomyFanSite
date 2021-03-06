﻿using GreyAnatomyFanSite.Models;
using GreyAnatomyFanSite.Models.Surveys;
using System.Collections.Generic;

namespace GreyAnatomyFanSite.ViewModels
{
    public class SurveyResultViewModel
    {
        private Survey survey;
        private List<Survey> surveys;
        private Membres membre;
        

        public Survey Survey { get => survey; set => survey = value; }
        public List<Survey> Surveys { get => surveys; set => surveys = value; }
        public Membres Membre { get => membre; set => membre = value; }
    }
}
