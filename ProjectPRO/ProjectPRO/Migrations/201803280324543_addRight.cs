namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "chgRight", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "chgRight");
        }
    }
}
