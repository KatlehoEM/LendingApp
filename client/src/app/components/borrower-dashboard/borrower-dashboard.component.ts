import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LoanApplication } from 'src/app/_models/loanApplication';
import { LoanApplicationService } from 'src/app/_services/loan-application.service';
import { PaymentService } from 'src/app/_services/payment.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Payment } from 'src/app/_models/payment';
import { PaymentModalComponentComponent } from '../payment-modal-component/payment-modal-component.component';
import { BlockchainService } from 'src/app/_services/blockchain.service';

@Component({
  selector: 'app-borrower-dashboard',
  templateUrl: './borrower-dashboard.component.html',
  styleUrls: ['./borrower-dashboard.component.css']
})
export class BorrowerDashboardComponent implements OnInit {
  loanApplications: LoanApplication[] = [];
  bsModalRef?: BsModalRef;
  paymentsByLoan: { [loanId: number]: Payment[] } = {};
  reputationScore: number = 0;


  constructor(
    private loanApplicationService: LoanApplicationService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private paymentService: PaymentService,
    private blockchainService: BlockchainService
  ) {}

  ngOnInit() {
    this.getLoanApplications();
    this.loadReputationScore();
  }

  getLoanApplications() {
    this.loanApplicationService.getBorrowerLoanApplications().subscribe({
      next: (applications: LoanApplication[]) => {
        this.loanApplications = applications;
        this.loadPaymentsForLoans();
      },
      error: error => {
        console.error('Error fetching loan applications', error);
      }
    });
  }

  loadReputationScore(): void {
    this.blockchainService.getReputationScore().subscribe(
      (score: number) => {
        this.reputationScore = score;
      },
      error => {
        console.error('Error loading reputation score:', error);
      }
    );
  }


  loadPaymentsForLoans() {
    this.loanApplications.forEach(application => {
      this.paymentService.getPaymentsByLoanId(application.loanOfferId).subscribe({
        next: (payments: Payment[]) => {
          this.paymentsByLoan[application.loanOfferId] = payments;
        },
        error: (error: any) => {
          console.error('Error fetching payments for loan', error);
        }
      });
    });
  }

  openPaymentModal(loanOfferId: number) {
    const initialState = { loanOfferId: loanOfferId };
    this.bsModalRef = this.modalService.show(PaymentModalComponentComponent, { initialState });
    this.bsModalRef.content.onClose.subscribe((result: { loanOfferId: number; amount: number }) => {
      if (result) {
        this.makePayment(result);
      }
    });
  }

  makePayment(result: { loanOfferId: number; amount: number }) {
    const paymentRequest = {
      loanOfferId: result.loanOfferId,
      amount: result.amount
    };

    this.paymentService.makePayment(paymentRequest).subscribe({
      next: response => {
        console.log('Payment processed successfully', response);
        this.toastr.success('Payment processed successfully', 'Success');
        this.getLoanApplications(); // Reload loan applications and payments
      },
      error: error => {
        console.error('Error processing payment', error);
        this.toastr.error(error.error.error, 'Error');
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
        this.toastr.error('Error withdrawing application', 'Error');
      }
    });
  }
}
