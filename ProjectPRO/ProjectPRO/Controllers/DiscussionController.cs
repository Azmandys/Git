using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPRO.ViewModels;
using ProjectPRO.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using System.Diagnostics;
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
            checkCR();
            var uid = User.Identity.GetUserId();
            var viewModel = new DiscussionViewModel();
            viewModel.gp = db.GroupPersons
                .Include(i => i.group)
                .Include(i => i.user);

            ViewBag.UserId = uid;

            List<GroupPerson> gpr = viewModel.gp.Where(u => u.user.Id == uid).ToList();
            List<Group> gro = new List<Group>();
            for (var i = 0; i < gpr.Count; i++)
                gro.Add(gpr[i].group);
            viewModel.groups = gro;
            //ask about connection IMPORTANT!!!!!! viewModel.groups
            //var userProfiles = _dataContext.UserProfile
            // .Where(t => idList.Contains(t.Id));

            if (gid != null)
            {
                List<Discussion> fil = new List<Discussion>();
                fil = db.Discussions.Where(f => f.group.gId == gid).ToList();
                ViewBag.GroupId = gid;
                viewModel.discussions = fil;
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
            checkCR();
            var viewModel = new LineViewModel();
            viewModel.discussions = db.Discussions;
            ViewBag.DiscLId = did;
            List<Line> fil = new List<Line>();
            fil = db.Lines.Where(f => f.disc.DiscId == did).ToList();
            viewModel.lines = fil;
            return View(viewModel);
        }

        public ActionResult createDisc(int? gid)
        {
            ViewBag.GrId = gid;
            return View();
        }

        public ActionResult createDis(int? grid, createDiscViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Discussions.Where(fg => fg.Name == model.Name).SingleOrDefault() != null)
                {
                    ModelState.AddModelError("Error", "This name is already occupied please think of another one");
                    return View("CreateDisc", model);

                }

                var discussion = new Discussion();
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
                discussion.group = grp;
                if (grp.discussions == null)
                {
                    grp.discussions = new List<Discussion>();
                }
                grp.discussions.Add(discussion);
                db.Discussions.Add(discussion);
                db.Groups.Attach(grp);
                db.SaveChanges();
                return RedirectToAction("Discussions", "Discussion");
            }
            return View(model);
        }

        public ActionResult addLine(int? did, LineViewModel model)
        {
            var line = new Line();
            line.text = model.crline;
            int maxId = 0;
            if (model.lines != null)
            {
                maxId = db.Lines.Max(l => l.LId);
            }
            ApplicationUser auth = db.Users.Find(User.Identity.GetUserId());
            Discussion dis = db.Discussions.Find(did);
            line.LId = maxId + 1;
            line.author = auth;
            line.disc = dis;
            db.Lines.Add(line);
            db.SaveChanges();

            return RedirectToAction("Line", "Discussion", new { did = did });
        }

        public void checkCR()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.chgRight == true)
            {
                ViewBag.chg = 1;
            }
            if (user.chgRight == false)
            {
                ViewBag.chg = 2;
            }
        }
    
    }
}