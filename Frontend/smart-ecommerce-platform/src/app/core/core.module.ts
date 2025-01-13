import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  ForgotPasswordComponent,
  LoginComponent,
  RegisterComponent,
} from './components';
import { SharedModule } from '@app/shared';

const COMPONENTS: any[] = [
  LoginComponent,
  RegisterComponent,
  ForgotPasswordComponent,
];

@NgModule({
  declarations: [...COMPONENTS],
  exports: [],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
  ],
  providers: [],
})
export class CoreModule {}
