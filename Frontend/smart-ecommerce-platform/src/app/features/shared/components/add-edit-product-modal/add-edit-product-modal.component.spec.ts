import { AddEditProductModalComponent } from './add-edit-product-modal.component';
import { MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormControl } from '@angular/forms';
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
      name: new FormControl(mockData.product?.name),
      type: new FormControl(mockData.product?.type),
      description: new FormControl(mockData.product?.description),
      price: new FormControl(mockData.product?.price),
      review: new FormControl(mockData.product?.review),
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

  it('should return true if form is invalid', () => {
    component.addProductForm.patchValue({
      name: '',
      type: '',
      description: '',
      price: '',
      review: '',
    });
    expect(component.isFormInvalid()).toBe(true);
  });

  it('should return false if form is valid and data matches', () => {
    component.addProductForm.patchValue({
      name: mockData.product?.name,
      type: mockData.product?.type,
      description: mockData.product?.description,
      price: mockData.product?.price,
      review: mockData.product?.review,
    });
    expect(component.isFormInvalid()).toBe(false);
  });

  it('should return true if form is valid but data does not match', () => {
    component.addProductForm.patchValue({
      name: 'Different Product',
      type: 'Different Type',
      description: 'Different description',
      price: 200,
      review: 4,
    });
    expect(component.isFormInvalid()).toBe(true);
  });
});
