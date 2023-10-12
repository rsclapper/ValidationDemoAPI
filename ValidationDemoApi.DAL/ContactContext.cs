using Microsoft.EntityFrameworkCore;
using ValidationDemoApi.CORE.Models;

namespace ValidationDemoApi.DAL
{
    public class ContactContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<Order>().ToTable("Order");

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Contact)
                .HasForeignKey(o => o.ContactId);

            modelBuilder.Entity<Contact>()
                .Property(c => c.Name)
                    .HasMaxLength(50)
                    .IsRequired();
            modelBuilder.Entity<Contact>()
                .Property(c => c.Email)
                    .HasMaxLength(50)
                    .IsRequired();
                    
                    

            modelBuilder.Entity<Contact>()
                .Property(c => c.Phone)
                    .HasMaxLength(12)
                    .IsRequired();
            modelBuilder.Entity<Contact>()
                .Property(c => c.City)
                    .HasMaxLength(50)
                    .IsRequired();
            modelBuilder.Entity<Contact>()
                .Property(c => c.Region)
                    .HasMaxLength(50)
                    .IsRequired();
            modelBuilder.Entity<Contact>()
               .Property(c => c.PostalCode)
                   .HasMaxLength(50)
                   .IsRequired();

            modelBuilder.Entity<Contact>()
              .Property(c => c.Address)
                  .HasMaxLength(50)
                  .IsRequired();
        }
    }   
   
}
