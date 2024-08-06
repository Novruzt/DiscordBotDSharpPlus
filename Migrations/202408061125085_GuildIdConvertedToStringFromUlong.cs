namespace FIrstDiscordBotC_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuildIdConvertedToStringFromUlong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "GuildID", c => c.String());
            AddColumn("dbo.Users", "GuildId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "GuildId");
            DropColumn("dbo.Servers", "GuildID");
        }
    }
}
