import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoanApplication, LoanApplicationStatus } from '../_models/loanApplication';
import { HttpClient } from '@angular/common/http';
import { LoanApplicationDto } from '../_models/loanApplicationDto';

@Injectable({
  providedIn: 'root'
})
export class LoanApplicationService {
  private apiUrl = 'https://localhost:5001/api/loanapplication'; 

  constructor(private http: HttpClient) {}

  createLoanApplication(createLoanApplicationDto: any): Observable<LoanApplication> {
    return this.http.post<LoanApplication>(this.apiUrl, createLoanApplicationDto);
  }

  getLoanApplication(id: number): Observable<LoanApplicationDto> {
    return this.http.get<LoanApplicationDto>(`${this.apiUrl}/${id}`);
  }

  getBorrowerLoanApplications(): Observable<LoanApplication[]> {
    return this.http.get<LoanApplication[]>(`${this.apiUrl}/borrower`);
  }

  getLoanOfferApplications(loanOfferId: number): Observable<LoanApplicationDto[]> {
    return this.http.get<LoanApplicationDto[]>(`${this.apiUrl}/lender/${loanOfferId}`);
  }

  updateLoanApplicationStatus(id: number, status: LoanApplicationStatus): Observable<LoanApplication> {
    return this.http.put<LoanApplication>(`${this.apiUrl}/${id}/status`, { status});
  }

  applyForLoan(loanOfferId: number): Observable<LoanApplication> {
    const createLoanApplicationDto = { loanOfferId: loanOfferId };
    return this.http.post<LoanApplication>(this.apiUrl, createLoanApplicationDto);
  }

  withdrawLoanApplication(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/withdraw`, {});
  }
}
