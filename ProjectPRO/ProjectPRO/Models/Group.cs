using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.Models
{
    public class Group
    {
        [Key]
        public int GId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Meeting> Meetings { get; set; }

        public byte[] Avatar { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
        public ICollection<File> Files { get; set; }
        public virtual ICollection<GroupPerson> Users { get; set; }
    }

}