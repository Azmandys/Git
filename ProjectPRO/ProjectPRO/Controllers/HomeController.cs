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
            checkCR();
            return View();
        }

        public ActionResult addGroups()
        {
            checkCR();
            if (ViewBag.chg == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                IEnumerable < SelectListItem > slItem = from x in db.Groups
                                                        select new SelectListItem
                                                        {
                                                            Selected = x.gId.ToString() == "Active",
                                                            Value = x.gId.ToString(),
                                                            Text = x.Name
                                                        };
                IEnumerable<SelectListItem> sluItem = from x in db.Users
                                                      select new SelectListItem
                                                      {
                                                          Selected = x.Id.ToString() == "Active",
                                                          Value = x.Id.ToString(),
                                                          Text = x.Name
                                                      };
                var model = new addGroupsViewModel()
                {
                    Groups = slItem,
                    Users = sluItem
                };



                return View(model);
            }
        }

        public ActionResult addGroupsParam()
        {
            checkCR();
            if (ViewBag.chg == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                IEnumerable<SelectListItem> slItem = from x in db.Groups
                                                     select new SelectListItem
                                                     {
                                                         Selected = x.gId.ToString() == "Active",
                                                         Value = x.gId.ToString(),
                                                         Text = x.Name
                                                     };
                IEnumerable<SelectListItem> sluItem = from x in db.Users
                                                      select new SelectListItem
                                                      {
                                                          Selected = x.Id.ToString() == "Active",
                                                          Value = x.Id.ToString(),
                                                          Text = x.Name
                                                      };
                var model = new addGroupsViewModel()
                {
                    Groups = slItem,
                    Users = sluItem
                };


                ModelState.AddModelError("Error", "This person is already in this group");
                return View("addGroups",model);
            }
        }



        public  ActionResult addGr(addGroupsViewModel model)
        {
            
            ApplicationUser user = db.Users.Where(u => u.Id == model.SelUser).Single();
            var gid = int.Parse(model.SelGroup);
            Group addGr = db.Groups.Where(g => g.gId == gid).Single();
            if (db.GroupPersons.Where(gg => gg.group.gId == addGr.gId && gg.user.Id == user.Id).SingleOrDefault() != null)
            { 
                return RedirectToAction("addGroupsParam", "Home");
            }
            else
            {
                GroupPerson gp = new GroupPerson();
                if (!db.GroupPersons.Any())
                {
                    gp.gpid = 1;
                }
                if (db.GroupPersons.Any())
                {
                    gp.gpid = db.GroupPersons.Max(grp => grp.gpid) + 1;
                }
                gp.group = addGr;
                gp.user = user;
                gp.role = model.Role;
                user.groups.Add(gp);
                addGr.users.Add(gp);
                db.Users.Attach(user);
                db.Groups.Attach(addGr);
                db.GroupPersons.Add(gp);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CreateG()
        {
            checkCR();
            if (ViewBag.chg == 2)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var model = new CreateGViewModel();
                return View(model);
            }
        }

        public ActionResult cG(CreateGViewModel model)
        {
            if (db.Groups.Where(g => g.Name == model.NameOfGroup).SingleOrDefault() != null)
            {
                ModelState.AddModelError("Error", "This name is already occupied please think of another one");
                return View("CreateG",model);
                
            }
            else
            {
                Group nGroup = new Group();
                if (!db.Groups.Any())
                {
                    nGroup.gId = 1;
                }
                else
                {
                    nGroup.gId = db.Groups.Max(g => g.gId) + 1;
                }
                nGroup.Name = model.NameOfGroup;
                db.Groups.Add(nGroup);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }


        public void checkCR()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (user.chgRight == true)
            {
                ViewBag.chg = 1;
            }
            if (user.chgRight==false)
            {
                ViewBag.chg = 2;
            }
        }

    }
}