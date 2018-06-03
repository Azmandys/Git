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
            checkCR();
            var uid = User.Identity.GetUserId();
            var viewModel = new FilesViewModel();
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
                List<File> fil = new List<File>();
                fil = db.Files.Where(f => f.group.gId == gid).ToList();
                ViewBag.GroupId = gid;
                viewModel.file = fil;
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
                if (db.Files.Where(fg => fg.name == model.name).SingleOrDefault() != null)
                {
                    ModelState.AddModelError("Error", "This name is already occupied please think of another one");
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
                Group gr = db.Groups.Where(g => g.gId == gro).Single();
                file.group = gr;
                file.name = model.name;
                file.link = model.link;
                if (gr.files == null)
                {
                    gr.files = new List<File>();
                }
                gr.files.Add(file);
                if (auth.files == null)
                {
                    auth.files = new List<File>();
                }
                auth.files.Add(file);
                db.Files.Add(file);
                db.Entry(gr).State = EntityState.Modified;
                db.Entry(auth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Files", "File");
            }
            return View(model);
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