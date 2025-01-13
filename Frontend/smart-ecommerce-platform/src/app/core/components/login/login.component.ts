import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '@app/core/services';
import { ErrorHandlerService } from '@app/shared';
import { ToastrService } from 'ngx-toastr';
import { finalize, Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnDestroy {
  loginForm: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
    ]),
  });

  isLoading: boolean = false;

  private _subs$: Subscription = new Subscription();

  constructor(
    private _router: Router,
    private _userService: UserService,
    private _toastrService: ToastrService,
    private _errorHandlerService: ErrorHandlerService
  ) {}

  ngOnDestroy(): void {
    this._subs$.unsubscribe();
  }

  loginUser(): void {
    this.isLoading = true;
    this._subs$.add(
      this._userService
        .loginUser({
          email: this.loginForm.value.email,
          password: this.loginForm.value.password,
        })
        .pipe(finalize(() => (this.isLoading = false)))
        .subscribe({
          next: () => {
            this._router.navigate(['/home']);
            this._userService.userLoggedIn$.next(true);
            this._toastrService.success('Login successful');
          },
          error: (error: HttpErrorResponse) => {
            if (error.status === 404) {
              this._toastrService.error('Email or password are incorrect');
            } else {
              this._errorHandlerService.handleError(error);
            }
          },
        })
    );
  }

  redirectToRegister(): void {
    this._router.navigate(['/register']);
  }

  redirectToForgotPassword(): void {
    this._router.navigate(['/forgot-password']);
  }
}
