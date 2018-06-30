namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cleaningupproperties : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.League", "Host");
        }
        
        public override void Down()
        {
            AddColumn("dbo.League", "Host", c => c.String(nullable: false));
        }
    }
}
