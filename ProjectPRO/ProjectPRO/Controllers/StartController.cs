using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPRO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using ProjectPRO;

namespace ProjectPRO.Controllers
{
    public class StartController : Controller
    {
        // GET: Start
        public ActionResult Index()
        {
            return View();
        }
    }
}