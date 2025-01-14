import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  constructor(private _toastrService: ToastrService, private _router: Router) {}

  handleError(error: HttpErrorResponse): void {
    if (error.status === 401) {
      this._toastrService.error('Session expired. Please login.');
      this._router.navigate(['/login']);
    } else {
      if (typeof error.error === 'string') {
        this._toastrService.error(
          error.error?.substring(
            0,
            error.error?.indexOf('.', error.error?.indexOf('.') + 1) + 1
          ) || 'Unexpected error occurred.'
        );
      } else {
        this._toastrService.error(
          error.error?.message || 'Unexpected error occurred.'
        );
      }
    }
  }
}
