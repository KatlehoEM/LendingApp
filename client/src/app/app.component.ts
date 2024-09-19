import { Component } from '@angular/core';
import { User } from './_models/user';
import { Observable } from 'rxjs';
import { AuthService } from './_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';

  user$: Observable<User | null>;

  constructor(private authService: AuthService, private router: Router){
    this.user$ = this.authService.currentUser;
  }
  ngOnInit(): void {
    this.user$ = this.authService.currentUser;
  }


  logout(): void {
    this.authService.logOut();
    this.router.navigateByUrl('/home');
  }
}
