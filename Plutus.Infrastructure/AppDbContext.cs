using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Plutus.Domain;
using Plutus.Domain.ValueObjects;

namespace Plutus.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }
        
        public AppDbContext(DbContextOptions options): base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(e =>
            {
                e.ToTable("Transaction");
                e.Property(t => t.Amount)
                    .HasConversion(a => a.Value, d => Amount.From(d))
                    .IsRequired()
                    ;

                e.Property(t => t.CreatedOnUtc).HasDefaultValue(DateTime.UtcNow).HasColumnType("timestamp with time zone");

                e.Property(t => t.Description)
                    .HasMaxLength(1024)
                    .HasConversion(d => d.Value, s => TransactionDescription.From(s));

                e.Property(t => t.Username)
                    .HasConversion(u => u.Value, s => Username.From(s))
                    .IsRequired();
                
                e.Property(t => t.CategoryId).IsRequired();
                
                e.Property(t => t.DateTime)
                    .HasConversion(d => d.Value, time => TransactionDateTime.From(time))
                    .HasColumnType("timestamp with time zone")
                    .IsRequired();
                
                e.Property(t => t.TransactionType).IsRequired();
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Category");
                e.HasKey(c => new {c.Name, c.TransactionType});
                e.Property(u => u.Name).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasIndex(u => u.Email).IsUnique();
                
                e.Property(u => u.CreatedOnUtc).HasDefaultValue(DateTime.UtcNow).HasColumnType("timestamp with time zone");
                e.Property(u => u.LastModifiedUtc).HasColumnType("timestamp with time zone");
                
                e.Property(u => u.FirstName).IsRequired().HasMaxLength(32);
                e.Property(u => u.LastName).IsRequired().HasMaxLength(32);

                e.OwnsOne(u => u.Password).Property(p => p.Value).HasColumnName("Password").HasMaxLength(32);

                e.HasKey(u => u.Username);
                
                e.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasConversion(u => u.Value, s => Username.From(s));

                e.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasConversion(em => em.Value, s => Email.From(s));
            });
        }
    }
}