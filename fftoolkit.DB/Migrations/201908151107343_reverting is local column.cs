namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revertingislocalcolumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DraftParticipant", "IsLocal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DraftParticipant", "IsLocal", c => c.Boolean(nullable: false));
        }
    }
}
