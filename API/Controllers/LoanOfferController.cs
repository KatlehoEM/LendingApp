using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LoanOfferController : BaseApiController
    {
        private readonly ILoanOfferRepository _loanOfferRepo;

        public LoanOfferController(ILoanOfferRepository loanOfferRepo)
        {
            _loanOfferRepo = loanOfferRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> CreateLoanOffer(LoanOfferDto loanOfferDto)
        {
            var lenderId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
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
            var loanOffers = await _loanOfferRepo.GetActiveLoanOffersAsync();
            return Ok(loanOffers);
        }

        [HttpGet("myloanoffers")]
        [Authorize(Roles = "Lender")]
        public async Task<ActionResult<IEnumerable<LoanOffer>>> GetMyLoanOffers()
        {
            var lenderId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var loanOffers = await _loanOfferRepo.GetLoanOffersByLenderIdAsync(lenderId);
            return Ok(loanOffers);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> UpdateLoanOffer(int id, UpdateLoanOfferDto updateLoanOfferDto)
        {
            var lenderId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
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
            var lenderId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
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
