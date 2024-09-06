using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LoanOfferRepository : ILoanOfferRepository
{

    private readonly DataContext _context;

    public LoanOfferRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<LoanOffer> CreateLoanOfferAsync(int lenderId, LoanOfferDto loanOfferDto)
    {
        var loanOffer = new LoanOffer
        {
            LenderId = lenderId,
            PrincipalAmount = loanOfferDto.PrincipalAmount,
            DurationInMonths = loanOfferDto.DurationInMonths,
            InterestRate = loanOfferDto.InterestRate,
        };

        await _context.LoanOffers.AddAsync(loanOffer);
        await _context.SaveChangesAsync();

        return loanOffer;
    }

    public async Task DeleteLoanOfferAsync(int id, int lenderId)
    {
        var loanOffer = await _context.LoanOffers
                .FirstOrDefaultAsync(lo => lo.Id == id && lo.LenderId == lenderId);

        if (loanOffer == null)
        {
            throw new ApplicationException("Loan offer not found or you don't have permission to delete it");
        }

        _context.LoanOffers.Remove(loanOffer);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<LoanOffer>> GetActiveLoanOffersAsync()
    {
        return await _context.LoanOffers
            .Where(lo => lo.IsActive)
            .Include(lo => lo.Lender)
            .ToListAsync();
    }

    public async Task<LoanOffer> UpdateLoanOfferAsync(int id, int lenderId, UpdateLoanOfferDto updateLoanOfferDto)
    {
        var loanOffer = await _context.LoanOffers
            .FirstOrDefaultAsync(lo => lo.Id == id && lo.LenderId == lenderId);

        if (loanOffer == null)
        {
            throw new ApplicationException("Loan offer not found or you don't have permission to update it");
        }

        if (updateLoanOfferDto.PrincipalAmount != 0m)
            loanOffer.PrincipalAmount = updateLoanOfferDto.PrincipalAmount;
        if (updateLoanOfferDto.DurationInMonths != 0)
            loanOffer.DurationInMonths = updateLoanOfferDto.DurationInMonths;
        if (updateLoanOfferDto.InterestRate != 0m)
            loanOffer.InterestRate = updateLoanOfferDto.InterestRate;
        if (updateLoanOfferDto.IsActive)
            loanOffer.IsActive = updateLoanOfferDto.IsActive;

        loanOffer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return loanOffer;
    }


    public async Task<LoanOffer> GetLoanOfferByIdAsync(int id)
    {
        return await _context.LoanOffers.FindAsync(id);
    }

    public async Task<IEnumerable<LoanOffer>> GetLoanOffersByLenderIdAsync(int lenderId)
    {
        return await _context.LoanOffers
                    .Where(lo => lo.LenderId == lenderId)
                    .ToListAsync();
    }
}
