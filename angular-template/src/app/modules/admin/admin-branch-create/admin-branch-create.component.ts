import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {BranchRestService} from '../../shared/services/rest/branch.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {RouterConstants} from '../../shared/typings/router-constants';
import {CompanyDTO} from '../../shared/models/company';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-branch-create',
  standalone: false,
  templateUrl: './admin-branch-create.component.html',
  styleUrl: './admin-branch-create.component.scss'
})
export class AdminBranchCreateComponent extends BaseComponent implements OnInit {
  branchForm: FormGroup;
  companies: CompanyDTO[] = [];

  constructor(
    private branchRestService: BranchRestService,
    private companyRestService: CompanyRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.updatePageName('Create Branch');
    this.branchForm = new FormGroup({
      'name': new FormControl(null, [Validators.required]),
      'code': new FormControl(null, [Validators.required]),
      'email': new FormControl(null, [Validators.email]),
      'phone': new FormControl(null),
      'description': new FormControl(null),
      'addressLine1': new FormControl(null),
      'addressLine2': new FormControl(null),
      'city': new FormControl(null),
      'state': new FormControl(null),
      'postalCode': new FormControl(null),
      'country': new FormControl(null),
      'branchType': new FormControl(null, [Validators.required]),
      'isHeadquarters': new FormControl(false),
      'businessHours': new FormControl(null),
      'companyId': new FormControl(null, [Validators.required]),
      'managerId': new FormControl(null)
    });
  }

  ngOnInit(): void {
    this.getCompanies();
  }

  getCompanies() {
    this.companyRestService.getAll().subscribe(response => {
      this.companies = response;
    });
  }

  onSubmit() {
    if (this.branchForm.valid) {
      this.branchRestService.create(this.branchForm.value).subscribe({
        next: (id) => {
          this.alertService.showSuccess('Branch created successfully');
          this.router.navigate([RouterConstants.ADMIN_BRANCH_VIEW + id]);
        },
        error: () => {
          this.alertService.showError('Failed to create branch');
        }
      });
    }
  }
}
