import { Loan } from "./loan";

export interface Payment {
    id: number;
    loanId: number;
    loanApplication: number;
    amount: number;
    balance: number;
    paymentDate: Date;
    loan: Loan;

  }