import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '@app/core/services';
import { ErrorHandlerService } from '@app/shared';
import { ToastrService } from 'ngx-toastr';
import { finalize, Subscription } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnDestroy {
  registerForm: FormGroup = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
    ]),
    email: new FormControl('', [Validators.required, Validators.email]),
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

  redirectToLogin(): void {
    this._router.navigate(['/login']);
  }

  registerUser(): void {
    this.isLoading = true;
    this._subs$.add(
      this._userService
        .registerUser({
          username: this.registerForm.value.username,
          password: this.registerForm.value.password,
          email: this.registerForm.value.email,
        })
        .pipe(finalize(() => (this.isLoading = false)))
        .subscribe({
          next: () => {
            this._router.navigate(['/login']);
            this._toastrService.success('Registration successful');
          },
          error: (error: HttpErrorResponse) => {
            this._errorHandlerService.handleError(error);
          },
        })
    );
  }
}
