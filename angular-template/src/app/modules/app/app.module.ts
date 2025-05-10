import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MaterialModule} from '../shared/material/material.module';
import {SharedModule} from '../shared/shared.module';
import {TablerIconsModule} from 'angular-tabler-icons';
import {NgScrollbarModule} from 'ngx-scrollbar';
import * as TablerIcons from 'angular-tabler-icons/icons';
import {AdminModule} from '../admin/admin.module';
import {AuthModule} from '../auth/auth.module';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {ErrorInterceptor} from '../shared/interceptors/error.interceptor';
import {HeaderInterceptor} from '../shared/interceptors/header.interceptor';
import {LoaderInterceptor} from '../shared/interceptors/loader.interceptor';
import {AuditModule} from '../audit/audit.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    SharedModule,
    TablerIconsModule.pick(TablerIcons),
    NgScrollbarModule,
    AdminModule,
    AuthModule,
    HttpClientModule,
    AuditModule


  ],
  exports: [TablerIconsModule],
  providers: [
    provideAnimationsAsync(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,

    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoaderInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
