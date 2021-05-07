namespace BakcellPhoneWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeliveryTimeChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DeliveryTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DeliveryTime", c => c.DateTime(nullable: false));
        }
    }
}
