import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Cart, Product } from '@app/features/models';
import { CartService } from '@app/features/services';
import { ToastrService } from 'ngx-toastr';
import { finalize, Subscription } from 'rxjs';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CheckoutComponent implements OnDestroy {
  products: Product[] = [];
  isLoading: boolean = false;

  checkoutData = {
    name: '',
    address: '',
    city: '',
    cardNumber: '',
    expiryDate: '',
    cvv: '',
  };

  private _subs$: Subscription = new Subscription();

  constructor(
    private _cartService: CartService,
    private _toastrService: ToastrService,
    private _router: Router
  ) {
    this.isLoading = true;
    this._subs$.add(
      this._cartService
        .getCartItems()
        .pipe(
          finalize(() => {
            this.isLoading = false;
          })
        )
        .subscribe((cart) => {
          this.products = cart.products;
        })
    );
  }

  ngOnDestroy(): void {
    this._subs$.unsubscribe();
  }

  onSubmit() {
    this.removeItemsFromCart();
    this._toastrService.success('Payment Submitted Successfully!');
    this._router.navigate(['/home']);
  }

  getTotalPrice(): number {
    return this.products.reduce((total, product) => total + product.price, 0);
  }

  private removeItemsFromCart(): void {
    this.products.forEach((product) => {
      this._subs$.add(
        this._cartService.removeFromCart(product.id!).subscribe()
      );
    });
    this._cartService.cartUpdated$.next();
  }
}
