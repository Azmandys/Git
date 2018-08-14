using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.Models
{
    public class Note
    {
        [Key]
        public int NId { get; set; }


        public string Text { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Meeting Meeting { get; set; }
    }
}