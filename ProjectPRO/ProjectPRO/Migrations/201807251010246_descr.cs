namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Description", c => c.String());
            AddColumn("dbo.Groups", "Avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Avatar");
            DropColumn("dbo.Groups", "Description");
        }
    }
}
