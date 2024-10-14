import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoanOffer } from 'src/app/_models/loan-offer';
import { AuthService } from 'src/app/_services/auth.service';
import { LoanOfferService } from 'src/app/_services/loan-offer.service';

@Component({
  selector: 'app-create-offer',
  templateUrl: './create-offer.component.html',
  styleUrls: ['./create-offer.component.css']
})
export class CreateOfferComponent {
  loanOffers: LoanOffer[] = [];
  newLoanOffer: Omit<LoanOffer, 'id' | 'lender' | 'createdAt' | 'updatedAt'> = {
    lenderId: 0,
    principalAmount: 0,
    interestRate: 0,
    durationInYears: 20, // Default to 20 years
    monthlyRepayment: 0,
    totalRepayment: 0,
    isActive: true,
    hasApplied: false,
  };

  constructor(private loanOfferService: LoanOfferService, public router: Router,
     public authService: AuthService, private toastr: ToastrService) { }

  createLoanOffer(): void {
    this.loanOfferService.createLoanOffer(this.newLoanOffer).subscribe({
      next: offer => {
        this.loanOffers.push(offer);
        this.resetNewLoanOffer();
        
        this.toastr.success('Loan offer created successfully', 'Success');
        console.log(offer);
        this.router.navigateByUrl('/lender-dashboard');
      },
      error: error => console.error('Error creating loan offer', error)
    });
  }

  formatDisplayValue(value: number): string {
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  updatePrincipalAmount(event: KeyboardEvent): void {
    const inputElement = event.target as HTMLInputElement | null;
    if (inputElement) {
      const numericValue = Number(inputElement.value.replace(/,/g, ''));
      if (!isNaN(numericValue)) {
        this.newLoanOffer.principalAmount = numericValue;
      }
    }
  }

  calculateMonthlyRepayment(): string {
    // Extracting necessary loan details (principal, interest rate, loan term in months)
    const principal = this.newLoanOffer.principalAmount;
    const monthlyInterestRate = this.newLoanOffer.interestRate / 100 / 12;
    const numberOfPayments = this.newLoanOffer.durationInYears * 12;

    // Handling edge cases where interest rate or duration is 0
    if (monthlyInterestRate === 0 || numberOfPayments === 0) {
      return '0.00';
    }

    // Amortization formula to calculate the monthly repayment amount
    const monthlyPayment = 
      (principal * monthlyInterestRate * Math.pow(1 + monthlyInterestRate, numberOfPayments)) /
      (Math.pow(1 + monthlyInterestRate, numberOfPayments) - 1);

    // Format the result to a string with two decimal places
    this.newLoanOffer.monthlyRepayment = Number(monthlyPayment.toFixed(2));;
    return this.formatDisplayValue(monthlyPayment);
  }

  calculateTotalRepayment(): string {
    // Convert loan term in years to total number of months
    const totalPayments = this.newLoanOffer.durationInYears * 12;

    // Calculate total repayment
    const totalRepayment = this.newLoanOffer.monthlyRepayment * totalPayments;
    this.newLoanOffer.totalRepayment = Number(totalRepayment.toFixed(2));
    return this.formatDisplayValue(totalRepayment);
}

  resetNewLoanOffer(): void {
    this.newLoanOffer = {
      lenderId: 0,
      principalAmount: 0,
      interestRate: 0,
      durationInYears: 20,
      monthlyRepayment: 0,
      totalRepayment: 0,
      isActive: true,
      hasApplied: false
    };
  }
}