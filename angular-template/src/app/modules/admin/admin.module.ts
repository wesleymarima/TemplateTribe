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
import { AdminPersonCreateComponent } from './admin-person-create/admin-person-create.component';
import { AdminCompaniesHomeComponent } from './admin-companies-home/admin-companies-home.component';
import { AdminCompanyCreateComponent } from './admin-company-create/admin-company-create.component';
import { AdminCompanyViewComponent } from './admin-company-view/admin-company-view.component';
import { AdminBranchesHomeComponent } from './admin-branches-home/admin-branches-home.component';
import { AdminBranchCreateComponent } from './admin-branch-create/admin-branch-create.component';
import { AdminBranchViewComponent } from './admin-branch-view/admin-branch-view.component';


@NgModule({
  declarations: [
    AdminHomeComponent,
    AdminPersonsHomeComponent,
    AdminPersonViewComponent,
    AdminPersonCreateComponent,
    AdminCompaniesHomeComponent,
    AdminCompanyCreateComponent,
    AdminCompanyViewComponent,
    AdminBranchesHomeComponent,
    AdminBranchCreateComponent,
    AdminBranchViewComponent
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
