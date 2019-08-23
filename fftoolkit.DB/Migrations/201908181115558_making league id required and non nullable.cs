namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makingleagueidrequiredandnonnullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Draft", "LeagueId", "dbo.League");
            DropIndex("dbo.Draft", new[] { "LeagueId" });
            AlterColumn("dbo.Draft", "LeagueId", c => c.Int(nullable: false));
            CreateIndex("dbo.Draft", "LeagueId");
            AddForeignKey("dbo.Draft", "LeagueId", "dbo.League", "LeagueId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Draft", "LeagueId", "dbo.League");
            DropIndex("dbo.Draft", new[] { "LeagueId" });
            AlterColumn("dbo.Draft", "LeagueId", c => c.Int());
            CreateIndex("dbo.Draft", "LeagueId");
            AddForeignKey("dbo.Draft", "LeagueId", "dbo.League", "LeagueId");
        }
    }
}
