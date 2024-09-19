
export interface Lender {
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