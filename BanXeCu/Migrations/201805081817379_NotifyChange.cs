namespace BanXeCu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotifyChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notifications", "Type", c => c.String(maxLength: 50));
            AlterColumn("dbo.Notifications", "PostID", c => c.String(maxLength: 10));
            AlterColumn("dbo.Notifications", "Content", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "Content", c => c.String());
            AlterColumn("dbo.Notifications", "PostID", c => c.String());
            AlterColumn("dbo.Notifications", "Type", c => c.String());
        }
    }
}
