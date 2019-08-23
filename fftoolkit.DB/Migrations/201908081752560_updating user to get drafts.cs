namespace fftoolkit.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingusertogetdrafts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Draft", "User_UserId", "dbo.User");
            DropIndex("dbo.Draft", new[] { "User_UserId" });
            DropColumn("dbo.Draft", "User_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Draft", "User_UserId", c => c.Int());
            CreateIndex("dbo.Draft", "User_UserId");
            AddForeignKey("dbo.Draft", "User_UserId", "dbo.User", "UserId");
        }
    }
}
