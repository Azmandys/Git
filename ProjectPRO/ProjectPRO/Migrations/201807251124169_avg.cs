namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class avg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Avatar", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Avatar");
        }
    }
}
