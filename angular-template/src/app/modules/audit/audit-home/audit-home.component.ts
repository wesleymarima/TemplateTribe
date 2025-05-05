import {Component} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {AuditRestService} from '../../shared/services/rest/audit.rest.service';
import {Audit} from '../../shared/models/audit';

@Component({
  selector: 'app-audit-home',
  standalone: false,
  templateUrl: './audit-home.component.html',
  styleUrl: './audit-home.component.scss'
})
export class AuditHomeComponent extends BaseComponent {

  rangeForm: FormGroup;
  auditItems: Audit[] = [];
  formSubmitted = false;

  constructor(private auditRestService: AuditRestService,) {
    super();
    this.rangeForm = new FormGroup({
      'startDate': new FormControl(null, [Validators.required]),
      'endDate': new FormControl(null, [Validators.required])
    });
  }

  filterData() {
    let item = this.rangeForm.value;
    item.pageNumber = this.pageNumber
    item.pageSize = this.pageSize;
    this.formSubmitted = true;
    this.auditRestService.filterData(item).subscribe(response => {
      this.auditItems = response.items;
    });
  }

}
