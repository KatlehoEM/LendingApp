import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { LoanOfferService } from 'src/app/_services/loan-offer.service';

@Component({
  selector: 'app-lender-dashboard',
  templateUrl: './lender-dashboard.component.html',
  styleUrls: ['./lender-dashboard.component.css']
})
export class LenderDashboardComponent implements OnInit {

  loanOffers: any[] = [];

  constructor(private loanOfferService: LoanOfferService, public authService: AuthService,
    private router: Router){}

    ngOnInit(): void {
      this.loadLoanOffers();
    }

  createLoanOffer(){  
      this.router.navigateByUrl('/create-offer');
  }

  loadLoanOffers(): void {
      this.loanOfferService.getMyLoanOffers().subscribe({
        next: offers => this.loanOffers = offers,
        error: error => console.error('Error fetching my loan offers', error)
      });
    }

  viewApplications(offerId: number) {
    this.router.navigate(['/loanapplication/lender', offerId]);
  }

}
