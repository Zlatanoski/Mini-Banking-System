namespace BankingAPI.DTOs.Requests;

public class CreateAccountRequest
{
    public String AccountNumber { get; set; } = string.Empty;

    public String AccountHolderName { get; set; } = string.Empty;

    public decimal InitialBalance { get; set; }
}