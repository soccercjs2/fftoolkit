namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingwritein : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DraftPick", "WriteInName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DraftPick", "WriteInName");
        }
    }
}
