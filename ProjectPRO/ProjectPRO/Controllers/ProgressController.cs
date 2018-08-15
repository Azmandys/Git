using Microsoft.AspNet.Identity.Owin;
using ProjectPRO.Models;
using ProjectPRO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectPRO.Controllers
{
    public class ProgressController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        public ProgressController()
        {

        }

        public ProgressController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult ProgressList(int gid)
        {
            ProgressListViewModel model = new ProgressListViewModel();
            List<Progress> liM = new List<Progress>();
            liM = db.Progress.Where(p => p.Group.GId == gid).ToList();
            model.GrId = gid;
            model.Progress = liM;
            return View(model);
        }

        public ActionResult CreateProgress(int gid)
        {
            CreateProgressViewModel model = new CreateProgressViewModel();
            model.GrId = gid;
            return View(model);
        }

        public ActionResult CrProgress(CreateProgressViewModel model, int gid)
        {
            Progress prog = new Progress();
            if (!db.Progress.Any())
            {
                prog.PId = 1;
            }
            if (db.Progress.Any())
            {
                prog.PId = db.Progress.Max(p => p.PId)+1;
            }
            prog.Title = model.Title;
            var gro = db.Groups.Where(g => g.GId == gid).Single();
            prog.Group = gro;
            var val = Request.Form["prgRange"].ToString();
            prog.Prog = int.Parse(val);
            var color = Request.Form["ColorIn"].ToString();
            prog.Color = color;
            db.Progress.Add(prog);
            gro.Progress.Add(prog);
            db.Groups.Attach(gro);
            db.SaveChanges();

            return RedirectToAction("ProgressList", "Progress", new { @gid = gid });
        }

        public ActionResult ChangeProgress(int gid,int pid)
        {
            ChangeProgressViewModel model = new ChangeProgressViewModel();
            model.GrId = gid;
            model.PId = pid;
            return View(model);
        }

        public ActionResult ChProgress(ChangeProgressViewModel model,int gid, int pid)
        {
            var prog = db.Progress.Where(p => p.PId == pid).Single();
            var val = Request.Form["prgRange"].ToString();
            prog.Prog = int.Parse(val);
            var color = Request.Form["ColorIn"].ToString();
            prog.Color = color;
            UpdateModel(prog);
            db.SaveChanges();
            return RedirectToAction("ProgressList", "Progress", new { @gid = gid });
        }
    }

}