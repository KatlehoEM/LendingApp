using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentRepository _paymentRepo;

        public PaymentController(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        [HttpPost("make")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _paymentRepo.ProcessPaymentAsync(request);

            if (result.Success)
            {
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
