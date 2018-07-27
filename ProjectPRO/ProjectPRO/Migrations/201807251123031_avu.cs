namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class avu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.Binary());
            DropColumn("dbo.Groups", "Avatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "Avatar", c => c.String());
            DropColumn("dbo.AspNetUsers", "Avatar");
        }
    }
}
