using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectPRO.Models;

namespace ProjectPRO.ViewModels
{
    public class ManageMeetingViewModel
    {
        public int MeetId { get; set; }
        public string Name { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public ICollection<MeetingInvitation> Participants { get; set; }
        
        public ICollection<Note> Notes { get; set; } 
    }
}