import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {CompanyDetailDTO, UpdateCompanyCommand} from '../../shared/models/company';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {BaseComponent} from '../../shared/typings/base-component';
import {RouterConstants} from '../../shared/typings/router-constants';
import {AlertService} from '../../shared/services/alert.service';

@Component({
  selector: 'app-admin-company-view',
  standalone: false,
  templateUrl: './admin-company-view.component.html',
  styleUrl: './admin-company-view.component.scss'
})
export class AdminCompanyViewComponent extends BaseComponent implements OnInit {
  id: number;
  company?: CompanyDetailDTO;
  companyForm: FormGroup;
  isEditing = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private companyRestService: CompanyRestService,
    private router: Router,
    private alertService: AlertService
  ) {
    super();
    this.id = this.activatedRoute.snapshot.params['id'];
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
      'fiscalYearStartMonth': new FormControl(1, [Validators.required, Validators.min(1), Validators.max(12)]),
      'isActive': new FormControl(true)
    });
  }

  ngOnInit(): void {
    this.getCompanyById();
  }

  getCompanyById() {
    this.companyRestService.getById(this.id).subscribe(response => {
      this.company = response;
      this.updatePageName('Company: ' + response.name);
      this.companyForm.patchValue(response);
    });
  }

  toggleEdit() {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.companyForm.patchValue(this.company!);
    }
  }

  onSubmit() {
    if (this.companyForm.valid) {
      const updateCommand: UpdateCompanyCommand = {
        id: this.id,
        ...this.companyForm.value
      };
      this.companyRestService.update(this.id, updateCommand).subscribe({
        next: () => {
          this.alertService.showSuccess('Company updated successfully');
          this.isEditing = false;
          this.getCompanyById();
        },
        error: () => {
          this.alertService.showError('Failed to update company');
        }
      });
    }
  }

  deleteCompany() {
    if (confirm('Are you sure you want to delete this company?')) {
      this.companyRestService.delete(this.id).subscribe({
        next: () => {
          this.alertService.showSuccess('Company deleted successfully');
          this.router.navigate([RouterConstants.ADMIN_COMPANIES]);
        },
        error: () => {
          this.alertService.showError('Failed to delete company');
        }
      });
    }
  }

  protected readonly RouterConstants = RouterConstants;
}
