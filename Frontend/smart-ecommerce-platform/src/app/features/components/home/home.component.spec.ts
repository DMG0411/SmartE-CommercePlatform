import { HomeComponent } from './home.component';
import { MatDialog } from '@angular/material/dialog';
import { ProductService } from '@app/features/services';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '@app/shared';
import { of, throwError, EMPTY } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let mockProductService: Partial<ProductService>;
  let mockDialog: Partial<MatDialog>;
  let mockToastrService: Partial<ToastrService>;
  let mockErrorHandlerService: Partial<ErrorHandlerService>;

  beforeEach(() => {
    mockProductService = {
      getProducts: jest.fn().mockReturnValue(of({ data: [], totalItems: 0 })),
      createProduct: jest.fn().mockReturnValue(of({})),
    };
    mockDialog = {
      open: jest.fn().mockReturnValue({
        afterClosed: jest.fn().mockReturnValue(of(true)),
      }),
    };
    mockToastrService = { success: jest.fn() };
    mockErrorHandlerService = { handleError: jest.fn() };

    component = new HomeComponent(
      mockProductService as ProductService,
      mockDialog as MatDialog,
      mockToastrService as ToastrService,
      mockErrorHandlerService as ErrorHandlerService
    );
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize and load products on init', () => {
    const getProductsSpy = jest.spyOn(component as any, 'getProducts');
    component.ngOnInit();
    expect(getProductsSpy).toHaveBeenCalledWith(
      component.pageNumber,
      component.pageSize
    );
  });

  it('should unsubscribe on destroy', () => {
    const unsubscribeSpy = jest.spyOn(component['_subs$'], 'unsubscribe');
    component.ngOnDestroy();
    expect(unsubscribeSpy).toHaveBeenCalled();
  });

  it('should update products on product update', () => {
    const getProductsSpy = jest.spyOn(component as any, 'getProducts');
    component.onProductUpdated();
    expect(getProductsSpy).toHaveBeenCalledWith(
      component.pageNumber,
      component.pageSize
    );
  });

  it('should open add product modal and add product on confirmation', () => {
    component.openAddProductModal();
    expect(mockDialog.open).toHaveBeenCalled();
    expect(mockProductService.createProduct).toHaveBeenCalled();
    expect(mockToastrService.success).toHaveBeenCalledWith(
      'Product added successfully'
    );
  });

  it('should handle error when adding product fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockProductService.createProduct as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );
    component.openAddProductModal();
    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });

  it('should update paginated products on page change', () => {
    const updatePaginatedProductsSpy = jest.spyOn(
      component as any,
      'updatePaginatedProducts'
    );
    component.onPageChange({ pageIndex: 1, pageSize: 5 });
    expect(updatePaginatedProductsSpy).toHaveBeenCalledWith(5, 10);
  });

  it('should update paginated products correctly', () => {
    component.products = Array.from(
      { length: 10 },
      (_, i) => ({ id: i + 1, name: `Product ${i + 1}` } as any)
    );
    component.updatePaginatedProducts(0, 5);
    expect(component.paginatedProducts.length).toBe(5);
    expect(component.paginatedProducts[0].name).toBe('Product 1');
  });

  it('should handle error when getting products fails', () => {
    const mockErrorResponse = new HttpErrorResponse({
      error: 'Error message',
      status: 400,
      statusText: 'Bad Request',
    });
    (mockProductService.getProducts as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );
    component['getProducts'](0, 5);
    expect(mockErrorHandlerService.handleError).toHaveBeenCalledWith(
      mockErrorResponse
    );
  });
});
