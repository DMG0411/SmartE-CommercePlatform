import { ProductService } from './product.service';
import { HttpClient } from '@angular/common/http';
import { of, throwError } from 'rxjs';
import { PaginatedResponse } from '@app/shared';
import { Product } from '../models';

describe('ProductService', () => {
  let service: ProductService;
  let httpClientMock: Partial<HttpClient>;

  beforeEach(() => {
    httpClientMock = {
      get: jest.fn(),
      post: jest.fn(),
      put: jest.fn(),
      delete: jest.fn(),
    };

    service = new ProductService(httpClientMock as HttpClient);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get products', () => {
    const mockResponse: PaginatedResponse<Product> = {
      data: [{ id: '1', name: 'Test Product' } as Product],
      totalItems: 1,
      pageSize: 0,
      pageNumber: 0,
      totalPages: 0,
    };
    (httpClientMock.get as jest.Mock).mockReturnValue(of(mockResponse));

    service.getProducts(1, 5).subscribe((response) => {
      expect(response).toEqual(mockResponse);
    });

    expect(httpClientMock.get).toHaveBeenCalledWith(
      `${service['_baseUrl']}`,
      expect.objectContaining({
        params: expect.any(Object),
        headers: expect.any(Object),
      })
    );
  });

  it('should create product', () => {
    const mockProduct: Product = { id: '1', name: 'Test Product' } as Product;
    (httpClientMock.post as jest.Mock).mockReturnValue(of(mockProduct));

    service.createProduct(mockProduct).subscribe((response) => {
      expect(response).toEqual(mockProduct);
    });

    expect(httpClientMock.post).toHaveBeenCalledWith(
      `${service['_baseUrl']}`,
      mockProduct,
      service['options']
    );
  });

  it('should update product', () => {
    const mockProduct: Product = {
      id: '1',
      name: 'Updated Product',
    } as Product;
    (httpClientMock.put as jest.Mock).mockReturnValue(of(mockProduct));

    service.updateProduct(mockProduct).subscribe((response) => {
      expect(response).toEqual(mockProduct);
    });

    expect(httpClientMock.put).toHaveBeenCalledWith(
      `${service['_baseUrl']}`,
      mockProduct,
      service['options']
    );
  });

  it('should delete product', () => {
    const mockId = '1';
    (httpClientMock.delete as jest.Mock).mockReturnValue(of(void 0));

    service.deleteProduct(mockId).subscribe((response) => {
      expect(response).toBeUndefined();
    });

    expect(httpClientMock.delete).toHaveBeenCalledWith(
      `${service['_baseUrl']}/${mockId}`,
      service['options']
    );
  });

  it('should get product by id', () => {
    const mockProduct: Product = { id: '1', name: 'Test Product' } as Product;
    (httpClientMock.get as jest.Mock).mockReturnValue(of(mockProduct));

    service.getProductById('1').subscribe((response) => {
      expect(response).toEqual(mockProduct);
    });

    expect(httpClientMock.get).toHaveBeenCalledWith(
      `${service['_baseUrl']}/1`,
      service['options']
    );
  });

  it('should handle error when getProducts fails', () => {
    const mockErrorResponse = {
      status: 404,
      statusText: 'Not Found',
    };
    (httpClientMock.get as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.getProducts(1, 5).subscribe(
      () => fail('should have failed with 404 error'),
      (error) => {
        expect(error.status).toBe(404);
        expect(error.statusText).toBe('Not Found');
      }
    );
  });

  it('should handle error when createProduct fails', () => {
    const mockProduct: Product = { id: '1', name: 'Test Product' } as Product;
    const mockErrorResponse = {
      status: 400,
      statusText: 'Bad Request',
    };
    (httpClientMock.post as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.createProduct(mockProduct).subscribe(
      () => fail('should have failed with 400 error'),
      (error) => {
        expect(error.status).toBe(400);
        expect(error.statusText).toBe('Bad Request');
      }
    );
  });

  it('should handle error when updateProduct fails', () => {
    const mockProduct: Product = {
      id: '1',
      name: 'Updated Product',
    } as Product;
    const mockErrorResponse = {
      status: 400,
      statusText: 'Bad Request',
    };
    (httpClientMock.put as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.updateProduct(mockProduct).subscribe(
      () => fail('should have failed with 400 error'),
      (error) => {
        expect(error.status).toBe(400);
        expect(error.statusText).toBe('Bad Request');
      }
    );
  });

  it('should handle error when deleteProduct fails', () => {
    const mockId = '1';
    const mockErrorResponse = {
      status: 404,
      statusText: 'Not Found',
    };
    (httpClientMock.delete as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.deleteProduct(mockId).subscribe(
      () => fail('should have failed with 404 error'),
      (error) => {
        expect(error.status).toBe(404);
        expect(error.statusText).toBe('Not Found');
      }
    );
  });

  it('should handle error when getProductById fails', () => {
    const mockErrorResponse = {
      status: 404,
      statusText: 'Not Found',
    };
    (httpClientMock.get as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.getProductById('1').subscribe(
      () => fail('should have failed with 404 error'),
      (error) => {
        expect(error.status).toBe(404);
        expect(error.statusText).toBe('Not Found');
      }
    );
  });
});
