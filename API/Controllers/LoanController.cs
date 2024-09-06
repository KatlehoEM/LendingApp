using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LoanController : BaseApiController
    {
        private readonly ILoanRepository _loanRepo;

        public LoanController(ILoanRepository loanRepository)
        {
            _loanRepo = loanRepository;
        }

        [HttpPost("accept/{loanApplicationId}")]
        public async Task<ActionResult<LoanApplication>> AcceptLoanApplication(int loanApplicationId)
        {
            var lenderId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            try
        {
            var acceptedApplication = await _loanRepo.AcceptLoanApplicationAsync(loanApplicationId, lenderId);
            return Ok(acceptedApplication);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
        
        }

        [HttpGet("pending/{loanOfferId}")]
    public async Task<ActionResult<IEnumerable<LoanApplication>>> GetPendingApplications(int loanOfferId)
    {
        var pendingApplications = await _loanRepo.GetPendingApplicationsForLoanOfferAsync(loanOfferId);
        return Ok(pendingApplications);
    }

    }
}
