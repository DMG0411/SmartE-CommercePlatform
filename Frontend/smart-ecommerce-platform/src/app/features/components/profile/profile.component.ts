import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UserService } from '@app/core';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandlerService } from '@app/shared';
import { finalize, Subscription } from 'rxjs';
import { Product } from '@app/features';
import { ProductService } from '@app/features/services';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  userId: number = 0;
  isLoading: boolean = false;
  products: Product[] = [];
  paginatedProducts: Product[] = [];
  totalCount: number = 0;
  pageNumber: number = 0;
  pageSize: number = 5;

  private _subs$: Subscription = new Subscription();

  constructor(
    private _fb: FormBuilder,
    private _userService: UserService,
    private _toastrService: ToastrService,
    private _errorHandlerService: ErrorHandlerService,
    private _productService: ProductService
  ) {}

  ngOnInit(): void {
    this.profileForm = this._fb.group({
      username: [''],
      email: [{ value: '', disabled: true }],
      phoneNumber: [''],
      city: [''],
    });
    this.loadProfile();
    this.getProducts(this.pageNumber, this.pageSize);
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

  onProductUpdated(): void {
    this.getProducts(this.pageNumber, this.pageSize);
  }

  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.pageNumber = event.pageIndex;
    this.pageSize = event.pageSize;
    this.getProducts(event.pageIndex, event.pageSize);
    const startIndex = event.pageIndex * event.pageSize;
    const endIndex = startIndex + event.pageSize;
    this.updatePaginatedProducts(startIndex, endIndex);
  }

  updatePaginatedProducts(startIndex: number, endIndex: number) {
    this.paginatedProducts = this.products.slice(startIndex, endIndex);
  }

  private getProducts(pageNumber: number, pageSize: number): void {
    this.isLoading = true;
    this._subs$.add(
      this._productService
        .getMyProducts(pageNumber, pageSize)
        .pipe(
          finalize(() => {
            this.isLoading = false;
          })
        )
        .subscribe({
          next: (response) => {
            this.products = response.data;
            this.totalCount = response.totalItems;
          },
          error: (error: HttpErrorResponse) => {
            this._errorHandlerService.handleError(error);
          },
        })
    );
  }
}
