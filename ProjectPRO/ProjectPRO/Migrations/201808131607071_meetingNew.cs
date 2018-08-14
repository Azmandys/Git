namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class meetingNew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "Time", c => c.String());
            AlterColumn("dbo.Meetings", "Date", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Meetings", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Meetings", "Time");
        }
    }
}
