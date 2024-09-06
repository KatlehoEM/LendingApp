using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateLoanApplicationDto
    {
        [Required]
        public int LoanOfferId { get; set; }

    }
