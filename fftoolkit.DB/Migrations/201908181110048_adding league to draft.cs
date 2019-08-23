namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingleaguetodraft : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Draft", "LeagueId", c => c.Int());
            CreateIndex("dbo.Draft", "LeagueId");
            AddForeignKey("dbo.Draft", "LeagueId", "dbo.League", "LeagueId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Draft", "LeagueId", "dbo.League");
            DropIndex("dbo.Draft", new[] { "LeagueId" });
            DropColumn("dbo.Draft", "LeagueId");
        }
    }
}
