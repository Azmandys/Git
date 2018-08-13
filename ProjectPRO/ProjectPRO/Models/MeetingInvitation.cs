using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRO.Models
{
    public class MeetingInvitation
    {
        [Key]
        public int InvId { get; set; }
        public ApplicationUser Invitee { get; set; }

        public Meeting Meeting { get; set; }

        public string Status { get; set; }
    }
}