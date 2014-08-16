namespace EbayWatcher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedattributestousers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EbaySessionId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EbaySessionId");
        }
    }
}
