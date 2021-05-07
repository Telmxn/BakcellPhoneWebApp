namespace BakcellPhoneWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeliveryTimeChanged2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DeliveryTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DeliveryTime", c => c.DateTime(nullable: false));
        }
    }
}
