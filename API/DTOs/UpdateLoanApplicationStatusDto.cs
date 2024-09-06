using System;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs;

public class UpdateLoanApplicationStatusDto
    {
        [Required]
        public LoanApplicationStatus Status { get; set; }
    }
