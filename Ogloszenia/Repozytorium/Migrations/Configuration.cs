namespace Repozytorium.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Repozytorium.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OGLContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OGLContext context)
        {
            // Do debugowania metody seed
         //    if (System.Diagnostics.Debugger.IsAttached == false)
           //     System.Diagnostics.Debugger.Launch();
            SeedRoles(context);
            SeedUsers(context);
            SeedOgloszenia(context);
            SeedKategorie(context);
            SeedOgloszenie_Kategoria(context);

        }


        private void SeedRoles(OGLContext context)
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>());

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Pracownik"))
            {
                var role = new IdentityRole();
                role.Name = "Pracownik";
                roleManager.Create(role);
            }
        }

        private void SeedUsers(OGLContext context)
        {
            var store = new UserStore<Uzytkownik>(context);
            var manager = new UserManager<Uzytkownik>(store);
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {

                var user = new Uzytkownik { UserName = "Admin@o2.pl"};

                var adminresult = manager.Create(user, "1234Abc,");

                if (adminresult.Succeeded)
                    manager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u=>u.UserName == "Marek"))
            {
                var user = new Uzytkownik { UserName = "marek@gmail.com" };
                var adminResult = manager.Create(user, "1234Abc,");
                if (adminResult.Succeeded)
                {
                    manager.AddToRole(user.Id, "Pracownik");
                }
            }
        }

        private void SeedOgloszenia(OGLContext context)
        {

            var idUzytkownika = context.Set<Uzytkownik>().Where(u => u.UserName == "Admin").FirstOrDefault().Id;
            for (int i = 1; i <= 10; i++)
            {
                var ogl = new Ogloszenie()
                {
                    Id = i,
                    UzytkownikId = idUzytkownika,
                    Tresc = "Treœæ og³oszenia" + i.ToString(),
                    Tytul = "Tytu³ og³oszenia" + i.ToString(),
                    DataDodania = DateTime.Now.AddDays(-i),
                    DataZakonczenia = DateTime.Now.AddDays(-i+14),
                    Cena = i*120,
                    Stan = "U¿ywane",
                };
                context.Set<Ogloszenie>().AddOrUpdate(ogl);
            }
            context.SaveChanges();
        }

        private void SeedKategorie(OGLContext context)
        {
            for (int i = 1; i <= 10; i++)
            {
                var kat = new Kategoria()
                {
                    Id = i,
                    Nazwa = "Nazwa kategorii" + i.ToString(),
                    Tresc = "Treœæ og³oszenia" + i.ToString(),
                    MetaTytul = "Tytu³ kategorii" + i.ToString(),
                    Metaopis = "Opis kategorii" + i.ToString(),
                    MetaSlowa = "S³owa kluczowe do kategorii" + i.ToString(),
                    ParentId = i
                };
                context.Set<Kategoria>().AddOrUpdate(kat);
            }
            context.SaveChanges();
        }

        private void SeedOgloszenie_Kategoria(OGLContext context)
        {
            for (int i = 1; i < 10; i++)
            {
                var okat = new Ogloszenie_Kategoria()
                {
                    Id = i,
                    OgloszenieId = i / 2 + 1,
                    KategoriaId = i / 2 + 2
                };

                context.Set<Ogloszenie_Kategoria>().AddOrUpdate(okat);
            }
            context.SaveChanges();
        }

    }
}
