import { HttpErrorResponse } from '@angular/common/http';
import { Component, HostListener, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '@app/core';
import { Product } from '@app/features';
import { CartService } from '@app/features/services';
import { ErrorHandlerService } from '@app/shared';
import { ToastrService } from 'ngx-toastr';
import { filter, Subscription, switchMap } from 'rxjs';

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
    private _errorHandlerService: ErrorHandlerService
  ) {
    if (localStorage.getItem('token') !== null) {
      this.initSubscriptions();
    }
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
    this.username = '';
    localStorage.clear();
    this._router.navigate(['/login']);
  }

  private initSubscriptions(): void {
    this._subs$.add(
      this._userService.userLoggedIn$
        .pipe(
          filter((value) => value),
          switchMap(() => this._userService.getUserDetails())
        )
        .subscribe({
          next: (user) => {
            this.username = user.username!;
          },
          error: (error: HttpErrorResponse) => {
            this._errorHandlerService.handleError(error);
          },
        })
    );
    this._subs$.add(
      this._cartService.cartUpdated$.subscribe(() => {
        this._subs$.add(
          this._cartService.getCartItems().subscribe({
            next: (cart) => {
              this.cartItems = cart.products;
            },
            error: (error: HttpErrorResponse) => {
              this._errorHandlerService.handleError(error);
            },
          })
        );
      })
    );
  }
}
