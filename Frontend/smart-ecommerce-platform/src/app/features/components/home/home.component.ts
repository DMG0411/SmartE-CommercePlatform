import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import {
  ADD_EDIT_PRODUCT_MODAL_CONFIG,
  AddEditProductModalComponent,
  Product,
} from '@app/features';
import { Types } from '@app/features/constants';
import { ProductService } from '@app/features/services';
import { ErrorHandlerService } from '@app/shared';
import { ToastrService } from 'ngx-toastr';
import { EMPTY, finalize, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'smart-ecommerce-platform-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnDestroy {
  @ViewChild('paginator') paginator: MatPaginator | undefined;
  isLoading: boolean = false;

  pageNumber: number = 0;
  pageSize: number = 5;
  totalCount: number = 0;
  products: Product[] = [];
  paginatedProducts: Product[] = [];
  typesDropdownSource: Types[] = Object.values(Types);
  productTypes: string[] = Object.values(Types);
  filterForm: FormGroup;

  private _subs$: Subscription = new Subscription();

  constructor(
    private _productService: ProductService,
    private _dialog: MatDialog,
    private _toastrService: ToastrService,
    private _errorHandlerService: ErrorHandlerService,
    fb: FormBuilder
  ) {
    this.filterForm = fb.group({
      type: [''],
      minPrice: [''],
      maxPrice: [''],
      minReview: [''],
    });
  }

  ngOnInit() {
    this.onPageChange({ pageIndex: this.pageNumber, pageSize: this.pageSize });
  }

  ngOnDestroy(): void {
    this._subs$.unsubscribe();
  }

  onProductUpdated(): void {
    this.getProducts(this.pageNumber, this.pageSize);
  }

  openAddProductModal(): void {
    this._subs$.add(
      this._dialog
        .open(AddEditProductModalComponent, {
          ...ADD_EDIT_PRODUCT_MODAL_CONFIG,
          data: { product: null, modalTitle: 'Add Product' },
        })
        .afterClosed()
        .pipe(
          switchMap((data) => {
            if (!data) return EMPTY;

            this.isLoading = true;

            return this._productService.createProduct(data).pipe(
              finalize(() => {
                this.isLoading = false;
              })
            );
          })
        )
        .subscribe({
          next: () => {
            this.getProducts(this.pageNumber, this.pageSize);
            this._toastrService.success('Product added successfully');
          },
          error: (error: HttpErrorResponse) => {
            this._errorHandlerService.handleError(error);
          },
        })
    );
  }

  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.pageNumber = event.pageIndex;
    this.pageSize = event.pageSize;
    this.getProducts(event.pageIndex, event.pageSize);
    const startIndex = event.pageIndex * event.pageSize;
    const endIndex = startIndex + event.pageSize;
    this.updatePaginatedProducts(startIndex, endIndex);
  }

  updatePaginatedProducts(startIndex: number, endIndex: number) {
    this.paginatedProducts = this.products.slice(startIndex, endIndex);
  }

  onApplyFilters(): void {
    const { type, minPrice, maxPrice, minReview } = this.filterForm.value;
    this.getProducts(
      this.pageNumber,
      this.pageSize,
      type,
      Number(minPrice),
      Number(maxPrice),
      Number(minReview)
    );
  }

  private getProducts(
    pageNumber: number,
    pageSize: number,
    type: string | null = null,
    minPrice: number | null = null,
    maxPrice: number | null = null,
    minReview: number | null = null
  ): void {
    this.isLoading = true;
    this._subs$.add(
      this._productService
        .getProducts(pageNumber, pageSize, type, minPrice, maxPrice, minReview)
        .pipe(
          finalize(() => {
            this.isLoading = false;
          })
        )
        .subscribe({
          next: (response) => {
            this.products = response.data;
            this.totalCount = response.totalItems;
          },
          error: (error: HttpErrorResponse) => {
            this._errorHandlerService.handleError(error);
          },
        })
    );
  }
}
