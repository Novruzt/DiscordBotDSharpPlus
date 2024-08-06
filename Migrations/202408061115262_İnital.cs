namespace FIrstDiscordBotC_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class İnital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        AvatarUrl = c.String(),
                        XP = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        ServerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servers", t => t.ServerId, cascadeDelete: true)
                .Index(t => t.ServerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "ServerId", "dbo.Servers");
            DropIndex("dbo.Users", new[] { "ServerId" });
            DropTable("dbo.Users");
            DropTable("dbo.Servers");
        }
    }
}
