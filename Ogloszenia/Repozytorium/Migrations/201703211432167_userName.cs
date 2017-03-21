namespace Repozytorium.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Imie", c => c.String(maxLength: 10));
            AlterColumn("dbo.AspNetUsers", "Nazwisko", c => c.String(maxLength: 35));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Nazwisko", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Imie", c => c.String());
        }
    }
}
