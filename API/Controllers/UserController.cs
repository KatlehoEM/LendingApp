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
                registerDto.Role,
                registerDto.Password,
                registerDto.ConfirmPassword,
                registerDto.DateOfBirth,
                registerDto.IdNumber,
                registerDto.StreetNumber,
                registerDto.StreetName,
                registerDto.City,
                registerDto.PostalCode,
                registerDto.PhoneNumber,
                registerDto.EmploymentStatus,
                registerDto.AnnualIncome,
                registerDto.CreditScore
            );
            
            await _blockchainService.CreateReputationScoreAsync(user.Id);
            
            return Ok(user);
                
        }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Registration failed", error = ex.Message });
            }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var (isValid, token, role, firstName) = await _userService.LoginAsync(loginDto.Email, loginDto.Password);
        if (isValid)
        {
            return Ok(new { firstname = firstName, token = token, role = role });
        }
        return Unauthorized(new { message = "Invalid username or password" });
    }
}

