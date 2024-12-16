import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HomeComponent } from './components';
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

const COMPONENTS: any[] = [
  HomeComponent,
  ProductCardComponent,
  AddEditProductModalComponent,
];

@NgModule({
  declarations: [
    HomeComponent,
    ProductCardComponent,
    AddEditProductModalComponent,
  ],
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
  ],
  providers: [],
})
export class FeaturesModule {}
