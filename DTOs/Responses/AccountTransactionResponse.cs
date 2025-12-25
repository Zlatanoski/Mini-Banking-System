namespace BankingAPI.DTOs.Responses;

public class AccountTransactionResponse
{
    public int AccountId { get; set; }
    public decimal newBalance { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public int Amount { get; set; }
    public int TransactionId { get; set; }
    

}