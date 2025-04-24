import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedDashboardComponent} from './components/shared-dashboard/shared-dashboard.component';
import {RouterModule} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MaterialModule} from '../material/material.module';
import {NgScrollbarModule} from 'ngx-scrollbar';
import {TablerIconsModule} from 'angular-tabler-icons';


@NgModule({
  declarations: [
    SharedDashboardComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    NgScrollbarModule,
    TablerIconsModule
  ]
})
export class SharedModule {
}
