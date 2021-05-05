namespace BakcellPhoneWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CreatedDate");
        }
    }
}
