using ProjectPRO.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProjectPRO.ViewModels
{
    public class AddGroupsViewModel
    {  
        public IEnumerable<ApplicationUser> Users { get; set; }

    }
}