import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

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


  getLoanPayments(loanId: number){
    return this.http.get(`${this.apiUrl}/loan/${loanId}`);
  }
}
