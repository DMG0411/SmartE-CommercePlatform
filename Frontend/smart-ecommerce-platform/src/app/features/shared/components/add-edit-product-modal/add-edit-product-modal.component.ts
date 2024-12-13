import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Types } from '@app/features/constants';
import { Product } from '@app/features/models';

@Component({
  selector: 'app-add-product-modal',
  templateUrl: './add-edit-product-modal.component.html',
  styleUrls: ['./add-edit-product-modal.component.scss'],
})
export class AddEditProductModalComponent {
  addProductForm: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
    type: new FormControl('', [Validators.required]),
    description: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
    ]),
    price: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d+(\.\d+)?$/),
    ]),
    review: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[1-5]$/),
    ]),
  });
  modalTitle: string = '';
  typesDropdownSource: Types[] = Object.values(Types);

  constructor(
    private _dialogRef: MatDialogRef<AddEditProductModalComponent>,
    @Inject(MAT_DIALOG_DATA)
    private _data: { product: Product; modalTitle: string }
  ) {
    if (_data.product) {
      this.addProductForm.patchValue(_data.product);
    }
    this.modalTitle = _data.modalTitle;
  }

  isFormInvalid(): boolean {
    return (
      this.addProductForm.invalid ||
      (this._data?.product?.name ===
        this.addProductForm.controls['name'].value &&
        this._data?.product?.type ===
          this.addProductForm.controls['type']?.value &&
        this._data?.product?.description ===
          this.addProductForm.controls['description']?.value &&
        this._data?.product?.price ===
          this.addProductForm.controls['price']?.value &&
        this._data?.product?.review ===
          this.addProductForm.controls['review']?.value)
    );
  }

  onProceedClick(): void {
    this._dialogRef.close(this.getFormData());
  }

  onClose(): void {
    this._dialogRef.close(null);
  }

  isControlInvalid(controlName: string): boolean {
    return (this.addProductForm.controls[controlName]?.invalid &&
      this.addProductForm.controls[controlName]?.touched)!;
  }

  private getFormData(): Product {
    return {
      name: this.addProductForm.controls['name']?.value,
      type: this.addProductForm.controls['type']?.value,
      description: this.addProductForm.controls['description']?.value,
      price: Number(this.addProductForm.controls['price']?.value),
      review: Number(this.addProductForm.controls['review']?.value),
    };
  }
}
