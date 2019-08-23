namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moredraftstuff : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DraftParticipant", "Draft_DraftId", "dbo.Draft");
            DropIndex("dbo.DraftParticipant", new[] { "Draft_DraftId" });
            RenameColumn(table: "dbo.DraftParticipant", name: "Draft_DraftId", newName: "DraftId");
            AlterColumn("dbo.DraftParticipant", "DraftId", c => c.Int(nullable: false));
            CreateIndex("dbo.DraftParticipant", "DraftId");
            AddForeignKey("dbo.DraftParticipant", "DraftId", "dbo.Draft", "DraftId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DraftParticipant", "DraftId", "dbo.Draft");
            DropIndex("dbo.DraftParticipant", new[] { "DraftId" });
            AlterColumn("dbo.DraftParticipant", "DraftId", c => c.Int());
            RenameColumn(table: "dbo.DraftParticipant", name: "DraftId", newName: "Draft_DraftId");
            CreateIndex("dbo.DraftParticipant", "Draft_DraftId");
            AddForeignKey("dbo.DraftParticipant", "Draft_DraftId", "dbo.Draft", "DraftId");
        }
    }
}
