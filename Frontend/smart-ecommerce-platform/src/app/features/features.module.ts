import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HomeComponent } from './components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductCardComponent } from './shared';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [HomeComponent, ProductCardComponent],
  exports: [HomeComponent, ProductCardComponent],
  imports: [BrowserModule, CommonModule, FormsModule, ReactiveFormsModule],
  providers: [],
})
export class FeaturesModule {}
