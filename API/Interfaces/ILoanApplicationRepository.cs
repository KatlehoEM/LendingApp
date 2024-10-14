using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface ILoanApplicationRepository
    {
        Task<LoanApplication> CreateLoanApplicationAsync(int borrowerId, CreateLoanApplicationDto createLoanApplicationDto);
        Task<LoanApplication> GetLoanApplicationByIdAsync(int id);
        Task<IEnumerable<LoanApplication>> GetLoanApplicationsByBorrowerIdAsync(int borrowerId);
        Task<IList<LoanApplication>> GetLoanApplicationsByLoanOfferIdAsync(int loanOfferId);
        Task<LoanApplication> UpdateLoanApplicationStatusAsync(int id, int lenderId, UpdateLoanApplicationStatusDto updateLoanApplicationStatusDto);
        Task<bool> WithdrawLoanApplicationAsync(int id, int borrowerId);
    }