namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makingdraftparticipantuseridnullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DraftParticipant", "UserId", "dbo.User");
            DropIndex("dbo.DraftParticipant", new[] { "UserId" });
            AlterColumn("dbo.DraftParticipant", "UserId", c => c.Int());
            CreateIndex("dbo.DraftParticipant", "UserId");
            AddForeignKey("dbo.DraftParticipant", "UserId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DraftParticipant", "UserId", "dbo.User");
            DropIndex("dbo.DraftParticipant", new[] { "UserId" });
            AlterColumn("dbo.DraftParticipant", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.DraftParticipant", "UserId");
            AddForeignKey("dbo.DraftParticipant", "UserId", "dbo.User", "UserId", cascadeDelete: true);
        }
    }
}
