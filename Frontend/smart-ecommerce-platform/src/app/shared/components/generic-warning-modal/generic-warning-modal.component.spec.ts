import { GenericWarningModalComponent } from './generic-warning-modal.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

class MatDialogRefMock {
  close = jest.fn();
}

describe('GenericWarningModalComponent', () => {
  let component: GenericWarningModalComponent;
  let dialogRefMock: MatDialogRefMock;

  beforeEach(() => {
    dialogRefMock = new MatDialogRefMock();
    const data = {
      title: 'Warning',
      message: 'Are you sure you want to proceed?',
      noButtonText: 'Cancel',
      proceedButtonText: 'Confirm',
    };

    component = new GenericWarningModalComponent(
      dialogRefMock as unknown as MatDialogRef<GenericWarningModalComponent>,
      data as any
    );
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should set title, content, and button texts from input data', () => {
    expect(component.title).toBe('Warning');
    expect(component.content).toBe('Are you sure you want to proceed?');
    expect(component.noButtonText).toBe('Cancel');
    expect(component.proceedButtonText).toBe('Confirm');
  });

  it('should set default button texts if not provided', () => {
    const defaultData = {
      title: 'Warning',
      message: 'Are you sure?',
    };

    const newComponent = new GenericWarningModalComponent(
      dialogRefMock as unknown as MatDialogRef<GenericWarningModalComponent>,
      defaultData as any
    );

    expect(newComponent.noButtonText).toBe('No');
    expect(newComponent.proceedButtonText).toBe('Proceed');
  });

  it('should call dialogRef.close(false) on onNoClick', () => {
    component.onNoClick();
    expect(dialogRefMock.close).toHaveBeenCalledWith(false);
  });

  it('should call dialogRef.close(true) on onProceedClick', () => {
    component.onProceedClick();
    expect(dialogRefMock.close).toHaveBeenCalledWith(true);
  });

  it('should call dialogRef.close() on onClose', () => {
    component.onClose();
    expect(dialogRefMock.close).toHaveBeenCalled();
  });
});
