import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedDashboardComponent} from './components/shared-dashboard/shared-dashboard.component';
import {RouterModule} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MaterialModule} from './material/material.module';
import {NgScrollbarModule} from 'ngx-scrollbar';
import {TablerIconsModule} from 'angular-tabler-icons';
import {SharedAuditTableComponent} from './components/shared-audit-table/shared-audit-table.component';
import {SharedFooterComponent} from './components/shared-footer/shared-footer.component';
import {SharedPaginateComponent} from './components/shared-paginate/shared-paginate.component';
import {SharedLoadingSpinnerComponent} from './components/shared-loading-spinner/shared-loading-spinner.component';


@NgModule({
  declarations: [
    SharedDashboardComponent,
    SharedAuditTableComponent,
    SharedFooterComponent,
    SharedPaginateComponent,
    SharedLoadingSpinnerComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    NgScrollbarModule,
    TablerIconsModule
  ],
  exports: [
    SharedDashboardComponent,
    SharedAuditTableComponent,
    SharedFooterComponent,
    SharedPaginateComponent,
    SharedLoadingSpinnerComponent,
  ]
})
export class SharedModule {
}
