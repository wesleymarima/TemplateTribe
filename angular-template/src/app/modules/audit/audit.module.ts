import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AuditHomeComponent} from './audit-home/audit-home.component';
import {RouterModule} from '@angular/router';
import {AuditRouting} from './audit.routing';
import {ReactiveFormsModule} from '@angular/forms';
import {SharedModule} from '../shared/shared.module';
import {MaterialModule} from '../shared/material/material.module';
import {TablerIconsModule} from 'angular-tabler-icons';


@NgModule({
  declarations: [
    AuditHomeComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    RouterModule.forChild(AuditRouting),
    ReactiveFormsModule,
    MaterialModule,
    SharedModule,
    TablerIconsModule

  ]
})
export class AuditModule {
}
