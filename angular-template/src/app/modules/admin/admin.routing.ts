import {Routes} from '@angular/router';
import {SharedDashboardComponent} from '../shared/components/shared-dashboard/shared-dashboard.component';
import {AdminHomeComponent} from './admin-home/admin-home.component';

export const AdminRoutes: Routes = [
  {
    path: '', component: SharedDashboardComponent, children: [
      {path: 'home', component: AdminHomeComponent}
    ]
  }
]
