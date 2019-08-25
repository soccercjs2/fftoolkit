namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingplayerrelationfordraftpick : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DraftPick", "PlayerId");
            AddForeignKey("dbo.DraftPick", "PlayerId", "dbo.Player", "PlayerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DraftPick", "PlayerId", "dbo.Player");
            DropIndex("dbo.DraftPick", new[] { "PlayerId" });
        }
    }
}
