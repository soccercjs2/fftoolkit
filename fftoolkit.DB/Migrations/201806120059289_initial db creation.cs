namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialdbcreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.League",
                c => new
                    {
                        LeagueId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Url = c.String(),
                        Quarterbacks = c.Int(nullable: false),
                        RunningBacks = c.Int(nullable: false),
                        WideReceivers = c.Int(nullable: false),
                        TightEnds = c.Int(nullable: false),
                        Flexes = c.Int(nullable: false),
                        PointsPerReception = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PointsPerPassingTouchdown = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PointsPerPassingYard = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PointsLostPerPassingYard = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.LeagueId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Position = c.String(),
                        Team = c.String(),
                        PassingYards = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PassingTouchdowns = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Interceptions = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RushingYards = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RushingTouchdowns = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Receptions = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceivingYards = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceivingTouchdowns = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PlayerId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        AspNetUserId = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.League", "UserId", "dbo.User");
            DropIndex("dbo.League", new[] { "UserId" });
            DropTable("dbo.User");
            DropTable("dbo.Player");
            DropTable("dbo.League");
        }
    }
}
