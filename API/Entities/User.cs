namespace API.Entities;

// Models/User.cs
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string WalletAddress { get; set; }
    public int CreditScore { get; set; }

    public string IdNumber { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string EmploymentStatus { get; set; }
    public decimal AnnualIncome { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<LoanOffer> LoanOffers { get; set; }
    public List<Rating> Ratings { get; set; } = new List<Rating>();
}
