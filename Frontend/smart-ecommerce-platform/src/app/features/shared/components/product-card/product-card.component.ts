import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  ADD_EDIT_PRODUCT_MODAL_CONFIG,
  AddEditProductModalComponent,
  Product,
} from '@app/features';
import { ProductService } from '@app/features/services';

@Component({
  selector: 'smart-ecommerce-platform-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss'],
})
export class ProductCardComponent {
  @Input() product!: Product;
  @Output() productUpdated: EventEmitter<void> = new EventEmitter<void>();
  currency: string = 'EUR';

  constructor(
    private _dialog: MatDialog,
    private _productService: ProductService
  ) {}

  openDeleteDialog(): void {
    this._productService.deleteProduct(this.product.id!).subscribe(() => {
      this.productUpdated.emit();
    });
  }

  openEditDialog(): void {
    this._dialog
      .open(AddEditProductModalComponent, {
        ...ADD_EDIT_PRODUCT_MODAL_CONFIG,
        data: { product: this.product, modalTitle: 'Edit Product' },
      })
      .afterClosed()
      .subscribe((updatedProduct: Product) => {
        if (updatedProduct) {
          updatedProduct.id = this.product.id;
          this._productService.updateProduct(updatedProduct).subscribe(() => {
            this.productUpdated.emit();
          });
        }
      });
  }
}
