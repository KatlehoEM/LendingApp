using API.Entities;

namespace API.Interfaces;

public interface IUserService
{
    Task<User> RegisterUserAsync(string firstName, string lastName, string email, Role role, string password,string confirmPassword, DateTime dateOfBirth,
      string idNumber, string address, string phoneNumber, string employmentStatus, decimal annualIncome, int creditScore);
    Task<(bool IsValid, string Token, Role role)> LoginAsync(string username, string password);
    Task<User> GetUserByIdAsync(int id);
    Task<User> UpdateUserAsync(User user);
}
