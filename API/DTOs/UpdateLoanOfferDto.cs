using System;

namespace API.DTOs;

public class UpdateLoanOfferDto
{
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int DurationInMonths { get; set; }

    public bool IsActive { get; set; } = true;
}
