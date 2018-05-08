using ProjectPRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPRO.ViewModels
{
    public class FilesViewModel
    {
        public IEnumerable<Group> groups { get; set; }

        public IEnumerable<GroupPerson> gp { get; set; }

        public IEnumerable<File> file { get; set; }
    }
}