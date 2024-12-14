import { AddEditProductModalComponent } from './add-edit-product-modal.component';
import { MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Product } from '@app/features/models';

describe('AddEditProductModalComponent', () => {
  let component: AddEditProductModalComponent;
  let mockDialogRef: Partial<MatDialogRef<AddEditProductModalComponent>>;
  let mockData: Partial<{ product: Product }>;

  beforeEach(() => {
    mockDialogRef = {
      close: jest.fn(),
    };
    mockData = {
      product: {
        name: 'Test Product',
        type: 'Test Type',
        description: 'Test Description',
        price: 100,
        review: 2,
      },
    };

    component = new AddEditProductModalComponent(
      mockDialogRef as MatDialogRef<AddEditProductModalComponent>,
      mockData as any
    );

    component.addProductForm = new FormGroup({
      name: new FormControl(mockData.product?.name, [Validators.required]),
      type: new FormControl(mockData.product?.type, [Validators.required]),
      description: new FormControl(mockData.product?.description, [
        Validators.required,
        Validators.minLength(10),
      ]),
      price: new FormControl(mockData.product?.price, [
        Validators.required,
        Validators.pattern(/^\d+(\.\d+)?$/),
      ]),
      review: new FormControl(mockData.product?.review, [
        Validators.required,
        Validators.pattern(/^[1-5]$/),
      ]),
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with product data if provided', () => {
    expect(component.addProductForm.value).toEqual({
      name: 'Test Product',
      type: 'Test Type',
      description: 'Test Description',
      price: 100,
      review: 2,
    });
  });

  it('should close dialog with form data on proceed click', () => {
    component.onProceedClick();
    expect(mockDialogRef.close).toHaveBeenCalledWith({
      name: 'Test Product',
      type: 'Test Type',
      description: 'Test Description',
      price: 100,
      review: 2,
    });
  });

  it('should close dialog with null on close', () => {
    component.onClose();
    expect(mockDialogRef.close).toHaveBeenCalledWith(null);
  });

  it('should return form data correctly', () => {
    const formData = component['getFormData']();
    expect(formData).toEqual({
      name: 'Test Product',
      type: 'Test Type',
      description: 'Test Description',
      price: 100,
      review: 2,
    });
  });

  it('should check if form data matches product data', () => {
    component.addProductForm.patchValue({
      name: 'Test Product',
      type: 'Test Type',
      description: 'Test Description',
      price: 100,
      review: 2,
    });
    expect(component.isFormInvalid()).toBe(false);

    component.addProductForm.patchValue({ name: 'Changed Product' });
    expect(component.isFormInvalid()).toBe(true);
  });

  it('should return true if form is invalid', () => {
    component.addProductForm.patchValue({
      name: '',
      type: '',
      description: 'short',
      price: 'invalid_price',
      review: '6',
    });
    expect(component.isFormInvalid()).toBe(true);
  });

  it('should return false if form is valid and data matches', () => {
    component.addProductForm.patchValue({
      name: mockData.product?.name,
      type: mockData.product?.type,
      description: mockData.product?.description,
      price: mockData.product?.price?.toString(),
      review: mockData.product?.review?.toString(),
    });
    expect(component.isFormInvalid()).toBe(false);
  });

  it('should return true if form is valid but data does not match', () => {
    component.addProductForm.patchValue({
      name: 'Different Product',
      type: 'Different Type',
      description: 'A completely different valid description',
      price: '200',
      review: '4',
    });
    expect(component.isFormInvalid()).toBe(true);
  });

  it('should return true if control is invalid and touched', () => {
    component.addProductForm.controls['name'].markAsTouched();
    component.addProductForm.controls['name'].setValue('');
    expect(component.isControlInvalid('name')).toBe(true);
  });

  it('should return false if control is valid and touched', () => {
    component.addProductForm.controls['name'].markAsTouched();
    component.addProductForm.controls['name'].setValue('Valid Name');
    expect(component.isControlInvalid('name')).toBe(false);
  });
});
