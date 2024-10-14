import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { BsModalRef} from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-payment-modal-component',
  templateUrl: './payment-modal-component.component.html',
  styleUrls: ['./payment-modal-component.component.css']
})
export class PaymentModalComponentComponent implements OnInit {
  paymentForm: FormGroup = new FormGroup({});
  loanOfferId: number = 0;
  formattedAmount: string = '';
  public onClose: Subject<{loanOfferId: number, amount: number}> = new Subject();

  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder) {}

  ngOnInit() {
    this.paymentForm = this.fb.group({
      cardNumber: ['', [Validators.required, Validators.minLength(19)]],
      cardName: ['', Validators.required],
      expiryDate: ['', [Validators.required, Validators.maxLength(5)]],
      cvv: ['', [Validators.required, Validators.maxLength(3)]],
      amount: ['', Validators.required],
    });
  }

  formatCardNumber(event: any) {
    let input = event.target.value.replace(/\D/g, ''); // Remove non-digits
    if (input.length > 16) {
      input = input.substr(0, 16); // Limit to 16 digits
    }
    event.target.value = input.replace(/(\d{4})(?=\d)/g, '$1 '); // Add space every 4 digits
  }

  // Format expiry date as "MM/YY"
  formatExpiryDate(event: any) {
    let input = event.target.value.replace(/\D/g, ''); // Remove non-digits
    if (input.length > 4) {
      input = input.substr(0, 4); // Limit to 4 digits (MMYY)
    }
    if (input.length > 2) {
      event.target.value = `${input.substr(0, 2)}/${input.substr(2, 2)}`; // Add slash between MM and YY
    } else {
      event.target.value = input;
    }
  }

  formatAmount(event: Event) {
    const input = (event.target as HTMLInputElement).value.replace(/[^\d.]/g, '');
    const parts = input.split('.');
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    this.formattedAmount = parts.join('.');
    this.paymentForm.get('amount')?.setValue(this.formattedAmount);
  }

  submitPayment() {
    if (this.paymentForm.valid) {
      const amountFormatted = this.paymentForm.get('amount')?.value ?? 0;
      const amount = Number(amountFormatted.replace(/,/g, ''));
      this.onClose.next({loanOfferId: this.loanOfferId, amount: amount});
      this.bsModalRef.hide();
    }
  }
}