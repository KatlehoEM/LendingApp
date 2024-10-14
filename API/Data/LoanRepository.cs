using System;
using System.Data;
using System.Transactions;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LoanRepository : ILoanRepository
{
    private readonly DataContext _context;

    public LoanRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Payment>> GetPaymentsForLoanOfferAsync(int loanOfferId)
{
    var loan = await _context.Loans
                             .Include(l => l.Payments)
                             .FirstOrDefaultAsync(l => l.LoanOfferId == loanOfferId);

    if (loan == null)
    {
        throw new Exception("No loan found for this loan offer");
    }

    return loan.Payments;
}



    public async Task<LoanApplication> AcceptLoanApplicationAsync(int loanApplicationId, int lenderId)
    {
        using var transations = await _context.Database.BeginTransactionAsync();

        try
        {
            var loanApplication = await _context.LoanApplications
                                      .Include(la => la.LoanOffer)
                                      .FirstOrDefaultAsync(la => la.Id == loanApplicationId);
            
            if (loanApplication == null)
            {
                throw new Exception("Loan application not found");
            }

            if (loanApplication.LoanOffer.LenderId != lenderId)
            {
                throw new UnauthorizedAccessException("You are not authorized to accept this loan application");
            }

            if(loanApplication.Status != LoanApplicationStatus.Pending){
                throw new InvalidOperationException("This loan application is no longer pending");
            }

            // Update the accepted loan application
            loanApplication.Status = LoanApplicationStatus.Approved;
            loanApplication.AcceptanceDate = DateTime.UtcNow;

            // Reject all other applications for this loan offer
            var otherApplications = await _context.LoanApplications
                            .Where(la => la.LoanOfferId == loanApplication.LoanOfferId && la.Id != loanApplicationId)
                            .ToListAsync();

            foreach(var app in otherApplications)
            {
                app.Status = LoanApplicationStatus.Rejected;
            } 

            loanApplication.LoanOffer.IsActive = true;

            await _context.SaveChangesAsync();
            await transations.CommitAsync();

            return loanApplication;

        }
        catch
        {
            await transations.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<LoanApplication>> GetPendingApplicationsForLoanOfferAsync(int loanOfferId)
    {
        return await _context.LoanApplications
            .Where(la => la.LoanOfferId == loanOfferId && la.Status == LoanApplicationStatus.Pending)
            .ToListAsync();
    }
}
