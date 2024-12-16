import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { WarningModal } from '@app/shared';

@Component({
  selector: 'app-generic-warning-modal',
  templateUrl: './generic-warning-modal.component.html',
  styleUrls: ['./generic-warning-modal.component.scss'],
})
export class GenericWarningModalComponent {
  title: string;
  content: string;
  noButtonText: string;
  proceedButtonText: string;

  constructor(
    public dialogRef: MatDialogRef<GenericWarningModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WarningModal
  ) {
    this.title = data.title;
    this.content = data.message;

    this.noButtonText = data.noButtonText || 'No';
    this.proceedButtonText = data.proceedButtonText || 'Proceed';
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onProceedClick(): void {
    this.dialogRef.close(true);
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
