namespace BakcellPhoneWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatetimeCreatedFixed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "CreatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
