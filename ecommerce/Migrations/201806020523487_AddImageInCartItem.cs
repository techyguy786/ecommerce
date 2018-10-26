namespace ecommerce.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddImageInCartItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItems", "ImageUrl", c => c.String(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.CartItems", "ImageUrl");
        }
    }
}
