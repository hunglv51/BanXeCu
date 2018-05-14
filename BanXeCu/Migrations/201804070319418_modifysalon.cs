namespace BanXeCu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifysalon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "IsSalon", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "IsSalon");
        }
    }
}
