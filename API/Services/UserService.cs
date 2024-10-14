using API.Entities;
using API.Interfaces;

namespace API.Services;


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<User> RegisterUserAsync(string firstName, string lastName, string email, Role role, string password,string confirmPassword, DateTime dateOfBirth,
      string idNumber, string streetNumber, string streetName, string city, string postalCode, string phoneNumber, string employmentStatus, decimal annualIncome, int creditScore)
    {
         // Add validation logic here (e.g., password strength, email format, credit score range)
        if (creditScore < 300 || creditScore > 850)
        {
            throw new ArgumentException("Credit score must be between 300 and 850.");
        }

        return await _userRepository.CreateUserAsync(firstName, lastName, email, role, password, confirmPassword, dateOfBirth,
      idNumber, streetNumber, streetName, city, postalCode, phoneNumber, employmentStatus,  annualIncome,creditScore, "");
    
    }

    public async Task<(bool IsValid, string Token, Role role, string firstName)> LoginAsync(string email, string password)
    {
        var isValid = await _userRepository.ValidateUserAsync(email, password);
        if (!isValid)
        {
            return (false, null, 0,"");
        }

        var user = await _userRepository.GetUserByEmailAsync(email);
        var token = _jwtService.GenerateToken(user);
        return (true, token, user.Role, user.FirstName);
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task AddRatingAsync(int lenderId, int borrowerId, int score)
    {
        if(score < 1 || score > 5)
        {
            throw new ArgumentException("Score must be between 1 and 5");
        }

        var rating = new Rating
        {
            LenderId= lenderId,
            BorrowerId = borrowerId,
            Score = score,
            CreatedAt = DateTime.UtcNow,
        };

        await _userRepository.AddRatingAsync(rating);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
       return await _userRepository.UpdateUserAsync(user);

    }
    
}