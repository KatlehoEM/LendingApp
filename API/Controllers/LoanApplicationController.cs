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
    public class LoanApplicationController : BaseApiController
    {
        private readonly ILoanApplicationRepository _loanApplicationRepo;
        private readonly ILoanOfferRepository _loanOfferRepo;
         private readonly IBlockchainService _blockchainService;
         private readonly DataContext _context;
        public LoanApplicationController(ILoanOfferRepository loanOfferRepo, ILoanApplicationRepository loanApplicationRepository, DataContext context, IBlockchainService blockchainService)
        {
            _loanOfferRepo = loanOfferRepo;
            _loanApplicationRepo = loanApplicationRepository;
            _blockchainService = blockchainService;
            _context = context;
        }

         [HttpPost]
        [Authorize(Roles = "Borrower")]
        public async Task<IActionResult> CreateLoanApplication(CreateLoanApplicationDto createLoanApplicationDto)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var borrowerId = user.Id;
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

            // Get the reputation score for the borrower
                var borrowerReputationScore = await _blockchainService.GetLatestReputationScoreAsync(loanApplication.Borrower.Id);
                var loanOffer =  await _loanOfferRepo.GetLoanOfferByIdAsync(loanApplication.LoanOfferId);

            var dto = new LoanApplicationDto
                {
                    Id = loanApplication.Id,
                    BorrowerId = loanApplication.BorrowerId,
                    BorrowerName = $"{loanApplication.Borrower.FirstName ?? "N/A"} {loanApplication.Borrower.LastName ?? "N/A"}", // Handle null names
                    BorrowerReputationScore = borrowerReputationScore,
                    LoanOfferAmount = loanOffer.PrincipalAmount,
                    CreditScore = loanApplication.Borrower.CreditScore,
                    LoanOfferInterestRate = loanOffer.InterestRate,
                    LoanOfferDuration = loanOffer.DurationInYears,
                    MonthlyRepayment = loanOffer.MonthlyRepayment,
                    TotalRepayment = loanOffer.TotalRepayment,
                    ApplicationStatus = loanApplication.Status,
                    Borrower = loanApplication.Borrower,
                    Payments = loanApplication.Payments,
                    LoanOfferId = loanOffer.Id
                };
            return Ok(dto);
        }

        [HttpGet("borrower")]
        [Authorize(Roles = "Borrower")]
        public async Task<ActionResult<IEnumerable<LoanApplication>>> GetBorrowerLoanApplications()
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var borrowerId = user.Id;
            var loanApplications = await _loanApplicationRepo.GetLoanApplicationsByBorrowerIdAsync(borrowerId);
            return Ok(loanApplications);
        }

       [HttpGet("lender/{loanOfferId}")]
        [Authorize(Roles = "Lender")]
        public async Task<ActionResult<IList<LoanApplicationDto>>> GetLoanOfferApplications(int loanOfferId)
        {
            var loanApplications = await _loanApplicationRepo.GetLoanApplicationsByLoanOfferIdAsync(loanOfferId);

        

            var loanApplicationDtos = new List<LoanApplicationDto>();

            // Loop through all loan applications
            foreach (var application in loanApplications)
            {
            

                // Get the reputation score for the borrower
                var borrowerReputationScore = await _blockchainService.GetLatestReputationScoreAsync(application.Borrower.Id);
                var loanOffer =  await _loanOfferRepo.GetLoanOfferByIdAsync(application.LoanOfferId);

                // Create the DTO for each loan application
                var dto = new LoanApplicationDto
                {
                    Id = application.Id,
                    BorrowerId = application.BorrowerId,
                    BorrowerName = $"{application.Borrower.FirstName ?? "N/A"} {application.Borrower.LastName ?? "N/A"}", // Handle null names
                    BorrowerReputationScore = borrowerReputationScore,
                    LoanOfferAmount = loanOffer.PrincipalAmount,
                    CreditScore = application.Borrower.CreditScore,
                    LoanOfferInterestRate = loanOffer.InterestRate,
                    LoanOfferDuration = loanOffer.DurationInYears,
                    MonthlyRepayment = loanOffer.MonthlyRepayment,
                    TotalRepayment = loanOffer.TotalRepayment,
                    ApplicationStatus = application.Status,
                    Borrower = application.Borrower,
                    Payments = application.Payments,
                    LoanOfferId = loanOffer.Id
                };

                // Add the DTO to the result list
                loanApplicationDtos.Add(dto);
            }

            return Ok(loanApplicationDtos);
        }


        [HttpPut("{id}/status")]
        [Authorize(Roles = "Lender")]
        public async Task<IActionResult> UpdateLoanApplicationStatus(int id, UpdateLoanApplicationStatusDto updateLoanApplicationStatusDto)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var application = await _loanApplicationRepo.GetLoanApplicationByIdAsync(id);
            var borrowerId = application.Borrower.Id;
            var lenderId = user.Id;
            try
            {
                var updatedLoanApplication = await _loanApplicationRepo.UpdateLoanApplicationStatusAsync(id, lenderId, updateLoanApplicationStatusDto);
                await _blockchainService.UpdateReputationScoreAsync(borrowerId);
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
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var borrowerId = user.Id;
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
