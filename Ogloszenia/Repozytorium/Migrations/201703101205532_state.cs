namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class state : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ogloszenie", "Stan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ogloszenie", "Stan", c => c.String(maxLength: 15));
        }
    }
}
