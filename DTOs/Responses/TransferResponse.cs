namespace BankingAPI.DTOs.Responses;

public class TransferResponse
{
    public bool Success { get; set; }
    public int TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Message { get; set; } = string.Empty;
    public int FromAccountId { get; set; }
    public decimal FromAccountNewBalance { get; set; }
    public decimal ToAccountNewBalance { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
}