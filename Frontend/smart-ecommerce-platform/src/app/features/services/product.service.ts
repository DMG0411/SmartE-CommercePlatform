import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Product } from '../models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  options: {
    headers: HttpHeaders;
  } = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  private _baseUrl: string = 'https://localhost:7078/api/v1/product';

  constructor(private _httpClient: HttpClient) {}

  getProducts(): Observable<Product[]> {
    return this._httpClient.get<Product[]>(this._baseUrl, this.options);
  }

  createProduct(product: Product) {
    return this._httpClient.post<Product>(this._baseUrl, product, this.options);
  }

  updateProduct(product: Product) {
    return this._httpClient.put(this._baseUrl, product, this.options);
  }

  deleteProduct(id: number) {
    return this._httpClient.delete<void>(
      `${this._baseUrl}/${id}`,
      this.options
    );
  }

  getProductById(id: number) {
    return this._httpClient.get<Product>(
      `${this._baseUrl}/${id}`,
      this.options
    );
  }
}
