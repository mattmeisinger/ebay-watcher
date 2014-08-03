namespace EbayWatcher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WishlistItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        CategoryId = c.Int(nullable: false),
                        Status = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.WishlistItemHistoricalItems",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        WishlistItemId = c.Int(nullable: false),
                        BuyItNowPrice = c.Double(),
                        AuctionPrice = c.Double(),
                        AuctionEndTime = c.DateTime(),
                        BidCount = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WishlistItems", t => t.WishlistItemId, cascadeDelete: true)
                .Index(t => t.WishlistItemId);
            
            CreateTable(
                "dbo.WishlistItemIgnoreItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WishlistItemId = c.Int(nullable: false),
                        EbayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WishlistItems", t => t.WishlistItemId, cascadeDelete: true)
                .Index(t => t.WishlistItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishlistItemIgnoreItems", "WishlistItemId", "dbo.WishlistItems");
            DropForeignKey("dbo.WishlistItemHistoricalItems", "WishlistItemId", "dbo.WishlistItems");
            DropForeignKey("dbo.WishlistItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.WishlistItemIgnoreItems", new[] { "WishlistItemId" });
            DropIndex("dbo.WishlistItemHistoricalItems", new[] { "WishlistItemId" });
            DropIndex("dbo.WishlistItems", new[] { "CategoryId" });
            DropTable("dbo.WishlistItemIgnoreItems");
            DropTable("dbo.WishlistItemHistoricalItems");
            DropTable("dbo.WishlistItems");
            DropTable("dbo.Categories");
        }
    }
}
