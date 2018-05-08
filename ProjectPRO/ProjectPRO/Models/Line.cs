using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectPRO.Models
{
    public class Line
    {
        [Key]
        public int LId { get; set; }
        public string text { get; set; }
        public ApplicationUser author { get; set; }

        public Discussion disc { get; set; }
    }
}