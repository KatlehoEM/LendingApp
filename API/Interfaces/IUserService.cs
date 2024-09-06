using API.Entities;

namespace API.Interfaces;

public interface IUserService
{
    Task<User> RegisterUserAsync(string firstName, string lastName, string email, string password,string confirmPassword, int creditScore,
     Role role, string idNumber, string address, string phoneNumber, string employmentStatus, decimal annualIncome);
    Task<(bool IsValid, string Token, Role role)> LoginAsync(string username, string password);
    Task<User> GetUserByIdAsync(int id);
    Task<User> UpdateUserAsync(User user);
}
