namespace BankingAPI.Controllers;
using BankingAPI.Services;
using BankingAPI.DTOs.Requests;
using BankingAPI.DTOs.Responses;
using BankingAPI.Models;
using Microsoft.AspNetCore.Mvc;

// we need POST /api/transactions/transfer
[ApiController]
[Route("api/[controller]")] // it removes Controller from TransactionsController and places in this placeholder
public class TransactionsController : ControllerBase
{
    
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    [HttpPost("transfer")] // POST api/transactions/transfer   // IActionResult - interface that return different types of HTTP responses
    public async Task<IActionResult> TransferMoney([FromBody] TransferRequest request)
    {
        try
        {
            var response = await _transactionService.TransferMoney(request); // we wait for response from the TransactionService
            return Ok(response); // 200 OK with the response data
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }  
    }
    [HttpPost("deposit")]
    public async Task<IActionResult> DepositMoney([FromBody] DepositRequest request)
    {
        try
        {
            var response = await _transactionService.Deposit(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> WithdrawMoney([FromBody] WithdrawRequest request)
    {
        try
        {
            var response = await _transactionService.Withdraw(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


}