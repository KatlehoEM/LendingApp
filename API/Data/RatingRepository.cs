using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class RatingRepository : IRatingRepository 
{
    private readonly DataContext _context;

    public RatingRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Rating> CreateBorrowerRatingAsync(int lenderId, CreateRatingDto createBorrowerRatingDto)
        {
            var loanApplication = await _context.LoanApplications
                .Include(la => la.LoanOffer)
                .FirstOrDefaultAsync(la => la.Id == createBorrowerRatingDto.LoanApplicationId);

            if (loanApplication == null || loanApplication.LoanOffer.LenderId != lenderId)
            {
                throw new ApplicationException("Invalid loan application or you don't have permission to rate this borrower");
            }

            if (loanApplication.Status != LoanApplicationStatus.Approved)
            {
                throw new ApplicationException("Can only rate borrowers for approved loan applications");
            }

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(br => br.LoanApplicationId == createBorrowerRatingDto.LoanApplicationId);

            if (existingRating != null)
            {
                throw new ApplicationException("A rating for this loan application already exists");
            }

            var borrowerRating = new Rating
            {
                LenderId = lenderId,
                BorrowerId = loanApplication.BorrowerId,
                LoanApplicationId = createBorrowerRatingDto.LoanApplicationId,
                Score = createBorrowerRatingDto.Score
            };

            await _context.Ratings.AddAsync(borrowerRating);
            await _context.SaveChangesAsync();

            return borrowerRating;
        }

        public async Task<Rating> GetBorrowerRatingByIdAsync(int id)
        {
            return await _context.Ratings
                .Include(br => br.Lender)
                .Include(br => br.Borrower)
                .Include(br => br.LoanApplication)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task<IEnumerable<Rating>> GetRatingsByBorrowerIdAsync(int borrowerId)
        {
            return await _context.Ratings
                .Where(br => br.BorrowerId == borrowerId)
                .Include(br => br.Lender)
                .Include(br => br.LoanApplication)
                .ToListAsync();
        }

        public async Task<double> GetAverageBorrowerRatingAsync(int borrowerId)
        {
            var ratings = await _context.Ratings
                .Where(br => br.BorrowerId == borrowerId)
                .ToListAsync();

            if (!ratings.Any())
            {
                return 0;
            }

            return ratings.Average(r => r.Score);
        }

        public async Task<Rating> UpdateBorrowerRatingAsync(int id, int lenderId, UpdateRatingDto updateBorrowerRatingDto)
        {
            var borrowerRating = await _context.Ratings
                .FirstOrDefaultAsync(br => br.Id == id && br.LenderId == lenderId);

            if (borrowerRating == null)
            {
                throw new ApplicationException("Borrower rating not found or you don't have permission to update it");
            }

            if (updateBorrowerRatingDto.Score != 0)
            {
                borrowerRating.Score = updateBorrowerRatingDto.Score;
            }

            borrowerRating.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return borrowerRating;
        }

    public async Task<IEnumerable<Rating>> GetBorrowerRatingsByBorrowerIdAsync(int borrowerId)
    {
        return await _context.Ratings
                .Where(br => br.BorrowerId == borrowerId)
                .Include(br => br.Lender)
                .Include(br => br.LoanApplication)
                .ToListAsync();
    }
}
