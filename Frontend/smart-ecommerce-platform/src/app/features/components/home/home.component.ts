import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  ADD_EDIT_PRODUCT_MODAL_CONFIG,
  AddEditProductModalComponent,
  Product,
} from '@app/features';
import { ProductService } from '@app/features/services';

@Component({
  selector: 'smart-ecommerce-platform-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  constructor(
    private _productService: ProductService,
    private _dialog: MatDialog
  ) {}

  products: Product[] = [];

  ngOnInit() {
    this.getProducts();
  }

  onProductUpdated(): void {
    this.getProducts();
  }

  openAddProductModal(): void {
    this._dialog
      .open(AddEditProductModalComponent, {
        ...ADD_EDIT_PRODUCT_MODAL_CONFIG,
        data: { product: null, modalTitle: 'Add Product' },
      })
      .afterClosed()
      .subscribe((data) => {
        if (data) {
          this._productService.createProduct(data).subscribe(() => {
            this.getProducts();
          });
        }
      });
  }

  private getProducts(): void {
    this._productService.getProducts().subscribe((products) => {
      this.products = products;
    });
  }
}
