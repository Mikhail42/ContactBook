using ContactBook.DbModel;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Db
{
    public class ContactBookContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PersonContact> PersonContacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=ContactBook;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
