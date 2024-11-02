import { Component } from '@angular/core';
import { Product } from '@app/features';
import { ProductService } from '@app/features/services';

@Component({
  selector: 'smart-ecommerce-platform-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  constructor(private _productService: ProductService) {}

  products: Product[] = [];

  ngOnInit() {
    this.getProducts();
  }

  private getProducts(): void {
    this._productService.getProducts().subscribe((products) => {
      this.products = products;
    });
  }
}
