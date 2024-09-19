// src/app/models/loan-application.model.ts

import { LoanOffer } from "./loan-offer";

export enum LoanApplicationStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2,
  Withdrawn = 3
}
  
export interface LoanApplication {
    id: number;
    loanOfferId: number;
    loanOffer: LoanOffer;
    borrowerId: number;
    borrower: Borrower;
    status: LoanApplicationStatus;
    acceptanceDate: string;
    createdAt: string;
    updatedAt: string;
  }
  interface Borrower {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    passwordHash: string;
    walletAddress: string;
    creditScore: number;
    idNumber: string;
    address: string;
    phoneNumber: string;
    employmentStatus: string;
    annualIncome: number;
    role: number;
    createdAt: string;
    updatedAt: string;
    loanOffers?: any;
    ratings: any[];
  }
