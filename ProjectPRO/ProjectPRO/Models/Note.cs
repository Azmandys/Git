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

        public ApplicationUser Author { get; set; }

        public Meeting Meeting { get; set; }
    }
}