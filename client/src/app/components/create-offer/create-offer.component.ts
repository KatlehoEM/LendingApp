import { Component } from '@angular/core';
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
    durationInMonths: 0,
    isActive: true
  };

  constructor(private loanOfferService: LoanOfferService, public authService: AuthService) { }

  createLoanOffer(): void {
    this.loanOfferService.createLoanOffer(this.newLoanOffer).subscribe({
      next: offer => {
        this.loanOffers.push(offer);
        this.resetNewLoanOffer();

        

        console.log(offer);
      },
      error: error => console.error('Error creating loan offer', error)
      });

    }

  resetNewLoanOffer(): void {
    this.newLoanOffer = {
      lenderId: 0,
      principalAmount: 0,
      interestRate: 0,
      durationInMonths: 0,
      isActive: true
    };
  }
}
