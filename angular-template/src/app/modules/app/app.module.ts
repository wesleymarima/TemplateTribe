import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MaterialModule} from '../material/material.module';
import {SharedModule} from '../shared/shared.module';
import {TablerIconsModule} from 'angular-tabler-icons';
import {NgScrollbarModule} from 'ngx-scrollbar';
import * as TablerIcons from 'angular-tabler-icons/icons';
import {AdminModule} from '../admin/admin.module';
import {AuthModule} from '../auth/auth.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    SharedModule,
    TablerIconsModule.pick(TablerIcons),
    NgScrollbarModule,
    AdminModule,
    AuthModule

  ],
  exports: [TablerIconsModule],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
