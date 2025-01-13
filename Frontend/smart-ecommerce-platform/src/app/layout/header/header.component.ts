import { HttpErrorResponse } from '@angular/common/http';
import { Component, HostListener, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { UserService } from '@app/core';
import { Product } from '@app/features';
import { CartService } from '@app/features/services';
import { ErrorHandlerService, GenericWarningModalComponent } from '@app/shared';
import { ToastrService } from 'ngx-toastr';
import { filter, Subscription, switchMap, tap } from 'rxjs';

@Component({
  selector: 'smart-ecommerce-platform-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnDestroy {
  username: string = '';
  isCartOpen = false;
  cartItems: Product[] = [];

  private _subs$: Subscription = new Subscription();

  constructor(
    private _userService: UserService,
    private _cartService: CartService,
    private _router: Router,
    private _toastrService: ToastrService,
    private _errorHandlerService: ErrorHandlerService,
    private _dialog: MatDialog
  ) {
    this.initSubscriptions();
  }

  ngOnDestroy(): void {
    this._subs$.unsubscribe();
  }

  toggleCart(): void {
    this.isCartOpen = !this.isCartOpen;
  }

  checkout(): void {
    this.isCartOpen = false;
  }

  goToProfile(): void {
    this._router.navigate(['/profile']);
  }

  goToHome(): void {
    this._router.navigate(['/home']);
  }

  removeItem(index: number): void {
    this._subs$.add(
      this._cartService.removeFromCart(this.cartItems[index].id!).subscribe({
        next: () => {
          this._toastrService.success('Item removed from cart');
          this._cartService.cartUpdated$.next();
        },
        error: (error: HttpErrorResponse) => {
          this._errorHandlerService.handleError(error);
        },
      })
    );
  }

  onLogoutClick(): void {
    this._dialog
      .open(GenericWarningModalComponent, {
        data: {
          title: 'Logout',
          message: 'Are you sure you want to logout?',
          proceedButtonText: 'Logout',
          noButtonText: 'Cancel',
        },
      })
      .afterClosed()
      .subscribe((proceed) => {
        if (proceed) {
          this.username = '';
          localStorage.clear();
          this._router.navigate(['/login']);
        }
      });
  }

  redirectToCheckout(): void {
    this._router.navigate(['/checkout']);
  }

  private initSubscriptions(): void {
    this._subs$.add(
      this._userService.userLoggedIn$
        .pipe(
          filter((value) => value),
          switchMap(() => this._userService.getUserDetails()),
          tap((user) => {
            this.username = user.username!;
          }),
          switchMap(() => this._cartService.cartUpdated$),
          filter(() => localStorage.getItem('token') !== null),
          switchMap(() => this._cartService.getCartItems())
        )
        .subscribe({
          next: (cart) => {
            this.cartItems = cart.products;
          },
          error: (error: HttpErrorResponse) => {
            this._errorHandlerService.handleError(error);
          },
        })
    );
  }
}
