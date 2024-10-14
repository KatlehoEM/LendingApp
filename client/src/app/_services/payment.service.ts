import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Payment } from '../_models/payment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'https://localhost:5001/api/payment';

  constructor(private http: HttpClient) { }

  makePayment(paymentRequest: { loanOfferId: number, amount: number }) {
    return this.http.post(this.apiUrl + '/make', paymentRequest);
  }

  getBorrowerPayments(borrowerId: number) {
    return this.http.get(`${this.apiUrl}/borrower/${borrowerId}`);
  }

  getPaymentsByLoanId(loanId: number): Observable<Payment[]> {
    return this.http.get<Payment[]>(`${this.apiUrl}/loan/${loanId}`);
  }
}
