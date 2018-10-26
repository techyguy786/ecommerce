namespace ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDiscountImagePriceInProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "DiscountPercentage", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "FinalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "ImageUrl", c => c.String(nullable: false));
            DropColumn("dbo.Products", "Discount");
            DropColumn("dbo.Products", "ImageId");
            DropColumn("dbo.Products", "ImageName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImageName", c => c.String(nullable: false));
            AddColumn("dbo.Products", "ImageId", c => c.String(nullable: false));
            AddColumn("dbo.Products", "Discount", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Products", "ImageUrl");
            DropColumn("dbo.Products", "FinalPrice");
            DropColumn("dbo.Products", "DiscountPercentage");
        }
    }
}
