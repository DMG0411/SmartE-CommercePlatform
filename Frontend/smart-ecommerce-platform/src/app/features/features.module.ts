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

@NgModule({
  declarations: [
    HomeComponent,
    ProductCardComponent,
    AddEditProductModalComponent,
  ],
  exports: [HomeComponent, ProductCardComponent, AddEditProductModalComponent],
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
  ],
  providers: [],
})
export class FeaturesModule {}
