import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '@app/core/services';
import { ErrorHandlerService } from '@app/shared';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent {
  forgotPasswordForm!: FormGroup;
  isLoading = false;

  constructor(
    fb: FormBuilder,
    private _router: Router,
    private _userService: UserService,
    private _errorHandlerService: ErrorHandlerService
  ) {
    this.forgotPasswordForm = fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  sendResetCode() {
    if (this.forgotPasswordForm.invalid) return;
    this.isLoading = true;
    const email = this.forgotPasswordForm.value.email;
    this._userService
      .sendResetPassCode(email)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {},
        error: (error) => {
          this._errorHandlerService.handleError(error);
        },
      });
  }

  redirectToLogin() {
    this._router.navigate(['/login']);
  }
}
