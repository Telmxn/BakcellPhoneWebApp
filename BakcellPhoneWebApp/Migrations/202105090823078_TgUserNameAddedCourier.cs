namespace BakcellPhoneWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TgUserNameAddedCourier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TgUsername", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TgUsername");
        }
    }
}
