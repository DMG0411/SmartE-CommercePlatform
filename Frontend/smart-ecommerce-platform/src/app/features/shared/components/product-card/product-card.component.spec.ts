import { ProductCardComponent } from './product-card.component';
import { MatDialog } from '@angular/material/dialog';
import { ProductService, CartService } from '@app/features/services';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService, GenericWarningModalComponent } from '@app/shared';
import { of, throwError, BehaviorSubject } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { EventEmitter } from '@angular/core';

describe('ProductCardComponent', () => {
  let component: ProductCardComponent;
  let mockDialog: Partial<MatDialog>;
  let mockProductService: Partial<ProductService>;
  let mockToastrService: Partial<ToastrService>;
  let mockCartService: Partial<CartService>;
  let mockErrorHandlerService: Partial<ErrorHandlerService>;

  beforeEach(() => {
    mockDialog = {
      open: jest.fn().mockReturnValue({
        afterClosed: jest.fn().mockReturnValue(of(true)),
      }),
    };
    mockProductService = {
      deleteProduct: jest.fn().mockReturnValue(of({})),
      updateProduct: jest.fn().mockReturnValue(of({})),
    };
    mockToastrService = { success: jest.fn() };
    mockCartService = {
      addToCart: jest.fn().mockReturnValue(of({})),
      cartUpdated$: new BehaviorSubject<void>(undefined),
    };
    if (mockCartService.cartUpdated$) {
      jest.spyOn(mockCartService.cartUpdated$, 'next');
    }
    mockErrorHandlerService = { handleError: jest.fn() };

    component = new ProductCardComponent(
      mockDialog as MatDialog,
      mockProductService as ProductService,
      mockToastrService as ToastrService,
      mockCartService as CartService,
      mockErrorHandlerService as ErrorHandlerService
    );

    component.product = { id: '1', name: 'Test Product' } as any;
    component.productUpdated = new EventEmitter<void>();
    jest.spyOn(component.productUpdated, 'emit');
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should open delete dialog and delete product on confirmation', () => {
    component.openDeleteDialog();
    expect(mockDialog.open).toHaveBeenCalledWith(GenericWarningModalComponent, {
      data: {
        title: 'Delete Product',
        message: 'Are you sure you want to delete this product?',
        noButtonText: 'No',
        proceedButtonText: 'Proceed',
      },
    });
    expect(mockProductService.deleteProduct).toHaveBeenCalledWith('1');
    expect(mockToastrService.success).toHaveBeenCalledWith(
      'Product deleted successfully'
    );
    expect(component.productUpdated.emit).toHaveBeenCalled();
  });

  it('should handle error when deleting product fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockProductService.deleteProduct as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );
    component.openDeleteDialog();
    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });

  it('should open edit dialog and update product on confirmation', () => {
    (mockDialog.open as jest.Mock).mockReturnValue({
      afterClosed: jest
        .fn()
        .mockReturnValue(of({ id: '1', name: 'Updated Product' })),
    });
    component.openEditDialog();
    expect(mockDialog.open).toHaveBeenCalled();
    expect(mockProductService.updateProduct).toHaveBeenCalledWith({
      id: '1',
      name: 'Updated Product',
    });
    expect(mockToastrService.success).toHaveBeenCalledWith(
      'Product updated successfully'
    );
    expect(component.productUpdated.emit).toHaveBeenCalled();
  });

  it('should handle error when updating product fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockProductService.updateProduct as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );
    (mockDialog.open as jest.Mock).mockReturnValue({
      afterClosed: jest
        .fn()
        .mockReturnValue(of({ id: '1', name: 'Updated Product' })),
    });
    component.openEditDialog();
    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });

  it('should add product to cart', () => {
    component.addToCart('1');
    expect(mockCartService.addToCart).toHaveBeenCalledWith('1');
    expect(mockToastrService.success).toHaveBeenCalledWith(
      'Product added to cart'
    );
    if (mockCartService.cartUpdated$) {
      expect(mockCartService.cartUpdated$.next).toHaveBeenCalled();
    }
  });

  it('should handle error when adding product to cart fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockCartService.addToCart as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );
    component.addToCart('1');
    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });

  it('should unsubscribe on destroy', () => {
    const unsubscribeSpy = jest.spyOn(component['_subs$'], 'unsubscribe');
    component.ngOnDestroy();
    expect(unsubscribeSpy).toHaveBeenCalled();
  });
});
