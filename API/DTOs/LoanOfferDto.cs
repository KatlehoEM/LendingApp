using API.Entities;

namespace API.DTOs;

public class LoanOfferDto
{
    public int Id { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int DurationInMonths { get; set; }

    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}
