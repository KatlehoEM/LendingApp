import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LoanOffer } from 'src/app/_models/loan-offer';
import { AuthService } from 'src/app/_services/auth.service';
import { LoanApplicationService } from 'src/app/_services/loan-application.service';
import { LoanOfferService } from 'src/app/_services/loan-offer.service';

@Component({
  selector: 'app-loan-offers',
  templateUrl: './loan-offers.component.html',
  styleUrls: ['./loan-offers.component.css']
})
export class LoanOffersComponent implements OnInit {
  loanOffers: LoanOffer[] = [];
  

  showMyOffersOnly: boolean = false;


  constructor(private loanOfferService: LoanOfferService, private toastr: ToastrService,
     public authService: AuthService, private loanApplicationService: LoanApplicationService) { }

  ngOnInit(): void {
    this.loadLoanOffers();
  }

  toggleMyOffers(): void {
    this.showMyOffersOnly = !this.showMyOffersOnly;
    this.loadLoanOffers();
  }

  loadLoanOffers(): void {
    if (this.showMyOffersOnly) {
      this.loanOfferService.getMyLoanOffers().subscribe({
        next: offers => this.loanOffers = offers,
        error: error => console.error('Error fetching my loan offers', error)
     });
    } else {
      this.loanOfferService.getLoanOffers().subscribe({
        next: offers => this.loanOffers = offers,
        error: error => console.error('Error fetching all loan offers', error)
        });
    }
  }
  

  updateLoanOffer(offer: LoanOffer): void {
    this.loanOfferService.updateLoanOffer(offer.id, offer).subscribe({
      next: updatedOffer => {
        const index = this.loanOffers.findIndex(o => o.id === updatedOffer.id);
        if (index !== -1) {
          this.loanOffers[index] = updatedOffer;
        }
      },
      error: error => console.error('Error updating loan offer', error)
     });
  }

  deleteLoanOffer(id: number): void {
    this.loanOfferService.deleteLoanOffer(id).subscribe({
      next: () => {
        this.loanOffers = this.loanOffers.filter(offer => offer.id !== id);
      },
      error: error => console.error('Error deleting loan offer', error)
      });
  }

  applyForLoan(offer: LoanOffer): void {
    this.loanApplicationService.applyForLoan(offer.id).subscribe({
      next: (application) => {
        console.log('Successfully applied for loan', application);
        this.toastr.success('Successfully applied for loan', 'Success');
      },
      error: (error) => {
        console.error('Error applying for loan', error);
        // Handle the error (e.g., show an error message to the user)
      }
    });
  }

  
}

