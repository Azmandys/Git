using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPRO.Models
{
    public class File
    {

        [Key]
        public int Fid { get; set; }
        public string name { get; set; }

        public string link { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Group group { get; set; }
    }
}