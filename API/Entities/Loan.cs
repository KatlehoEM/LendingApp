using System;

namespace API.Entities;

public class Loan
{
     public int Id { get; set; }
    public int BorrowerId { get; set; }
    public int LoanOfferId { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public decimal InterestRate { get; set; }
    public int DurationInMonths { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LoanStatus Status { get; set; }
    public virtual LoanOffer LoanOffer { get; set; }
    public virtual User Borrower { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
}

public enum LoanStatus
    {
        Active,
        Paid,
        Defaulted
    }
