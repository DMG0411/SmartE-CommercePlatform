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
    price: new FormControl('', [Validators.required]),
    review: new FormControl('', [Validators.required]),
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
      !(
        this._data?.product.name === this.addProductForm.get('name')?.value &&
        this._data?.product.type === this.addProductForm.get('type')?.value &&
        this._data?.product.description ===
          this.addProductForm.get('description')?.value &&
        this._data?.product.price === this.addProductForm.get('price')?.value &&
        this._data?.product.review === this.addProductForm.get('review')?.value
      )
    );
  }

  onProceedClick(): void {
    this._dialogRef.close(this.getFormData());
  }

  onClose(): void {
    this._dialogRef.close(null);
  }

  private getFormData(): Product {
    return {
      name: this.addProductForm.get('name')?.value,
      type: this.addProductForm.get('type')?.value,
      description: this.addProductForm.get('description')?.value,
      price: Number(this.addProductForm.get('price')?.value),
      review: Number(this.addProductForm.get('review')?.value),
    };
  }
}
