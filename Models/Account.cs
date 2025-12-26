using System.ComponentModel.DataAnnotations;

namespace BankingAPI.Models;

public class Account
{
    public int Id { get; set; }
    
    public string AccountNumber { get; set; } = string.Empty;
    
    public string AccountHolderName { get; set; } = string.Empty;
    
    public decimal Balance { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [Timestamp]  
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ICollection<Transaction> FromTransactions { get; set; } = new List<Transaction>();
    
    public ICollection<Transaction> ToTransactions { get; set; } = new List<Transaction>();
}