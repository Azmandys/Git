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

        public Group Group { get; set; }

        public ICollection<MeetingInvitation> Participants { get; set; }

        public ICollection<Note> Notes { get; set; }
    }
}