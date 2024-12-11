import { LoginComponent } from './login.component';
import { Router } from '@angular/router';
import { UserService } from '@app/core/services';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '@app/shared';
import { HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let mockRouter: Partial<Router>;
  let mockUserService: Partial<UserService>;
  let mockToastrService: Partial<ToastrService>;
  let mockErrorHandlerService: Partial<ErrorHandlerService>;

  beforeEach(() => {
    mockRouter = { navigate: jest.fn() };
    mockUserService = {
      loginUser: jest.fn().mockReturnValue({
        pipe: jest.fn().mockReturnValue({
          subscribe: jest.fn(),
        }),
      }),
    };
    mockToastrService = { success: jest.fn() };
    mockErrorHandlerService = { handleError: jest.fn() };

    component = new LoginComponent(
      mockRouter as Router,
      mockUserService as UserService,
      mockToastrService as ToastrService,
      mockErrorHandlerService as ErrorHandlerService
    );
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize loginForm with empty values', () => {
    expect(component.loginForm.value).toEqual({ email: '', password: '' });
  });

  it('should call loginUser and navigate to home on success', () => {
    const mockUserLoggedIn$ = new BehaviorSubject<boolean>(false);

    const mockLoginResponse = {
      pipe: jest.fn().mockReturnValue({
        subscribe: jest.fn().mockImplementation((callback) => {
          callback.next();
        }),
      }),
    };

    mockUserService.userLoggedIn$ = mockUserLoggedIn$;
    mockUserService.loginUser = jest.fn().mockReturnValue(mockLoginResponse);

    component.loginUser();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/home']);
    expect(mockToastrService.success).toHaveBeenCalledWith('Login successful');
    expect(mockUserLoggedIn$.value).toBe(true);
  });

  it('should call handleError on login failure', () => {
    const mockError = new HttpErrorResponse({
      error: 'Login failed',
      status: 400,
    });
    mockUserService.loginUser = jest.fn().mockReturnValue({
      pipe: jest.fn().mockReturnValue({
        subscribe: jest
          .fn()
          .mockImplementation(({ error }) => error(mockError)),
      }),
    });

    component.loginUser();

    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(mockError);
  });

  it('should set isLoading to true during login and false after completion', () => {
    const mockLoginResponse = {
      pipe: jest.fn().mockReturnValue({ subscribe: jest.fn() }),
    };
    mockUserService.loginUser = jest.fn().mockReturnValue(mockLoginResponse);

    component.loginUser();

    expect(component.isLoading).toBe(true);

    mockLoginResponse.pipe().subscribe(() => {
      expect(component.isLoading).toBe(false);
    });
  });

  it('should redirect to register page', () => {
    component.redirectToRegister();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/register']);
  });
});
