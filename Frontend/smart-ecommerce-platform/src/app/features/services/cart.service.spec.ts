import { CartService } from './cart.service';
import { HttpClient } from '@angular/common/http';
import { of, throwError } from 'rxjs';
import { Cart } from '../models';

describe('CartService', () => {
  let service: CartService;
  let httpClientMock: Partial<HttpClient>;

  beforeEach(() => {
    httpClientMock = {
      get: jest.fn(),
      put: jest.fn(),
    };

    service = new CartService(httpClientMock as HttpClient);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get cart items', () => {
    const mockCart: Cart = {
      items: [{ productId: '1', quantity: 1 }],
    } as unknown as Cart;
    (httpClientMock.get as jest.Mock).mockReturnValue(of(mockCart));

    service.getCartItems().subscribe((response) => {
      expect(response).toEqual(mockCart);
    });

    expect(httpClientMock.get).toHaveBeenCalledWith(
      `${service['_baseUrl']}`,
      service['options']
    );
  });

  it('should add product to cart', () => {
    const productId = '1';
    (httpClientMock.put as jest.Mock).mockReturnValue(of(void 0));

    service.addToCart(productId).subscribe((response) => {
      expect(response).toBeUndefined();
    });

    expect(httpClientMock.put).toHaveBeenCalledWith(
      `${service['_baseUrl']}/add/${productId}`,
      {},
      service['options']
    );
  });

  it('should remove product from cart', () => {
    const productId = '1';
    (httpClientMock.put as jest.Mock).mockReturnValue(of(void 0));

    service.removeFromCart(productId).subscribe((response) => {
      expect(response).toBeUndefined();
    });

    expect(httpClientMock.put).toHaveBeenCalledWith(
      `${service['_baseUrl']}/remove/${productId}`,
      service['options']
    );
  });

  it('should handle error when getCartItems fails', () => {
    const mockErrorResponse = {
      status: 404,
      statusText: 'Not Found',
    };
    (httpClientMock.get as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.getCartItems().subscribe(
      () => fail('should have failed with 404 error'),
      (error) => {
        expect(error.status).toBe(404);
        expect(error.statusText).toBe('Not Found');
      }
    );
  });

  it('should handle error when addToCart fails', () => {
    const productId = '1';
    const mockErrorResponse = {
      status: 400,
      statusText: 'Bad Request',
    };
    (httpClientMock.put as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.addToCart(productId).subscribe(
      () => fail('should have failed with 400 error'),
      (error) => {
        expect(error.status).toBe(400);
        expect(error.statusText).toBe('Bad Request');
      }
    );
  });

  it('should handle error when removeFromCart fails', () => {
    const productId = '1';
    const mockErrorResponse = {
      status: 404,
      statusText: 'Not Found',
    };
    (httpClientMock.put as jest.Mock).mockReturnValue(
      throwError(() => mockErrorResponse)
    );

    service.removeFromCart(productId).subscribe(
      () => fail('should have failed with 404 error'),
      (error) => {
        expect(error.status).toBe(404);
        expect(error.statusText).toBe('Not Found');
      }
    );
  });
});
