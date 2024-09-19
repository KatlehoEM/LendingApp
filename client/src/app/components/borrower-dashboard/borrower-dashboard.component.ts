import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LoanApplication } from 'src/app/_models/loanApplication';
import { LoanApplicationService } from 'src/app/_services/loan-application.service';
import { PaymentService } from 'src/app/_services/payment.service';
import { PaymentModalComponentComponent } from '../payment-modal-component/payment-modal-component.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-borrower-dashboard',
  templateUrl: './borrower-dashboard.component.html',
  styleUrls: ['./borrower-dashboard.component.css']
})
export class BorrowerDashboardComponent implements OnInit {
    loanApplications: LoanApplication[] = [];
    bsModalRef?: BsModalRef //<PaymentModalComponentComponent> = new BsModalRef<PaymentModalComponentComponent>() ;
  
    constructor(private loanApplicationService: LoanApplicationService,
       private toastr: ToastrService,
       private modalService: BsModalService,
       private paymentService: PaymentService) {}
  
    ngOnInit() {
      this.getLoanApplications();
    }
  
    getLoanApplications() {
      this.loanApplicationService.getBorrowerLoanApplications().subscribe({
        next: (applications: LoanApplication[]) => {
          this.loanApplications = applications;
        },
        error: error => {
          console.error('Error fetching loan applications', error);
          // Handle error (show message to user, etc.)
        }
      });
    }

    openPaymentModal(loanOfferId: number) {
      const initialState = {
        loanOfferId: loanOfferId
      };
      this.bsModalRef = this.modalService.show(PaymentModalComponentComponent, { initialState });
      this.bsModalRef.content.onClose.subscribe((result: {loanOfferId: number, amount: number}) => {
        if (result) {
          this.makePayment(result);
        }
      });
    }
  


    
    makePayment(result: {loanOfferId: number, amount: number}){
      const paymentRequest = {
        loanOfferId: result.loanOfferId,
        amount: result.amount
      };

     
        this.paymentService.makePayment(paymentRequest).subscribe({
          next: (response: any) => {
            console.log('Payment processed successfully', response);
            this.toastr.success('Payment processed successfully', 'Success');
          },
          error: (error: any) => {
            console.error('Error processing payment', error);
            this.toastr.error('Error processing payment', 'Error');
          }
        });

    }
  
    withdrawApplication(id: number) {
      this.loanApplicationService.withdrawLoanApplication(id).subscribe({
        next: () => {
          console.log('Application withdrawn successfully');
          this.getLoanApplications(); // Refresh the list
          this.toastr.success('Application withdrawn successfully', 'Success');
        },
        error: error => {
          console.error('Error withdrawing application', error);
          // Handle error (show message to user, etc.)
          this.toastr.error('Error withdrawing application', 'Error');

        }
      });
    }
  }
