using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User> CreateUserAsync(string firstName, string lastName, string email, Role role, string password,string confirmPassword, DateTime dateOfBirth,
      string idNumber, string address, string phoneNumber, string employmentStatus, decimal annualIncome, int creditScore, string walletAddress);
    
    Task<User> GetUserByUsernameAsync(string fullname);
    Task<User> GetUserByIdAsync(int id);
    Task<bool> ValidateUserAsync(string fullname, string password);
    Task<List<Rating>> GetUserRatingsAsync(int userId);
    Task AddRatingAsync(Rating rating);
    Task<User> UpdateUserAsync(User user);
}
