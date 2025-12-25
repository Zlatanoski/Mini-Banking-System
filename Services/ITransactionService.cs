using BankingAPI.DTOs.Requests;
using BankingAPI.DTOs.Responses;

namespace BankingAPI.Services;

public interface ITransactionService
{
    Task<TransferResponse> TransferMoney(TransferRequest request);
    
    Task<TransactionResponse?> GetTransactionById(int TransactionId);
    
    Task<List<TransactionResponse>> GetTransactionsByAccountId(int accountId);
    
    Task<AccountTransactionResponse> Deposit(DepositRequest request);
    
    Task<AccountTransactionResponse> Withdraw(WithdrawRequest request);
}