import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {authGuard} from '../shared/guards/auth.guard';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('../auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'admin',
    loadChildren: () => import('../admin/admin.module').then(m => m.AdminModule),
    canActivate: [authGuard]
  },
  {
    path: 'audit',
    loadChildren: () => import('../audit/audit.module').then(m => m.AuditModule),
    canActivate: [authGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: true,
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
