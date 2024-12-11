import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { UserService } from './user.service';
import { User } from '@app/shared';
import { environment } from 'environments/environment';
import { of } from 'rxjs';

describe('UserService', () => {
  let service: UserService;
  let httpClientMock: jest.Mocked<HttpClient>;

  const mockUser: User = {
    id: 1,
    username: 'testuser',
    email: 'test@example.com',
    password: 'password',
  };

  const mockJwtToken = 'sample.jwt.token';
  const mockUserDetails: User = {
    id: 1,
    username: 'testuser',
    email: 'test@example.com',
  };

  beforeEach(() => {
    httpClientMock = {
      post: jest.fn(),
      get: jest.fn(),
    } as any;

    service = new UserService(httpClientMock);
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call loginUser and set the token in localStorage', () => {
    const mockResponse = new HttpResponse({
      status: 200,
      statusText: 'OK',
      headers: new HttpHeaders().set('Authorization', `Bearer ${mockJwtToken}`),
    });

    httpClientMock.post.mockReturnValue(of(mockResponse));

    service.loginUser(mockUser).subscribe();

    expect(httpClientMock.post).toHaveBeenCalledWith(
      `${environment.apiUrl}/user/login`,
      mockUser,
      expect.objectContaining({
        headers: expect.objectContaining({
          'Content-Type': 'application/json',
        }),
      })
    );

    expect(localStorage.getItem('token')).toBe(mockJwtToken);
    expect(localStorage.getItem('userId')).toBe('1');
  });

  it('should call registerUser and return void', () => {
    httpClientMock.post.mockReturnValue(of(null));

    service.registerUser(mockUser).subscribe((response) => {
      expect(response).toBeUndefined();
    });

    expect(httpClientMock.post).toHaveBeenCalledWith(
      `${environment.apiUrl}/user/register`,
      mockUser,
      expect.objectContaining({
        headers: expect.objectContaining({
          'Content-Type': 'application/json',
        }),
      })
    );
  });

  it('should call getUserDetails and return the correct user data', () => {
    httpClientMock.get.mockReturnValue(of(mockUserDetails));

    service.getUserDetails().subscribe((userData: User) => {
      expect(userData).toEqual(mockUserDetails);
    });

    expect(httpClientMock.get).toHaveBeenCalledWith(
      `${environment.apiUrl}/user/details`,
      expect.objectContaining({
        headers: expect.objectContaining({
          'Content-Type': 'application/json',
        }),
      })
    );
  });

  it('should correctly parse JWT and set token in localStorage', () => {
    const response = new HttpResponse<object>({
      status: 200,
      statusText: 'OK',
      headers: new HttpHeaders().set('Authorization', `Bearer ${mockJwtToken}`),
    });

    service['setUpJwtToken'](response);

    const storedToken = localStorage.getItem('token');
    const storedUserId = localStorage.getItem('userId');

    expect(storedToken).toBe(mockJwtToken);
    expect(storedUserId).toBe('1');
  });
});
