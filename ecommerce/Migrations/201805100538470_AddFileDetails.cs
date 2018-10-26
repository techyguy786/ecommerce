namespace ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(nullable: false),
                        Extension = c.String(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            DropColumn("dbo.Products", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImageUrl", c => c.String(nullable: false));
            DropForeignKey("dbo.FileDetails", "ProductId", "dbo.Products");
            DropIndex("dbo.FileDetails", new[] { "ProductId" });
            DropTable("dbo.FileDetails");
        }
    }
}
