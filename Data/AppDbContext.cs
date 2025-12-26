using Microsoft.EntityFrameworkCore;
using BankingAPI.Models;

namespace BankingAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.AccountNumber).IsUnique();
            entity.Property(e => e.AccountHolderName).IsRequired();
            entity.Property(e => e.Balance).HasPrecision(18, 2);
            
            // ✅ FINAL FIX: SQLite-compatible RowVersion
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("randomblob(8)");  // SQLite function!
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(e => e.ToAccount)
                .WithMany(a => a.ToTransactions)
                .HasForeignKey(e => e.ToAccountId)
                .OnDelete(DeleteBehavior.Restrict);
                 
            entity.HasOne(e => e.FromAccount)
                .WithMany(a => a.FromTransactions)
                .HasForeignKey(e => e.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}