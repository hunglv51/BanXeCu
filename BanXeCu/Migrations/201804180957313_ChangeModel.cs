namespace BanXeCu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "IsSalon", c => c.Boolean(nullable: false));
            AddColumn("dbo.Salons", "MemID", c => c.Int(nullable: false));
            DropColumn("dbo.Posts", "ImageUrl");
            DropColumn("dbo.Salons", "PhoneNumber");
            DropColumn("dbo.Salons", "City");
            DropColumn("dbo.Salons", "Name");
            DropColumn("dbo.Salons", "UserName");
            DropColumn("dbo.Salons", "Password");
            DropColumn("dbo.Salons", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Salons", "Email", c => c.String(maxLength: 50));
            AddColumn("dbo.Salons", "Password", c => c.String(maxLength: 50));
            AddColumn("dbo.Salons", "UserName", c => c.String(maxLength: 50));
            AddColumn("dbo.Salons", "Name", c => c.String(maxLength: 50));
            AddColumn("dbo.Salons", "City", c => c.String(maxLength: 20));
            AddColumn("dbo.Salons", "PhoneNumber", c => c.String(maxLength: 20));
            AddColumn("dbo.Posts", "ImageUrl", c => c.String(maxLength: 100));
            DropColumn("dbo.Salons", "MemID");
            DropColumn("dbo.Members", "IsSalon");
        }
    }
}
