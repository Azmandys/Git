namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkr : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "chgRight");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "chgRight", c => c.Boolean(nullable: false));
        }
    }
}
