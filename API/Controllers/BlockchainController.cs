using System.Security.Claims;
using API.BlockchainStructure;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

public class BlockchainController : BaseApiController
{
    private readonly DataContext _context;

    private readonly IBlockchainService _blockchainService;

        public BlockchainController(IBlockchainService blockchainService, DataContext context)
        {
            _blockchainService = blockchainService;
            _context = context;
        }

        [HttpPost("create-wallet")]
        public async Task<ActionResult<Wallet>> CreateWallet(int userId)
        {
            try
            {
                var wallet = await _blockchainService.CreateWalletAsync(userId);
                return Ok(wallet);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-wallet/{userId}")]
        public async Task<ActionResult<string>> GetUserWallet(int userId)
        {
            try
            {
                var walletAddress = await _blockchainService.GetUserWallet(userId);
                return Ok(walletAddress);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("update-reputation")]
        public async Task<ActionResult> UpdateReputationScore(int userId)
        {
            try
            {
                await _blockchainService.UpdateReputationScoreAsync(userId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("reputation-score")]
        public async Task<ActionResult<decimal>> GetReputationScore()
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            try
            {
                var score = await _blockchainService.GetLatestReputationScoreAsync(user.Id);
                return Ok(score);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
}
