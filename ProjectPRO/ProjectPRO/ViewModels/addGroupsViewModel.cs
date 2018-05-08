using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectPRO.ViewModels
{
    public class addGroupsViewModel
    {
        public string SelGroup { get; set; }
        public string SelUser { get; set; }

        public string Role { get; set;}
       
        public IEnumerable<SelectListItem> Groups { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

    }
}