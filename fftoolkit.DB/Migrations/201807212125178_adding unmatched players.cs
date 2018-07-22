namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingunmatchedplayers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NameMapping",
                c => new
                    {
                        NameMappingId = c.Int(nullable: false, identity: true),
                        OldName = c.String(),
                        NewName = c.String(),
                    })
                .PrimaryKey(t => t.NameMappingId);
            
            CreateTable(
                "dbo.UnmatchedPlayer",
                c => new
                    {
                        UnmatchedPlayerId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Position = c.String(nullable: false),
                        Team = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UnmatchedPlayerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UnmatchedPlayer");
            DropTable("dbo.NameMapping");
        }
    }
}
