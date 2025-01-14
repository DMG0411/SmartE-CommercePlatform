import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {
  CheckoutComponent,
  HomeComponent,
  ProfileComponent,
} from './components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddEditProductModalComponent, ProductCardComponent } from './shared';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SharedModule } from '@app/shared';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { MatTabsModule } from '@angular/material/tabs';

const COMPONENTS: any[] = [
  HomeComponent,
  ProductCardComponent,
  AddEditProductModalComponent,
  ProfileComponent,
  CheckoutComponent,
];

@NgModule({
  declarations: [...COMPONENTS],
  exports: [...COMPONENTS],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatIconModule,
    MatTooltipModule,
    SharedModule,
    MatPaginatorModule,
    MatTableModule,
    NgxMaskDirective,
    NgxMaskPipe,
    MatTabsModule,
  ],
  providers: [],
})
export class FeaturesModule {}
