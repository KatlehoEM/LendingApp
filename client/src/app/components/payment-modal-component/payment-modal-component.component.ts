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
  public onClose: Subject<{loanOfferId: number, amount: number}> = new Subject();

  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder) {}

  ngOnInit() {
    this.paymentForm = this.fb.group({
      amount: ['', [Validators.required, Validators.min(0)]]
    });
  }

  submitPayment() {
    if (this.paymentForm.valid) {
      const amount = this.paymentForm.get('amount')?.value ?? 0;
      this.onClose.next({loanOfferId: this.loanOfferId, amount: amount});
      this.bsModalRef.hide();
    }
  }
}