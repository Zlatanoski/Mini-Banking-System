//interface for account service 
using BankingAPI.DTOs.Requests;


namespace BankingAPI.Services;


public interface IAccountService
{
    // in Data Transfer Object we have clearly defined how request and response should look when creating an account
Task<AccountResponse> CreateAccount(CreateAccountRequest request); // we receive an object of the CreateAccountRequest type and return an object of AccountResponse type
Task<List<AccountResponse>> GetAllAccounts();
Task<AccountResponse?> GetAccountById(int id);

}