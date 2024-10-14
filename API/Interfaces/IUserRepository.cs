using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User> CreateUserAsync(string firstName, string lastName, string email, Role role, string password,string confirmPassword, DateTime dateOfBirth,
      string idNumber,string streetNumber, string streetName, string city, string postalCode, string phoneNumber, string employmentStatus, decimal annualIncome, int creditScore, string walletAddress);
    
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(int id);
    Task<bool> ValidateUserAsync(string email, string password);
    Task<List<Rating>> GetUserRatingsAsync(int userId);
    Task AddRatingAsync(Rating rating);
    Task<User> UpdateUserAsync(User user);
}
