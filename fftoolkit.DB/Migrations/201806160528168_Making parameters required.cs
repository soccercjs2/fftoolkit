namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Makingparametersrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.League", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.League", "Url", c => c.String(nullable: false));
            AlterColumn("dbo.League", "Host", c => c.String(nullable: false));
            AlterColumn("dbo.Player", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Player", "Position", c => c.String(nullable: false));
            AlterColumn("dbo.Player", "Team", c => c.String(nullable: false));
            AlterColumn("dbo.User", "AspNetUserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "AspNetUserId", c => c.String());
            AlterColumn("dbo.Player", "Team", c => c.String());
            AlterColumn("dbo.Player", "Position", c => c.String());
            AlterColumn("dbo.Player", "Name", c => c.String());
            AlterColumn("dbo.League", "Host", c => c.String());
            AlterColumn("dbo.League", "Url", c => c.String());
            AlterColumn("dbo.League", "Name", c => c.String());
        }
    }
}
