import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

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

  

 

removeCartItem(id: number): Observable<any> {
  return this.http.delete(`${this.baseUrl}/${id}`, { responseType: 'text' });
}



clearCart(): Observable<any> {
  return this.http.delete(`https://localhost:7148/api/Cart/clear`, {
    responseType: 'text'
  });
}


}
