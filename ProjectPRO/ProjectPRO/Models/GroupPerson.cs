using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.Models
{
    public class GroupPerson
    {
        [Key]
        public int Gpid { get; set; }

        public string Role { get; set; }

        public string Status { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Group Group { get; set; }

    }
}