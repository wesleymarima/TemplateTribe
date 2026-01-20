import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {BaseComponent} from '../../shared/typings/base-component';
import {FinancialPeriodRestService} from '../../shared/services/rest/financial-period.rest.service';
import {CompanyRestService} from '../../shared/services/rest/company.rest.service';
import {AlertService} from '../../shared/services/alert.service';
import {Router} from '@angular/router';
import {RouterConstants} from '../../shared/typings/router-constants';
import {CompanyDTO} from '../../shared/models/company';

@Component({
  selector: 'app-admin-financial-period-create',
  standalone: false,
  templateUrl: './admin-financial-period-create.component.html',
  styleUrl: './admin-financial-period-create.component.scss'
})
export class AdminFinancialPeriodCreateComponent extends BaseComponent implements OnInit {

  companies: CompanyDTO[] = [];

  form = new FormGroup({
    name: new FormControl('', [Validators.required]),
    periodNumber: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    fiscalYear: new FormControl<number | null>(null, [Validators.required]),
    startDate: new FormControl('', [Validators.required]),
    endDate: new FormControl('', [Validators.required]),
    companyId: new FormControl<number | null>(null, [Validators.required])
  });

  constructor(
    private financialPeriodRestService: FinancialPeriodRestService,
    private companyRestService: CompanyRestService,
    private alertService: AlertService,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.baseSharedService.updateTitle('Create Financial Period');
    this.loadCompanies();

    // Set default fiscal year to current year
    const currentYear = new Date().getFullYear();
    this.form.patchValue({fiscalYear: currentYear});
  }

  loadCompanies() {
    this.companyRestService.getAll().subscribe(companies => {
      this.companies = companies;
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
      name: this.form.value.name!,
      periodNumber: this.form.value.periodNumber!,
      fiscalYear: this.form.value.fiscalYear!,
      startDate: this.form.value.startDate!,
      endDate: this.form.value.endDate!,
      companyId: this.form.value.companyId!
    };

    this.financialPeriodRestService.create(command).subscribe({
      next: (id) => {
        this.alertService.showSuccess('Financial period created successfully');
        this.router.navigate([RouterConstants.ADMIN_FINANCIAL_PERIODS]);
      },
      error: (error) => {
        this.alertService.showError('Failed to create financial period: ' + error.error?.message);
      }
    });
  }

  cancel() {
    this.router.navigate([RouterConstants.ADMIN_FINANCIAL_PERIODS]);
  }
}

