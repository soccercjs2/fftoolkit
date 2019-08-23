namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingroundstodraft : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Draft", "Rounds", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Draft", "Rounds");
        }
    }
}
