import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BlockchainService {
  private apiUrl = 'https://localhost:5001/api/blockchain';

  constructor(private http: HttpClient) { }

  getReputationScore() {
    return this.http.get<number>(`${this.apiUrl}/reputation-score`);
  }
}
