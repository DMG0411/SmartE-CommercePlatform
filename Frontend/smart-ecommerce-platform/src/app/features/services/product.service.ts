import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Product } from '../models';
import { Observable } from 'rxjs';
import { PaginatedResponse } from '@app/shared';
import { environment } from 'environments/environment';
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

  private _baseUrl: string = `${environment.apiUrl}/product`;

  constructor(private _httpClient: HttpClient) {}

  getProducts(
    pageNumber: number,
    pageSize: number
  ): Observable<PaginatedResponse<Product>> {
    let params: HttpParams = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this._httpClient.get<PaginatedResponse<Product>>(this._baseUrl, {
      ...this.options,
      params,
    });
  }

  createProduct(product: Product) {
    return this._httpClient.post<Product>(this._baseUrl, product, this.options);
  }

  updateProduct(product: Product) {
    return this._httpClient.put(this._baseUrl, product, this.options);
  }

  deleteProduct(id: string) {
    return this._httpClient.delete<void>(
      `${this._baseUrl}/${id}`,
      this.options
    );
  }

  getProductById(id: string) {
    return this._httpClient.get<Product>(
      `${this._baseUrl}/${id}`,
      this.options
    );
  }
}
