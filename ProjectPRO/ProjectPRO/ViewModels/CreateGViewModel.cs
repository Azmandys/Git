using ProjectPRO.Models;
using System.Collections.Generic;
using System.Web;

namespace ProjectPRO.ViewModels
{
    public class CreateGViewModel
    {
        public string NameOfGroup { get; set; }

        public string Description { get; set; }

        public IEnumerable<ApplicationUser> MainAdvisorCandidats { get; set; }
    }
}