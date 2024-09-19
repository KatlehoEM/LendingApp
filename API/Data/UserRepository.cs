using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
   }

    public async Task<User> CreateUserAsync(string firstName, string lastName, string email, Role role, string password,string confirmPassword, DateTime dateOfBirth,
      string idNumber, string address, string phoneNumber, string employmentStatus, decimal annualIncome, int creditScore, string walletAddress)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = HashPassword(password),
            CreditScore = creditScore,
            WalletAddress = walletAddress,
            IdNumber = idNumber,
            Address = address,
            PhoneNumber = phoneNumber,
            EmploymentStatus = employmentStatus,
            AnnualIncome = annualIncome,
            Role = role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUserByUsernameAsync(string firstName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.FirstName == firstName);
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> ValidateUserAsync(string fullname, string password)
    {
        var user = await GetUserByUsernameAsync(fullname);
        if (user == null)
            return false;

        return VerifyPassword(password, user.PasswordHash);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }

    public async Task<List<Rating>> GetUserRatingsAsync(int userId)
    {
        return await _context.Ratings.Where(r => r.BorrowerId == userId).ToListAsync();  
    }

    public async Task AddRatingAsync(Rating rating)
    {
        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
}
