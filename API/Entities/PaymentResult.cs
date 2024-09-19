using System;

namespace API.Entities;

public class PaymentResult
{
    public bool Success { get; set; }
    public int PaymentId { get; set; }
    public string ErrorMessage { get; set; }

}
