namespace EbayWatcher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddfullnametoCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "FullName");
        }
    }
}
