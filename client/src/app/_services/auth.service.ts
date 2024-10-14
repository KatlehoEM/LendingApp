import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { User } from '../_models/user';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnInit {
  private apiUrl = 'https://localhost:5001/api/user';
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>;

  constructor(private http: HttpClient, private router: Router) 
  {
    this.currentUserSubject = new BehaviorSubject<User | null>(JSON.parse(localStorage.getItem('user') || 'null'));
    this.currentUser = this.currentUserSubject.asObservable();
   }
  ngOnInit(): void {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      const user: User = JSON.parse(storedUser);
      this.setCurrentUser(user);
    }
  }

   public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  login(model: any){
    return this.http.post<User>(this.apiUrl + '/login', model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user)
        }
      })
    )
  }

  isLoggedIn(): boolean {
    if(this.currentUserValue){
      return true;
    }
    else{
      return false;
    }
  }

  register(model: any){
    return this.http.post<User>(this.apiUrl + '/register',model);
   }


  setCurrentUser(user: User){
    localStorage.setItem('user',JSON.stringify(user))
    this.currentUserSubject.next(user);
    return user;
   }
 
   logOut(){
     localStorage.removeItem('user');
     this.currentUserSubject.next(null);
    this.router.navigate(['/home']);
   }

   getUserRole(): number | null {
    const currentUser = this.currentUserSubject.value;
    return currentUser ? currentUser.role : null;
  }

  isLender(): boolean {
    return this.getUserRole() === 1; 
  }

  isBorrower(): boolean {
    return this.getUserRole() === 0; 
  }
}
