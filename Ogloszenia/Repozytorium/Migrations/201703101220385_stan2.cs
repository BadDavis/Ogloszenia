namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stan2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ogloszenie", "Stan", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ogloszenie", "Stan");
        }
    }
}
