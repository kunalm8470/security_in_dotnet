using Core.Entities;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
