namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class acceptfordraftinvite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DraftInvite", "Accepted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DraftInvite", "Accepted");
        }
    }
}
