namespace BakcellPhoneWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerPhoneNumber = c.String(nullable: false),
                        OrderedPhoneNumber = c.String(nullable: false),
                        City = c.String(nullable: false),
                        District = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        ManagerId = c.String(maxLength: 128),
                        VendorId = c.String(maxLength: 128),
                        CourierId = c.String(maxLength: 128),
                        NumberPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustomerInfo = c.String(nullable: false),
                        DeliveryTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Image = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CourierId)
                .ForeignKey("dbo.AspNetUsers", t => t.ManagerId)
                .ForeignKey("dbo.AspNetUsers", t => t.VendorId)
                .Index(t => t.ManagerId)
                .Index(t => t.VendorId)
                .Index(t => t.CourierId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "VendorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "ManagerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "CourierId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Orders", new[] { "CourierId" });
            DropIndex("dbo.Orders", new[] { "VendorId" });
            DropIndex("dbo.Orders", new[] { "ManagerId" });
            DropTable("dbo.Orders");
        }
    }
}
