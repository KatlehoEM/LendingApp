using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PaymentRepository : IPaymentRepository
{
    private readonly DataContext _context;

    public PaymentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Payment>> GetBorrowerPaymentsAsync(int borrowerId)
    {
        return await _context.Payments
                .Include(p => p.Loan)
                .Where(p => p.Loan.BorrowerId == borrowerId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetLoanPaymentsAsync(int loanId)
    {
        return await _context.Payments
                .Where(p => p.LoanId == loanId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var loan = await _context.Loans
                    .Include(l => l.Payments)
                    .Include(l => l.LoanOffer)
                    .FirstOrDefaultAsync(l => l.LoanOfferId == request.LoanOfferId);

                if (loan == null)
                {
                    return new PaymentResult { Success = false, ErrorMessage = "Loan not found" };
                }

                if (loan.RemainingBalance < request.Amount)
                {
                    return new PaymentResult { Success = false, ErrorMessage = "Payment amount exceeds remaining balance" };
                }

                var payment = new Payment
                {
                    LoanId = loan.Id,
                    Amount = request.Amount,
                    PaymentDate = DateTime.UtcNow
                };

                _context.Payments.Add(payment);
                loan.RemainingBalance -= request.Amount;

                // Check if the loan is fully paid
                if (loan.RemainingBalance <= 0)
                {
                    loan.Status = LoanStatus.Paid;
                    loan.LoanOffer.IsActive = false;
                    loan.LoanOffer.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new PaymentResult { Success = true, PaymentId = payment.Id };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new PaymentResult { Success = false, ErrorMessage = ex.Message };
            }

    }
}
