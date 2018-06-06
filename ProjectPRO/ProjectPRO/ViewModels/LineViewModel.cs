using ProjectPRO.Models;
using System.Collections.Generic;

namespace ProjectPRO.ViewModels
{
    public class LineViewModel
    {
        public string Crline { get; set; }
        public IEnumerable<Discussion> Discussions { get; set; }

        public IEnumerable<Line> Lines { get; set; }
    }
}