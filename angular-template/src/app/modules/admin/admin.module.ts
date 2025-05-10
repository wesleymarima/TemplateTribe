import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AdminHomeComponent} from './admin-home/admin-home.component';
import {RouterModule} from '@angular/router';
import {ReactiveFormsModule} from '@angular/forms';
import {MaterialModule} from '../shared/material/material.module';
import {SharedModule} from '../shared/shared.module';
import {AdminRoutes} from './admin.routing';
import {AdminPersonsHomeComponent} from './admin-persons-home/admin-persons-home.component';
import { AdminPersonViewComponent } from './admin-person-view/admin-person-view.component';


@NgModule({
  declarations: [
    AdminHomeComponent,
    AdminPersonsHomeComponent,
    AdminPersonViewComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MaterialModule,
    SharedModule,
    RouterModule.forChild(AdminRoutes)
  ]
})
export class AdminModule {
}
