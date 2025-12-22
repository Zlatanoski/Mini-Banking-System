// all operations on Accounts 

namespace BankingAPI.Controllers;
using BankingAPI.Services;
using BankingAPI.Models;
using BankingAPI.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly  IAccountService _service; // interface 

    public  AccountsController(IAccountService service)
    {
        _service = service;
    
    }

    [HttpPost] // POST api/accounts
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        try
        {
            var response = await _service.CreateAccount(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
   





}