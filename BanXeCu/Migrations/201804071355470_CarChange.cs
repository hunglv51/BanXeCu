namespace BanXeCu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Price", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Distance", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Length", c => c.String(maxLength: 10));
            AlterColumn("dbo.Posts", "Weight", c => c.String(maxLength: 10));
            AlterColumn("dbo.Posts", "CylinderCapacity", c => c.String(maxLength: 10));
            AlterColumn("dbo.Posts", "FuelCapacity", c => c.String(maxLength: 10));
            AlterColumn("dbo.Posts", "MaxSeatingCapacity", c => c.String(maxLength: 10));
            AlterColumn("dbo.Posts", "NumDoor", c => c.String(maxLength: 10));
            DropColumn("dbo.Posts", "CarID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "CarID", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "NumDoor", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "MaxSeatingCapacity", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "FuelCapacity", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "CylinderCapacity", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Weight", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Length", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Distance", c => c.Long(nullable: false));
            AlterColumn("dbo.Posts", "Price", c => c.Long(nullable: false));
        }
    }
}
