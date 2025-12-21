
using BankingAPI.DTOs.Requests;
using BankingAPI.Data; 
using Microsoft.EntityFrameworkCore;
using BankingAPI.Models;
namespace BankingAPI.Services;

public class CreateAccountService : IAccountService
{

    private readonly AppDbContext _context;

    public CreateAccountService(AppDbContext context)
    {
        _context = context;
    }

	public async Task<AccountResponse> CreateAccount(CreateAccountRequest request)
	{   // request.AccountNumber request.AccountHolderName request.InitialBalance

        if(string.IsNullOrWhiteSpace(request.AccountNumber))
        {
            throw new Exception("Account number and account holder name cannot be empty.");
        }
        if(string.IsNullOrWhiteSpace(request.AccountHolderName))
        {
            throw new Exception("Account number and account holder name cannot be empty.");
        }
        if(request.InitialBalance < 0)
        {
            throw new Exception("Initial balance cannot be negative.");
        }
        var exists = await _context.Accounts
            .AnyAsync(a => a.AccountNumber == request.AccountNumber);
            if(exists)
            {
                throw new Exception("Account with the same account number already exists.");
            }

        var account = new Account
        {
            AccountNumber = request.AccountNumber,
            AccountHolderName = request.AccountHolderName,
            Balance = request.InitialBalance
        };
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        var response = new AccountResponse
        {
            Id = account.Id,
            AccountNumber =  account.AccountNumber,
            AccountHolderName = account.AccountHolderName,
            Balance = account.Balance,
            createdAt = account.createdAt,
            RowVersion = account.RowVersion
        };

        return response;

	}

     public async Task<List<AccountResponse>> GetAllAccounts()
    {
        throw new NotImplementedException();
    }
    
    public async Task<AccountResponse?> GetAccountById(int id)
    {
        throw new NotImplementedException();
    }


}