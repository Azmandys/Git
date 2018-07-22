namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discAuth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discussions", "Creator_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Discussions", "Creator_Id");
            AddForeignKey("dbo.Discussions", "Creator_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Discussions", "Creator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Discussions", new[] { "Creator_Id" });
            DropColumn("dbo.Discussions", "Creator_Id");
        }
    }
}
