namespace BankingAPI.DTOs.Requests;

public class DepositRequest
{
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty; 
}