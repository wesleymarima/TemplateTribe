import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../shared/typings/base-component';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-company-create',
  standalone: false,
  templateUrl: './admin-company-create.component.html',
  styleUrl: './admin-company-create.component.scss'
})
export class AdminCompanyCreateComponent extends BaseComponent implements OnInit {
  companyForm: FormGroup;

  constructor(
    private companyRestService: CompanyRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.updatePageName('Create Company');
    this.companyForm = new FormGroup({
      'name': new FormControl(null, [Validators.required]),
      'legalName': new FormControl(null, [Validators.required]),
      'taxId': new FormControl(null),
      'registrationNumber': new FormControl(null),
      'email': new FormControl(null, [Validators.email]),
      'phone': new FormControl(null),
      'website': new FormControl(null),
      'logoUrl': new FormControl(null),
      'addressLine1': new FormControl(null),
      'addressLine2': new FormControl(null),
      'city': new FormControl(null),
      'state': new FormControl(null),
      'postalCode': new FormControl(null),
      'country': new FormControl(null),
      'currencyId': new FormControl(null, [Validators.required]),
      'fiscalYearStartMonth': new FormControl(1, [Validators.required, Validators.min(1), Validators.max(12)])
    });
  }

  ngOnInit(): void {
  }

  onSubmit() {
    if (this.companyForm.valid) {
      this.companyRestService.create(this.companyForm.value).subscribe({
        next: (id) => {
          this.alertService.showSuccess('Company created successfully');
          this.router.navigate([RouterConstants.ADMIN_COMPANY_VIEW + id]);
        },
        error: () => {
          this.alertService.showError('Failed to create company');
        }
      });
    }
  }
}
