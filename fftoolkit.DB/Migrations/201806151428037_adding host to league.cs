namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinghosttoleague : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.League", "Host", c => c.String());
            AddColumn("dbo.League", "PointsLostPerInterception", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.League", "PointsLostPerPassingYard");
        }
        
        public override void Down()
        {
            AddColumn("dbo.League", "PointsLostPerPassingYard", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.League", "PointsLostPerInterception");
            DropColumn("dbo.League", "Host");
        }
    }
}
