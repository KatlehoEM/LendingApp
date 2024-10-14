import { Borrower, LoanApplicationStatus } from "./loanApplication";
import { Payment } from "./payment";

export interface LoanApplicationDto {
    id: number;
    borrowerId: number;
    borrowerName: string;
    loanOfferId: number;
    creditScore: number;
    monthlyRepayment:number;
    totalRepayment:number;
    borrowerReputationScore: number;
    loanOfferAmount: number;
    loanOfferInterestRate: number;
    loanOfferDuration: number;
    applicationStatus: LoanApplicationStatus;
    borrower: Borrower;
    payments: Payment[];
  }