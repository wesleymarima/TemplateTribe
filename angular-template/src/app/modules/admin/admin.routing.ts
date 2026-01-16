import {Routes} from '@angular/router';
import {SharedDashboardComponent} from '../shared/components/shared-dashboard/shared-dashboard.component';
import {AdminHomeComponent} from './admin-home/admin-home.component';
import {AdminPersonsHomeComponent} from './admin-persons-home/admin-persons-home.component';
import {AdminPersonViewComponent} from './admin-person-view/admin-person-view.component';
import {AdminPersonCreateComponent} from './admin-person-create/admin-person-create.component';
import {AdminCompaniesHomeComponent} from './admin-companies-home/admin-companies-home.component';
import {AdminCompanyViewComponent} from './admin-company-view/admin-company-view.component';
import {AdminCompanyCreateComponent} from './admin-company-create/admin-company-create.component';
import {AdminBranchesHomeComponent} from './admin-branches-home/admin-branches-home.component';
import {AdminBranchViewComponent} from './admin-branch-view/admin-branch-view.component';
import {AdminBranchCreateComponent} from './admin-branch-create/admin-branch-create.component';

export const AdminRoutes: Routes = [
  {
    path: '', component: SharedDashboardComponent, children: [
      {path: 'home', component: AdminHomeComponent},
      {path: 'persons', component: AdminPersonsHomeComponent},
      {path: 'person-create', component: AdminPersonCreateComponent},
      {path: 'person-view/:id', component: AdminPersonViewComponent},
      {path: 'companies', component: AdminCompaniesHomeComponent},
      {path: 'company-create', component: AdminCompanyCreateComponent},
      {path: 'company-view/:id', component: AdminCompanyViewComponent},
      {path: 'branches', component: AdminBranchesHomeComponent},
      {path: 'branch-create', component: AdminBranchCreateComponent},
      {path: 'branch-view/:id', component: AdminBranchViewComponent}
    ]
  }
]
