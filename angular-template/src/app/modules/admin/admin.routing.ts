import {Routes} from '@angular/router';
import {SharedDashboardComponent} from '../shared/components/shared-dashboard/shared-dashboard.component';
import {AdminHomeComponent} from './admin-home/admin-home.component';
import {AdminPersonsHomeComponent} from './admin-persons-home/admin-persons-home.component';
import {AdminPersonViewComponent} from './admin-person-view/admin-person-view.component';

export const AdminRoutes: Routes = [
  {
    path: '', component: SharedDashboardComponent, children: [
      {path: 'home', component: AdminHomeComponent},
      {path: 'persons', component: AdminPersonsHomeComponent},
      {path: 'person-view/:id', component: AdminPersonViewComponent}
    ]
  }
]
