namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingdraftpickstake2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DraftPick",
                c => new
                    {
                        DraftPickId = c.Int(nullable: false, identity: true),
                        DraftId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                        Round = c.Int(nullable: false),
                        Pick = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DraftPickId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DraftPick");
        }
    }
}
