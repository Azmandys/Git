using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPRO.ViewModels
{
    public class InviteManagementViewModel
    {
        public ICollection<Group> Groups { get; set; }
        public ICollection<GroupPerson> Invites { get; set; }
    }
}