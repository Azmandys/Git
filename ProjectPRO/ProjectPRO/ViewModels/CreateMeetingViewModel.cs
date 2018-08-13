using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPRO.ViewModels
{
    public class CreateMeetingViewModel
    {
        public string Name { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}