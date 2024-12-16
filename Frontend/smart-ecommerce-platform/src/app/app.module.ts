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
import { ToastrModule } from 'ngx-toastr';
import { provideEnvironmentNgxMask } from 'ngx-mask';

const SMART_E_COMMERCE_PLATORM_MODULES: any[] = [
  LayoutModule,
  FeaturesModule,
  CoreModule,
  SharedModule,
];

@NgModule({
  declarations: [AppComponent],
  imports: [
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ...SMART_E_COMMERCE_PLATORM_MODULES,
  ],
  providers: [provideEnvironmentNgxMask()],
  bootstrap: [AppComponent],
})
export class AppModule {}
