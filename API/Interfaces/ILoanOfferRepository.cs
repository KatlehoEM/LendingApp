using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface ILoanOfferRepository
{
    Task<LoanOffer> CreateLoanOfferAsync(int lenderId, LoanOfferDto loanOfferDto);
    Task<LoanOffer> GetLoanOfferByIdAsync(int id);
    Task<IEnumerable<LoanOffer>> GetActiveLoanOffersAsync();
    Task<IEnumerable<LoanOffer>> GetLoanOffersByLenderIdAsync(int lenderId);
    Task<LoanOffer> UpdateLoanOfferAsync(int id, int lenderId, UpdateLoanOfferDto updateLoanOfferDto);
    Task DeleteLoanOfferAsync(int id, int lenderId);

}