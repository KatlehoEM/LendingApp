import { LoanApplication } from "./loanApplication";
import { User } from "./user";

export interface Rating {
    id: number;
    lenderId: number;
    lender: User;
    borrowerId: number;
    borrower: User;
    loanApplicationId: number;
    loanApplication: LoanApplication;
    score: number;
    createdAt: string;
    updatedAt: string;
}
