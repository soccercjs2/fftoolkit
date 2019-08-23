namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingdraftstuff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DraftInvite",
                c => new
                    {
                        DraftInviteId = c.Int(nullable: false, identity: true),
                        DraftId = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        Guid = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DraftInviteId)
                .ForeignKey("dbo.Draft", t => t.DraftId, cascadeDelete: true)
                .Index(t => t.DraftId);
            
            CreateTable(
                "dbo.DraftParticipant",
                c => new
                    {
                        DraftParticipantId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DraftPosition = c.Int(nullable: false),
                        IsLocal = c.Boolean(nullable: false),
                        LocalName = c.String(),
                        Draft_DraftId = c.Int(),
                    })
                .PrimaryKey(t => t.DraftParticipantId)
                .ForeignKey("dbo.Draft", t => t.Draft_DraftId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.Draft_DraftId);
            
            CreateTable(
                "dbo.Draft",
                c => new
                    {
                        DraftId = c.Int(nullable: false, identity: true),
                        OwnerUserId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Teams = c.Int(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.DraftId)
                .ForeignKey("dbo.User", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DraftParticipant", "UserId", "dbo.User");
            DropForeignKey("dbo.Draft", "User_UserId", "dbo.User");
            DropForeignKey("dbo.DraftParticipant", "Draft_DraftId", "dbo.Draft");
            DropForeignKey("dbo.DraftInvite", "DraftId", "dbo.Draft");
            DropIndex("dbo.Draft", new[] { "User_UserId" });
            DropIndex("dbo.DraftParticipant", new[] { "Draft_DraftId" });
            DropIndex("dbo.DraftParticipant", new[] { "UserId" });
            DropIndex("dbo.DraftInvite", new[] { "DraftId" });
            DropTable("dbo.Draft");
            DropTable("dbo.DraftParticipant");
            DropTable("dbo.DraftInvite");
        }
    }
}
