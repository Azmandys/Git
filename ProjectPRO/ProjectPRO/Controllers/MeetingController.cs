using System.Collections.Generic;
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
    public class MeetingController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MeetingList()
        {
            MeetingListViewModel model = new MeetingListViewModel();
            var uid = User.Identity.GetUserId();
            var invu = db.MeetingInvitations.Where(m => m.Invitee.Id == uid).ToList();
            List<Meeting> meet = new List<Meeting>();
            for (var i = 0; i < invu.Count; i++)
            {
                meet.Add(invu[i].Meeting);
            }
            model.Meetings = meet;
            return View(model);
        }

        public ActionResult CreateMeeting()
        {
            CreateMeetingViewModel model = new CreateMeetingViewModel();
            var uid = User.Identity.GetUserId();
            var gp = db.GroupPersons.Where(g => g.User.Id == uid).ToList();
            List<Group> gro = new List<Group>();
            for (var i = 0; i < gp.Count; i++)
            {
                gro.Add(gp[i].Group);
            }
            model.Groups = gro;
            return View(model);
        }

        public ActionResult CrMeeting(CreateMeetingViewModel model)
        {
            Meeting newM = new Meeting();
            newM.Name = model.Name;
            var Date = Request.Form["Datepck"].ToString();
            var Time = Request.Form["Timepck"].ToString();
            newM.Date = Date;
            newM.Time = Time;
            var Gro = Request.Form["selGroup"].ToString();
            int groid = int.Parse(Gro);
            var gr = db.Groups.Where(g => g.GId == groid).Single();
            newM.Group = gr;

            MeetingInvitation mi = new MeetingInvitation();
            mi.Meeting = newM;
            var usi = User.Identity.GetUserId();
            var us = db.Users.Where(u => u.Id == usi).Single();
            mi.Invitee = us;
            mi.Status = "Accepted";
            newM.Participants.Add(mi);
            us.InvitationsToMeetings.Add(mi);

            db.Meetings.Add(newM);
            db.MeetingInvitations.Add(mi);
            db.Users.Attach(us);
            db.SaveChanges();
            return RedirectToAction("MeetingList","Meeting");
        }

        public ActionResult CreateNote(int meetid)
        {
            CreateNoteViewModel model = new CreateNoteViewModel();
            model.MeetId = meetid;
            return View(model);
        }

        public ActionResult CrNote(CreateNoteViewModel model)
        {
            Note NewNote = new Note();
            var usi = User.Identity.GetUserId();
            var us = db.Users.Where(u => u.Id == usi).Single();
            NewNote.Author = us;
            Meeting met = db.Meetings.Where(m => m.MId == model.MeetId).Single();
            NewNote.Meeting = met;
            NewNote.Text = model.Text;
            db.Notes.Add(NewNote);
            met.Notes.Add(NewNote);
            us.Notes.Add(NewNote);
            db.Meetings.Attach(met);
            db.Users.Attach(us);
            db.SaveChanges();

            return RedirectToAction("ManageMeeting", "Meeting",new { @meetid = model.MeetId });
        }

        public ActionResult AcceptInvite(int meetid,int invid)
        {
            var invite = db.MeetingInvitations.Where(mi => mi.InvId == invid).Single();
            invite.Status = "Accepted";
            db.MeetingInvitations.Attach(invite);
            db.SaveChanges();
            return RedirectToAction("InviteList", "Meeting", new {@meetid=meetid });
        }

        public ActionResult RejectInvite(int meetid,int invid)
        {
            var invite = db.MeetingInvitations.Where(mi => mi.InvId == invid).Single();
            invite.Status = "Rejected";
            db.MeetingInvitations.Attach(invite);
            db.SaveChanges();
            return RedirectToAction("InviteList", "Meeting", new { @meetid = meetid });
        }

        public ActionResult InvitePerson(int meetid,string uid)
        {
            MeetingInvitation inv = new MeetingInvitation();
            inv.Status = "Invited";
            var meet = db.Meetings.Where(m => m.MId == meetid).Single();
            inv.Meeting = meet;
            var us = db.Users.Where(u => u.Id == uid).Single();
            inv.Invitee = us;
            db.MeetingInvitations.Add(inv);
            us.InvitationsToMeetings.Add(inv);
            meet.Participants.Add(inv);
            db.Meetings.Attach(meet);
            db.Users.Attach(us);
            return RedirectToAction("InviteToMeeting", "Meeting", new { @meetid = meetid });
        }

        public ActionResult ManageMeeting(int meetid)
        {
            ManageMeetingViewModel model = new ManageMeetingViewModel();
            var meet = db.Meetings.Where(m => m.MId == meetid).Single();
            model.Date = meet.Date;
            model.Time = meet.Time;
            model.MeetId = meet.MId;
            model.Participants = db.MeetingInvitations.Where(p => p.Meeting.MId == meetid).ToList();
            model.Notes = db.Notes.Where(n => n.Meeting.MId == meetid).ToList();
            model.Name = meet.Name;
            return View(model);
        }

        public ActionResult InviteToMeeting(int meetid)
        {
            InviteToMeetingViewModel model = new InviteToMeetingViewModel();
            var usi = User.Identity.GetUserId();
            var us = db.Users.Where(u => u.Id == usi).Single();
            List<GroupPerson> gp = db.GroupPersons.Where(g => g.User.Id == usi).ToList();
            List<Group> gro = new List<Group>();
            for (var i = 0; i < gp.Count; i++)
            {
                gro.Add(gp[i].Group);
            }
            List<ApplicationUser> users = new List<ApplicationUser>();
            gp = new List<GroupPerson>();
            for (var j = 0; j < gro.Count; j++)
            {
                gp.AddRange(db.GroupPersons.Where(r => r.Group.GId == gro[j].GId).ToList());
            }
            for (var t = 0; t < gp.Count; t++)
            {
                users.Add(gp[t].User);
            }
            model.Users = users;
            model.MeetId = meetid;
            return View(model);
        }

        public ActionResult InviteList(int meetid)
        {
            InviteListViewModel model = new InviteListViewModel();
            model.Meetingid = meetid;
            return View(model);
        }
    }
}