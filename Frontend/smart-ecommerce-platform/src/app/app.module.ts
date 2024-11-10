import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './layout';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FeaturesModule } from './features';
import { CoreModule } from './core';
import { SharedModule } from './shared';

const SMART_E_COMMERCE_PLATORM_MODULES: any[] = [
  LayoutModule,
  FeaturesModule,
  CoreModule,
  SharedModule,
];

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SMART_E_COMMERCE_PLATORM_MODULES,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
