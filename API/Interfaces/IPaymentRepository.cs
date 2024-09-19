using System;
using API.Entities;

namespace API.Interfaces;

public interface IPaymentRepository
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        Task<IEnumerable<Payment>> GetBorrowerPaymentsAsync(int borrowerId);
        Task<IEnumerable<Payment>> GetLoanPaymentsAsync(int loanId);

}
