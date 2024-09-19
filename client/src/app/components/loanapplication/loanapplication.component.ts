import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { response } from 'express';
import { ToastrService } from 'ngx-toastr';
import { LoanApplication, LoanApplicationStatus } from 'src/app/_models/loanApplication';
import { Rating } from 'src/app/_models/rating';
import { LoanApplicationService } from 'src/app/_services/loan-application.service';
import { RatingService } from 'src/app/_services/rating.service';

@Component({
  selector: 'app-loanapplication',
  templateUrl: './loanapplication.component.html',
  styleUrls: ['./loanapplication.component.css']
})
export class LoanapplicationComponent {
  loanOfferId: number;
  loanApplications: LoanApplication[] = [];
  loanApplicationStatus = LoanApplicationStatus;
  userRating: number = 0;
  relevantRating!: Rating;
  

  constructor(
    private route: ActivatedRoute,
    private loanApplicationService: LoanApplicationService,
    private toastr: ToastrService,
    private ratingService: RatingService
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
      next: (applications: LoanApplication[]) => {
        this.loanApplications = applications;
        // Load existing ratings for approved applications
        this.loanApplications.forEach(app => {
          if (app.status === LoanApplicationStatus.Approved) {
            this.loadRating(app);
          }
        });
      },
      error: (error: any) => {
        console.error('Error fetching loan applications', error);
        // Handle error (show message to user, etc.)
      }
      });
  }

  updateStatus(id: number, status: LoanApplicationStatus) {
    this.loanApplicationService.updateLoanApplicationStatus(id, status).subscribe({
      next: (updatedApplication: LoanApplication) => {
        console.log('Application status updated successfully', updatedApplication);
        this.toastr.success(`Application ${status.toString().toLowerCase()} successfully`, 'Success');
        this.getLoanApplications(this.loanOfferId); // Refresh the list
      },
      error: (error: any) => {
        console.error('Error updating application status', error);
        // Handle error (show message to user, etc.)
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
    const application = this.loanApplications.find(app => app.id === applicationId);
    if (application) {
      if(this.relevantRating) {
        this.ratingService.updateRating(this.relevantRating.id, {score:newRating}).subscribe({
          next: (response) => {
            console.log('Rating updated successfully:', response);
          this.toastr.success('Rating updated successfully','Success');
          },
          error: (error) => {
            console.error('Error updating rating:', error)
          this.toastr.error('Error updating rating','Error');
          }
        });
      }
      else{
        this.ratingService.createRating({
          loanApplicationId: applicationId,
          score: newRating
        }).subscribe(
          (response) => {
            console.log('Rating added successfully:', response);
            this.toastr.success('Rating added successfully','Success');
          },
          (error) => {
            console.error('Error adding rating:', error)
            this.toastr.error('Error adding rating','Error');
          }
        );
      }
      
    }
  }
}
