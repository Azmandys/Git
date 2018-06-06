namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resh : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Discussions", new[] { "group_gId" });
            DropIndex("dbo.Files", new[] { "group_gId" });
            DropIndex("dbo.GroupPersons", new[] { "group_gId" });
            DropIndex("dbo.GroupPersons", new[] { "user_Id" });
            DropIndex("dbo.Lines", new[] { "author_Id" });
            DropIndex("dbo.Lines", new[] { "disc_DiscId" });
            CreateIndex("dbo.Discussions", "Group_GId");
            CreateIndex("dbo.Files", "Group_GId");
            CreateIndex("dbo.GroupPersons", "Group_GId");
            CreateIndex("dbo.GroupPersons", "User_Id");
            CreateIndex("dbo.Lines", "Author_Id");
            CreateIndex("dbo.Lines", "Disc_DiscId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Lines", new[] { "Disc_DiscId" });
            DropIndex("dbo.Lines", new[] { "Author_Id" });
            DropIndex("dbo.GroupPersons", new[] { "User_Id" });
            DropIndex("dbo.GroupPersons", new[] { "Group_GId" });
            DropIndex("dbo.Files", new[] { "Group_GId" });
            DropIndex("dbo.Discussions", new[] { "Group_GId" });
            CreateIndex("dbo.Lines", "disc_DiscId");
            CreateIndex("dbo.Lines", "author_Id");
            CreateIndex("dbo.GroupPersons", "user_Id");
            CreateIndex("dbo.GroupPersons", "group_gId");
            CreateIndex("dbo.Files", "group_gId");
            CreateIndex("dbo.Discussions", "group_gId");
        }
    }
}
