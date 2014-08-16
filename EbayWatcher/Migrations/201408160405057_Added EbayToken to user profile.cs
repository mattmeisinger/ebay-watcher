namespace EbayWatcher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEbayTokentouserprofile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EbayToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EbayToken");
        }
    }
}
