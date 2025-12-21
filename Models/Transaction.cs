namespace BankingAPI.Models;

public class Transaction
{
    
public int Id { get; set; } // primary key

public int ToAccountId { get; set; }

public int FromAccountId { get; set; }

public decimal Amount { get; set; }

public string Description { get; set; } = string.Empty;

public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

public string Status  { get; set; } = "Completed";

//object reference to the Account class for each of the 2 instances gives us object shortcut to access
public Account? toAccount { get; set; }
public Account? fromAccount { get; set; }

}