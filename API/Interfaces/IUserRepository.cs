using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User> CreateUserAsync(string firstName, string lastName, string email, string password,string confirmPassword, int creditScore,
     Role role, string walletAddress, string idNumber, string address, string phoneNumber, string employmentStatus, decimal annualIncome);
    
    Task<User> GetUserByUsernameAsync(string fullname);
    Task<User> GetUserByIdAsync(int id);
    Task<bool> ValidateUserAsync(string fullname, string password);
    Task<List<Rating>> GetUserRatingsAsync(int userId);
    Task AddRatingAsync(Rating rating);
    Task<User> UpdateUserAsync(User user);
}
