namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class progress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Progresses",
                c => new
                    {
                        PId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Color = c.String(),
                        Prog = c.Int(nullable: false),
                        Group_GId = c.Int(),
                    })
                .PrimaryKey(t => t.PId)
                .ForeignKey("dbo.Groups", t => t.Group_GId)
                .Index(t => t.Group_GId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Progresses", "Group_GId", "dbo.Groups");
            DropIndex("dbo.Progresses", new[] { "Group_GId" });
            DropTable("dbo.Progresses");
        }
    }
}
