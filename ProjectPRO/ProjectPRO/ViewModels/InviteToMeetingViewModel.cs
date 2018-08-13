using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPRO.ViewModels
{
    public class InviteToMeetingViewModel
    {
        public int MeetId { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}