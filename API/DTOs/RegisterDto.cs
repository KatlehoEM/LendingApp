using API.Entities;

namespace API.DTOs;

public class RegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public int CreditScore { get; set; }
    public Role Role { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string IdNumber { get; set; }
    public string StreetNumber { get; set; } 
    public string StreetName { get; set; } 
    public string City { get; set; } 
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public string EmploymentStatus { get; set; }
    public decimal AnnualIncome { get; set; }
    public bool termsAndConditions {get;set;}
}
