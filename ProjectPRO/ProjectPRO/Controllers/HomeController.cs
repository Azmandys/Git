using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProjectPRO.Models;
using ProjectPRO.ViewModels;

namespace ProjectPRO.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        public HomeController()
        {

        }

        public HomeController(ApplicationUserManager userManager)
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
 
        public ActionResult Index()
        {
            CheckCr();
            return View();
        }

        public ActionResult AddGroups()
        {
            CheckCr();
            if (ViewBag.chg == 2)
            {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable < SelectListItem > slItem = from x in db.Groups
                select new SelectListItem
                {
                    Selected = x.GId.ToString() == "Active",
                    Value = x.GId.ToString(),
                    Text = x.Name
                };
            IEnumerable<SelectListItem> sluItem = from x in db.Users
                select new SelectListItem
                {
                    Selected = x.Id == "Active",
                    Value = x.Id,
                    Text = x.Name
                };
            var model = new AddGroupsViewModel
            {
                Groups = slItem,
                Users = sluItem
            };



            return View(model);
        }

        public ActionResult AddGroupsParam()
        {
            CheckCr();
            if (ViewBag.chg == 2)
            {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<SelectListItem> slItem = from x in db.Groups
                select new SelectListItem
                {
                    Selected = x.GId.ToString() == "Active",
                    Value = x.GId.ToString(),
                    Text = x.Name
                };
            IEnumerable<SelectListItem> sluItem = from x in db.Users
                select new SelectListItem
                {
                    Selected = x.Id == "Active",
                    Value = x.Id,
                    Text = x.Name
                };
            var model = new AddGroupsViewModel
            {
                Groups = slItem,
                Users = sluItem
            };


            ModelState.AddModelError("Error", @"This person is already in this group");
            return View("addGroups",model);
        }



        public  ActionResult AddGr(AddGroupsViewModel model)
        {
            
            ApplicationUser user = db.Users.Where(u => u.Id == model.SelUser).Single();
            var gid = int.Parse(model.SelGroup);
            Group addGr = db.Groups.Where(g => g.GId == gid).Single();
            if (db.GroupPersons.Where(gg => gg.Group.GId == addGr.GId && gg.User.Id == user.Id).SingleOrDefault() != null)
            { 
                return RedirectToAction("AddGroupsParam", "Home");
            }

            GroupPerson gp = new GroupPerson();
            if (!db.GroupPersons.Any())
            {
                gp.Gpid = 1;
            }
            if (db.GroupPersons.Any())
            {
                gp.Gpid = db.GroupPersons.Max(grp => grp.Gpid) + 1;
            }
            gp.Group = addGr;
            gp.User = user;
            gp.Role = model.Role;
            user.Groups.Add(gp);
            addGr.Users.Add(gp);
            db.Users.Attach(user);
            db.Groups.Attach(addGr);
            db.GroupPersons.Add(gp);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateG()
        {
            CheckCr();
            if (ViewBag.chg == 2)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new CreateGViewModel();
            return View(model);
        }

        public ActionResult Cg(CreateGViewModel model)
        {
            if (db.Groups.Where(g => g.Name == model.NameOfGroup).SingleOrDefault() != null)
            {
                ModelState.AddModelError("Error", @"This name is already occupied please think of another one");
                return View("CreateG",model);
                
            }

            Group nGroup = new Group();
            if (!db.Groups.Any())
            {
                nGroup.GId = 1;
            }
            else
            {
                nGroup.GId = db.Groups.Max(g => g.GId) + 1;
            }
            nGroup.Name = model.NameOfGroup;
            db.Groups.Add(nGroup);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        public void CheckCr()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.ChgRight)
            {
                ViewBag.chg = 1;
            }
            if (user.ChgRight==false)
            {
                ViewBag.chg = 2;
            }
        }

    }
}