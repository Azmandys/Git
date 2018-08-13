using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace ProjectPRO.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string IndexNumber { get; set; }

        public string Specialization { get; set; }

        public byte[] Avatar { get; set; }

        public bool ChgRight { get; set; }
        public ICollection<Line> Lines { get; set; }

        public ICollection<File> Files { get; set; }

        public ICollection<MeetingInvitation> InvitationsToMeetings { get; set; }

        public ICollection<Note> Notes { get; set; }

        public virtual ICollection<GroupPerson> Groups { get; set; } 
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupPerson> GroupPersons { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Discussion> Discussions { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<MeetingInvitation> MeetingInvitations { get; set; }

        public DbSet<Meeting> Meetings { get; set; }
        
        public DbSet<File> Files { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


    }
}