import { HeaderComponent } from './header.component';
import { UserService } from '@app/core';
import { CartService } from '@app/features/services';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '@app/shared';
import { of, throwError, BehaviorSubject } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let mockUserService: Partial<UserService>;
  let mockCartService: Partial<CartService>;
  let mockRouter: Partial<Router>;
  let mockToastrService: Partial<ToastrService>;
  let mockErrorHandlerService: Partial<ErrorHandlerService>;

  beforeEach(() => {
    mockUserService = {
      getUserDetails: jest.fn().mockReturnValue(of({ username: 'testuser' })),
    };
    mockCartService = {
      removeFromCart: jest.fn().mockReturnValue(of({})),
      getCartItems: jest.fn().mockReturnValue(of({ products: [] })),
      cartUpdated$: new BehaviorSubject<void>(undefined),
    };
    mockRouter = { navigate: jest.fn() };
    mockToastrService = { success: jest.fn() };
    mockErrorHandlerService = { handleError: jest.fn() };

    component = new HeaderComponent(
      mockUserService as UserService,
      mockCartService as CartService,
      mockRouter as Router,
      mockToastrService as ToastrService,
      mockErrorHandlerService as ErrorHandlerService
    );
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize subscriptions if token is present', () => {
    localStorage.setItem('token', 'test-token');
    component = new HeaderComponent(
      mockUserService as UserService,
      mockCartService as CartService,
      mockRouter as Router,
      mockToastrService as ToastrService,
      mockErrorHandlerService as ErrorHandlerService
    );
    expect(mockUserService.getUserDetails).toHaveBeenCalled();
  });

  it('should toggle cart visibility', () => {
    component.toggleCart();
    expect(component.isCartOpen).toBe(true);
    component.toggleCart();
    expect(component.isCartOpen).toBe(false);
  });

  it('should close cart on checkout', () => {
    component.isCartOpen = true;
    component.checkout();
    expect(component.isCartOpen).toBe(false);
  });

  it('should remove item from cart', () => {
    component.cartItems = [
      {
        id: '1',
        name: 'Product 1',
        price: 100,
        type: 'Type',
        review: 2,
        description: 'Description',
      },
    ];
    component.removeItem(0);
    expect(mockCartService.removeFromCart).toHaveBeenCalledWith('1');
    expect(mockToastrService.success).toHaveBeenCalledWith(
      'Item removed from cart'
    );
  });

  it('should handle error when removing item from cart fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockCartService.removeFromCart as jest.Mock).mockReturnValue(
      throwError(mockErrorResponse)
    );
    component.cartItems = [
      {
        id: '1',
        name: 'Product 1',
        price: 100,
        type: 'Type',
        review: 2,
        description: 'Description',
      },
    ];
    component.removeItem(0);
    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });

  it('should clear username and navigate to login on logout', () => {
    component.onLogoutClick();
    expect(component.username).toBe('');
    expect(localStorage.getItem('token')).toBeNull();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });
  it('should handle error when getting user details fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockUserService.getUserDetails as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    component = new HeaderComponent(
      mockUserService as UserService,
      mockCartService as CartService,
      mockRouter as Router,
      mockToastrService as ToastrService,
      mockErrorHandlerService as ErrorHandlerService
    );

    component.ngOnDestroy(); // Unsubscribe previous subscriptions
    (component as any).initSubscriptions();

    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });

  it('should handle error when getting cart items fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockCartService.getCartItems as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    component.ngOnDestroy();
    (component as any).initSubscriptions();

    mockCartService.cartUpdated$?.next();

    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });
});
