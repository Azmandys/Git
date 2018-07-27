﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProjectPRO.Models;
using ProjectPRO.ViewModels;
using System.Data.Entity;
using System;
using System.IO;

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

        public ActionResult ProfilePage()
        {
            CheckCr();
            var uid = User.Identity.GetUserId();
            var viewModel = new ProfilePageViewModel();
            var userda = db.Users.Where(y => y.Id == uid).Single();
            viewModel.NameUs = userda.Name;
            viewModel.IndexNumber = userda.IndexNumber;
            viewModel.Specialization = userda.Specialization;

            viewModel.Gp = db.GroupPersons
                .Include(i => i.Group)
                .Include(i => i.User);

            ViewBag.UserId = uid;

            List<GroupPerson> gpr = viewModel.Gp.Where(u => u.User.Id == uid).ToList();
            List<Group> gro = new List<Group>();           
            for (var i = 0; i < gpr.Count; i++)
                gro.Add(gpr[i].Group);
            viewModel.Groups = gro;
            List<GroupPerson> adgp = viewModel.Gp.Where(g => g.Role == "Main Advisor").ToList();
            List<GroupPerson> advgp = new List<GroupPerson>();
            for (var j = 0; j < gro.Count; j++)
            {
                for (var t = 0; t < adgp.Count; t++)
                {
                    if (adgp[t].Group == gro[j])
                        advgp.Add(adgp[t]);
                }
            }
            viewModel.AdvGp = advgp;
            List<Discussion> dis = db.Discussions.Where(d => d.Creator.Id == userda.Id).ToList();
            List<Models.File> fil = db.Files.Where(f => f.Author.Id == userda.Id).ToList();
            DateTime compar = DateTime.Now;
            compar = compar.AddDays(-7);
            List<Discussion> finalDis = new List<Discussion>();
            List<Models.File> finalFile = new List<Models.File>();
            for (var it = 0; it < dis.Count; it++)
            {
                if (dis[it].Created > compar)
                    finalDis.Add(dis[it]);
            }
            for (var jt = 0; jt < fil.Count; jt++)
            {
                if (fil[jt].Created > compar)
                    finalFile.Add(fil[jt]);
            }
            viewModel.NewDiscussions = finalDis;
            viewModel.NewFiles = finalFile;
            return View(viewModel);
        }

        public ActionResult AddGroups(int? gid)
        {

            var model = new AddGroupsViewModel();
            model.Users = db.Users.ToList();
            ViewBag.GId = gid;
            return View(model);
        }

        public ActionResult AddGroupsParam(int? gid)
        {
            var model = new AddGroupsViewModel();
            model.Users = db.Users.ToList();
            ViewBag.GId = gid;
            ModelState.AddModelError("Error", @"This person is already in this group");
            return View("addGroups",model);
        }



        public  ActionResult AddGr(AddGroupsViewModel model, int? gid)
        {
            string selU = Request.Form["selUser"].ToString();
            ApplicationUser user = db.Users.Where(u => u.Id == selU).Single();
            string role = Request.Form["roleSel"].ToString();
            Group addGr = db.Groups.Where(g => g.GId == gid).Single();
            if (db.GroupPersons.Where(gg => gg.Group.GId == addGr.GId && gg.User.Id == user.Id).SingleOrDefault() != null)
            { 
                return RedirectToAction("AddGroupsParam", "Home", new { @gid = gid });
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
            gp.Role = role;
            user.Groups.Add(gp);
            addGr.Users.Add(gp);
            db.Users.Attach(user);
            db.Groups.Attach(addGr);
            db.GroupPersons.Add(gp);
            db.SaveChanges();
            return RedirectToAction("GroupManagement", "Home", new {@gid=gid });
        }

        public ActionResult CreateG()
        {
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
            nGroup.Description = model.Description;
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["Avatar"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }
            nGroup.Avatar = imageData;
            db.Groups.Add(nGroup);
            db.SaveChanges();
            return RedirectToAction("GroupManagement", "Home", new {@gid = nGroup.GId });
        }

        public ActionResult GroupManagement(int gid)
        {
            var grou = db.Groups.Where(g => g.GId == gid).Single();
            var model = new GroupManagementViewModel();
            ViewBag.groupId = gid;
            model.gNmae = grou.Name;
            model.gDesc = grou.Description;

            List<GroupPerson> gp = new List<GroupPerson>();
            List<ApplicationUser> use = new List<ApplicationUser>();

            gp = db.GroupPersons.Where(g => g.Group.GId == gid).ToList();
            for (var i = 0; i < gp.Count; i++)
            {
                use.Add(gp[i].User);
            }
            model.gUsers = use;
            model.gGro = gp;

            return View(model);
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

       public FileContentResult GroupAvatar(int gid)
        {
            var gr = db.Groups.Where(g => g.GId==gid).Single();
            if (gr.Avatar == null)
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/avatar.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");
            }
            else
            return new FileContentResult(gr.Avatar, "image/jpeg");
        }
       
        public FileContentResult UserAvatar()
        {
            string userId = User.Identity.GetUserId();

            var bu = db.Users.Where(b => b.Id == userId).Single();
            if (bu.Avatar == null)
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/avatar.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");
            }
            else
                return new FileContentResult(bu.Avatar, "image/jpeg");
        }

    }
}