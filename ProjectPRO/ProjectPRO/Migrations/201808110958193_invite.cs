namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupPersons", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupPersons", "Status");
        }
    }
}
