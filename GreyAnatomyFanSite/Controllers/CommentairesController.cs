﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GreyAnatomyFanSite.Controllers
{
    public class CommentairesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}