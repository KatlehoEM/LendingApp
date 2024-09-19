import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoanOffer } from '../_models/loan-offer';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoanOfferService {
  private apiUrl = 'https://localhost:5001/api/loanoffer';

  constructor(private http: HttpClient) { }

  getLoanOffers(): Observable<LoanOffer[]> {
    return this.http.get<LoanOffer[]>(this.apiUrl);
  }

  getLoanOffer(id: number): Observable<LoanOffer> {
    return this.http.get<LoanOffer>(`${this.apiUrl}/${id}`);
  }

  getMyLoanOffers(): Observable<LoanOffer[]> {
    return this.http.get<LoanOffer[]>(this.apiUrl + '/myloanoffers');
  }

  createLoanOffer(loanOffer: Omit<LoanOffer, 'id' | 'lender' | 'createdAt' | 'updatedAt'>): Observable<LoanOffer> {
    return this.http.post<LoanOffer>(this.apiUrl, loanOffer);
  }

  updateLoanOffer(id: number, loanOffer: Partial<LoanOffer>): Observable<LoanOffer> {
    return this.http.put<LoanOffer>(`${this.apiUrl}/${id}`, loanOffer);
  }

  deleteLoanOffer(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }


}
