using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPRO.Models
{
    public class Progress
    {
        [Key]
        public int PId { get; set; }

        public string Title { get; set; }

        public string Color { get; set; }

        public virtual Group Group { get; set; }

        public int Prog { get; set; }
    }
}