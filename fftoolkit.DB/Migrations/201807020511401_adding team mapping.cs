namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingteammapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamMapping",
                c => new
                    {
                        TeamMappingId = c.Int(nullable: false, identity: true),
                        OldTeam = c.String(),
                        NewTeam = c.String(),
                    })
                .PrimaryKey(t => t.TeamMappingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TeamMapping");
        }
    }
}
