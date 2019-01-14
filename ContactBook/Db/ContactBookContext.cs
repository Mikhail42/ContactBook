using ContactBook.DbModel;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Db
{
    public class ContactBookContext : DbContext
    {
        private static readonly ILog log = LogManager.GetLogger("ContactBookContext");

        public DbSet<Person> Persons { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PersonContact> PersonContacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            log.Info("OnConfiguring");
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=ContactBook;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            log.Info("OnModelCreating");
            modelBuilder.Entity<PersonContact>()
                .HasKey(pc => new { pc.ContactId, pc.PersonId });
            modelBuilder.Entity<PersonContact>()
                .HasOne(pc => pc.Person)
                .WithMany(p => p.PersonContacts)
                .HasForeignKey(pc => pc.PersonId)
                .HasPrincipalKey(p => p.Id);
        }
    }
}
