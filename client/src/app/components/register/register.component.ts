import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

enum Role {
  Borrower = 0,
  Lender = 1,
  Both = 2
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  registrationForm:FormGroup = new FormGroup({});
  randomCreditScore: number = 0;
  formattedPhoneNumber: number = 0;
  formattedIncome: string = '';
  roles = [
    { id: Role.Borrower, name: 'Borrower' },
    { id: Role.Lender, name: 'Lender' },
    { id: Role.Both, name: 'Both (Lender and Borrower)' }
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    
  }
  ngOnInit(): void {
    this.randomCreditScore = this.generateRandomCreditScore();
    this.registrationForm = this.fb.group({
      firstname: ['', [Validators.required, Validators.minLength(3)]],
      lastname: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      role: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      idNumber: ['', Validators.required],
      streetNumber: ['', Validators.required],
      streetName: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      employmentStatus: ['', Validators.required],
      annualIncome: ['', Validators.required],
      creditScore: [{ value: this.randomCreditScore, disabled: true }, [Validators.required, Validators.min(300), Validators.max(850)]],
      termsAndConditions: ['', Validators.required],
      
    }, {validators: this.passwordMatchValidator  });
  }

  formatIncome(event: Event) {
    const input = (event.target as HTMLInputElement).value.replace(/[^\d.]/g, '');
    const parts = input.split('.');
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    this.formattedIncome = parts.join('.');
    this.registrationForm.get('annualIncome')?.setValue(this.formattedIncome);
  }

  formatPhoneNumber(): void {
    const phoneNumberControl = this.registrationForm.get('phoneNumber');
    if (phoneNumberControl) {
      const rawValue = phoneNumberControl.value.replace(/\s+/g, ''); // Remove existing spaces
      if (rawValue) {
        // Separate the last 4 digits
        const lastFourDigits = rawValue.slice(-4);
        const mainPart = rawValue.slice(0, -4);
  
        // Format the main part with spaces after every 3 digits
        const formattedMainPart = mainPart.replace(/(\d{3})(?=\d)/g, '$1 ');
  
        // Combine the formatted main part with the last four digits
        const formattedValue = formattedMainPart + (formattedMainPart ? ' ' : '') + lastFourDigits;
  
        phoneNumberControl.setValue(formattedValue.trim());
      }
    }
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { 'passwordMismatch': true };
    }

    return null;
  }

  onSubmit(): void {
    if (this.registrationForm.valid) {
      const formData = { ...this.registrationForm.value };
      formData.role = Number(formData.role); // Ensure role is sent as a number
      formData.creditScore = this.randomCreditScore;
      formData.annualIncome = Number(formData.annualIncome.replace(/,/g, ''));

      console.log(formData);

      this.authService.register(formData).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
          this.router.navigate(['/login']);
        },
        error: (error) => {
          console.error('Registration failed', error);
          // Handle error (e.g., display error message to user)
        }
      });
    }
  }

  generateRandomCreditScore(): number {
    return Math.floor(Math.random() * (850 - 300 + 1)) + 300;
  }

}
