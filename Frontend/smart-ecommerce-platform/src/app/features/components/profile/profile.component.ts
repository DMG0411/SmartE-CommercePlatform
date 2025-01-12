import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { UserService } from '@app/core';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '@app/shared';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  userId: number = 0;
  isLoading: boolean = false;

  constructor(
    private _fb: FormBuilder,
    private _userService: UserService,
    private _toastrService: ToastrService,
    private _errorHandlerService: ErrorHandlerService
  ) {}

  ngOnInit(): void {
    this.profileForm = this._fb.group({
      username: [''],
      email: [{ value: '', disabled: true }],
      phoneNumber: [''],
      city: [''],
    });

    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this._userService
      .getUserDetails()
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe((user) => {
        this.userId = user.id!;
        this.profileForm.patchValue(user);
      });
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      this.isLoading = true;
      const updatedProfile = this.profileForm.getRawValue();
      updatedProfile.password = '';
      this._userService
        .editProfile({ ...updatedProfile, id: this.userId })
        .pipe(finalize(() => (this.isLoading = false)))
        .subscribe({
          next: () => {
            this._toastrService.success('Profile updated successfully');
            this._userService.getUserDetails().subscribe((user) => {
              this.profileForm.patchValue(user);
            });
          },
          error: (error) => {
            this._errorHandlerService.handleError(error);
          },
        });
    }
  }
}
