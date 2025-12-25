namespace BankingAPI.DTOs.Requests;
public class TransferRequest
{   
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty; // possibly have option to leave a note 
}