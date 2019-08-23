namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingislocaltodraftparticipant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DraftParticipant", "IsLocal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DraftParticipant", "IsLocal");
        }
    }
}
