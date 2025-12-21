namespace BankingAPI.Models;

public class Account
{
    public int Id{get; set;} // primary key
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

//Can store only transactions objects 
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

}