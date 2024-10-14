using System;

namespace API.DTOs;

public class UpdateLoanOfferDto
{
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int DurationInYears { get; set; }
    public decimal MonthlyRepayment {get;set;}
    public decimal TotalRepayment {get;set;}


    public bool IsActive { get; set; } = true;
}
