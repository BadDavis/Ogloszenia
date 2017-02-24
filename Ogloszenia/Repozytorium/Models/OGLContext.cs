using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Repozytorium.Models
{
    // You can add profile data for the user by adding more properties to your Uzytkownik class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class OGLContext : IdentityDbContext
    {
        public OGLContext()
            : base("DefaultConnection")
        {
        }

        public static OGLContext Create()
        {
            return new OGLContext();
        }

        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<Ogloszenie> Ogloszenia { get; set; }
        public DbSet<Uzytkownik> Uzytkownik { get; set; }
        public DbSet<Ogloszenie_Kategoria> Ogloszenie_Kategoria { get; set; }

        #region notPlurazingTableName

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //wyłącza konwncje l. mnogiej
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //wył. CascadeDelete
            //wł. za pomoca FluentApi
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //używamy FluentApi do ustalenia powiązań pomiędzy tabelami i włączamy dla nich CascadeDelete
            //tylko relacja Ogloszenie-Uzytkownik
            modelBuilder.Entity<Ogloszenie>()
                            .HasRequired(x => x.Uzykownik)
                            .WithMany(x => x.Ogloszenia)
                            .HasForeignKey(x => x.UzytkownikId)
                            .WillCascadeOnDelete(true);
        }

        #endregion
    }
}