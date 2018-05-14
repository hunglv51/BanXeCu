namespace BanXeCu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "ForAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "ForAdmin");
        }
    }
}
