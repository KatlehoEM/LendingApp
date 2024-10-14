import { Lender } from "./lender";

export interface LoanOffer {
  id: number;
  lenderId: number;
  lender: Lender;
  principalAmount: number; // Using number for decimal in TypeScript
  interestRate: number;
  durationInYears: number;
  monthlyRepayment: number;
  totalRepayment: number;
  isActive: boolean;
  hasApplied: boolean; 
  createdAt: Date;
  updatedAt: Date;
}