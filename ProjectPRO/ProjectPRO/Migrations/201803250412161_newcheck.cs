namespace ProjectPRO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newcheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Discussions",
                c => new
                    {
                        DiscId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        group_gId = c.Int(),
                    })
                .PrimaryKey(t => t.DiscId)
                .ForeignKey("dbo.Groups", t => t.group_gId)
                .Index(t => t.group_gId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        gId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.gId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Fid = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        link = c.String(),
                        Author_Id = c.String(maxLength: 128),
                        group_gId = c.Int(),
                    })
                .PrimaryKey(t => t.Fid)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Groups", t => t.group_gId)
                .Index(t => t.Author_Id)
                .Index(t => t.group_gId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        chgRight = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GroupPersons",
                c => new
                    {
                        gpid = c.Int(nullable: false, identity: true),
                        role = c.String(),
                        group_gId = c.Int(),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.gpid)
                .ForeignKey("dbo.Groups", t => t.group_gId)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.group_gId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Lines",
                c => new
                    {
                        LId = c.Int(nullable: false, identity: true),
                        text = c.String(),
                        author_Id = c.String(maxLength: 128),
                        disc_DiscId = c.Int(),
                    })
                .PrimaryKey(t => t.LId)
                .ForeignKey("dbo.AspNetUsers", t => t.author_Id)
                .ForeignKey("dbo.Discussions", t => t.disc_DiscId)
                .Index(t => t.author_Id)
                .Index(t => t.disc_DiscId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Files", "group_gId", "dbo.Groups");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Lines", "disc_DiscId", "dbo.Discussions");
            DropForeignKey("dbo.Lines", "author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupPersons", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupPersons", "group_gId", "dbo.Groups");
            DropForeignKey("dbo.Files", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Discussions", "group_gId", "dbo.Groups");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Lines", new[] { "disc_DiscId" });
            DropIndex("dbo.Lines", new[] { "author_Id" });
            DropIndex("dbo.GroupPersons", new[] { "user_Id" });
            DropIndex("dbo.GroupPersons", new[] { "group_gId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Files", new[] { "group_gId" });
            DropIndex("dbo.Files", new[] { "Author_Id" });
            DropIndex("dbo.Discussions", new[] { "group_gId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Lines");
            DropTable("dbo.GroupPersons");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Files");
            DropTable("dbo.Groups");
            DropTable("dbo.Discussions");
        }
    }
}
