using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Plutus.Application;
using Plutus.Domain;
using Plutus.Domain.Enums;
using Plutus.Domain.ValueObjects;

namespace Plutus.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(e =>
            {
                e.ToTable("Transaction");

                e.HasKey(t => t.Id);

                e.Property(t => t.Amount)
                    .HasConversion(a => a.Value, d => Amount.From(d))
                    .IsRequired()
                    ;

                e.Property(t => t.CreatedOnUtc).HasDefaultValue(DateTime.UtcNow)
                    .HasColumnType("timestamp with time zone");

                e.Property(t => t.Description)
                    .HasMaxLength(1024)
                    .HasConversion(d => d.Value, s => TransactionDescription.From(s));

                e.Property(t => t.Username)
                    .HasConversion(u => u.Value, s => Username.From(s))
                    .IsRequired();

                // e.HasOne(typeof(Transaction), "_user").WithMany("_transaction").HasForeignKey("Username");
                e.HasOne(t => t.User).WithMany(u => u.Transactions).HasForeignKey(t => t.Username);

                e.HasOne(t => t.Category).WithMany(c => c.Transactions).HasForeignKey(t => t.CategoryId);
                
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
                // e.HasKey(c => new {c.Name, c.TransactionType});
                e.Property(u => u.Name).IsRequired().HasMaxLength(20);
                e.Property(c => c.Id).IsRequired();

                e.HasIndex(c => new {c.Name, c.TransactionType}).IsUnique();

                e.HasData(
                    new Category(Guid.NewGuid(), "Food & Drinks", TransactionType.Expense),
                    new Category(Guid.NewGuid(), "Online Shopping", TransactionType.Expense),
                    new Category(Guid.NewGuid(), "Travel", TransactionType.Expense),
                    new Category(Guid.NewGuid(), "Transfer", TransactionType.Expense),
                    new Category(Guid.NewGuid(), "Bills", TransactionType.Expense),
                    new Category(Guid.NewGuid(), "Salary", TransactionType.Income),
                    new Category(Guid.NewGuid(), "Transfer", TransactionType.Income)
                );
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasIndex(u => u.Email).IsUnique();

                e.Property(u => u.CreatedOnUtc).HasDefaultValue(DateTime.UtcNow)
                    .HasColumnType("timestamp with time zone");
                e.Property(u => u.LastModifiedUtc).HasColumnType("timestamp with time zone");

                e.Property(u => u.FirstName).IsRequired().HasMaxLength(32);
                e.Property(u => u.LastName).IsRequired().HasMaxLength(32);

                e.Property(u => u.Password).HasColumnName("Password").HasMaxLength(128).HasConversion(p => p.Value, s => Password.From(s));

                // e.OwnsOne(u => u.Password).Property(p => p.Value).HasColumnName("Password").HasMaxLength(32);

                e.HasKey(u => u.Username);

                e.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasConversion(u => u.Value, s => Username.From(s));

                e.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasConversion(em => em.Value, s => Email.From(s));


                var passwordHelper = new PasswordHelper();
                const string username = "sanjay";
                var hashedPassword = passwordHelper.GeneratePasswordHash("Sanjay_11!", username);
                
                e.HasData(
                    new User(username, "sanjay11@outlook.com", hashedPassword,"Sanjay", "Idpuganti")
                );
            });
        }
    }
}