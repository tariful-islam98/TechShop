namespace TechShop.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserEmailToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "UserEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "UserEmail");
        }
    }
}
