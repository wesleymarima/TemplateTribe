import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {DepartmentRestService} from '../../shared/services/rest/department.rest.service';
import {DepartmentDTO} from '../../shared/models/department';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-departments-home',
  standalone: false,
  templateUrl: './admin-departments-home.component.html',
  styleUrl: './admin-departments-home.component.scss'
})
export class AdminDepartmentsHomeComponent extends BaseComponent implements OnInit {

  departments: DepartmentDTO[] = [];
  displayedColumns: string[] = ['id', 'code', 'name', 'companyName', 'parentDepartmentName', 'isActive', 'actions'];

  constructor(
    private departmentRestService: DepartmentRestService,
    private alertService: AlertService
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Departments');
    this.getAll();
  }

  getAll() {
    this.departmentRestService.getAll().subscribe(response => {
      this.departments = response;
    });
  }

  deleteDepartment(id: number) {
    if (confirm('Are you sure you want to delete this department?')) {
      this.departmentRestService.delete(id).subscribe(() => {
        this.alertService.showSuccess('Department deleted successfully');
        this.getAll();
      }, error => {
        this.alertService.showError('Failed to delete department: ' + error.error?.message);
      });
    }
  }

  protected readonly RouterConstants = RouterConstants;
}

