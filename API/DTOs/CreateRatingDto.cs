using System;

namespace API.DTOs;

public class CreateRatingDto
{
    public int LoanApplicationId { get; set; }
    public int Score { get; set; }

}
