import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/shared';
import { environment } from 'environments/environment';
import { BehaviorSubject, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  options: {
    headers: HttpHeaders;
  } = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  userLoggedIn$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  private _baseUrl: string = `${environment.apiUrl}/user`;

  constructor(private _httpClient: HttpClient) {
    if (localStorage.getItem('token') !== null) {
      this.userLoggedIn$.next(true);
    }
  }

  loginUser(user: User): Observable<void> {
    return this._httpClient
      .post<HttpResponse<object>>(`${this._baseUrl}/login`, user, {
        ...this.options,
        observe: 'response',
      })
      .pipe(
        map((response: HttpResponse<object>) => this.setUpJwtToken(response))
      );
  }

  registerUser(user: User): Observable<void> {
    return this._httpClient.post<void>(
      `${this._baseUrl}/register`,
      user,
      this.options
    );
  }

  getUserDetails(): Observable<User> {
    return this._httpClient.get<User>(`${this._baseUrl}/details`, this.options);
  }

  editProfile(user: User): Observable<void> {
    return this._httpClient.put<void>(
      `${this._baseUrl}/edit`,
      user,
      this.options
    );
  }

  sendResetPassCode(email: string): Observable<void> {
    return this._httpClient.post<void>(
      `${this._baseUrl}/forgot-password`,
      { email },
      this.options
    );
  }

  private setUpJwtToken(response: HttpResponse<object>): void {
    const token: string = this.getToken(response);
    const userData: User = this.parseJwt(token);
    localStorage.clear();
    localStorage.setItem('token', token);
    localStorage.setItem('userId', userData.id!.toString());
  }

  private getToken(response: HttpResponse<object>): string {
    const authorizationHeader: string | null =
      response.headers.get('Authorization');
    return authorizationHeader
      ? authorizationHeader.replace('Bearer ', '')
      : '';
  }

  private parseJwt(token: string): User {
    const payloadBase64: string = token.split('.')[1];
    return JSON.parse(atob(payloadBase64));
  }
}
