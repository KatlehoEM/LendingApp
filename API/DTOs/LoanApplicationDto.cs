using API.Entities;

namespace API.DTOs;

public class LoanApplicationDto
{
   public int Id { get; set; }
    public int BorrowerId { get; set; }
   public User Borrower { get; set; }
   public int LoanOfferId { get; set; }

    public string BorrowerName { get; set; }
    public decimal BorrowerReputationScore { get; set; }
    public int CreditScore { get; set; }
    public decimal LoanOfferAmount { get; set; }
    public decimal LoanOfferInterestRate { get; set; }
    public int LoanOfferDuration { get; set; }
    public decimal MonthlyRepayment {get;set;}
    public decimal TotalRepayment {get;set;}
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public LoanApplicationStatus ApplicationStatus { get; set; }
    }