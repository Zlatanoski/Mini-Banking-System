namespace BankingAPI.Services;
using BankingAPI.DTOs.Requests;
using BankingAPI.DTOs.Responses;
using BankingAPI.Models;
using BankingAPI.Data;
using Microsoft.EntityFrameworkCore;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TransferResponse> TransferMoney(TransferRequest request)
    {   
        var fromAccount = await _context.Accounts.FindAsync(request.FromAccountId);
        var toAccount = await _context.Accounts.FindAsync(request.ToAccountId);

        if (fromAccount == null)
        {
            throw new Exception("Sender account does not exist.");
        }
        
        if (toAccount == null)
        {
            throw new Exception("Recipient account does not exist.");
        }
        
        if (request.Amount <= 0)
        {
            throw new Exception("Transfer amount must be greater than zero.");
        }
        
        if (fromAccount.Balance < request.Amount)
        {
            throw new Exception("Insufficient funds in the sender's account.");
        }
        
        if (request.FromAccountId == request.ToAccountId)
        {
            throw new Exception("Cannot transfer to the same account.");
        }
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;

            var transactionRecord = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                Description = request.Description,
                TransactionDate = DateTime.UtcNow,
                Status = "Completed" 
            };
            
            _context.Transactions.Add(transactionRecord);
            await _context.SaveChangesAsync(); 
            await transaction.CommitAsync();

            return new TransferResponse
            {
                Success = true,
                TransactionId = transactionRecord.Id,
                FromAccountId = fromAccount.Id,
                ToAccountId = toAccount.Id,
                Amount = transactionRecord.Amount,
                FromAccountNewBalance = fromAccount.Balance,
                ToAccountNewBalance = toAccount.Balance,
                TransactionDate = transactionRecord.TransactionDate,
                Message = "Transfer completed successfully."
            };
        }
        catch (DbUpdateConcurrencyException)  
        {
            await transaction.RollbackAsync(); 
            throw new Exception("The account was modified by another user. Please refresh and try again.");
        }
        catch (Exception)  
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<TransactionResponse?> GetTransactionById(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        
        if (transaction == null)
        {
            return null;
        }
        
        return new TransactionResponse
        {
            Id = transaction.Id,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            Description = transaction.Description,
            Status = transaction.Status
        };
    }
    
    public async Task<List<TransactionResponse>> GetTransactionsByAccountId(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        
        if (account == null)
        {
            throw new Exception("Account does not exist.");
        }
        
        var transactions = await _context.Transactions
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
            
        var response = transactions.Select(transaction => new TransactionResponse
        {
            Id = transaction.Id,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            Description = transaction.Description,
            Status = transaction.Status
        }).ToList();
        
        return response;
    }

    public async Task<AccountTransactionResponse> Deposit(DepositRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new Exception("Deposit amount must be greater than zero.");
        }
        
        var account = await _context.Accounts.FindAsync(request.ToAccountId);
        
        if (account == null)
        {
            throw new Exception("Account does not exist.");
        }
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            account.Balance += request.Amount;
            
            var transactionRecord = new Transaction
            {
                FromAccountId = null,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                Description = request.Description,
                TransactionDate = DateTime.UtcNow,
                Status = "Completed"
            };
            
            _context.Transactions.Add(transactionRecord);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return new AccountTransactionResponse
            {
                Success = true,
                TransactionId = transactionRecord.Id,
                AccountId = account.Id,
                newBalance = account.Balance,
                Amount = (int)transactionRecord.Amount,
                TransactionDate = transactionRecord.TransactionDate,
                Message = "Deposit completed successfully."
            };
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();
            throw new Exception("The account was modified by another user. Please refresh and try again.");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<AccountTransactionResponse> Withdraw(WithdrawRequest request)
    {
        // Validation
        if (request.Amount <= 0)
        {
            throw new Exception("Withdrawal amount must be greater than zero.");
        }
        
        var account = await _context.Accounts.FindAsync(request.FromAccountId);
        
        if (account == null)
        {
            throw new Exception("Account does not exist.");
        }
        
        if (account.Balance < request.Amount)
        {
            throw new Exception("Insufficient funds in the account.");
        }
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            account.Balance -= request.Amount;
            
            var transactionRecord = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = null,
                Amount = request.Amount,
                Description = request.Description,
                TransactionDate = DateTime.UtcNow,
                Status = "Completed"
            };
            
            _context.Transactions.Add(transactionRecord);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return new AccountTransactionResponse
            {
                Success = true,
                TransactionId = transactionRecord.Id,
                AccountId = account.Id,
                newBalance = account.Balance,
                Amount = (int)transactionRecord.Amount,
                TransactionDate = transactionRecord.TransactionDate,
                Message = "Withdrawal completed successfully."
            };
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();
            throw new Exception("The account was modified by another user. Please refresh and try again.");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}