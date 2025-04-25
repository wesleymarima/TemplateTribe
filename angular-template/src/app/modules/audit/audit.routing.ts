import {Routes} from '@angular/router';
import {SharedDashboardComponent} from '../shared/components/shared-dashboard/shared-dashboard.component';
import {AuditHomeComponent} from './audit-home/audit-home.component';

export const AuditRouting: Routes = [
  {
    path: '', component: SharedDashboardComponent, children: [
      {path: 'home', component: AuditHomeComponent}
    ]
  }
]
