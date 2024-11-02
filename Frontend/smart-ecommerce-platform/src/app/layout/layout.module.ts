import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HeaderComponent } from './header';

@NgModule({
  declarations: [HeaderComponent],
  exports: [HeaderComponent],
  imports: [BrowserModule],
  providers: [],
})
export class LayoutModule {}
