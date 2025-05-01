import {Component, Input} from '@angular/core';
import {Audit} from '../../models/audit';

@Component({
  selector: 'app-shared-audit-table',
  standalone: false,
  templateUrl: './shared-audit-table.component.html',
  styleUrl: './shared-audit-table.component.scss'
})
export class SharedAuditTableComponent {

  @Input() auditItems: Audit[] = [];

  auditTableHeaders = ['tableName', 'userId', 'type', 'oldValues', 'newValues', 'affectedColumns', 'created', 'primaryKey'];

}
