using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly DataContext _context;

    public LoanApplicationRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<LoanApplication> CreateLoanApplicationAsync(int borrowerId, CreateLoanApplicationDto createLoanApplicationDto)
    {
        var loanOffer = await _context.LoanOffers.FindAsync(createLoanApplicationDto.LoanOfferId);
        if(loanOffer == null || !loanOffer.IsActive)
        {
            throw new ApplicationException("Invalid or inactive loan offer");
        }

        var loanApplication = new LoanApplication
        {
            LoanOfferId = createLoanApplicationDto.LoanOfferId,
            BorrowerId = borrowerId,
            Status = LoanApplicationStatus.Pending,
        };

        await _context.LoanApplications.AddAsync(loanApplication);
        await _context.SaveChangesAsync();

        return loanApplication;
    }

    public async Task<LoanApplication> GetLoanApplicationByIdAsync(int id)
    {
        return await _context.LoanApplications
                .Include(la => la.LoanOffer)
                .Include(la => la.Borrower)
                .FirstOrDefaultAsync(la => la.Id == id);
    }

    public async Task<IEnumerable<LoanApplication>> GetLoanApplicationsByBorrowerIdAsync(int borrowerId)
    {
        return await _context.LoanApplications
                .Where(la => la.BorrowerId == borrowerId)
                .Include(la => la.LoanOffer)
                .ToListAsync();
    }

    public async Task<IEnumerable<LoanApplication>> GetLoanApplicationsByLoanOfferIdAsync(int loanOfferId)
    {
        return await _context.LoanApplications
                .Where(la => la.LoanOfferId == loanOfferId)
                .Include(la => la.Borrower)
                .ToListAsync();
    }

    public async Task<LoanApplication> UpdateLoanApplicationStatusAsync(int id, int lenderId, UpdateLoanApplicationStatusDto updateLoanApplicationStatusDto)
    {
        var loanApplication = await _context.LoanApplications
                .Include(la => la.LoanOffer)
                .FirstOrDefaultAsync(la => la.Id == id && la.LoanOffer.LenderId == lenderId);

            if (loanApplication == null)
            {
                throw new ApplicationException("Loan application not found or you don't have permission to update it");
            }

            if (loanApplication.Status != LoanApplicationStatus.Pending)
            {
                throw new ApplicationException("Can only update pending loan applications");
            }

            loanApplication.Status = updateLoanApplicationStatusDto.Status;
            loanApplication.UpdatedAt = DateTime.UtcNow;

            if (updateLoanApplicationStatusDto.Status == LoanApplicationStatus.Approved)
            {
                // Mark other applications for this loan offer as rejected
                var otherApplications = await _context.LoanApplications
                    .Where(la => la.LoanOfferId == loanApplication.LoanOfferId && la.Id != loanApplication.Id)
                    .ToListAsync();

                foreach (var app in otherApplications)
                {
                    app.Status = LoanApplicationStatus.Rejected;
                    app.UpdatedAt = DateTime.UtcNow;
                }

                // Mark the loan offer as inactive
                loanApplication.LoanOffer.IsActive = false;
                loanApplication.LoanOffer.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return loanApplication;
    }

    public async Task<bool> WithdrawLoanApplicationAsync(int id, int borrowerId)
    {
        var loanApplication = await _context.LoanApplications
                .FirstOrDefaultAsync(la => la.Id == id && la.BorrowerId == borrowerId);

            if (loanApplication == null)
            {
                return false;
            }

            if (loanApplication.Status != LoanApplicationStatus.Pending)
            {
                throw new ApplicationException("Can only withdraw pending loan applications");
            }

            loanApplication.Status = LoanApplicationStatus.Withdrawn;
            loanApplication.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
    }
}
