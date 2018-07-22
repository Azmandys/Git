namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class profile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discussions", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Files", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "IndexNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "Specialization", c => c.String());
            AddColumn("dbo.Lines", "Created", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lines", "Created");
            DropColumn("dbo.AspNetUsers", "Specialization");
            DropColumn("dbo.AspNetUsers", "IndexNumber");
            DropColumn("dbo.Files", "Created");
            DropColumn("dbo.Discussions", "Created");
        }
    }
}
