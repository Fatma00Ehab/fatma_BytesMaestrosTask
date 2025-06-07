import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class CartService {
  private baseUrl = 'https://localhost:7148/api/Cart';

  constructor(private http: HttpClient) {}

  getCart() {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }

  addToCart(productId: number, quantity: number) {
    return this.http.post(`${this.baseUrl}`, { productId, quantity });
  }

  clearCart() {
    return this.http.delete(`${this.baseUrl}/clear`);
  }
}
