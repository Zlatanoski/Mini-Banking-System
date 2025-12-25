namespace BankingAPI.Services;
using BankingAPI.DTOs.Requests;
using BankingAPI.DTOs.Responses;
using BankingAPI.Models;

public interface ITransactionService
{
    
    Task<TransferResponse> TransferMoney(TransferRequest request);

    Task<TransactionResponse?> GetTransactionById(int TransactionId);

    Task<List<TransactionResponse>> GetTransactionsByAccountId(int accountId);

}