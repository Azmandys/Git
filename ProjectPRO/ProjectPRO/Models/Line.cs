using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.Models
{
    public class Line
    {
        [Key]
        public int LId { get; set; }
        public string Text { get; set; }
        public ApplicationUser Author { get; set; }

        public Discussion Disc { get; set; }
    }
}