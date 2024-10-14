using System;
using API.Entities;

namespace API.Interfaces;

public interface ILoanRepository
{
    Task<LoanApplication> AcceptLoanApplicationAsync(int loanApplicationId, int lenderId);
    Task<IEnumerable<LoanApplication>> GetPendingApplicationsForLoanOfferAsync(int loanOfferId);
    Task<IEnumerable<Payment>> GetPaymentsForLoanOfferAsync(int loanOfferId);

}
