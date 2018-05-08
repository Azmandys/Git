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
        // GET: Home
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

        public ActionResult Discussions(int? gid,int? did)
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

            if (gid!=null)
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

        public ActionResult AddF(int? gro,AddFileViewModel model)
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
                return RedirectToAction("Files", "Home");
            }
            return View(model);
        }

        public ActionResult createDisc(int? gid)
        {
            ViewBag.GrId = gid;
            return View();
        }

        public ActionResult createDis(int? grid,createDiscViewModel model)
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
                } else {
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
                return RedirectToAction("Discussions", "Home");
            }
            return View(model);
        }

        public ActionResult addLine(int? did,LineViewModel model)
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

            return RedirectToAction("Line", "Home", new { did = did });
        }

        public ActionResult gro()
        {
            Group gro1 = new Group();
            gro1.gId = 1;
            gro1.Name = "sys1";
            Group gro2 = new Group();
            gro2.gId = 2;
            gro2.Name = "sys2";
            Group gro3 = new Group();
            gro3.gId = 3;
            gro3.Name = "sys3";
            db.Groups.Add(gro1);
            db.Groups.Add(gro2);
            db.Groups.Add(gro3);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult adGr()
        {
         
                var gr1 = db.Groups.Find(1);
                var user = db.Users.Find(User.Identity.GetUserId());
                GroupPerson gp = new GroupPerson();
            gp.user = user;
            gp.group = gr1;
                gr1.users.Add(gp);
                db.Groups.Attach(gr1);
                db.SaveChanges();
      
            return RedirectToAction("Index", "Home");
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