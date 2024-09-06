using API.Controllers;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly IBlockchainService _blockchainService;

    public UserController(IUserService userService, IBlockchainService blockchainService)
    {
        _userService = userService;
        _blockchainService = blockchainService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var user = await _userService.RegisterUserAsync(
                registerDto.FirstName,
                registerDto.LastName, 
                registerDto.Email, 
                registerDto.Password,
                registerDto.ConfirmPassword,
                registerDto.CreditScore, 
                registerDto.Role,
                registerDto.IdNumber,
                registerDto.Address,
                registerDto.PhoneNumber,
                registerDto.EmploymentStatus,
                registerDto.AnnualIncome
            );
            var wallet = await _blockchainService.CreateWalletAsync(user.Id);
            user.WalletAddress = wallet.Address;

            await _userService.UpdateUserAsync(user);
            

            return Ok(new { message = "User registered successfully", userId = user.Id, walletAddress = user.WalletAddress });
       }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Registration failed", error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var (isValid, token, role) = await _userService.LoginAsync(loginDto.FirstName, loginDto.Password);
        if (isValid)
        {
            return Ok(new { user = loginDto.FirstName, token = token, role = role });
        }
        return Unauthorized(new { message = "Invalid username or password" });
    }
}

