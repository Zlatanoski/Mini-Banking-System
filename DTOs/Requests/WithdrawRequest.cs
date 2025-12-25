namespace BankingAPI.DTOs.Requests;

public class WithdrawRequest
{
    public int FromAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}