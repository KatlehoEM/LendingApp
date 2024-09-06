using System;

namespace API.Entities;

public class LoanOffer
{
   public int Id { get; set; }
    public int LenderId { get; set; }
    public User Lender {get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int DurationInMonths { get; set; }
    public bool IsActive {get; set; }
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;



}

