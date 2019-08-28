namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makingplayernullableondraftpick : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DraftPick", "PlayerId", "dbo.Player");
            DropIndex("dbo.DraftPick", new[] { "PlayerId" });
            AlterColumn("dbo.DraftPick", "PlayerId", c => c.Int());
            CreateIndex("dbo.DraftPick", "PlayerId");
            AddForeignKey("dbo.DraftPick", "PlayerId", "dbo.Player", "PlayerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DraftPick", "PlayerId", "dbo.Player");
            DropIndex("dbo.DraftPick", new[] { "PlayerId" });
            AlterColumn("dbo.DraftPick", "PlayerId", c => c.Int(nullable: false));
            CreateIndex("dbo.DraftPick", "PlayerId");
            AddForeignKey("dbo.DraftPick", "PlayerId", "dbo.Player", "PlayerId", cascadeDelete: true);
        }
    }
}
