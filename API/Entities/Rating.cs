using System;

namespace API.Entities;

public class Rating
{
    public int Id { get; set; }
    public int LenderId { get; set; }
    public User Lender { get; set;}
    public int BorrowerId { get; set; }
    public User Borrower { get; set;}

    public int LoanApplicationId { get; set;}
    public LoanApplication LoanApplication { get; set;}
    public int Score { get; set; } // Score from 1 to 5
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
