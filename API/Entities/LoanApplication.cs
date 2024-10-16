using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class LoanApplication
    {
        public int Id { get; set; }

        [Required]
        public int LoanOfferId { get; set; }
        public LoanOffer LoanOffer { get; set; }

        [Required]
        public int BorrowerId { get; set; }
        public User Borrower { get; set; }
        public decimal MonthlyRepayment {get;set;}
        public decimal TotalRepayment {get;set;}

        [Required]
        public LoanApplicationStatus Status { get; set; }

        public DateTime AcceptanceDate { get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

      public enum LoanApplicationStatus
    {
        Pending,
        Approved,
        Rejected,
        Withdrawn
    }
