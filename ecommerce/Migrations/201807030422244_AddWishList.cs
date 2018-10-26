namespace ecommerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWishList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        WishListId = c.Int(nullable: false, identity: true),
                        wishId = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ImageUrl = c.String(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WishListId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishLists", "ProductId", "dbo.Products");
            DropIndex("dbo.WishLists", new[] { "ProductId" });
            DropTable("dbo.WishLists");
        }
    }
}
