import { UserService } from './user.service';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { of, throwError } from 'rxjs';
import { User } from '@app/shared';

describe('UserService', () => {
  let service: UserService;
  let httpClientMock: Partial<HttpClient>;

  const mockJwtToken = 'mock-jwt-token';
  const mockUser: User = {
    id: 1,
    username: 'Test User',
    email: 'test@example.com',
  };

  beforeEach(() => {
    httpClientMock = {
      get: jest.fn(),
      post: jest.fn(),
    };

    service = new UserService(httpClientMock as HttpClient);
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

    (httpClientMock.post as jest.Mock).mockReturnValue(of(mockResponse));

    service.loginUser(mockUser).subscribe((response) => {
      expect(response).toBeUndefined();
      const storedToken = localStorage.getItem('token');
      const storedUserId = localStorage.getItem('userId');
      expect(storedToken).toBe(mockJwtToken);
      expect(storedUserId).toBe(mockUser.id!.toString());
    });

    expect(httpClientMock.post).toHaveBeenCalledWith(
      `${service['_baseUrl']}/login`,
      mockUser,
      expect.objectContaining({
        headers: expect.any(HttpHeaders),
        observe: 'response',
      })
    );
  });

  it('should handle error when loginUser fails', () => {
    const mockErrorResponse = {
      status: 401,
      statusText: 'Unauthorized',
    };

    (httpClientMock.post as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.loginUser(mockUser).subscribe(
      () => fail('should have failed with 401 error'),
      (error) => {
        expect(error.status).toBe(401);
        expect(error.statusText).toBe('Unauthorized');
      }
    );

    expect(httpClientMock.post).toHaveBeenCalledWith(
      `${service['_baseUrl']}/login`,
      mockUser,
      expect.objectContaining({
        headers: expect.any(HttpHeaders),
        observe: 'response',
      })
    );
  });

  it('should call registerUser and return void', () => {
    (httpClientMock.post as jest.Mock).mockReturnValue(of(void 0));

    service.registerUser(mockUser).subscribe((response) => {
      expect(response).toBeUndefined();
    });

    expect(httpClientMock.post).toHaveBeenCalledWith(
      `${service['_baseUrl']}/register`,
      mockUser,
      expect.objectContaining({
        headers: expect.any(HttpHeaders),
      })
    );
  });

  it('should handle error when registerUser fails', () => {
    const mockErrorResponse = {
      status: 400,
      statusText: 'Bad Request',
    };

    (httpClientMock.post as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.registerUser(mockUser).subscribe(
      () => fail('should have failed with 400 error'),
      (error) => {
        expect(error.status).toBe(400);
        expect(error.statusText).toBe('Bad Request');
      }
    );

    expect(httpClientMock.post).toHaveBeenCalledWith(
      `${service['_baseUrl']}/register`,
      mockUser,
      expect.objectContaining({
        headers: expect.any(HttpHeaders),
      })
    );
  });

  it('should get user details', () => {
    (httpClientMock.get as jest.Mock).mockReturnValue(of(mockUser));

    service.getUserDetails().subscribe((response) => {
      expect(response).toEqual(mockUser);
    });

    expect(httpClientMock.get).toHaveBeenCalledWith(
      `${service['_baseUrl']}/details`,
      expect.objectContaining({
        headers: expect.any(HttpHeaders),
      })
    );
  });

  it('should handle error when getUserDetails fails', () => {
    const mockErrorResponse = {
      status: 404,
      statusText: 'Not Found',
    };

    (httpClientMock.get as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.getUserDetails().subscribe(
      () => fail('should have failed with 404 error'),
      (error) => {
        expect(error.status).toBe(404);
        expect(error.statusText).toBe('Not Found');
      }
    );

    expect(httpClientMock.get).toHaveBeenCalledWith(
      `${service['_baseUrl']}/details`,
      expect.objectContaining({
        headers: expect.any(HttpHeaders),
      })
    );
  });

  it('should set up JWT token correctly', () => {
    const mockResponse = new HttpResponse<object>({
      status: 200,
      statusText: 'OK',
      headers: new HttpHeaders().set('Authorization', `Bearer ${mockJwtToken}`),
    });

    jest.spyOn(service as any, 'parseJwt').mockReturnValue({ id: mockUser.id });

    service['setUpJwtToken'](mockResponse);

    const storedToken = localStorage.getItem('token');
    const storedUserId = localStorage.getItem('userId');

    expect(storedToken).toBe(mockJwtToken);
    expect(storedUserId).toBe(mockUser.id!.toString());
  });
});
