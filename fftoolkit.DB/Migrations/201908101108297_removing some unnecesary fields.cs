namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingsomeunnecesaryfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DraftInvite", "User_UserId", c => c.Int());
            AddColumn("dbo.DraftParticipant", "Name", c => c.String());
            CreateIndex("dbo.DraftInvite", "User_UserId");
            AddForeignKey("dbo.DraftInvite", "User_UserId", "dbo.User", "UserId");
            DropColumn("dbo.DraftParticipant", "IsLocal");
            DropColumn("dbo.DraftParticipant", "LocalName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DraftParticipant", "LocalName", c => c.String());
            AddColumn("dbo.DraftParticipant", "IsLocal", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.DraftInvite", "User_UserId", "dbo.User");
            DropIndex("dbo.DraftInvite", new[] { "User_UserId" });
            DropColumn("dbo.DraftParticipant", "Name");
            DropColumn("dbo.DraftInvite", "User_UserId");
        }
    }
}
