import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private baseUrl = 'https://localhost:7148/api/Orders';

  constructor(private http: HttpClient) { }



  getAvailableSlots() {
    return this.http.get<any[]>('https://localhost:7148/api/Orders/available-slots');
  }







  placeOrder(deliveryTime: string): Observable<any> {
    return this.http.post(
      'https://localhost:7148/api/Orders/place',
      JSON.stringify(deliveryTime),
      {
        headers: { 'Content-Type': 'application/json' }
      }
    );
  }



}

