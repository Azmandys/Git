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
    public class FileController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        public FileController()
        {

        }

        public FileController(ApplicationUserManager userManager)
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
        public ActionResult Files(int? gid, int? did)
        {
            CheckCr();
            var uid = User.Identity.GetUserId();
            var viewModel = new FilesViewModel();
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
                List<File> fil;
                fil = db.Files.Where(f => f.Group.GId == gid).ToList();
                ViewBag.GroupId = gid;
                viewModel.File = fil;
            }
            return View(viewModel);
        }

        public ActionResult AddFile(int? gid)
        {
            ViewBag.GroId = gid;
            return View();
        }

        public ActionResult AddF(int? gro, AddFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Files.Where(fg => fg.Name == model.Name).SingleOrDefault() != null)
                {
                    ModelState.AddModelError("Error", @"This name is already occupied please think of another one");
                    return View("AddFile", model);

                }
                var file = new File();
                var uid = User.Identity.GetUserId();
                ApplicationUser auth = db.Users.Where(u => u.Id.Equals(uid)).Single();
                if (!db.Files.Any())
                {
                    file.Fid = 1;
                }
                else
                {
                    file.Fid = db.Files.Max(f => f.Fid) + 1;
                }
                file.Author = auth;
                Group gr = db.Groups.Where(g => g.GId == gro).Single();
                file.Group = gr;
                file.Name = model.Name;
                file.Link = model.Link;
                file.Created = DateTime.Now;
                if (gr.Files == null)
                {
                    gr.Files = new List<File>();
                }
                gr.Files.Add(file);
                if (auth.Files == null)
                {
                    auth.Files = new List<File>();
                }
                auth.Files.Add(file);
                db.Files.Add(file);
                db.Entry(gr).State = EntityState.Modified;
                db.Entry(auth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Files", "File");
            }
            return View(model);
        }
        public void CheckCr()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.ChgRight)
            {
                ViewBag.chg = 1;
            }
            if (user.ChgRight == false)
            {
                ViewBag.chg = 2;
            }
        }
    }
}