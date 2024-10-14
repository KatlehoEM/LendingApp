using System;

namespace API.Entities;

public class Payment
{
     public int Id { get; set; }
        public int LoanId { get; set; }
        public int LoanApplicationId { get; set; }      
        public Loan Loan { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set;}
        public DateTime PaymentDate { get; set; }

}
