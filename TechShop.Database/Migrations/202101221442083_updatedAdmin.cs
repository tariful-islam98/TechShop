namespace TechShop.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedAdmin : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Admins", "ConfirmPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Admins", "ConfirmPassword", c => c.String());
        }
    }
}
