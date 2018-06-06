using System.Collections.Generic;
using System.Web.Mvc;

namespace ProjectPRO.ViewModels
{
    public class AddGroupsViewModel
    {
        public string SelGroup { get; set; }
        public string SelUser { get; set; }

        public string Role { get; set;}
       
        public IEnumerable<SelectListItem> Groups { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

    }
}