import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private api = 'https://localhost:7148/api/Products/products';

  constructor(private http: HttpClient) {}

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(this.api);
  
  }

  getByType(type: string): Observable<any[]> {
  const url = type ? `${this.api}?type=${type}` : this.api;
  return this.http.get<any[]>(url);
}

}


 
