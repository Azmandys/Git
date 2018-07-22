using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.Models
{
    public class File
    {

        [Key]
        public int Fid { get; set; }
        public string Name { get; set; }

        public string Link { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Group Group { get; set; }

        public DateTime Created { get; set; }
    }
}