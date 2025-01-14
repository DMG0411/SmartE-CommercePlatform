import { ErrorHandlerService } from './error-handler.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

describe('ErrorHandlerService', () => {
  let service: ErrorHandlerService;
  let mockToastrService: Partial<ToastrService>;
  let mockRouter: Partial<Router>;

  beforeEach(() => {
    mockToastrService = {
      error: jest.fn(),
    };
    mockRouter = {
      navigate: jest.fn(),
    };

    service = new ErrorHandlerService(
      mockToastrService as ToastrService,
      mockRouter as Router
    );
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should show session expired message and navigate to login on 401 error', () => {
    const mockErrorResponse = new HttpErrorResponse({
      status: 401,
      statusText: 'Unauthorized',
    });

    service.handleError(mockErrorResponse);

    expect(mockToastrService.error).toHaveBeenCalledWith(
      'Session expired. Please login.'
    );
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should show error message from error.error if it is a string', () => {
    const mockErrorResponse = new HttpErrorResponse({
      status: 400,
      statusText: 'Bad Request',
      error: 'This is a test error message.',
    });

    service.handleError(mockErrorResponse);

    expect(mockToastrService.error).toHaveBeenCalledWith(
      'This is a test error message.'
    );
  });

  it('should show error message from error.error.message if it is an object', () => {
    const mockErrorResponse = new HttpErrorResponse({
      status: 400,
      statusText: 'Bad Request',
      error: { message: 'This is a test error message.' },
    });

    service.handleError(mockErrorResponse);

    expect(mockToastrService.error).toHaveBeenCalledWith(
      'This is a test error message.'
    );
  });

  it('should show default error message if error.error is not a string or object', () => {
    const mockErrorResponse = new HttpErrorResponse({
      status: 400,
      statusText: 'Bad Request',
      error: null,
    });

    service.handleError(mockErrorResponse);

    expect(mockToastrService.error).toHaveBeenCalledWith(
      'Unexpected error occurred.'
    );
  });

  it('should show truncated error message if error.error is a long string', () => {
    const longErrorMessage =
      'This is a long error message. It has multiple sentences. This is the second sentence.';
    const truncatedErrorMessage =
      'This is a long error message. It has multiple sentences.';
    const mockErrorResponse = new HttpErrorResponse({
      status: 400,
      statusText: 'Bad Request',
      error: longErrorMessage,
    });

    service.handleError(mockErrorResponse);

    expect(mockToastrService.error).toHaveBeenCalledWith(truncatedErrorMessage);
  });
});
