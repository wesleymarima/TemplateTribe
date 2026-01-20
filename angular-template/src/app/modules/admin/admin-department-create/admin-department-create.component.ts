import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {BaseComponent} from '../../shared/typings/base-component';
import {DepartmentRestService} from '../../shared/services/rest/department.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {AlertService} from '../../shared/services/alert.service';
import {Router} from '@angular/router';
import {RouterConstants} from '../../shared/typings/router-constants';
import {CompanyDTO} from '../../shared/models/company';
import {DepartmentDTO} from '../../shared/models/department';

@Component({
  selector: 'app-admin-department-create',
  standalone: false,
  templateUrl: './admin-department-create.component.html',
  styleUrl: './admin-department-create.component.scss'
})
export class AdminDepartmentCreateComponent extends BaseComponent implements OnInit {

  companies: CompanyDTO[] = [];
  departments: DepartmentDTO[] = [];

  form = new FormGroup({
    code: new FormControl('', [Validators.required]),
    name: new FormControl('', [Validators.required]),
    description: new FormControl(''),
    managerId: new FormControl<string | null>(null),
    parentDepartmentId: new FormControl<number | null>(null),
    companyId: new FormControl<number | null>(null, [Validators.required])
  });

  constructor(
    private departmentRestService: DepartmentRestService,
    private companyRestService: CompanyRestService,
    private alertService: AlertService,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Department');
    this.loadCompanies();
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe(companies => {
      this.companies = companies;
    });
  }

  onCompanyChange(companyId: number) {
    this.departmentRestService.getByCompany(companyId).subscribe(departments => {
      this.departments = departments;
    });
  }

  get f() {
    return this.form.controls;
  }

  submit() {
    if (this.form.invalid) {
      this.alertService.showError('Please fill all required fields');
      return;
    }

    const command = {
      code: this.form.value.code!,
      name: this.form.value.name!,
      description: this.form.value.description!,
      managerId: this.form.value.managerId || null,
      parentDepartmentId: this.form.value.parentDepartmentId || null,
      companyId: this.form.value.companyId!
    };

    this.departmentRestService.create(command).subscribe({
      next: (id) => {
        this.alertService.showSuccess('Department created successfully');
        this.router.navigate([RouterConstants.ADMIN_DEPARTMENTS]);
      },
      error: (error) => {
        this.alertService.showError('Failed to create department: ' + error.error?.message);
      }
    });
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_DEPARTMENTS]);
  }
}

