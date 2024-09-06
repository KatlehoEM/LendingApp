using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IRatingRepository
{
    Task<Rating> CreateBorrowerRatingAsync(int lenderId, CreateRatingDto createBorrowerRatingDto);
    Task<Rating> GetBorrowerRatingByIdAsync(int id);
    Task<IEnumerable<Rating>> GetBorrowerRatingsByBorrowerIdAsync(int borrowerId);
    Task<double> GetAverageBorrowerRatingAsync(int borrowerId);
    Task<Rating> UpdateBorrowerRatingAsync(int id, int lenderId, UpdateRatingDto updateBorrowerRatingDto);
}
