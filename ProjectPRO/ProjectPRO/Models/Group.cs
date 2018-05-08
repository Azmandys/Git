using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPRO.Models
{
    public class Group
    {
        [Key]
        public int gId { get; set; }
        public string Name { get; set; }

        public ICollection<Discussion> discussions { get; set; }
        public ICollection<File> files { get; set; }
        public virtual ICollection<GroupPerson> users { get; set; }
    }

}