import { RegisterComponent } from './register.component';
import { Router } from '@angular/router';
import { UserService } from '@app/core/services';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '@app/shared';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let mockRouter: Partial<Router>;
  let mockUserService: Partial<UserService>;
  let mockToastrService: Partial<ToastrService>;
  let mockErrorHandlerService: Partial<ErrorHandlerService>;

  beforeEach(() => {
    mockRouter = { navigate: jest.fn() };
    mockUserService = {
      registerUser: jest.fn(),
    };
    mockToastrService = { success: jest.fn() };
    mockErrorHandlerService = { handleError: jest.fn() };

    component = new RegisterComponent(
      mockRouter as Router,
      mockUserService as UserService,
      mockToastrService as ToastrService,
      mockErrorHandlerService as ErrorHandlerService
    );
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize registerForm with empty values', () => {
    expect(component.registerForm.value).toEqual({
      username: '',
      password: '',
      email: '',
    });
  });

  it('should call registerUser and navigate to login on success', () => {
    const mockRegisterResponse = {
      pipe: jest.fn().mockReturnValue({
        subscribe: jest.fn().mockImplementation((callback) => {
          callback.next();
        }),
      }),
    };

    mockUserService.registerUser = jest
      .fn()
      .mockReturnValue(mockRegisterResponse);
    component.registerUser();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
    expect(mockToastrService.success).toHaveBeenCalledWith(
      'Registration successful'
    );
  });

  it('should call handleError on registration failure', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });

    (mockUserService.registerUser as jest.Mock).mockReturnValue(
      throwError(mockErrorResponse)
    );

    component.registerUser();

    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });

  it('should disable submit button when form is invalid', () => {
    component.registerForm.controls['username'].setValue('');
    component.registerForm.controls['password'].setValue('');
    component.registerForm.controls['email'].setValue('');

    expect(component.registerForm.invalid).toBe(true);
  });

  it('should enable submit button when form is valid', () => {
    component.registerForm.controls['username'].setValue('testuser');
    component.registerForm.controls['password'].setValue('password123');
    component.registerForm.controls['email'].setValue('testuser@example.com');

    expect(component.registerForm.valid).toBe(true);
  });
});
