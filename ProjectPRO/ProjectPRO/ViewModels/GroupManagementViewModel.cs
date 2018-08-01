using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPRO.ViewModels
{
    public class GroupManagementViewModel
    {
        public string gNmae { get; set; }

        public string gDesc { get; set; }

        public int GroupId { get; set; }

        public IEnumerable<ApplicationUser> gUsers { get; set; }

        public IEnumerable<GroupPerson> gGro { get; set; }
    }
}