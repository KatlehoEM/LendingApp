import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RatingService {
  private apiUrl = 'https://localhost:5001/api/rating';

  constructor(private http: HttpClient) {}

  createRating(ratingData: any) {
    return this.http.post(`${this.apiUrl}`, ratingData);
  }

  getRatingById(id: number){
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  getRatingsByBorrowerId(borrowerId: number){
    return this.http.get(`${this.apiUrl}/borrower/${borrowerId}`);
  }

  getAverageBorrowerRating(borrowerId: number){
    return this.http.get<number>(`${this.apiUrl}/average/${borrowerId}`);
  }

  updateRating(id: number, ratingData: any){
    return this.http.put(`${this.apiUrl}/${id}`, ratingData);
  }
}
