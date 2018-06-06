using ProjectPRO.Models;
using System.Collections.Generic;

namespace ProjectPRO.ViewModels
{
    public class FilesViewModel
    {
        public IEnumerable<Group> Groups { get; set; }

        public IEnumerable<GroupPerson> Gp { get; set; }

        public IEnumerable<File> File { get; set; }
    }
}