using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.Models
{
    public class Discussion
    {
        [Key]
        public int DiscId { get; set; }
        public string Name { get; set; }
        public Group Group { get; set; }

        public ApplicationUser Creator { get; set; }

        public ICollection<Line> Lines { get; set; }

        public DateTime Created { get; set; }
    }
}