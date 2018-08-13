namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class meeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        MId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        Group_GId = c.Int(),
                    })
                .PrimaryKey(t => t.MId)
                .ForeignKey("dbo.Groups", t => t.Group_GId)
                .Index(t => t.Group_GId);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        NId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Author_Id = c.String(maxLength: 128),
                        Meeting_MId = c.Int(),
                    })
                .PrimaryKey(t => t.NId)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Meetings", t => t.Meeting_MId)
                .Index(t => t.Author_Id)
                .Index(t => t.Meeting_MId);
            
            CreateTable(
                "dbo.MeetingInvitations",
                c => new
                    {
                        InvId = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        Invitee_Id = c.String(maxLength: 128),
                        Meeting_MId = c.Int(),
                    })
                .PrimaryKey(t => t.InvId)
                .ForeignKey("dbo.AspNetUsers", t => t.Invitee_Id)
                .ForeignKey("dbo.Meetings", t => t.Meeting_MId)
                .Index(t => t.Invitee_Id)
                .Index(t => t.Meeting_MId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeetingInvitations", "Meeting_MId", "dbo.Meetings");
            DropForeignKey("dbo.MeetingInvitations", "Invitee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notes", "Meeting_MId", "dbo.Meetings");
            DropForeignKey("dbo.Notes", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Meetings", "Group_GId", "dbo.Groups");
            DropIndex("dbo.MeetingInvitations", new[] { "Meeting_MId" });
            DropIndex("dbo.MeetingInvitations", new[] { "Invitee_Id" });
            DropIndex("dbo.Notes", new[] { "Meeting_MId" });
            DropIndex("dbo.Notes", new[] { "Author_Id" });
            DropIndex("dbo.Meetings", new[] { "Group_GId" });
            DropTable("dbo.MeetingInvitations");
            DropTable("dbo.Notes");
            DropTable("dbo.Meetings");
        }
    }
}
