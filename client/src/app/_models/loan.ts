import { LoanOffer } from "./loan-offer";
import { Payment } from "./payment";
import { User } from "./user";

export interface Loan {
    id: number;
    borrowerId: number;
    loanOfferId: number;
    principalAmount: number;
    remainingBalance: number;
    interestRate: number;
    durationInYears: number;
    monthlyRepayment:number;
    totalRepayment:number;
    startDate: Date;
    endDate: Date;
    status: LoanStatus;
    loanOffer: LoanOffer;
    borrower: User;
    payments: Payment[];
}

export enum LoanStatus {
    Active = 'Active',
    Paid = 'Paid',
    Defaulted = 'Defaulted'
}