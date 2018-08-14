using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.Models
{
    public class Meeting
    {

        [Key]
        public int MId { get; set; }
        public string Name { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<MeetingInvitation> Participants { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}