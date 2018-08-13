using ProjectPRO.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPRO.ViewModels;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;

namespace ProjectPRO.Controllers
{
    public class DiscussionController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public DiscussionController()
        {

        }
        public DiscussionController(ApplicationUserManager userManager)
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
        public ActionResult Discussions(int? gid, int? did)
        {
            CheckCr();
            var uid = User.Identity.GetUserId();
            var viewModel = new DiscussionViewModel();
            viewModel.Gp = db.GroupPersons
                .Include(i => i.Group)
                .Include(i => i.User);

            ViewBag.UserId = uid;

            List<GroupPerson> gpr = viewModel.Gp.Where(u => u.User.Id == uid && u.Status=="Confirmed").ToList();
            List<Group> gro = new List<Group>();
            for (var i = 0; i < gpr.Count; i++)
                gro.Add(gpr[i].Group);
            viewModel.Groups = gro;
            //ask about connection IMPORTANT!!!!!! viewModel.groups
            //var userProfiles = _dataContext.UserProfile
            // .Where(t => idList.Contains(t.Id));

            if (gid != null)
            {
                List<Discussion> fil;
                fil = db.Discussions.Where(f => f.Group.GId == gid).ToList();
                ViewBag.GroupId = gid;
                viewModel.Discussions = fil;
            }
            if (did != null)
            {
                ViewBag.DiscId = did;
                ViewBag.DiscName = db.Discussions.Where(d => d.DiscId == did).Single().Name;
            }
            return View(viewModel);
        }

        public ActionResult Line(int? did)
        {
            CheckCr();
            var viewModel = new LineViewModel();
            viewModel.Discussions = db.Discussions;
            ViewBag.DiscLId = did;
            List<Line> fil;
            fil = db.Lines.Where(f => f.Disc.DiscId == did).ToList();
            viewModel.Lines = fil;
            return View(viewModel);
        }

        public ActionResult CreateDisc(int? gid)
        {
            ViewBag.GrId = gid;
            return View();
        }

        public ActionResult CreateDis(int? grid, CreateDiscViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Discussions.Where(fg => fg.Name == model.Name).SingleOrDefault() != null)
                {
                    ModelState.AddModelError("Error", @"This name is already occupied please think of another one");
                    return View("CreateDisc", model);

                }

                var discussion = new Discussion();
                discussion.Created = DateTime.Now;
                if (!db.Discussions.Any())
                {
                    discussion.DiscId = 1;
                }
                else
                {
                    discussion.DiscId = db.Discussions.Max(d => d.DiscId) + 1;
                }
                discussion.Name = model.Name;
                Group grp = db.Groups.Find(grid);
                discussion.Group = grp;
                var userid = User.Identity.GetUserId();
                discussion.Creator = db.Users.Where(c => c.Id == userid).Single();
                if (grp != null && grp.Discussions == null)
                {
                    grp.Discussions = new List<Discussion>();
                }

                if (grp != null)
                {
                    grp.Discussions.Add(discussion);
                    db.Discussions.Add(discussion);
                    db.Groups.Attach(grp);
                }

                db.SaveChanges();
                return RedirectToAction("Discussions", "Discussion");
            }
            return View(model);
        }

        public ActionResult AddLine(int? did, LineViewModel model)
        {
            var line = new Line();
            line.Text = model.Crline;
            int maxId = 0;
            if (model.Lines != null)
            {
                maxId = db.Lines.Max(l => l.LId);
            }
            ApplicationUser auth = db.Users.Find(User.Identity.GetUserId());
            Discussion dis = db.Discussions.Find(did);
            line.LId = maxId + 1;
            line.Author = auth;
            line.Disc = dis;
            line.Created = DateTime.Now;
            db.Lines.Add(line);
            db.SaveChanges();

            return RedirectToAction("Line", "Discussion", new {did });
        }

        public void CheckCr()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            switch (user.ChgRight)
            {
                case true:
                    ViewBag.chg = 1;
                    break;
                case false:
                    ViewBag.chg = 2;
                    break;
            }
        }
    
    }
}