import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Cart } from '../models';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  options: {
    headers: HttpHeaders;
  } = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  cartUpdated$: BehaviorSubject<void> = new BehaviorSubject<void>(undefined);

  private _baseUrl: string = `${environment.apiUrl}/cart`;

  constructor(private _httpClient: HttpClient) {}

  getCartItems(): Observable<Cart> {
    return this._httpClient.get<Cart>(this._baseUrl, this.options);
  }

  addToCart(productId: string): Observable<void> {
    return this._httpClient.put<void>(
      `${this._baseUrl}/add/${productId}`,
      {},
      this.options
    );
  }

  removeFromCart(productId: string): Observable<void> {
    return this._httpClient.put<void>(
      `${this._baseUrl}/remove/${productId}`,
      this.options
    );
  }
}
