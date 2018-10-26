namespace ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoryTypeCategoryProductFull : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        CategoryTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.CategoryTypes", t => t.CategoryTypeId, cascadeDelete: true)
                .Index(t => t.CategoryTypeId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        ImageId = c.String(nullable: false),
                        ImageName = c.String(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Details = c.String(nullable: false),
                        ArrivalTime = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        BrandName = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.CategoryTypes",
                c => new
                    {
                        CategoryTypeId = c.Int(nullable: false, identity: true),
                        CategoryTypeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "CategoryTypeId", "dbo.CategoryTypes");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Categories", new[] { "CategoryTypeId" });
            DropTable("dbo.CategoryTypes");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
