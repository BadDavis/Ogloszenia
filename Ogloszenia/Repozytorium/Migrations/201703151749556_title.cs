namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class title : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ogloszenie", "Tytul", c => c.String(nullable: false, maxLength: 72));
            AlterColumn("dbo.Ogloszenie", "Tresc", c => c.String(nullable: false, maxLength: 2500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ogloszenie", "Tresc", c => c.String(maxLength: 2500));
            AlterColumn("dbo.Ogloszenie", "Tytul", c => c.String(maxLength: 72));
        }
    }
}
