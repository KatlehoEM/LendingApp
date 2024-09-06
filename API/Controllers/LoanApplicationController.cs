using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [Authorize]
    public class LoanApplicationController : BaseApiController
    {
        private readonly ILoanApplicationRepository _loanApplicationRepo;
        
        public LoanApplicationController(ILoanApplicationRepository loanApplicationRepository)
        {
            _loanApplicationRepo = loanApplicationRepository;
        }

         [HttpPost]
        [Authorize(Roles = "Borrower")]
        public async Task<IActionResult> CreateLoanApplication(CreateLoanApplicationDto createLoanApplicationDto)
        {
            var borrowerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var loanApplication = await _loanApplicationRepo.CreateLoanApplicationAsync(borrowerId, createLoanApplicationDto);
                return CreatedAtAction(nameof(GetLoanApplication), new { id = loanApplication.Id }, loanApplication);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanApplication(int id)
        {
            var loanApplication = await _loanApplicationRepo.GetLoanApplicationByIdAsync(id);
            if (loanApplication == null)
            {
                return NotFound();
            }
            return Ok(loanApplication);
        }

        [HttpGet("borrower")]
        [Authorize(Roles = "Borrower")]
        public async Task<ActionResult<IEnumerable<LoanApplication>>> GetBorrowerLoanApplications()
        {
            var borrowerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var loanApplications = await _loanApplicationRepo.GetLoanApplicationsByBorrowerIdAsync(borrowerId);
            return Ok(loanApplications);
        }

        [HttpGet("lender/{loanOfferId}")]
        [Authorize(Roles = "Lender")]
        public async Task<ActionResult<IEnumerable<LoanApplication>>> GetLoanOfferApplications(int loanOfferId)
        {
            var loanApplications = await _loanApplicationRepo.GetLoanApplicationsByLoanOfferIdAsync(loanOfferId);
            return Ok(loanApplications);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> UpdateLoanApplicationStatus(int id, UpdateLoanApplicationStatusDto updateLoanApplicationStatusDto)
        {
            var lenderId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var updatedLoanApplication = await _loanApplicationRepo.UpdateLoanApplicationStatusAsync(id, lenderId, updateLoanApplicationStatusDto);
                return Ok(updatedLoanApplication);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/withdraw")]
        [Authorize(Roles = "Borrower")]
        public async Task<IActionResult> WithdrawLoanApplication(int id)
        {
            var borrowerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var result = await _loanApplicationRepo.WithdrawLoanApplicationAsync(id, borrowerId);
                if (result)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
