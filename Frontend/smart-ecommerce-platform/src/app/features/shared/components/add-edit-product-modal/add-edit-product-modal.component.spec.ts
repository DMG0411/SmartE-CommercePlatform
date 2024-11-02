import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddEditProductModalComponent } from './add-edit-product-modal.component';

describe('AddEditProductModalComponent', () => {
  let component: AddEditProductModalComponent;
  let fixture: ComponentFixture<AddEditProductModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddEditProductModalComponent],
    });
    fixture = TestBed.createComponent(AddEditProductModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
