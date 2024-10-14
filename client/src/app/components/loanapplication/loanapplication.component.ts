import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoanApplication, LoanApplicationStatus } from 'src/app/_models/loanApplication';
import { LoanApplicationDto } from 'src/app/_models/loanApplicationDto';
import { Rating } from 'src/app/_models/rating';
import { LoanApplicationService } from 'src/app/_services/loan-application.service';
import { RatingService } from 'src/app/_services/rating.service';
import { PaymentService } from 'src/app/_services/payment.service';  // Import the payment service
import { Payment } from 'src/app/_models/payment';

@Component({
  selector: 'app-loanapplication',
  templateUrl: './loanapplication.component.html',
  styleUrls: ['./loanapplication.component.css']
})
export class LoanapplicationComponent {
  loanOfferId: number;
  loanApplicationDtos: LoanApplicationDto[] = [];
  loanApplications: LoanApplication[] = [];
  loanApplicationStatus = LoanApplicationStatus;
  userRating: number = 0;
  relevantRating!: Rating;
  paymentsByLoan: { [key: number]: Payment[] } = {};  // Store payments by loan application ID

  constructor(
    private route: ActivatedRoute,
    private loanApplicationService: LoanApplicationService,
    private toastr: ToastrService,
    private ratingService: RatingService,
    private paymentService: PaymentService  // Inject payment service
  ) {
    this.loanOfferId = 0;
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['loanOfferId']) {
        this.loanOfferId = +params['loanOfferId'];
        this.getLoanApplications(this.loanOfferId);
      }
    });
  }

  getLoanApplications(loanOfferId: number) {
    this.loanApplicationService.getLoanOfferApplications(loanOfferId).subscribe({
      next: (applications: LoanApplicationDto[]) => {
        this.loanApplicationDtos = applications;
        this.loanApplicationDtos.forEach(app => {
          if (app.applicationStatus === LoanApplicationStatus.Approved) {
            this.loadRating(app);
            this.loadPayments(app.loanOfferId);  // Load payments for the application
          }
        });
      },
      error: (error: any) => {
        console.error('Error fetching loan applications', error);
      }
    });
  }

  updateStatus(id: number, status: LoanApplicationStatus) {
    this.loanApplicationService.updateLoanApplicationStatus(id, status).subscribe({
      next: (updatedApplication: LoanApplication) => {
        this.toastr.success(`Application ${status.toString().toLowerCase()} successfully`, 'Success');
        this.getLoanApplications(this.loanOfferId); // Refresh the list
      },
      error: (error: any) => {
        this.toastr.error('Error updating status', 'Error');
      }
    });
  }

  loadRating(application: any) {
    this.ratingService.getRatingsByBorrowerId(application.borrower.id).subscribe(
      (ratings: any) => {
        this.relevantRating = ratings.find((r: { loanApplicationId: any; }) => r.loanApplicationId === application.id);
        this.userRating = this.relevantRating ? this.relevantRating.score : 0;
      },
      (error) => console.error('Error loading rating:', error)
    );
  }

  onRatingChange(applicationId: number, newRating: number) {
    const application = this.loanApplicationDtos.find(app => app.id === applicationId);
    if (application) {
      if(this.relevantRating) {
        this.ratingService.updateRating(this.relevantRating.id, {score:newRating}).subscribe({
          next: (response) => {
            this.toastr.success('Rating updated successfully','Success');
          },
          error: (error) => {
            this.toastr.error('Error updating rating','Error');
          }
        });
      } else {
        this.ratingService.createRating({
          loanApplicationId: applicationId,
          score: newRating
        }).subscribe({
          next: (response) => {
            this.toastr.success('Rating created successfully', 'Success');
          },
          error: (error) => {
            this.toastr.error('Error creating rating', 'Error');
          }
        });
      }
    }
  }

  loadPayments(applicationId: number) {
    this.paymentService.getPaymentsByLoanId(applicationId).subscribe({
      next: (payments: Payment[]) => {
        this.paymentsByLoan[applicationId] = payments;
        console.log(this.paymentsByLoan[applicationId])
      },
      error: (error) => {
        console.error('Error loading payments', error);
      }
    });
  }
}
