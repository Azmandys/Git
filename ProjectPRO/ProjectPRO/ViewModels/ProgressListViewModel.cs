using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectPRO.Models;

namespace ProjectPRO.ViewModels
{
    public class ProgressListViewModel
    {
        public ICollection<Progress> Progress { get; set; }

        public int GrId { get; set; }
    }
}