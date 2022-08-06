namespace TechShop.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPaymentMethod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PaymentID", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "PaymentMethod", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PaymentMethod");
            DropColumn("dbo.Orders", "PaymentID");
        }
    }
}
