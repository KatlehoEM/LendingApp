import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private router: Router, private toastr: ToastrService)
    {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      firstname: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

    onSubmit() {
      if (this.loginForm.valid) {
        const loginCredentials = this.loginForm.value;
        this.authService.login(loginCredentials).subscribe({
          next: _ => {
            console.log(loginCredentials);

            if(this.authService.isBorrower()){
              this.router.navigateByUrl('/borrower-dashboard');
            }
            else{
              this.router.navigateByUrl('/lender-dashboard');
            }
            // this.loginForm.reset();
          },
          error: error => {
            console.error('Login error:', error);
            let errorMessage = 'An unexpected error occurred. Please try again.';
  
            if (error.status === 401) {
              errorMessage = 'Invalid username or password. Please check your credentials and try again.';
            } else if (error.status === 403) {
              errorMessage = 'Your account does not have permission to access this resource.';
            } else if (error.status === 500) {
              errorMessage = 'There was a problem with the server. Please try again later.';
            } else if (error.error && typeof error.error === 'string') {
              errorMessage = error.error;
            } else if (error.message) {
              errorMessage = error.message;
            }
  
            // Display the error message using Toastr
            this.toastr.error(errorMessage, 'Error');
          }
        });
      }
    }
}
