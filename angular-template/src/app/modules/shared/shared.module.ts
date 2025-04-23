import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedDashboardComponent} from './components/shared-dashboard/shared-dashboard.component';
import {RouterModule} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MaterialModule} from '../material/material.module';
import {NgScrollbarModule} from 'ngx-scrollbar';
import {SidebarComponent} from './components/sidebar/sidebar.component';
import {BrandingComponent} from './components/branding.component';
import {NavItemComponent} from './components/nav-item/nav-item.component';
import {TablerIconsModule} from 'angular-tabler-icons';
import {HeaderComponent} from './components/header/header.component';
import {AppHorizontalSidebarComponent} from './components/horizontal/sidebar/sidebar.component';
import {AppBreadcrumbComponent} from './components/breadcrumb/breadcrumb.component';
import {CustomizerComponent} from './components/customizer/customizer.component';
import {
  AppHorizontalHeaderComponent,
  AppHorizontalSearchDialogComponent
} from './components/full/header/header.component';
import {AppHorizontalNavItemComponent} from './components/horizontal/sidebar/nav-item/nav-item.component';


@NgModule({
  declarations: [
    SharedDashboardComponent,
    SidebarComponent,
    BrandingComponent,
    NavItemComponent,
    HeaderComponent,
    AppHorizontalSidebarComponent,
    AppBreadcrumbComponent,
    CustomizerComponent,
    AppHorizontalHeaderComponent,
    AppHorizontalSearchDialogComponent,
    AppHorizontalNavItemComponent
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
