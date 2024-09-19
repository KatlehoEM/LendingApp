using System;

namespace API.Entities;

public class PaymentRequest
{
    public int LoanOfferId { get; set; }
    public decimal Amount { get; set; }
}
