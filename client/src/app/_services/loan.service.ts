import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Loan } from '../_models/loan';
import { Payment } from '../_models/payment';

@Injectable({
  providedIn: 'root'
})
export class LoanService {
  private apiUrl = 'https://localhost:5001/api/loan'; 
  constructor(private http: HttpClient) { }

  // Fetch loan details by loan offer id
  getLoanByOfferId(loanOfferId: number): Observable<Loan> {
    return this.http.get<Loan>(`${this.apiUrl}/loans/offer/${loanOfferId}`);
  }

  // Fetch payments for a loan by loan offer id
  getPaymentsForLoanOffer(loanOfferId: number): Observable<Payment[]> {
    return this.http.get<Payment[]>(`${this.apiUrl}/loanoffers/${loanOfferId}/payments`);
  }

  // Fetch pending loan applications for a specific loan offer
  getPendingApplicationsForLoanOffer(loanOfferId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/loanoffers/${loanOfferId}/pending-applications`);
  }

  // Accept a loan application
  acceptLoanApplication(loanApplicationId: number, lenderId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/loanapplications/${loanApplicationId}/accept`, { lenderId });
  }
}
