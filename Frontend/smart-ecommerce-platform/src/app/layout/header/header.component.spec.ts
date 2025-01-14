import { HeaderComponent } from './header.component';
import { UserService } from '@app/core';
import { CartService } from '@app/features/services';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '@app/shared';
import { MatDialog } from '@angular/material/dialog';
import { of, throwError, BehaviorSubject } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let mockUserService: Partial<UserService>;
  let mockCartService: Partial<CartService>;
  let mockRouter: Partial<Router>;
  let mockToastrService: Partial<ToastrService>;
  let mockErrorHandlerService: Partial<ErrorHandlerService>;
  let mockDialog: Partial<MatDialog>;

  beforeEach(() => {
    mockUserService = {
      userLoggedIn$: new BehaviorSubject<boolean>(true),
      getUserDetails: jest.fn().mockReturnValue(of({ username: 'Test User' })),
    };
    mockCartService = {
      removeFromCart: jest.fn().mockReturnValue(of(void 0)),
      cartUpdated$: new BehaviorSubject<void>(void 0),
      getCartItems: jest.fn().mockReturnValue(of({ products: [] })),
    };
    mockRouter = {
      navigate: jest.fn(),
    };
    mockToastrService = {
      success: jest.fn(),
    };
    mockErrorHandlerService = {
      handleError: jest.fn(),
    };
    mockDialog = {
      open: jest.fn().mockReturnValue({
        afterClosed: jest.fn().mockReturnValue(of(true)),
      }),
    };

    component = new HeaderComponent(
      mockUserService as UserService,
      mockCartService as CartService,
      mockRouter as Router,
      mockToastrService as ToastrService,
      mockErrorHandlerService as ErrorHandlerService,
      mockDialog as MatDialog
    );
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should toggle cart visibility', () => {
    component.isCartOpen = false;
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

  it('should remove item from cart and show success message', () => {
    component.cartItems = [
      {
        id: '1',
        name: 'Product 1',
        type: 'Type',
        review: 2,
        description: 'Description',
        price: 5,
        userId: '1',
      },
    ];
    component.removeItem(0);
    expect(mockCartService.removeFromCart).toHaveBeenCalledWith('1');
    expect(mockToastrService.success).toHaveBeenCalledWith(
      'Item removed from cart'
    );
    expect(mockCartService.cartUpdated$).toBeTruthy();
  });

  it('should handle error when removing item from cart fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockCartService.removeFromCart as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );
    component.cartItems = [
      {
        id: '1',
        name: 'Product 1',
        type: 'Type',
        review: 2,
        description: 'Description',
        price: 5,
        userId: '1',
      },
    ];
    component.removeItem(0);
    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });

  it('should clear username and navigate to login on logout if confirmed', () => {
    (mockDialog.open as jest.Mock).mockReturnValue({
      afterClosed: jest.fn().mockReturnValue(of(true)),
    });

    component.onLogoutClick();
    expect(component.username).toBe('');
    expect(localStorage.getItem('token')).toBeNull();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should not clear username and navigate to login on logout if not confirmed', () => {
    (mockDialog.open as jest.Mock).mockReturnValue({
      afterClosed: jest.fn().mockReturnValue(of(false)),
    });

    component.username = 'Test User';
    localStorage.setItem('token', 'mock-token');
    component.onLogoutClick();
    expect(component.username).toBe('Test User');
    expect(localStorage.getItem('token')).toBe('mock-token');
    expect(mockRouter.navigate).not.toHaveBeenCalled();
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
    component['initSubscriptions']();
    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });
});
