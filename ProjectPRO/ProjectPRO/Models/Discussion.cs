using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPRO.Models
{
    public class Discussion
    {
        [Key]
        public int DiscId { get; set; }
        public string Name { get; set; }
        public Group group { get; set; }

        public ICollection<Line> lines { get; set; }
    }
}