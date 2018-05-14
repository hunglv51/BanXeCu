namespace BanXeCu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(maxLength: 20),
                        City = c.String(maxLength: 20),
                        Name = c.String(maxLength: 50),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        SalonAddress = c.String(maxLength: 50),
                        Introduction = c.String(maxLength: 50),
                        SalonName = c.String(maxLength: 30),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 50),
                        Content = c.String(nullable: false, maxLength: 1000),
                        MemID = c.Int(nullable: false),
                        ExpiredDate = c.DateTime(nullable: false),
                        ImageUrl = c.String(maxLength: 100),
                        VideoLink = c.String(maxLength: 100),
                        PostType = c.String(maxLength: 20),
                        TimeStart = c.DateTime(nullable: false),
                        Sold = c.Boolean(nullable: false),
                        CarID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        CarModel = c.String(nullable: false, maxLength: 50),
                        Family = c.String(nullable: false, maxLength: 50),
                        Price = c.Long(nullable: false),
                        ManufactureYear = c.Int(nullable: false),
                        ComeFrom = c.String(nullable: false, maxLength: 50),
                        Used = c.String(maxLength: 50),
                        Distance = c.Long(nullable: false),
                        Size = c.String(maxLength: 50),
                        Length = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        CylinderCapacity = c.Int(nullable: false),
                        FuelCapacity = c.Int(nullable: false),
                        TireInfo = c.String(maxLength: 50),
                        WheelBase = c.String(maxLength: 50),
                        MaxSeatingCapacity = c.Int(nullable: false),
                        DriveType = c.String(maxLength: 50),
                        Transmission = c.String(maxLength: 50),
                        NumDoor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Posts");
            DropTable("dbo.Members");
            DropTable("dbo.Admins");
        }
    }
}
