// connection between app and db
using Microsoft.EntityFrameworkCore;
using BankingAPI.Models; // import models of the DB tables

namespace BankingAPI.Data;

public class AppDbContext : DbContext // DbContext class provided by EF Core // access and extend 
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // receive configuration for EF Core
    {
        
    }

    public DbSet<Account> Accounts { get; set; } = null!; // represent Accounts table in the DB
    public DbSet<Transaction> Transactions { get; set; } = null!; // represent Transactions table in the DB

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity=>
        {
            entity.HasKey(e => e.Id); // primary key
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.AccountNumber).IsUnique(); // unique constraint
            entity.Property(e => e.AccountHolderName).IsRequired();
            entity.Property(e => e.Balance).HasPrecision(18, 2);
            entity.Property(e => e.RowVersion).IsRowVersion();
            //I implemented optimistic concurrency using row versioning to prevent lost updates
        });

        modelBuilder.Entity<Transaction>(entity=>
        {
            entity.HasKey(e => e.Id); // primary key of that table
            entity.Property(e => e.Amount).HasPrecision(18, 2); // up to 18 digits, max 2 decimal places allowed    
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(e => e.toAccount).WithMany(a => a.Transactions).HasForeignKey(e => e.ToAccountId).OnDelete(DeleteBehavior.Restrict);
                 
            entity.HasOne(e => e.fromAccount).WithMany(a => a.Transactions).HasForeignKey(e => e.FromAccountId).OnDelete(DeleteBehavior.Restrict);


        });
    
    }
}