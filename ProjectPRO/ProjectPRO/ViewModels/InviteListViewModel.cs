using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPRO.ViewModels
{
    public class InviteListViewModel
    {
        public ICollection<MeetingInvitation> Invites { get; set; }
    }
}