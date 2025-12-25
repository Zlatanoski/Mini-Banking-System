namespace BankingAPI.DTOs.Responses;

public class TransactionResponse
{
    public int Id { get; set; }
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
}