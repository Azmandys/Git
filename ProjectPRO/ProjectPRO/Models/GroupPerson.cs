using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPRO.Models
{
    public class GroupPerson
    {
        [Key]
        public int gpid { get; set; }

        public string role { get; set; }

        public virtual ApplicationUser user { get; set; }

        public virtual Group group { get; set; }

    }
}