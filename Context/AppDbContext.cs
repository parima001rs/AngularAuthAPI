using Microsoft.EntityFrameworkCore;
using UserAuth.Models;

namespace UserAuth.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Customer>().ToTable("customers");
            modelBuilder.Entity<Device>().ToTable("devices");

            /*
            modelBuilder.Entity<Device>()
                .HasOne(cd => cd.Customer)
                .WithMany()
                .HasForeignKey(cd => cd.CustId);
            */

            modelBuilder.Entity<Device>()
                .HasOne(d => d.Customer)
                .WithMany(c => c.Devices)
                .HasForeignKey(d => d.CustId)
                .HasPrincipalKey(c => c.CustomerId);
        }
}
}
