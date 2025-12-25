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
        // validate the accounts in the request exist
        var fromAccount = await _context.Accounts.FindAsync(request.FromAccountId);
        var toAccount = await _context.Accounts.FindAsync(request.ToAccountId);

        if(fromAccount == null)
        {
            throw new Exception("Sender account does not exist.");
        }
        if(toAccount == null)
        {
            throw new Exception("Recipient account does not exist.");
        }
        if(request.Amount <= 0)
        {
            throw new Exception("Transfer amount must be greater than zero.");
        }
        if(fromAccount.Balance < request.Amount)
        {
            throw new Exception("Insufficient funds in the sender's account.");
        }
        if(request.FromAccountId == request.ToAccountId)
        {
            throw new Exception("Cannot transfer to the same account.");
        }
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;

            var TransactionRecord = new Transaction
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                Description = request.Description,
                TransactionDate = DateTime.UtcNow
            };
            _context.Transactions.Add(TransactionRecord); // add transaction to the transaction table
            await _context.SaveChangesAsync(); 
            await transaction.CommitAsync();

            return new TransferResponse
            {
                TransactionId = TransactionRecord.Id,
                FromAccountId = fromAccount.Id,
                ToAccountId = toAccount.Id,
                Amount = TransactionRecord.Amount,
                TransactionDate = TransactionRecord.TransactionDate,
                Success = true,
                FromAccountNewBalance = fromAccount.Balance,
                ToAccountNewBalance = toAccount.Balance,
                Message = "Transfer completed successfully."
               
            };
            
        }catch(Exception)
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
        else
        {
            var response = new TransactionResponse
            {
                Id = transaction.Id,
                FromAccountId = transaction.FromAccountId,
                ToAccountId = transaction.ToAccountId,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Description = transaction.Description,
                Status = transaction.Status
            };
            return response;
        }
    }
    
    public async Task<List<TransactionResponse>> GetTransactionsByAccountId(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null){
            throw new Exception("Account does not exist.");
        }
        else
        {
            var transactions = await _context.Transactions.Where(t=> t.FromAccountId == accountId || t.ToAccountId == accountId).ToListAsync();
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
    }
}