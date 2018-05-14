namespace BanXeCu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Salon : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Members", "IsSalon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "IsSalon", c => c.Boolean());
        }
    }
}
