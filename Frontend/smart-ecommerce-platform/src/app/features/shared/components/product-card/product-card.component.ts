import { Component, Input } from '@angular/core';
import { Product } from '@app/features';

@Component({
  selector: 'smart-ecommerce-platform-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss'],
})
export class ProductCardComponent {
  @Input() product!: Product;
  currency: string = 'EUR';
}
