using ProjectPRO.Models;
using System.Collections.Generic;

namespace ProjectPRO.ViewModels
{
    public class DiscussionViewModel
    {
        public IEnumerable<Group> Groups { get; set; }

        public IEnumerable<GroupPerson> Gp { get; set; }

        public IEnumerable<Discussion> Discussions { get; set; }

        public IEnumerable<Line> Lines { get; set; }
    }
}