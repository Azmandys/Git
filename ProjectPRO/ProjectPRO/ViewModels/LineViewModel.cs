using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPRO.ViewModels
{
    public class LineViewModel
    {
        public string crline { get; set; }
        public IEnumerable<Discussion> discussions { get; set; }

        public IEnumerable<Line> lines { get; set; }
    }
}