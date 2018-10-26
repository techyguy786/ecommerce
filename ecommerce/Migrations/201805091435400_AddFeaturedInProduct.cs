namespace ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFeaturedInProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Featured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Featured");
        }
    }
}
