using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectPRO.Models;

namespace ProjectPRO.ViewModels
{
    public class ProfilePageViewModel
    {
        public string NameUs { get; set; }
        public string IndexNumber { get; set; }
        public string Specialization { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<GroupPerson> Gp { get; set; }

        public IEnumerable<GroupPerson> AdvGp { get; set; }

        public IEnumerable<File> NewFiles { get; set; }

        public IEnumerable<Discussion> NewDiscussions { get; set; }
    }
}