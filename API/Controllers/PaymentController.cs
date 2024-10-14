using System.Security.Claims;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IBlockchainService _blockchainService;
         private readonly DataContext _context;

        public PaymentController(IPaymentRepository paymentRepo, DataContext context, IBlockchainService blockchainService)
        {
            _paymentRepo = paymentRepo;
            _blockchainService = blockchainService;
            _context = context;
        }

        [HttpPost("make")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == username);
            var borrowerId = user.Id;

            var result = await _paymentRepo.ProcessPaymentAsync(request);

            if (result.Success)
            {
                await _blockchainService.UpdateReputationScoreAsync(borrowerId);
                return Ok(new { Message = "Payment processed successfully", PaymentId = result.PaymentId });
            }
            else
            {
                return BadRequest(new { Message = "Payment processing failed", Error = result.ErrorMessage });
            }
        }

        [HttpGet("borrower/{borrowerId}")]
        public async Task<IActionResult> GetBorrowerPayments(int borrowerId)
        {
            var payments = await _paymentRepo.GetBorrowerPaymentsAsync(borrowerId);
            return Ok(payments);
        }

        [HttpGet("loan/{loanId}")]
        public async Task<IActionResult> GetLoanPayments(int loanId)
        {
            var payments = await _paymentRepo.GetLoanPaymentsAsync(loanId);
            return Ok(payments);
        }
    }
}
