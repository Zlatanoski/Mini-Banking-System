namespace BankingAPI.DTOs.Responses;

public class AccountResponse
{
    public int Id{get; set;} // primary key
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime createdAt { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}