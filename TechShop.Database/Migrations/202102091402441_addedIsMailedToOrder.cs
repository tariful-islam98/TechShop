namespace TechShop.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedIsMailedToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsMailed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsMailed");
        }
    }
}
