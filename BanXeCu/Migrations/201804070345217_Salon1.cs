namespace BanXeCu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Salon1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Members", newName: "Salons");
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
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Salons", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Salons", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropTable("dbo.Members");
            RenameTable(name: "dbo.Salons", newName: "Members");
        }
    }
}
