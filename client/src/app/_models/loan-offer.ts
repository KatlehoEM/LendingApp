import { Lender } from "./lender";

export interface LoanOffer {
  id: number;
  lenderId: number;
  lender: Lender;
  principalAmount: number; // Using number for decimal in TypeScript
  interestRate: number;
  durationInMonths: number;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
}