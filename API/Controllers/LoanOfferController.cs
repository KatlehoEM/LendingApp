using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class LoanOfferController : BaseApiController
    {
        private readonly ILoanOfferRepository _loanOfferRepo;
        private readonly DataContext _context;
        public LoanOfferController(ILoanOfferRepository loanOfferRepo, DataContext context)
        {
            _loanOfferRepo = loanOfferRepo;
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> CreateLoanOffer(LoanOfferDto loanOfferDto)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var lenderId = user.Id;
            var loanOffer = await _loanOfferRepo.CreateLoanOfferAsync(lenderId, loanOfferDto);
            return CreatedAtAction(nameof(GetLoanOffer), new { id = loanOffer.Id }, loanOffer);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanOffer(int id)
        {
            var loanOffer = await _loanOfferRepo.GetLoanOfferByIdAsync(id);
            if (loanOffer == null)
            {
                return NotFound();
            }
            return Ok(loanOffer);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LoanOffer>>> GetActiveLoanOffers()
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            int borrowerId = user.Id;
            var loanOffers = await _loanOfferRepo.GetActiveLoanOffersAsync(borrowerId);
            return Ok(loanOffers);
        }

        [HttpGet("myloanoffers")]
        [Authorize(Roles = "Lender")]
        public async Task<ActionResult<IEnumerable<LoanOffer>>> GetMyLoanOffers()
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var loanOffers = await _loanOfferRepo.GetLoanOffersByLenderIdAsync(user.Id);
            return Ok(loanOffers);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> UpdateLoanOffer(int id, UpdateLoanOfferDto updateLoanOfferDto)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var lenderId = user.Id;
            try
            {
                var updatedLoanOffer = await _loanOfferRepo.UpdateLoanOfferAsync(id, lenderId, updateLoanOfferDto);
                return Ok(updatedLoanOffer);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> DeleteLoanOffer(int id)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var lenderId = user.Id;
            try
            {
                await _loanOfferRepo.DeleteLoanOfferAsync(id, lenderId);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
